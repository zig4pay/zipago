using Microsoft.EntityFrameworkCore;
using ZREL.ZiPago.Datos.Configuraciones.Afiliacion;
using ZREL.ZiPago.Datos.Configuraciones.Comun;
using ZREL.ZiPago.Datos.Configuraciones.Seguridad;
using ZREL.ZiPago.Entidad.Afiliacion;
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
        public DbSet<UbigeoZiPago> UbigeosZiPago { get; set; }

        public DbSet<ComercioCuentaZiPago> ComerciosCuentasZiPago { get; set; }
        public DbSet<ComercioZiPagoReg> ComerciosZiPagoReg { get; set; }
        public DbSet<CuentaBancariaZiPago> CuentasBancariasZiPago { get; set; }
        public DbSet<DomicilioZiPago> DomiciliosZiPago { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder
                .ApplyConfiguration(new UsuarioZiPagoConfiguracion())

                .ApplyConfiguration(new TablaDetalleConfiguracion())
                .ApplyConfiguration(new BancoZiPagoConfiguracion())
                .ApplyConfiguration(new UbigeoZiPagoConfiguracion())

                .ApplyConfiguration(new ComercioCuentaZiPagoConfiguracion())
                .ApplyConfiguration(new ComercioZiPagoConfiguracion())
                .ApplyConfiguration(new CuentaBancariaZiPagoConfiguracion())
                .ApplyConfiguration(new DomicilioZiPagoConfiguracion())
                ;

            base.OnModelCreating(modelBuilder);

        }

    }
}
