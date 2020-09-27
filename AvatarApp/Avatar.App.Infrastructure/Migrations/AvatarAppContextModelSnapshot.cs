﻿// <auto-generated />
using System;
using Avatar.App.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Avatar.App.Infrastructure.Migrations
{
    [DbContext(typeof(AvatarAppContext))]
    partial class AvatarAppContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Avatar.App.Core.Entities.Battle", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("WinnersNumber")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Battles");
                });

            modelBuilder.Entity("Avatar.App.Core.Entities.BattleSemifinalist", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<long>("BattleId")
                        .HasColumnType("bigint");

                    b.Property<long>("SemifinalistId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("BattleId");

                    b.HasIndex("SemifinalistId");

                    b.ToTable("BattleSemifinalists");
                });

            modelBuilder.Entity("Avatar.App.Core.Entities.BattleVote", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<long>("BattleId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long>("SemifinalistId")
                        .HasColumnType("bigint");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("BattleId");

                    b.HasIndex("SemifinalistId");

                    b.HasIndex("UserId");

                    b.ToTable("BattleVote");
                });

            modelBuilder.Entity("Avatar.App.Core.Entities.LikedVideo", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<long>("VideoId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("VideoId");

                    b.ToTable("LikedVideos");
                });

            modelBuilder.Entity("Avatar.App.Core.Entities.Semifinalist", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<string>("VideoName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Semifinalists");
                });

            modelBuilder.Entity("Avatar.App.Core.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool?>("ConsentToGeneralEmail")
                        .HasColumnType("boolean");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FireBaseId")
                        .HasColumnType("text");

                    b.Property<Guid>("Guid")
                        .HasColumnType("uuid");

                    b.Property<string>("InstagramLogin")
                        .HasColumnType("text");

                    b.Property<bool>("IsEmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<string>("ProfilePhoto")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Guid")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Avatar.App.Core.Entities.Video", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<double>("EndTime")
                        .HasColumnType("double precision");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<bool?>("IsApproved")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<double>("StartTime")
                        .HasColumnType("double precision");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Videos");
                });

            modelBuilder.Entity("Avatar.App.Core.Entities.WatchedVideo", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<long>("VideoId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("VideoId");

                    b.ToTable("WatchedVideos");
                });

            modelBuilder.Entity("Avatar.App.Core.Entities.BattleSemifinalist", b =>
                {
                    b.HasOne("Avatar.App.Core.Entities.Battle", "Battle")
                        .WithMany("BattleSemifinalists")
                        .HasForeignKey("BattleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Avatar.App.Core.Entities.Semifinalist", "Semifinalist")
                        .WithMany("BattleSemifinalists")
                        .HasForeignKey("SemifinalistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Avatar.App.Core.Entities.BattleVote", b =>
                {
                    b.HasOne("Avatar.App.Core.Entities.Battle", "Battle")
                        .WithMany("Votes")
                        .HasForeignKey("BattleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Avatar.App.Core.Entities.Semifinalist", "Semifinalist")
                        .WithMany("Votes")
                        .HasForeignKey("SemifinalistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Avatar.App.Core.Entities.User", "User")
                        .WithMany("BattleVotes")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Avatar.App.Core.Entities.LikedVideo", b =>
                {
                    b.HasOne("Avatar.App.Core.Entities.User", "User")
                        .WithMany("LikedVideos")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Avatar.App.Core.Entities.Video", "Video")
                        .WithMany("LikedBy")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Avatar.App.Core.Entities.Semifinalist", b =>
                {
                    b.HasOne("Avatar.App.Core.Entities.User", "User")
                        .WithOne("Semifinalist")
                        .HasForeignKey("Avatar.App.Core.Entities.Semifinalist", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Avatar.App.Core.Entities.Video", b =>
                {
                    b.HasOne("Avatar.App.Core.Entities.User", "User")
                        .WithMany("LoadedVideos")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Avatar.App.Core.Entities.WatchedVideo", b =>
                {
                    b.HasOne("Avatar.App.Core.Entities.User", "User")
                        .WithMany("WatchedVideos")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Avatar.App.Core.Entities.Video", "Video")
                        .WithMany("WatchedBy")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
