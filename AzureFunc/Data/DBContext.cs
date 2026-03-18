using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AzureFunc.Models;

namespace AzureFunc.Data
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) 
        {
        }

        public DbSet<Salesrequest> salesrequest { get; set; } = null;
        public DbSet<GroceryItem> GroceryItems { get; set; } = null;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Salesrequest>(entity => 
            {
                entity.HasKey(e => e.Id);
            });
        }
    }
}
