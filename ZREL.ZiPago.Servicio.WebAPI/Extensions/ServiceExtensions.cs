using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ZREL.ZiPago.Datos;
using ZREL.ZiPago.Negocio;
using ZREL.ZiPago.Negocio.Contracts;
using ZREL.ZiPago.Negocio.Seguridad;
using Microsoft.Extensions.Configuration;
using ZREL.ZiPago.Negocio.Comun;

namespace ZREL.ZiPago.Servicio.WebAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                var multiVersionReader = new HeaderApiVersionReader("x-version");
                options.ApiVersionReader = multiVersionReader;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });
        }

        public static void ConfigureRepository(this IServiceCollection services)
        {
            services.Add(new ServiceDescriptor(typeof(IUsuarioZiPagoService), typeof(UsuarioZiPagoService), ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(typeof(IBancoZiPagoService), typeof(BancoZiPagoService), ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(typeof(ITablaDetalleService), typeof(TablaDetalleService), ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(typeof(IUbigeoZiPagoService), typeof(UbigeoZiPagoService), ServiceLifetime.Transient));
        }

        public static void ConfigureEF(this IServiceCollection services, IConfiguration configuration) {
            services.AddDbContext<ZiPagoDBContext>(builder =>
            {
                builder.UseSqlServer(Libreria.Seguridad.Criptografia.Desencriptar(configuration["ZRELZiPago:ZZiPagoBD"]));
            });
        }

    }
}
