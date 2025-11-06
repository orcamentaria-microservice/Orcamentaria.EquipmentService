using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orcamentaria.EquipamentService.Domain.Models;

namespace Orcamentaria.EquipamentService.Infrastructure.Configurations
{
    public class EquipamentMaintenanceConfiguration : IEntityTypeConfiguration<EquipamentMaintenance>
    {
        public void Configure(EntityTypeBuilder<EquipamentMaintenance> builder)
        {
            builder.ToTable("T_EQUIPAMENT_MAINTENANCE");
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

            builder.Property(p => p.EquipamentId)
                .HasColumnName("EQUIPAMENT_ID")
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

            builder.Ignore(p => p.Equipament);

            builder
                .HasOne(e => e.Equipament)
                .WithMany()
                .HasForeignKey(e => e.EquipamentId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_T_MAINTENANCE_T_EQUIPAMENT");
        }
    }
}
