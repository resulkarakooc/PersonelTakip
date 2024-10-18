﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Karakoç.Models
{
    public partial class ResulContext : DbContext
    {
        public ResulContext(DbContextOptions<ResulContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Calisan> Calisans { get; set; } = null!;
        public virtual DbSet<Giderler> Giderlers { get; set; } = null!;
        public virtual DbSet<Mesai> Mesais { get; set; } = null!;
        public virtual DbSet<Odemeler> Odemelers { get; set; } = null!;
        public virtual DbSet<Yevmiyeler> Yevmiyelers { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Calisan>(entity =>
            {
                entity.HasKey(e => e.CalısanId);

                entity.ToTable("Calisan");

                entity.Property(e => e.CalısanId).HasColumnName("CalısanID");

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.Email).HasMaxLength(60);

                entity.Property(e => e.KayıtTarihi).HasColumnType("date");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(64);

                entity.Property(e => e.Surname).HasMaxLength(50);
            });

            modelBuilder.Entity<Giderler>(entity =>
            {
                entity.HasKey(e => e.GiderId);

                entity.ToTable("Giderler");

                entity.Property(e => e.GiderId).HasColumnName("GiderID");

                entity.Property(e => e.CalisanId).HasColumnName("CalisanID");

                entity.Property(e => e.Description).HasMaxLength(150);

                entity.Property(e => e.Tarih).HasColumnType("date");

                entity.HasOne(d => d.Calisan)
                    .WithMany(p => p.Giderlers)
                    .HasForeignKey(d => d.CalisanId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Giderler_Calisan");
            });

            modelBuilder.Entity<Mesai>(entity =>
            {
                entity.ToTable("Mesai");

                entity.Property(e => e.MesaiId).HasColumnName("MesaiID");

                entity.Property(e => e.IsWorked).HasColumnName("isWorked");

                entity.Property(e => e.Tarih).HasColumnType("date");

                entity.HasOne(d => d.Calisan)
                    .WithMany(p => p.Mesais)
                    .HasForeignKey(d => d.CalisanId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Mesai_Calisan");
            });

            modelBuilder.Entity<Odemeler>(entity =>
            {
                entity.HasKey(e => e.OdemeId);

                entity.ToTable("Odemeler");

                entity.Property(e => e.OdemeId).HasColumnName("OdemeID");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.CalisanId).HasColumnName("CalisanID");

                entity.Property(e => e.Description).HasMaxLength(120);

                entity.Property(e => e.Tarih).HasColumnType("date");

                entity.HasOne(d => d.Calisan)
                    .WithMany(p => p.Odemelers)
                    .HasForeignKey(d => d.CalisanId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Odemeler_Calisan");
            });

            modelBuilder.Entity<Yevmiyeler>(entity =>
            {
                entity.HasKey(e => e.YevmiyeId);

                entity.ToTable("Yevmiyeler");

                entity.Property(e => e.YevmiyeId).HasColumnName("YevmiyeID");

                entity.Property(e => e.IsWorked).HasColumnName("isWorked");

                entity.Property(e => e.Tarih).HasColumnType("date");

                entity.HasOne(d => d.Calisan)
                    .WithMany(p => p.Yevmiyelers)
                    .HasForeignKey(d => d.CalisanId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Yevmiyeler_Calisan");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
