using System.Collections.Immutable;
using DispatchRecordSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DispatchRecordSystem;


public class CompanyConfig : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.HasMany(c => c.Drivers)
            .WithOne(d => d.Company)
            .HasForeignKey(d => d.CompanyId);
        builder.HasMany(c => c.Trucks)
            .WithOne(t => t.Company)
            .HasForeignKey(t => t.CompanyId);
    }
}

public class StationConfig : IEntityTypeConfiguration<Station>
{
    public void Configure(EntityTypeBuilder<Station> builder)
    {
        builder.HasMany(s => s.Drivers)
            .WithOne(d => d.Location)
            .HasForeignKey(d => d.StationId);
        builder.HasMany(s => s.Trucks)
            .WithOne(t => t.Location)
            .HasForeignKey(t => t.StationId);
        builder.HasMany(s => s.Doors)
            .WithOne(d => d.Station)
            .HasForeignKey(d => d.StationId);
        builder.HasMany(s => s.Trailers)
            .WithOne(t => t.Location)
            .HasForeignKey(t => t.StationId);
        builder.HasMany(s => s.Inbound)
            .WithOne(d => d.Destination)
            .HasForeignKey(d => d.DestinationId);
        builder.HasMany(s => s.Outbound)
            .WithOne(d => d.Origin)
            .HasForeignKey(d => d.OriginId);
        builder.HasMany(s => s.Users)
            .WithOne(u => u.Domistile)
            .HasForeignKey(u => u.StationId);

    }
}

public class CustomerConfig : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasMany(c => c.Inbound)
            .WithOne(c => c.Recipient)
            .HasForeignKey(c => c.RecipientId);
        builder.HasMany(c => c.Outbound)
            .WithOne(c => c.Sender)
            .HasForeignKey(c => c.SenderId);
    }
}

public class DispatchConfig : IEntityTypeConfiguration<DispatchRecord>
{
    public void Configure(EntityTypeBuilder<DispatchRecord> builder)
    {
        builder.HasOne(dr => dr.AssignedTruck);
        builder.HasMany(dr => dr.Trailers)
            .WithOne(t => t.Dispatch)
            .HasForeignKey(t => t.DispatchRecordId);
        builder.Property(e => e.Status)
            .HasConversion(
                v => v.ToString(),
                v => (DispatchRecord.DispatchStatus)Enum.Parse(typeof(DispatchRecord.DispatchStatus), v))
            .HasMaxLength(10);
    }
}

public class DriverConfig : IEntityTypeConfiguration<Driver>
{
    public void Configure(EntityTypeBuilder<Driver> builder)
    {
        builder.Property(e => e.Status)
            .HasConversion(
                v => v.ToString(),
                v => (Driver.DriverStatus)Enum.Parse(typeof(Driver.DriverStatus), v))
            .HasMaxLength(10);
    }
}

public class TrailerConfig : IEntityTypeConfiguration<Trailer>
{
    public void Configure(EntityTypeBuilder<Trailer> builder)
    {
        builder.Property(e => e.Status)
            .HasConversion(
                v => v.ToString(),
                v => (Trailer.TrailerStatus)Enum.Parse(typeof(Trailer.TrailerStatus), v))
            .HasMaxLength(10);
    }
}
 
public class ParcelConfig : IEntityTypeConfiguration<Parcel>
{
    public void Configure(EntityTypeBuilder<Parcel> builder)
    {
        builder.Property(e => e.Status)
            .HasConversion(
                v => v.ToString(),
                v => (Parcel.ParcelStatus)Enum.Parse(typeof(Parcel.ParcelStatus), v))
            .HasMaxLength(20);
    }
}

public class TruckConfig : IEntityTypeConfiguration<Truck>
{
    public void Configure(EntityTypeBuilder<Truck> builder)
    {
        builder.Property(e => e.Status)
            .HasConversion(
                v => v.ToString(),
                v => (Truck.TruckStatus)Enum.Parse(typeof(Truck.TruckStatus), v))
            .HasMaxLength(10);
        
    }
}

public class DoorConfig : IEntityTypeConfiguration<Door>
{
    public void Configure(EntityTypeBuilder<Door> builder)
    {
        builder.Property(e => e.Type)
            .HasConversion(
                v => v.ToString(),
                v => (Door.DoorType)Enum.Parse(typeof(Door.DoorType), v))
            .HasMaxLength(10);
    }
}