using System;
using System.Collections.Generic;
using System.Text;
using Avatar.App.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Avatar.App.Context
{
    public sealed class VideoContext : DbContext
    {
        public DbSet<Video> Videos { get; set; }
        public VideoContext(DbContextOptions<VideoContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
