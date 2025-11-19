using System;
using Microsoft.EntityFrameworkCore;
using RoshedTehran.Models;

namespace RoshedTehran.Data
{
	public class ApplicationDbContext :DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)
		{}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Entity> Entities { get; set; }
        public DbSet<QueryEntity> Queries { get; set; }
    }
}

