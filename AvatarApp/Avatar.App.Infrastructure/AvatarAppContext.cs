using Avatar.App.Core.Entities;
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
        public DbSet<Message> Messages { get; set; }
        public DbSet<Semifinalist> Semifinalists { get; set; }
        public AvatarAppContext(DbContextOptions options) : base(options)
        {
           // Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasMany(u => u.SentMessages).WithOne(m => m.From);
            modelBuilder.Entity<User>().HasMany(u => u.ReceivedMessages).WithOne(m => m.To);
            modelBuilder.Entity<User>()
                .HasIndex(b => b.Guid)
                .IsUnique();
            modelBuilder.Entity<Video>()
                .HasIndex(b => b.Name)
                .IsUnique();

            modelBuilder.Entity<Battle>().HasMany(b => b.BattleVotes).WithOne(bv => bv.Battle);

            modelBuilder.Entity<BattleSemifinalist>()
                .HasOne(bs => bs.Battle)
                .WithMany(b => b.BattleSemifinalists)
                .HasForeignKey(bs => bs.BattleId);

            modelBuilder.Entity<BattleSemifinalist>()
                .HasOne(bs => bs.Semifinalist)
                .WithMany(s => s.BattleSemifinalists)
                .HasForeignKey(bs => bs.SemifinalistId);
        }
    }
}
