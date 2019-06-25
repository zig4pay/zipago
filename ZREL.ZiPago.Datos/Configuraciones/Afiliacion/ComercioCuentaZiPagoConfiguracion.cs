using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZREL.ZiPago.Entidad.Afiliacion;

namespace ZREL.ZiPago.Datos.Configuraciones.Afiliacion
{
    public class ComercioCuentaZiPagoConfiguracion : IEntityTypeConfiguration<ComercioCuentaZiPago>
    {
        public void Configure(EntityTypeBuilder<ComercioCuentaZiPago> builder)
        {
            // Mapping for table 
            builder.ToTable("COMERCIOCUENTAZIPAGO", "dbo");

            // Set key for entity
            builder.HasKey(p => new { p.IdComercioZiPago, p.IdCuentaBancaria });

            // Set mapping for columns
            builder.Property(p => p.IdComercioZiPago).HasColumnType("int").IsRequired();
            builder.Property(p => p.IdCuentaBancaria).HasColumnType("int").IsRequired();
            builder.Property(p => p.Activo).HasColumnType("char(1)").IsRequired();
            builder.Property(p => p.FechaCreacion).HasColumnType("Datetime").IsRequired();
            builder.Property(p => p.FechaActualizacion).HasColumnType("Datetime");

            builder.HasOne(x => x.ComercioZiPago)
                   .WithMany(x => x.ComercioCuentaZiPago)
                   .HasForeignKey(x => x.IdComercioZiPago);

            builder.HasOne(x => x.CuentaBancariaZiPago)
                   .WithMany(x => x.ComercioCuentaZiPago)
                   .HasForeignKey(x => x.IdCuentaBancaria);

        }
    }
}
