using Avatar.App.Infrastructure.Models.Casting;
using Avatar.App.Infrastructure.Models.Final;
using Avatar.App.Infrastructure.Models.Semifinal;
using Microsoft.EntityFrameworkCore;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace Avatar.App.Infrastructure
{
    internal sealed class AvatarAppContext : DbContext
    {
        public DbSet<UserDb> Users { get; set; }
        public DbSet<VideoDb> Videos { get; set; }
        public DbSet<WatchedVideoDb> WatchedVideos { get; set; }
        public DbSet<SemifinalistDb> Semifinalists { get; set; }
        public DbSet<BattleDb> Battles { get; set; }
        public DbSet<BattleSemifinalistDb> BattleSemifinalists { get; set; }
        public DbSet<BattleVoteDb> BattleVotes { get; set; }
        public DbSet<FinalistDb> Finalists { get; set; }
        public DbSet<FinalDb> Finals { get; set; }
        public DbSet<FinalVoteDb> FinalVotes { get; set; }

        public AvatarAppContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDb>()
                .HasIndex(b => b.Guid)
                .IsUnique();
            modelBuilder.Entity<VideoDb>()
                .HasIndex(b => b.Name)
                .IsUnique();
        }
    }
}
