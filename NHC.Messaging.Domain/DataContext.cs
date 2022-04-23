using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHC.Messaging.Domain
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<CustomerMessage> CustomerMessages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Customer>().ToTable("Customer").HasMany(x => x.CustomerMessages).WithOne().HasForeignKey(x => x.CustomerId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Message>().ToTable("Message").HasMany(x => x.CustomerMessages).WithOne().HasForeignKey(x => x.MessageId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<CustomerMessage>().ToTable("CustomerMessage").HasKey(x => new { x.MessageId, x.CustomerId });

        }
    }
}
