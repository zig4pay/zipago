using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZREL.ZiPago.Entidad.Seguridad;

namespace ZREL.ZiPago.Datos.Configuraciones.Seguridad
{
    public class UsuarioZiPagoConfiguracion : IEntityTypeConfiguration<UsuarioZiPago>
    {
        public void Configure(EntityTypeBuilder<UsuarioZiPago> builder)
        {
            // Mapping for table
            builder.ToTable("USUARIOZIPAGO", "dbo");

            // Set key for entity
            builder.HasKey(p => p.IdUsuarioZiPago);

            // Set identity for entity (auto increment)
            builder.Property(p => p.IdUsuarioZiPago).UseIdentityColumn();

            // Set mapping for columns
            builder.Property(p => p.IdUsuarioZiPago).HasColumnType("int").IsRequired();
            builder.Property(p => p.Clave1).HasColumnType("varchar(100)").IsRequired();
            builder.Property(p => p.Clave2).HasColumnType("varchar(500)").IsRequired();
            builder.Property(p => p.ApellidosUsuario).HasColumnType("varchar(200)").IsRequired();            
            builder.Property(p => p.NombresUsuario).HasColumnType("varchar(100)").IsRequired();
            builder.Property(p => p.CodigoRubroNegocio).HasColumnType("varchar(20)");
            builder.Property(p => p.OtroRubroNegocio).HasColumnType("varchar(100)");
            builder.Property(p => p.CodigoTipoPersona).HasColumnType("varchar(20)");
            builder.Property(p => p.CodigoTipoDocumento).HasColumnType("varchar(20)");
            builder.Property(p => p.NumeroDocumento).HasColumnType("varchar(15)");
            builder.Property(p => p.RazonSocial).HasColumnType("varchar(100)");            
            builder.Property(p => p.CodigoTipoDocumentoContacto).HasColumnType("varchar(20)");
            builder.Property(p => p.NumeroDocumentoContacto).HasColumnType("varchar(15)");
            builder.Property(p => p.Nombres).HasColumnType("varchar(100)");
            builder.Property(p => p.ApellidoPaterno).HasColumnType("varchar(100)");
            builder.Property(p => p.ApellidoMaterno).HasColumnType("varchar(100)");
            builder.Property(p => p.Sexo).HasColumnType("char(1)");
            builder.Property(p => p.FechaNacimiento).HasColumnType("smalldatetime");
            builder.Property(p => p.TelefonoMovil).HasColumnType("varchar(20)");
            builder.Property(p => p.TelefonoFijo).HasColumnType("varchar(15)");
            builder.Property(p => p.AceptoTerminos).HasColumnType("char(1)").IsRequired();
            builder.Property(p => p.EstadoRegistro).HasColumnType("char(1)").IsRequired();
            builder.Property(p => p.Activo).HasColumnType("char(1)").IsRequired();
            builder.Property(p => p.ClaveRecuperacion).HasColumnType("varchar(500)");
            builder.Property(p => p.FechaGeneracionClave).HasColumnType("datetime");
            builder.Property(p => p.FechaCreacion).HasColumnType("datetime").IsRequired();
            builder.Property(p => p.FechaActualizacion).HasColumnType("datetime");
        }
    }
}
