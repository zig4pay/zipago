using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZREL.ZiPago.Entidad.Afiliacion;

namespace ZREL.ZiPago.Datos.Configuraciones.Afiliacion
{
    public class ComercioZiPagoConfiguracion : IEntityTypeConfiguration<ComercioZiPago>
    {
        public void Configure(EntityTypeBuilder<ComercioZiPago> builder)
        {
            // Mapping for table 
            builder.ToTable("COMERCIOZIPAGO", "dbo");

            // Set key for entity
            builder.HasKey(p => p.IdComercioZiPago);

            // Set identity for entity (auto increment)
            builder.Property(p => p.IdComercioZiPago).UseSqlServerIdentityColumn();

            // Set mapping for columns
            builder.Property(p => p.IdComercioZiPago).HasColumnType("int").IsRequired();
            builder.Property(p => p.CodigoComercio).HasColumnType("varchar(14)").IsRequired();
            builder.Property(p => p.IdUsuarioZiPago).HasColumnType("int").IsRequired();
            builder.Property(p => p.Descripcion).HasColumnType("varchar(30)").IsRequired();
            builder.Property(p => p.CorreoNotificacion).HasColumnType("varchar(100)").IsRequired();
            builder.Property(p => p.Confirmado).HasColumnType("char(1)");
            builder.Property(p => p.Activo).HasColumnType("char(1)").IsRequired();
            builder.Property(p => p.FechaCreacion).HasColumnType("Datetime").IsRequired();
            builder.Property(p => p.FechaActualizacion).HasColumnType("Datetime");
            builder.Ignore(p => p.CodigoCuenta);

        }
    }
}
