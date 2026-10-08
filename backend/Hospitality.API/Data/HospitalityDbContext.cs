namespace Hospitality.API.Data;

using Hospitality.API.Models;
using Microsoft.EntityFrameworkCore;

public class HospitalityDbContext : DbContext
{
    public HospitalityDbContext(DbContextOptions<HospitalityDbContext> options)
        : base(options)
    {
    }

    public DbSet<BuffetWasteRecord> BuffetWasteRecords => Set<BuffetWasteRecord>();
    public DbSet<CanceledOrderRecord> CanceledOrderRecords => Set<CanceledOrderRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BuffetWasteRecord>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ItemName).HasMaxLength(200).IsRequired();
            entity.Property(e => e.TrayCapacityKg).HasPrecision(18, 2);
            entity.Property(e => e.RemainingPercentage).HasPrecision(18, 2);
            entity.Property(e => e.WastedWeightKg).HasPrecision(18, 2);
            entity.Property(e => e.EstimatedLossUsd).HasPrecision(18, 2);
        });

        modelBuilder.Entity<CanceledOrderRecord>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.BookingName).HasMaxLength(200).IsRequired();
            entity.Property(e => e.CancellationRate).HasPrecision(18, 2);
            entity.Property(e => e.EstimatedWasteCost).HasPrecision(18, 2);
        });
    }
}
