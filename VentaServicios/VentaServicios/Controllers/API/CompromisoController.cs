using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Data.Entity;
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

        // POST: api/Compromiso
        [HttpPost]
        [Route("Insertar")]
        public async Task<Response> InsertarCompromiso(Compromiso compromiso)
        {
            try
            {
                db.Compromiso.Add(compromiso);
                await db.SaveChangesAsync();
                return new Response { IsSuccess = true, };

            }
            catch (Exception ex)
            {
                return new Response {
                    IsSuccess = false,
                    Message= ex.Message
               };
            }
        }

        // POST: api/Compromiso
        /// <summary>
        /// Devuelve una lista  de compromisa para el vendedor
        /// </summary>
        /// <param name="vendedor"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ListaSinSolucion")]
        public async Task <Response> ListaSinSolucion(VendedorRequest vendedor)
        {

            try
            { 

            db.Configuration.ProxyCreationEnabled = false;
                //Esta  es para  obtener los 5 ultimos productos de esta fecha
                var list = (from t in db.Compromiso
                            where t.Visita.IdVendedor == vendedor.IdVendedor
                            orderby t.Visita.Fecha
                            select t).Take(5);

                var lista = await db.Compromiso.Where(x => x.Visita.IdVendedor == vendedor.IdVendedor).ToListAsync();


                return new Response
                {
                    IsSuccess=true,
                    Resultado=lista,
                };
            }
            catch(Exception ex)
            {
                return new Response
                {
                    IsSuccess=false,
                    Message=ex.Message
                    
                };
            }




        }

    }
}
