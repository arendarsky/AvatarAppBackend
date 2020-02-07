using System;
using System.Collections.Generic;
using System.Text;
using Avatar.App.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Avatar.App.Context
{
    public sealed class AvatarContext : DbContext
    {
        public DbSet<User> Users { get; set; } 
        public DbSet<Video> Videos { get; set; }
        public AvatarContext(DbContextOptions<AvatarContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
