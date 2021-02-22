﻿// <auto-generated />
using System;
using Avatar.App.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Avatar.App.Infrastructure.Migrations
{
    [DbContext(typeof(AvatarAppContext))]
    [Migration("20210222152949_AddFinalModelsMigration2")]
    partial class AddFinalModelsMigration2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Avatar.App.Infrastructure.Models.Casting.UserDb", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ConfirmationCode")
                        .HasColumnType("text");

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

            modelBuilder.Entity("Avatar.App.Infrastructure.Models.Casting.VideoDb", b =>
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

            modelBuilder.Entity("Avatar.App.Infrastructure.Models.Casting.WatchedVideoDb", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsLiked")
                        .HasColumnType("boolean");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<long>("VideoId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("VideoId");

                    b.ToTable("WatchedVideos");
                });

            modelBuilder.Entity("Avatar.App.Infrastructure.Models.Final.FinalDb", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsVotingStarted")
                        .HasColumnType("boolean");

                    b.Property<string>("VideoUrl")
                        .HasColumnType("text");

                    b.Property<int>("WinnersNumber")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Finals");
                });

            modelBuilder.Entity("Avatar.App.Infrastructure.Models.Final.FinalVoteDb", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<long>("FinalistId")
                        .HasColumnType("bigint");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("FinalistId");

                    b.HasIndex("UserId");

                    b.ToTable("FinalVotes");
                });

            modelBuilder.Entity("Avatar.App.Infrastructure.Models.Final.FinalistDb", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Finalists");
                });

            modelBuilder.Entity("Avatar.App.Infrastructure.Models.Semifinal.BattleDb", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("Closed")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("WinnersNumber")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Battles");
                });

            modelBuilder.Entity("Avatar.App.Infrastructure.Models.Semifinal.BattleSemifinalistDb", b =>
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

            modelBuilder.Entity("Avatar.App.Infrastructure.Models.Semifinal.BattleVoteDb", b =>
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

                    b.ToTable("BattleVotes");
                });

            modelBuilder.Entity("Avatar.App.Infrastructure.Models.Semifinal.SemifinalistDb", b =>
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

            modelBuilder.Entity("Avatar.App.Infrastructure.Models.Casting.VideoDb", b =>
                {
                    b.HasOne("Avatar.App.Infrastructure.Models.Casting.UserDb", "User")
                        .WithMany("LoadedVideos")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Avatar.App.Infrastructure.Models.Casting.WatchedVideoDb", b =>
                {
                    b.HasOne("Avatar.App.Infrastructure.Models.Casting.UserDb", "User")
                        .WithMany("WatchedVideos")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Avatar.App.Infrastructure.Models.Casting.VideoDb", "Video")
                        .WithMany("WatchedBy")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("Video");
                });

            modelBuilder.Entity("Avatar.App.Infrastructure.Models.Final.FinalVoteDb", b =>
                {
                    b.HasOne("Avatar.App.Infrastructure.Models.Final.FinalistDb", "Finalist")
                        .WithMany("Votes")
                        .HasForeignKey("FinalistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Avatar.App.Infrastructure.Models.Casting.UserDb", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Finalist");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Avatar.App.Infrastructure.Models.Final.FinalistDb", b =>
                {
                    b.HasOne("Avatar.App.Infrastructure.Models.Casting.UserDb", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Avatar.App.Infrastructure.Models.Semifinal.BattleSemifinalistDb", b =>
                {
                    b.HasOne("Avatar.App.Infrastructure.Models.Semifinal.BattleDb", "Battle")
                        .WithMany("BattleSemifinalists")
                        .HasForeignKey("BattleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Avatar.App.Infrastructure.Models.Semifinal.SemifinalistDb", "Semifinalist")
                        .WithMany("BattleSemifinalists")
                        .HasForeignKey("SemifinalistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Battle");

                    b.Navigation("Semifinalist");
                });

            modelBuilder.Entity("Avatar.App.Infrastructure.Models.Semifinal.BattleVoteDb", b =>
                {
                    b.HasOne("Avatar.App.Infrastructure.Models.Semifinal.BattleDb", "Battle")
                        .WithMany("Votes")
                        .HasForeignKey("BattleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Avatar.App.Infrastructure.Models.Semifinal.SemifinalistDb", "Semifinalist")
                        .WithMany("Votes")
                        .HasForeignKey("SemifinalistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Avatar.App.Infrastructure.Models.Casting.UserDb", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Battle");

                    b.Navigation("Semifinalist");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Avatar.App.Infrastructure.Models.Semifinal.SemifinalistDb", b =>
                {
                    b.HasOne("Avatar.App.Infrastructure.Models.Casting.UserDb", "User")
                        .WithOne("Semifinalist")
                        .HasForeignKey("Avatar.App.Infrastructure.Models.Semifinal.SemifinalistDb", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Avatar.App.Infrastructure.Models.Casting.UserDb", b =>
                {
                    b.Navigation("LoadedVideos");

                    b.Navigation("Semifinalist");

                    b.Navigation("WatchedVideos");
                });

            modelBuilder.Entity("Avatar.App.Infrastructure.Models.Casting.VideoDb", b =>
                {
                    b.Navigation("WatchedBy");
                });

            modelBuilder.Entity("Avatar.App.Infrastructure.Models.Final.FinalistDb", b =>
                {
                    b.Navigation("Votes");
                });

            modelBuilder.Entity("Avatar.App.Infrastructure.Models.Semifinal.BattleDb", b =>
                {
                    b.Navigation("BattleSemifinalists");

                    b.Navigation("Votes");
                });

            modelBuilder.Entity("Avatar.App.Infrastructure.Models.Semifinal.SemifinalistDb", b =>
                {
                    b.Navigation("BattleSemifinalists");

                    b.Navigation("Votes");
                });
#pragma warning restore 612, 618
        }
    }
}
