using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using VentaServicios.ModeloDato;
using VentaServicios.ObjectRequest;
using VentaServicios.Utils;

namespace VentaServicios.Controllers.API
{
    [RoutePrefix("api/Vista")]
    public class VisitaController : ApiController
    {
        private readonly ModelVentas db = new ModelVentas();
        [HttpPost]
        [Route("ListarVisitas")]
        public async Task<List<VisitaRequest>> ListarVisitas(SupervisorRequest supervisorRequest)
        {

            //Solo necesita el IdEmpresa
            // solo muestra vendedores con estado 1("Activado")

            var listavisita = new List<VisitaRequest>();

            try
            {
                listavisita =  db.Visita.Select(x => new VisitaRequest
                {
                    IdVendedor = x.IdVendedor,
                    Calificacion =x.Calificacion,
                    Nombre = x.Cliente.Nombre,
                    Apellido = x.Cliente.Apellido,
                    identificacion = x.Cliente.Identificacion

                }

                ).Where(x => x.IdVendedor == supervisorRequest.IdVendedor && x.idCliente == Convert.ToInt32(supervisorRequest.IdCliente) &&  x.Fecha == Convert.ToDateTime(supervisorRequest.FechaInicio)).ToList();



                return listavisita;
            }
            catch (Exception ex)
            {
                return listavisita;
            }
        }
    }
}
