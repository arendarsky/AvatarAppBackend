using Avatar.App.Core.Entities;
using Avatar.App.Infrastructure.Models;
using Avatar.App.Infrastructure.Models.Final;
using Avatar.App.Infrastructure.Models.Semifinal;
using Microsoft.EntityFrameworkCore;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace Avatar.App.Infrastructure
{
    internal sealed class AvatarAppContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<WatchedVideo> WatchedVideos { get; set; }
        public DbSet<LikedVideo> LikedVideos { get; set; }
        public DbSet<SemifinalistDb> Semifinalists { get; set; }
        public DbSet<BattleDb> Battles { get; set; }
        public DbSet<BattleSemifinalistDb> BattleSemifinalists { get; set; }
        public DbSet<BattleVoteDb> BattleVotes { get; set; }
        public DbSet<FinalistDb> Finalists { get; set; }

        public AvatarAppContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(b => b.Guid)
                .IsUnique();
            modelBuilder.Entity<Video>()
                .HasIndex(b => b.Name)
                .IsUnique();
        }
    }
}
