using System;
using System.ComponentModel.DataAnnotations.Schema;
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

                entity.HasIndex(e => e.Name)
                    .HasName("UQ_GENRE_NAME")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME")
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<Singer>(entity =>
            {
                entity.ToTable("SINGER");

                entity.HasIndex(e => e.Name)
                    .HasName("UQ_SINGER_NAME")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME")
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<Soundtrack>(entity =>
            {
                entity.ToTable("SOUNDTRACK");

                entity.HasIndex(e => e.ServerFilename)
                    .HasName("UQ_SOUNDTRACK_SRVFILENAME")
                    .IsUnique();

                entity.HasIndex(e => new { e.SingerId, e.Title })
                    .HasName("UQ_SOUNDTRACK_SINGERTITLE")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClientFilename)
                    .IsRequired()
                    .HasColumnName("CLIENT_FILENAME");

                entity.Property(e => e.GenreId).HasColumnName("GENRE_ID");

                entity.Property(e => e.ServerFilename)
                    .IsRequired()
                    .HasColumnName("SERVER_FILENAME")
                    .HasMaxLength(150);

                entity.Property(e => e.SingerId).HasColumnName("SINGER_ID");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("TITLE")
                    .HasMaxLength(150);

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
