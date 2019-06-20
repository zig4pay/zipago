using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZREL.ZiPago.Entidad.Comun;

namespace ZREL.ZiPago.Datos.Configuraciones.Comun
{
    public class TablaDetalleConfiguracion : IEntityTypeConfiguration<TablaDetalle>
    {
        public void Configure(EntityTypeBuilder<TablaDetalle> builder)
        {
            // Mapping for table
            builder.ToTable("TD_TABLA_TABLAS", "dbo");

            builder.HasKey(p => new { p.Cod_Tabla, p.Valor });

            builder.Property(p => p.Cod_Tabla).HasColumnType("varchar(15)").IsRequired();
            builder.Property(p => p.Valor).HasColumnType("varchar(20)").IsRequired();
            builder.Property(p => p.Descr_Valor).HasColumnType("varchar(120)");

        }
    }
}
