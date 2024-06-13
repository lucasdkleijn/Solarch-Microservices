﻿using Microsoft.EntityFrameworkCore;

namespace Medical_Record_System;

public partial class MedicalRecordEventStoreContext : DbContext
{
    public MedicalRecordEventStoreContext(DbContextOptions<MedicalRecordEventStoreContext> options)
        : base(options)
    {
    }

    private const string ConnectionString =
        "Host=localhost;Database=medical-record-event-store;Username=postgres;Password=1234;Port=5420";

    public virtual DbSet<Event> Events { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseNpgsql(
        ConnectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("events_pkey");

            entity.ToTable("events");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Body)
                .HasColumnType("jsonb")
                .HasColumnName("body");
            entity.Property(e => e.InsertedAt)
                .HasDefaultValueSql("statement_timestamp()")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("inserted_at");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.Uuid).HasColumnName("uuid");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}