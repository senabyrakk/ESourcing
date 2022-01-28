using EventBusRabbitMq.Events.Abstract;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Net.Sockets;
using System.Text;

namespace EventBusRabbitMq.Producer
{
    public class EventBusRabbitMqProducer
    {
        private readonly IRabbitMQPersistentConnection _rabbitMQPersistentConnection;
        private readonly ILogger<EventBusRabbitMqProducer> _logger;
        private readonly int _retryCount;

        public EventBusRabbitMqProducer(IRabbitMQPersistentConnection rabbitMQPersistentConnection, ILogger<EventBusRabbitMqProducer> logger, int retryCount=5)
        {
            _rabbitMQPersistentConnection = rabbitMQPersistentConnection;
            _logger = logger;
            _retryCount = retryCount;
        }


        public void Publish(string queueName,IEvent @event)
        {
            if (!_rabbitMQPersistentConnection.IsConnected)
            {
                _rabbitMQPersistentConnection.TryConnect();
            }

            var policy = RetryPolicy.Handle<SocketException>()
                             .Or<BrokerUnreachableException>()
                             .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                             {
                                 _logger.LogWarning("RabbiMq client could not connect after {timeout}s ({ExceptionMessage})", $"{time.TotalSeconds:n1}", ex.Message);

                             });

            using (var channel = _rabbitMQPersistentConnection.CreateModel())
            {
                channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                var message = JsonConvert.SerializeObject(@event);

                var body = Encoding.UTF8.GetBytes(message);

                policy.Execute(() => {
                    IBasicProperties properties = channel.CreateBasicProperties();
                    properties.Persistent = true;
                    properties.DeliveryMode = 2;

                    channel.ConfirmSelect();
                    channel.BasicPublish(
                       exchange:"",
                       routingKey: queueName,
                       mandatory:true,
                       basicProperties:properties,
                       body:body
                        );

                    channel.WaitForConfirmsOrDie();

                    channel.BasicAcks += (sender, eventArgs) =>
                    {
                        Console.WriteLine("Sent RabbitMQ");
                    };
                 
                });
            }
        }

    }
}
