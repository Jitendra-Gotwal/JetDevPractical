using JetDevsPrcatical.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace JetDevsPrcatical.Data
{
    public class JetDevsPrcaticalContext : DbContext
    {
        public JetDevsPrcaticalContext(DbContextOptions<JetDevsPrcaticalContext> options)
            : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
             .Entity<Users>(builder =>
              {
                  builder.HasKey(c => c.UserId);
              });

        }
    }
}