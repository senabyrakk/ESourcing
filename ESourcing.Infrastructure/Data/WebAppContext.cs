using ESourcing.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ESourcing.Infrastructure.Data
{
    public class WebAppContext : IdentityDbContext<User>
    {
        public WebAppContext(DbContextOptions options):base(options)
        {

        }

        public DbSet<User> Users{ get; set; }
    }
}
