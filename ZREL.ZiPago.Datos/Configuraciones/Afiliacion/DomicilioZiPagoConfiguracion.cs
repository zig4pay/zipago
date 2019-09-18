using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZREL.ZiPago.Entidad.Afiliacion;

namespace ZREL.ZiPago.Datos.Configuraciones.Afiliacion
{
    public class DomicilioZiPagoConfiguracion : IEntityTypeConfiguration<DomicilioZiPago>
    {
        public void Configure(EntityTypeBuilder<DomicilioZiPago> builder)
        {
            // Mapping for table
            builder.ToTable("DOMICILIOZIPAGO", "dbo");

            // Set key for entity
            builder.HasKey(p => p.IdDomicilioZiPago );

            // Set identity for entity (auto increment)
            builder.Property(p => p.IdDomicilioZiPago).UseSqlServerIdentityColumn();

            // Set mapping for columns
            builder.Property(p => p.IdDomicilioZiPago).HasColumnType("int").IsRequired();
            builder.Property(p => p.IdUsuarioZiPago).HasColumnType("int").IsRequired();            
            builder.Property(p => p.CodigoDepartamento).HasColumnType("varchar(20)").IsRequired();
            builder.Property(p => p.CodigoProvincia).HasColumnType("varchar(20)").IsRequired();
            builder.Property(p => p.CodigoDistrito).HasColumnType("varchar(20)").IsRequired();
            builder.Property(p => p.Via).HasColumnType("varchar(80)").IsRequired();
            builder.Property(p => p.DireccionFacturacion).HasColumnType("varchar(150)");
            builder.Property(p => p.Referencia).HasColumnType("varchar(200)");
            builder.Property(p => p.Activo).HasColumnType("char(1)").IsRequired();
            builder.Property(p => p.FechaCreacion).HasColumnType("datetime").IsRequired();
            builder.Property(p => p.FechaActualizacion).HasColumnType("datetime");
            
        }
    }
}
