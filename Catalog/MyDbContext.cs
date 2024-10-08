using Catalog;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog
{
    public class MyDbContext : DbContext
    {
        public DbSet<Product> Products_ { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            optionsBuilder.UseSqlite("Data Source=C:\\Users\\artem\\Desktop\\DataBase.bd");
        }
        private int GetTotalProductsFromDatabase()
        {
            using (var context = new MyDbContext())
            {
                return context.Products_.Count(); // Assuming Products_ is your DbSet
            }
        }

    }
}