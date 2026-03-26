using Insights.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Insights.Infrastructure.Data.Configurations
{
    public class AuditEntryConfiguration: IEntityTypeConfiguration<AuditEntry>
    {
        public void Configure(EntityTypeBuilder<AuditEntry> builder)
        {
            builder.ToTable("audit_entries");
            builder.HasKey(e => e.Id);
            builder.Property(e =>e.Id).HasColumnName("id");
            builder.OwnsOne(e => e.Coordinates, coordinates => 
            {
                coordinates.Property(c=>c.Lat).HasColumnName("lat");
                coordinates.Property(c => c.Lon).HasColumnName("lon");
            });
            builder.Property(e => e.CityName).HasColumnName("city_name");
            builder.OwnsOne(e => e.CountryCode, countryCode => 
            {
                countryCode.Property(c => c.Code).HasColumnName("country_code");
            });
            builder.Property(e => e.ResolvedCityName).HasColumnName("resolved_city_name");
            builder.HasIndex(e => e.ResolvedCityName);
            builder.Property(e => e.ReceivedAt).HasColumnName("received_at").IsRequired();
            builder.HasIndex(e => e.ReceivedAt);
            builder.Property(e => e.RequestedAt).HasColumnName("requested_at").IsRequired();
            builder.HasIndex(e => e.RequestedAt);
            

        }

    }
}
