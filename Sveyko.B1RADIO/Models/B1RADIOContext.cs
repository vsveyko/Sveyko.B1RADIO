using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Sveyko.B1RADIO.Models
{
    public partial class B1RADIOContext : DbContext
    {
        public B1RADIOContext()
        {
        }

        public B1RADIOContext(DbContextOptions<B1RADIOContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Genre> Genre { get; set; }
        public virtual DbSet<Singer> Singer { get; set; }
        public virtual DbSet<Soundtrack> Soundtrack { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=B1RADIO;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("GENRE");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME")
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<Singer>(entity =>
            {
                entity.ToTable("SINGER");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME")
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<Soundtrack>(entity =>
            {
                entity.ToTable("SOUNDTRACK");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Filepath)
                    .IsRequired()
                    .HasColumnName("FILEPATH");

                entity.Property(e => e.GenreId).HasColumnName("GENRE_ID");

                entity.Property(e => e.SingerId).HasColumnName("SINGER_ID");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("TITLE");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.Soundtrack)
                    .HasForeignKey(d => d.GenreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SOUNDTRACK_GENRE_ID");

                entity.HasOne(d => d.Singer)
                    .WithMany(p => p.Soundtrack)
                    .HasForeignKey(d => d.SingerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SOUNDTRACK_SINGER_ID");
            });
        }
    }
}
