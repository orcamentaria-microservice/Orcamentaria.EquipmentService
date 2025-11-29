using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orcamentaria.EquipmentService.Domain.Models;

namespace Orcamentaria.EquipmentService.Infrastructure.Configurations
{
    public class EquipmentTypeConfiguration : IEntityTypeConfiguration<EquipmentType>
    {
        public void Configure(EntityTypeBuilder<EquipmentType> builder)
        {
            builder.ToTable("T_EQUIPMENT_TYPE");
            builder.HasKey(x => x.Id);
            builder.Property(p => p.Id)
                .HasColumnName("ID")
                .HasColumnType("BIGINT")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(p => p.Name)
                .HasColumnName("NAME")
                .HasColumnType("VARCHAR(40)")
                .IsRequired();

            builder.Property(p => p.Active)
                .HasColumnName("ACTIVE")
                .HasColumnType("BIT")
                .HasDefaultValue(true)
                .IsRequired();

            builder.Property(p => p.CompanyId)
                .HasColumnName("COMPANY_ID")
                .HasColumnType("BIGINT")
                .IsRequired();

            builder.Property(p => p.CreatedAt)
                .HasColumnName("CREATED_AT")
                .HasColumnType("DATETIME")
                .IsRequired();

            builder.Property(p => p.CreatedBy)
                .HasColumnName("CREATED_BY")
                .HasColumnType("BIGINT")
                .IsRequired();

            builder.Property(p => p.UpdatedAt)
                .HasColumnName("UPDATED_AT")
                .HasColumnType("DATETIME")
                .IsRequired();

            builder.Property(p => p.UpdatedBy)
                .HasColumnName("UPDATED_BY")
                .HasColumnType("BIGINT")
                .IsRequired();
        }
    }
}
