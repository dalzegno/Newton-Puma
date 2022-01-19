﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace PumaDbLibrary
{
    public partial class PumaDbContext : DbContext
    {
        public PumaDbContext() 
        {
        }

        public PumaDbContext(DbContextOptions<PumaDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Grading> Gradings { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<PoiGrading> PoiGradings { get; set; }
        public virtual DbSet<PoiTag> PoiTags { get; set; }
        public virtual DbSet<PointOfInterest> PointOfInterests { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasOne(d => d.PointOfInterest)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.PointOfInterestId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Grading>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.Gradings)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<PoiGrading>(entity =>
            {
                entity.HasOne(d => d.Grading)
                    .WithMany(p => p.PoiGradings)
                    .HasForeignKey(d => d.GradingId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.PointOfInterest)
                    .WithMany(p => p.PoiGradings)
                    .HasForeignKey(d => d.PointOfInterestId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<PoiTag>(entity =>
            {
                entity.HasOne(d => d.PointOfInterest)
                    .WithMany(p => p.PoiTags)
                    .HasForeignKey(d => d.PointOfInterestId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.PoiTags)
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<PointOfInterest>(entity =>
            {
                entity.HasOne(d => d.Location)
                    .WithMany(p => p.PointOfInterests)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}