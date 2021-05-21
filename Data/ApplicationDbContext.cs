using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Asset_Web.Models;

namespace Asset_Web.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Asset_Web.Models.Category> Category { get; set; }
        public DbSet<Asset_Web.Models.Currency> Currency { get; set; }
        public DbSet<Asset_Web.Models.Office> Office { get; set; }
        public DbSet<Asset_Web.Models.Brand> Brand { get; set; }
        public DbSet<Asset_Web.Models.BrandModel> BrandModel { get; set; }
        public DbSet<Asset_Web.Models.Product> Product { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Currency>().Property(x => x.ExRateUSD).HasPrecision(18, 6);
        }
    }
}
