using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZREL.ZiPago.Entidad.Comun;

namespace ZREL.ZiPago.Datos.Configuraciones.Comun
{
    public class BancoZiPagoConfiguracion : IEntityTypeConfiguration<BancoZiPago>
    {
        public void Configure(EntityTypeBuilder<BancoZiPago> builder) {

            // Mapping for table
            builder.ToTable("BANCOZIPAGO", "dbo");

            // Set key for entity
            builder.HasKey(p => p.IdBancoZiPago);

            // Set mapping for columns
            builder.Property(p => p.IdBancoZiPago).HasColumnType("int").IsRequired();
            builder.Property(p => p.NombreLargo).HasColumnType("varchar(60)").IsRequired();
            builder.Property(p => p.NombreCorto).HasColumnType("varchar(20)");
            builder.Property(p => p.Activo).HasColumnType("char(1)").IsRequired();
            builder.Property(p => p.FechaCreacion).HasColumnType("datetime").IsRequired();
            builder.Property(p => p.FechaActualizacion).HasColumnType("datetime").IsRequired();            

        }
    }
}
