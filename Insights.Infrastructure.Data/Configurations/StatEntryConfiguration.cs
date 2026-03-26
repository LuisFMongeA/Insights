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
            builder.OwnsOne(e => e.Coordinates, coordinates =>
            {
                coordinates.Property(c => c.Lat).HasColumnName("lat");
                coordinates.Property(c => c.Lon).HasColumnName("lon");
            });
            builder.Property(e => e.CityName).HasColumnName("city_name");
            builder.HasIndex(e => e.CityName);
            builder.OwnsOne(e => e.CountryCode, countryCode => 
            {
                countryCode.Property(c => c.Code).HasColumnName("country_code");
                countryCode.HasIndex(c => c.Code);
            });
            builder.Property(e => e.RequestedAt).HasColumnName("requested_at").IsRequired();
            builder.HasIndex(e => e.RequestedAt);

        }
    }
}
