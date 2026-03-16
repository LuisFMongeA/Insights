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
            builder.Property(e => e.Lat).HasColumnName("lat");
            builder.Property(e => e.Lon).HasColumnName("lon");
            builder.Property(e => e.CityName).HasColumnName("city_name");
            builder.Property(e => e.CountryCode).HasColumnName("country_code");
            builder.Property(e => e.ResolvedCityName).HasColumnName("resolved_city_name");
            builder.HasIndex(e => e.ResolvedCityName);
            builder.Property(e => e.ReceivedAt).HasColumnName("received_at").IsRequired();
            builder.HasIndex(e => e.ReceivedAt);
            builder.Property(e => e.RequestedAt).HasColumnName("requested_at").IsRequired();
            builder.HasIndex(e => e.RequestedAt);
            

        }

    }
}
