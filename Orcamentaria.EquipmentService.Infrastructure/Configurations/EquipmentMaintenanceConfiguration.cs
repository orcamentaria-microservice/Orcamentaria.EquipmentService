using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orcamentaria.EquipmentService.Domain.Models;

namespace Orcamentaria.EquipmentService.Infrastructure.Configurations
{
    public class EquipmentMaintenanceConfiguration : IEntityTypeConfiguration<EquipmentMaintenance>
    {
        public void Configure(EntityTypeBuilder<EquipmentMaintenance> builder)
        {
            builder.ToTable("T_EQUIPMENT_MAINTENANCE");
            builder.HasKey(x => x.Id);
            builder.Property(p => p.Id)
                .HasColumnName("ID")
                .HasColumnType("BIGINT")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(p => p.Description)
                .HasColumnName("DESCRIPTION")
                .HasColumnType("VARCHAR(256)")
                .IsRequired();

            builder.Property(p => p.EquipmentId)
                .HasColumnName("EQUIPMENT_ID")
                .HasColumnType("BIGINT")
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

            builder.Ignore(p => p.Equipment);

            builder
                .HasOne(e => e.Equipment)
                .WithMany()
                .HasForeignKey(e => e.EquipmentId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_T_MAINTENANCE_T_EQUIPMENT");
        }
    }
}
