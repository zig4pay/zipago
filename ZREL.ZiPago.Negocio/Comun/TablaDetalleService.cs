using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NLog;
using ZREL.ZiPago.Datos;
using ZREL.ZiPago.Datos.Comun;
using ZREL.ZiPago.Entidad.Comun;
using ZREL.ZiPago.Negocio.Contracts;
using ZREL.ZiPago.Negocio.Responses;

namespace ZREL.ZiPago.Negocio.Comun
{
    public class TablaDetalleService : Service, ITablaDetalleService
    {
        public TablaDetalleService(ZiPagoDBContext dbContext) : base(dbContext)
        {

        }

        public async Task<ListResponse<TablaDetalle>> ListarTablaDetalleAsync(Logger logger, TablaDetalle entidad)
        {
            var response = new ListResponse<TablaDetalle>();
            logger.Info("[{0}] | TablaDetalle: [{1}] | Inicio.", nameof(ListarTablaDetalleAsync), entidad.Cod_Tabla);
            try
            {
                response.Model = DbContext.ObtenerTablaDetalle(entidad.Cod_Tabla);
                //if (response.Model != null && (Criptografia.Desencriptar(response.Model.Clave2.Trim()) == Criptografia.Desencriptar(entidad.Clave2)))
                //{
                //    response.Model.Clave2 = "";
                //    response.Mensaje = "1";
                //}
                //else
                //{
                //    response.Model = null;
                //    response.Mensaje = Constantes.strMensajeUsuarioIncorrecto;
                //}
                //logger.Info("[{0}] | UsuarioZiPago: [{1}] | Mensaje: [{2}].", nameof(AutenticarUsuarioZiPagoAsync), entidad.Clave1, response.Mensaje);
            }
            catch (Exception ex)
            {
                response.Model = null;
                response.SetError(logger, nameof(ListarTablaDetalleAsync), nameof(TablaDetalle), ex);
            }
            return response;
        }
    }
}
