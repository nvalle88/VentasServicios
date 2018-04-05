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
using System.Data.Entity.Infrastructure;

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
        // POST: api/Compromiso
        [HttpPost]
        [Route("VerEstadisticos")]
        public async Task<EstadisticoSupervisorRequest> VerEstadisticos(SupervisorRequest supervisorRequest)
        {
            EstadisticoSupervisorRequest estadisticoSupervisorRequest = new EstadisticoSupervisorRequest();


            

            try
            {
                

                // Lógica para estadísticos pasteles (tipo de compromiso)
                var listaCompromiso = await db.Compromiso
                    .Join(db.TipoCompromiso, com => com.IdTipoCompromiso, tc => tc.IdTipoCompromiso, (com, tc) => new { tcom = com, ttc = tc })
                    .Join(db.Visita, conjunto1 => conjunto1.tcom.idVisita, visita => visita.idVisita, (conjunto1, visita) => new { Aconjunto1 = conjunto1, Avis = visita })
                    .Join(db.Vendedor, conjunto2 => conjunto2.Avis.IdVendedor, ven => ven.IdVendedor, (conjunto2, ven) => new { AConjunto2 = conjunto2, Aven = ven })

                //.Where(y => y.Aven == supervisorRequest.IdVendedor)
                .Select(x => new TipoCompromisoRequest
                {
                    IdTipoCompromiso = x.AConjunto2.Aconjunto1.ttc.IdTipoCompromiso,
                    Descripcion = x.AConjunto2.Aconjunto1.ttc.Descripcion

                }

                ).GroupBy(z => z.Descripcion).ToListAsync();


                var listaTipoCompromisos = new List<TipoCompromisoRequest>();

                for (int i = 0; i < listaCompromiso.Count; i++)
                {
                    var num = listaCompromiso.ElementAt(i).Count();

                    listaTipoCompromisos.Add(
                        new TipoCompromisoRequest
                        {
                            Descripcion = listaCompromiso.ElementAt(i).ElementAt(0).Descripcion,
                            CantidadCompromiso = num
                        }
                    );


                }


                // Lógica para compromisos cumplidos - incumplidos

                var cumplidos = await db.Compromiso
                    .Join(db.Visita, com => com.idVisita, v => v.idVisita, (com, v) => new { tcom = com, tv = v })
                    .Join(db.Vendedor, conjunto => conjunto.tv.IdVendedor, ven => ven.IdVendedor, (conjunto, ven) => new { varConjunto = conjunto, tven = ven })
                    //.Where(y => y.tven.IdVendedor == vendedorRequest.IdVendedor && !String.IsNullOrEmpty(y.varConjunto.tcom.Solucion))
                    .ToListAsync();

                var incumplidos = await db.Compromiso
                    .Join(db.Visita, com => com.idVisita, v => v.idVisita, (com, v) => new { tcom = com, tv = v })
                    .Join(db.Vendedor, conjunto => conjunto.tv.IdVendedor, ven => ven.IdVendedor, (conjunto, ven) => new { varConjunto = conjunto, tven = ven })
                    //.Where(y => y.tven.IdVendedor == vendedorRequest.IdVendedor && String.IsNullOrEmpty(y.varConjunto.tcom.Solucion))
                    .ToListAsync();


                //estadisticoVendedorRequest.IdVendedor = vendedorRequest.IdVendedor;
                //estadisticoSupervisorRequest.CalificacionPromedio = promedio;
                estadisticoSupervisorRequest.ListaTipoCompromiso = listaTipoCompromisos;
                estadisticoSupervisorRequest.CompromisosCumplidos = cumplidos.Count();
                estadisticoSupervisorRequest.CompromisosIncumplidos = incumplidos.Count();

                return estadisticoSupervisorRequest;
            }
            catch (Exception ex)
            {
                return estadisticoSupervisorRequest;
            }
        }

        //PUT: api/Compromiso
        [HttpPost]
        [Route("ActualizarCompromiso")]
        public async Task<IHttpActionResult> PutCompromiso( Compromiso compromisoAux)
        {
            
            var compromiso = await db.Compromiso.Where(x => x.IdCompromiso == compromisoAux.IdCompromiso).FirstOrDefaultAsync();
            compromiso.Solucion = compromisoAux.Solucion;

            db.Entry(compromiso).State = EntityState.Modified;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {                
                 throw;                
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

    }
}
