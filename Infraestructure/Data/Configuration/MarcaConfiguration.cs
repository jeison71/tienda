using Core.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;

namespace Infraestructure.Data.Configuration
{
    public class MarcaConfiguration : IEntityTypeConfiguration<Marca>
    {
        public void Configure(EntityTypeBuilder<Marca> builder)
        {
            builder.ToTable("Marca");
            builder.Property(p => p.Id)
            .IsRequired();
            builder.Property(p => p.Nombre)
               .IsRequired()
               .HasMaxLength(100);
        }

    }
}