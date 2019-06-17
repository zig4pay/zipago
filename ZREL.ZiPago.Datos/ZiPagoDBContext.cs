using Microsoft.EntityFrameworkCore;
using ZREL.ZiPago.Entidad.Seguridad;
using ZREL.ZiPago.Datos.Configuraciones.Seguridad;
using System.Collections.Generic;
using System.Text;

namespace ZREL.ZiPago.Datos
{
    public class ZiPagoDBContext : DbContext
    {

        public ZiPagoDBContext(DbContextOptions<ZiPagoDBContext> options) : base(options)
        {
        }

        public DbSet<UsuarioZiPago> UsuariosZiPago { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder
                .ApplyConfiguration(new UsuarioZiPagoConfiguracion());

            base.OnModelCreating(modelBuilder);

        }

    }
}
