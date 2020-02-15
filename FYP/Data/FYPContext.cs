using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FYP.Models;

namespace FYP.Models
{
    public class FYPContext : DbContext
    {
        public FYPContext (DbContextOptions<FYPContext> options)
            : base(options)
        {
        }


        // Specify DbSet properties etc
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountModule>()
                .HasKey(c => new { c.ModuleId, c.AccountId });
        }

        public DbSet<FYP.Models.Account> Account { get; set; }
        public DbSet<FYP.Models.LogHistory> LogHistory { get; set; }
        public DbSet<FYP.Models.Course> Course { get; set; }
        public DbSet<FYP.Models.Document> Document { get; set; }
        public DbSet<FYP.Models.Module> Module { get; set; }
        public DbSet<FYP.Models.TracingHistory> TracingHistory { get; set; }
        public DbSet<FYP.Models.AccountModule> AccountModule { get; set; }
    }
}
