using DotNetCoreK6StressTest.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DotNetCoreK6StressTest.Infrastructure.Data
{
    public class ProductDBContext : DbContext
    {
        public ProductDBContext(DbContextOptions<ProductDBContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
    }
}
