using Course.Services.Order.Domain.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Services.Order.Infrastucture
{
    public class OrderDbContext:DbContext
    {
        public const string DEFAULT_SCHEMA = "ordering";
        public OrderDbContext(DbContextOptions<OrderDbContext> options):base(options)
        {
                
        }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Order.Domain.OrderAggregate.Order> Orders { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.OrderAggregate.Order>().ToTable("Orders", DEFAULT_SCHEMA);
            modelBuilder.Entity<OrderItem>().ToTable("OrdersItem", DEFAULT_SCHEMA);
            modelBuilder.Entity<Domain.OrderAggregate.OrderItem>().Property(x => x.Price).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Domain.OrderAggregate.Order>().OwnsOne(x => x.Address).WithOwner();


            base.OnModelCreating(modelBuilder);

        }
    }
}
