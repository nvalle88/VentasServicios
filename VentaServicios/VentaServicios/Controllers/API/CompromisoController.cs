using System;
using System.Collections.Generic;
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
    [RoutePrefix("api/Compromiso")]
    public class CompromisoController : ApiController
    {
        private readonly ModelVentas db = new ModelVentas();
        [HttpPost]
        [Route("ListarCompromiso")]
        public async Task<SupervisorRequest> ListarCompromiso(SupervisorRequest supervisorRequest)
        {

            var listacompromiso = new List<CompromisoRequest>();

            try
            {

                listacompromiso = db.Compromiso.Select(x => new CompromisoRequest
                {
                    IdCompromiso = x.IdTipoCompromiso,
                    tipocompromiso = x.TipoCompromiso.Descripcion,
                    Descripcion = x.Descripcion,
                    Solucion = x.Solucion
                }
                ).Where(x => x.idVisita == supervisorRequest.Idvisita).ToList();


                supervisorRequest.ListaVendedores = db.Vendedor.Where(m => m.IdSupervisor == supervisorRequest.IdSupervisor && m.AspNetUsers.Estado == 1).Select(x => new VendedorRequest
                {
                    IdVendedor = x.IdVendedor,
                    Nombres = x.AspNetUsers.Nombres,
                    Apellidos = x.AspNetUsers.Apellidos,
                    Identificacion = x.AspNetUsers.Identificacion

                }).ToList();
                supervisorRequest.Listarcompromiso = listacompromiso;
                //supervisorRequest.ListaVendedores = listarvendedor;
                return supervisorRequest;
            }
            catch (Exception ex)
            {
                return supervisorRequest;
            }
        }
    }
}
