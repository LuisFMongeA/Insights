using Insights.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Insights.Infrastructure.Data.Configurations;

public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("event_messages");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Type).HasColumnName("type");
        builder.Property(e => e.Payload).HasColumnName("payload");
        builder.Property(e => e.CreatedAt).HasColumnName("created_at");
        builder.HasIndex(e => e.CreatedAt);
        builder.Property(e => e.ProcessedAt).HasColumnName("processed_at");
        builder.HasIndex(e => e.ProcessedAt);
        builder.Property(e => e.RetryCount).HasColumnName("retry_count");
        builder.Property(e => e.Error).HasColumnName("error");
        builder.HasIndex(e => new { e.ProcessedAt, e.CreatedAt });

    }
}

