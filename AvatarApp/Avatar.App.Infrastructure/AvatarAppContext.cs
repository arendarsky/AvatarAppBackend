using Avatar.App.Core.Entities;
using Avatar.App.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace Avatar.App.Infrastructure
{
    public sealed class AvatarAppContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<WatchedVideo> WatchedVideos { get; set; }
        public DbSet<LikedVideo> LikedVideos { get; set; }
        public DbSet<Semifinalist> Semifinalists { get; set; }
        public DbSet<Battle> Battles { get; set; }
        public DbSet<BattleSemifinalist> BattleSemifinalists { get; set; }
        public DbSet<BattleVote> BattleVotes { get; set; }
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
