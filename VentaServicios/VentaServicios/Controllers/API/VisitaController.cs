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
        public async Task<SupervisorRequest> ListarVisitas(SupervisorRequest supervisorRequest)
        {

            var listavisita = new List<VisitaRequest>();
             
            try
            {
                db.Configuration.ProxyCreationEnabled = false;

                var listacompleta =  db.Visita.Where(x=> x.idCliente == supervisorRequest.IdCliente && 
                    x.IdVendedor == supervisorRequest.IdVendedor
                 && x.Fecha >= supervisorRequest.FechaInicio || x.Fecha <= supervisorRequest.FechaFin).ToList();
                foreach (var item in listacompleta)
                {
                    var a = new VisitaRequest
                    {
                        idVisita = item.idVisita
                    };
                    listavisita.Add(a);
                }

                supervisorRequest.Listarvisita = listavisita;
                return supervisorRequest;
            }
            catch (Exception ex)
            {
                return supervisorRequest;
            }
        }
    }
}
