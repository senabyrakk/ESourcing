using ESourcing.Orders.Consumers;
using ESourcing.Orders.Extensions;
using EventBusRabbitMq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orders.Application;
using Orders.Domain.Repositories;
using Orders.Domain.Repositories.Base;
using Orders.Infrastructure.Data;
using Orders.Infrastructure.Repositories;
using Orders.Infrastructure.Repositories.Base;
using RabbitMQ.Client;

namespace ESourcing.Orders
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            services.AddDbContext<OrderContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("ConnectionString")));
            services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddTransient<IOrderRepository, OrderRespoitory>();
            services.AddApplication();

            services.AddAutoMapper(typeof(Startup));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Title = "ESourcing.Order",
                        Version = "v1"
                    });
            });

            services.AddSingleton<IRabbitMQPersistentConnection>(sp => {
                var logger = sp.GetRequiredService<ILogger<RabbitMQPersistentConnection>>();
                var factory = new ConnectionFactory()
                {
                    HostName = Configuration["EventBus:HostName"]
                };

                if (!string.IsNullOrWhiteSpace(Configuration["EventBus:UserName"]))
                    factory.UserName = Configuration["EventBus:UserName"];

                if (!string.IsNullOrWhiteSpace(Configuration["EventBus:Password"]))
                    factory.Password = Configuration["EventBus:Password"];

                var retryCount = 5;

                if (!string.IsNullOrWhiteSpace(Configuration["EventBus:RetryCount"]))
                    retryCount = int.Parse(Configuration["EventBus:RetryCount"]);

                return new RabbitMQPersistentConnection(factory, retryCount, logger);
            });

            services.AddSingleton<EventBusOrderCreateConsumer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ESourcing.Order"));

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseEventBusListener();

        }
    }
}
