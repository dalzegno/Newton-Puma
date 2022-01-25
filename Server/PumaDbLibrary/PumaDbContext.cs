using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PumaDbLibrary.Entities;

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
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<PoiTag> PoiTags { get; set; }
        public virtual DbSet<PointOfInterest> PointOfInterests { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Api> Apis { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasOne(d => d.PointOfInterest)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.PointOfInterestId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Grading>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.Gradings)
                    .HasForeignKey(d => d.UserId);

                entity.HasOne(d => d.PointOfInterest)
                      .WithMany(p => p.Gradings)
                      .HasForeignKey(d => d.PointOfInterestId);
            });

            modelBuilder.Entity<PoiTag>(entity =>
            {
                entity.HasOne(d => d.PointOfInterest)
                    .WithMany(p => p.PoiTags)
                    .HasForeignKey(d => d.PointOfInterestId);

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.PoiTags)
                    .HasForeignKey(d => d.TagId);
            });

            modelBuilder.Entity<PointOfInterest>(entity =>
            {
                entity.HasOne(d => d.Position)
                    .WithMany(p => p.PointOfInterests)
                    .HasForeignKey(d => d.PositionId);

                entity.HasOne(d => d.Address)
                      .WithMany(p => p.PointOfInterests)
                      .HasForeignKey(d => d.AddressId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
