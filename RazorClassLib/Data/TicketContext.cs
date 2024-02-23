using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RazorClassLib.Data;

namespace RazorClassLib.Data;

public partial class TicketContext : DbContext
{
    public TicketContext()
    {
    }

    public TicketContext(DbContextOptions<TicketContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Occasion> Occasions { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseNpgsql("Name=pec_tickets");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresExtension("pg_catalog", "azure")
            .HasPostgresExtension("pg_catalog", "pgaadauth");

        modelBuilder.Entity<Occasion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("occasion_pkey");

            entity.ToTable("occasion", "ticketsareus");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.OccasionName)
                .HasMaxLength(300)
                .HasColumnName("occasion_name");
            entity.Property(e => e.TimeOfDay).HasColumnName("time_of_day");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ticket_pkey");

            entity.ToTable("ticket", "ticketsareus");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Guid).HasColumnName("guid");
            entity.Property(e => e.IsUsed).HasColumnName("is_used");
            entity.Property(e => e.OccasionId).HasColumnName("occasion_id");

            entity.HasOne(d => d.Occasion).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.OccasionId)
                .HasConstraintName("ticket_occasion_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
