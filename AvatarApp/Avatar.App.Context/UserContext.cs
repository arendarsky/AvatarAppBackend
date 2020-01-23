using System;
using System.Collections.Generic;
using System.Text;
using Avatar.App.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Avatar.App.Context
{
    public sealed class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; } 
        public UserContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
