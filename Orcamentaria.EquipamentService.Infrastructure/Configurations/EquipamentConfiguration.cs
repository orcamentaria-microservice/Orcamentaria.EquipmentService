using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orcamentaria.EquipamentService.Domain.Models;

namespace Orcamentaria.EquipamentService.Infrastructure.Configurations
{
    public class EquipamentConfiguration : IEntityTypeConfiguration<Equipament>
    {
        public void Configure(EntityTypeBuilder<Equipament> builder)
        {
            builder.ToTable("T_EQUIPAMENT");
            builder.HasKey(x => x.Id);
            builder.Property(p => p.Id)
                .HasColumnName("ID")
                .HasColumnType("BIGINT")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(p => p.Name)
                .HasColumnName("NAME")
                .HasColumnType("VARCHAR(60)")
                .IsRequired();

            builder.Property(p => p.Description)
                .HasColumnName("DESCRIPTION")
                .HasColumnType("VARCHAR(256)");

            builder.Property(p => p.Manufacturer)
                .HasColumnName("MANUFACTURER")
                .HasColumnType("VARCHAR(150)");

            builder.Property(p => p.TypeId)
                .HasColumnName("EQUIPAMENT_TYPE_ID")
                .HasColumnType("BIGINT")
                .IsRequired();

            builder.Property(p => p.MaintenancePeriod)
                .HasColumnName("MAINTENANCE_PERIOD")
                .HasColumnType("INT")
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

            builder.Ignore(p => p.Type);

            builder
                .HasOne(e => e.Type)
                .WithMany()
                .HasForeignKey(e => e.TypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_T_EQUIPAMENT_T_EQUIPAMENT_TYPE");
        }
    }
}
