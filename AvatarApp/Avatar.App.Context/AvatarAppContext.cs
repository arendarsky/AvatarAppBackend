using System;
using System.Collections.Generic;
using System.Text;
using Avatar.App.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Avatar.App.Context
{
    public sealed class AvatarAppContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Video> Videos { get; set; }
        public AvatarAppContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(b => b.Guid)
                .IsUnique();
        }
    }
}
