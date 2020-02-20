using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZREL.ZiPago.Entidad.Afiliacion;

namespace ZREL.ZiPago.Datos.Configuraciones.Afiliacion
{
    public class CuentaBancariaZiPagoConfiguracion : IEntityTypeConfiguration<CuentaBancariaZiPago>
    {
        public void Configure(EntityTypeBuilder<CuentaBancariaZiPago> builder)
        {
            // Mapping for table 
            builder.ToTable("CUENTABANCARIAZIPAGO", "dbo");

            // Set key for entity
            builder.HasKey(p => p.IdCuentaBancaria);

            // Set identity for entity (auto increment)
            builder.Property(p => p.IdCuentaBancaria).UseIdentityColumn();

            // Set mapping for columns
            builder.Property(p => p.IdCuentaBancaria).HasColumnType("int").IsRequired();
            builder.Property(p => p.IdUsuarioZiPago).HasColumnType("int").IsRequired();
            builder.Property(p => p.IdBancoZiPago).HasColumnType("int").IsRequired();
            builder.Property(p => p.NumeroCuenta).HasColumnType("varchar(20)").IsRequired();
            builder.Property(p => p.CodigoTipoCuenta).HasColumnType("varchar(20)").IsRequired();
            builder.Property(p => p.CodigoTipoMoneda).HasColumnType("varchar(20)").IsRequired();
            builder.Property(p => p.CCI).HasColumnType("varchar(20)");
            builder.Property(p => p.Activo).HasColumnType("char(1)").IsRequired();
            builder.Property(p => p.FechaCreacion).HasColumnType("datetime").IsRequired();
            builder.Property(p => p.FechaActualizacion).HasColumnType("datetime");
            builder.Ignore(p => p.CodigoCuenta);
        }
    }
}
