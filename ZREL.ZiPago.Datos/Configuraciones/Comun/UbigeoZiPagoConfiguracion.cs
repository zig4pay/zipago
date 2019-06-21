using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZREL.ZiPago.Entidad.Comun;

namespace ZREL.ZiPago.Datos.Configuraciones.Comun
{
    public class UbigeoZiPagoConfiguracion : IEntityTypeConfiguration<UbigeoZiPago>
    {

        public void Configure(EntityTypeBuilder<UbigeoZiPago> builder)
        {

            // Mapping for table
            builder.ToTable("UBIGEOZIPAGO", "dbo");

            // Set key for entity
            builder.HasKey(p => p.IdUbigeoZiPago);

            // Set mapping for columns
            builder.Property(p => p.IdUbigeoZiPago).HasColumnType("int").IsRequired();
            builder.Property(p => p.CodigoUbigeo).HasColumnType("varchar(6)").IsRequired();
            builder.Property(p => p.CodigoUbigeoPadre).HasColumnType("varchar(6)");
            builder.Property(p => p.Nombre).HasColumnType("varchar(20)").IsRequired();
            builder.Property(p => p.Activo).HasColumnType("char(1)").IsRequired();
            builder.Property(p => p.FechaCreacion).HasColumnType("datetime").IsRequired();
            builder.Property(p => p.FechaActualizacion).HasColumnType("datetime");

        }

    }
}
