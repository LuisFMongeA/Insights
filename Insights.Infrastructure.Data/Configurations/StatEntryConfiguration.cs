using Insights.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Insights.Infrastructure.Data.Configurations
{
    public class StatEntryConfiguration : IEntityTypeConfiguration<StatEntry>
    {
        public void Configure(EntityTypeBuilder<StatEntry> builder)
        {
            builder.ToTable("stat_entries");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("id").IsRequired();
            builder.Property(e => e.Lat).HasColumnName("lat");
            builder.Property(e => e.Lon).HasColumnName("lon");
            builder.Property(e => e.CityName).HasColumnName("city_name");
            builder.HasIndex(e => e.CityName);
            builder.Property(e => e.CountryCode).HasColumnName("country_code");
            builder.HasIndex(e => e.CountryCode);
            builder.Property(e => e.RequestedAt).HasColumnName("requested_at").IsRequired();
            builder.HasIndex(e => e.RequestedAt);

        }
    }
}
