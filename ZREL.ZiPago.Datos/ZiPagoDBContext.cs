using Microsoft.EntityFrameworkCore;
using ZREL.ZiPago.Datos.Configuraciones.Comun;
using ZREL.ZiPago.Datos.Configuraciones.Seguridad;
using ZREL.ZiPago.Entidad.Comun;
using ZREL.ZiPago.Entidad.Seguridad;

namespace ZREL.ZiPago.Datos
{
    public class ZiPagoDBContext : DbContext
    {

        public ZiPagoDBContext(DbContextOptions<ZiPagoDBContext> options) : base(options)
        {
        }

        public DbSet<UsuarioZiPago> UsuariosZiPago { get; set; }
        public DbSet<TablaDetalle> TablasDetalle { get; set; }
        public DbSet<BancoZiPago> BancosZiPago { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder
                .ApplyConfiguration(new UsuarioZiPagoConfiguracion())
                .ApplyConfiguration(new TablaDetalleConfiguracion())
                .ApplyConfiguration(new BancoZiPagoConfiguracion())
                ;

            base.OnModelCreating(modelBuilder);

        }

    }
}
