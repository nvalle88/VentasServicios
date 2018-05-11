using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
    [RoutePrefix("api/LogRutaVendedors")]

    public class LogRutaVendedorsController : ApiController
    {
        private ModelVentas db = new ModelVentas();

        // GET: api/LogRutaVendedors
        public IQueryable<LogRutaVendedor> GetLogRutaVendedor()
        {
            return db.LogRutaVendedor;
        }
        /// <summary>
        /// Metodo para obtener la última posición de los vendedores segun la empresa
        /// 
        /// </summary>
        /// <param name="empresaActual"></param>
        /// <returns>Lista de vendedores con su última posición</returns>
        [HttpPost]
        [Route("VendedoresPorEmpresa")]
        public async Task<Response> VendedoresPorEmpresa(EmpresaActual empresaActual)
        {
            var listaVendedores = new List<VendedorRequest>();
            try
            {
                listaVendedores = await db.Vendedor.Select(x => new VendedorRequest
                {
                    IdVendedor = x.IdVendedor,
                    TiempoSeguimiento = x.TiempoSeguimiento,
                    IdSupervisor = x.IdSupervisor,
                    IdUsuario = x.AspNetUsers.Id,
                    NombreApellido = x.AspNetUsers.Nombres + " " + x.AspNetUsers.Apellidos,
                    TokenContrasena = x.AspNetUsers.TokenContrasena,
                    Foto = x.AspNetUsers.Foto,
                    Estado = x.AspNetUsers.Estado,
                    Correo = x.AspNetUsers.Email,
                    Direccion = x.AspNetUsers.Direccion,
                    Identificacion = x.AspNetUsers.Identificacion,
                    Nombres = x.AspNetUsers.Nombres,
                    Apellidos = x.AspNetUsers.Apellidos,
                    Telefono = x.AspNetUsers.Telefono,
                    idEmpresa = x.AspNetUsers.IdEmpresa
                }
                ).Where(x => x.idEmpresa == empresaActual.IdEmpresa
                    && x.Estado == 1
                ).ToListAsync();


                List<VendedorPositionRequest> listPositionRequests = new List<VendedorPositionRequest>();

                foreach (var Vendedor in listaVendedores)
                {
                    var ultimaposicionVendedor =  await db.LogRutaVendedor
                        .Where(x => x.IdVendedor == Vendedor.IdVendedor)
                        .OrderByDescending(x => x.Fecha)
                        .Select(
                        x => new VendedorPositionRequest
                        {
                            VendedorId = x.IdVendedor,
                            Nombre = x.Vendedor.AspNetUsers.Nombres,
                            Lat = (float)x.Latitud,
                            Lon = (float)x.Longitud,
                            Fecha = (DateTime)x.Fecha,
                            EmpresaId = x.Vendedor.AspNetUsers.IdEmpresa,
                            urlFoto = x.Vendedor.AspNetUsers.Foto,

                        })
                        .FirstOrDefaultAsync();
                    if (ultimaposicionVendedor!=null)
                    {
                        listPositionRequests.Add(ultimaposicionVendedor);

                    }
                }


                return new Response
                {
                    IsSuccess = true,
                    Resultado = listPositionRequests,
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [HttpPost]
        [Route("VendedoresPorSupervisor")]
        public async Task<Response> VendedoresPorSupervisor(SupervisorRequest supervisorRequest)
        {
            var listaVendedores = new List<VendedorRequest>();
            try
            {
                listaVendedores = await db.Vendedor.Select(x => new VendedorRequest
                {
                    IdVendedor = x.IdVendedor,
                    TiempoSeguimiento = x.TiempoSeguimiento,
                    IdSupervisor = x.IdSupervisor,
                    IdUsuario = x.AspNetUsers.Id,
                    NombreApellido = x.AspNetUsers.Nombres + " " + x.AspNetUsers.Apellidos,
                    TokenContrasena = x.AspNetUsers.TokenContrasena,
                    Foto = x.AspNetUsers.Foto,
                    Estado = x.AspNetUsers.Estado,
                    Correo = x.AspNetUsers.Email,
                    Direccion = x.AspNetUsers.Direccion,
                    Identificacion = x.AspNetUsers.Identificacion,
                    Nombres = x.AspNetUsers.Nombres,
                    Apellidos = x.AspNetUsers.Apellidos,
                    Telefono = x.AspNetUsers.Telefono,
                    idEmpresa = x.AspNetUsers.IdEmpresa
                }
                ).Where(x => x.IdUsuario == supervisorRequest.IdUsuario
                    && x.Estado == 1
                ).ToListAsync();


                List<VendedorPositionRequest> listPositionRequests = new List<VendedorPositionRequest>();

                foreach (var Vendedor in listaVendedores)
                {
                    var ultimaposicionVendedor = await db.LogRutaVendedor
                        .Where(x => x.IdVendedor == Vendedor.IdVendedor)
                        .OrderByDescending(x => x.Fecha)
                        .Select(
                        x => new VendedorPositionRequest
                        {
                            VendedorId = x.IdVendedor,
                            Nombre = x.Vendedor.AspNetUsers.Nombres,
                            Lat = (float)x.Latitud,
                            Lon = (float)x.Longitud,
                            Fecha = (DateTime)x.Fecha,
                            EmpresaId = x.Vendedor.AspNetUsers.IdEmpresa,
                            urlFoto= x.Vendedor.AspNetUsers.Foto,
                            
                        })
                        .FirstOrDefaultAsync();
                    if (ultimaposicionVendedor != null)
                    {
                        listPositionRequests.Add(ultimaposicionVendedor);
                    }
                }
                return new Response
                {
                    IsSuccess=true,
                    Resultado= listPositionRequests,
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // GET: api/LogRutaVendedors/5
        [ResponseType(typeof(LogRutaVendedor))]
        public IHttpActionResult GetLogRutaVendedor(int id)
        {
            LogRutaVendedor logRutaVendedor = db.LogRutaVendedor.Find(id);
            if (logRutaVendedor == null)
            {
                return NotFound();
            }

            return Ok(logRutaVendedor);
        }

        // PUT: api/LogRutaVendedors/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLogRutaVendedor(int id, LogRutaVendedor logRutaVendedor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != logRutaVendedor.IdLogRutaVendedor)
            {
                return BadRequest();
            }

            db.Entry(logRutaVendedor).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LogRutaVendedorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/LogRutaVendedors
        [ResponseType(typeof(LogRutaVendedor))]
        public IHttpActionResult PostLogRutaVendedor(LogRutaVendedor logRutaVendedor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.LogRutaVendedor.Add(logRutaVendedor);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = logRutaVendedor.IdLogRutaVendedor }, logRutaVendedor);
        }

        // DELETE: api/LogRutaVendedors/5
        [ResponseType(typeof(LogRutaVendedor))]
        public IHttpActionResult DeleteLogRutaVendedor(int id)
        {
            LogRutaVendedor logRutaVendedor = db.LogRutaVendedor.Find(id);
            if (logRutaVendedor == null)
            {
                return NotFound();
            }

            db.LogRutaVendedor.Remove(logRutaVendedor);
            db.SaveChanges();

            return Ok(logRutaVendedor);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LogRutaVendedorExists(int id)
        {
            return db.LogRutaVendedor.Count(e => e.IdLogRutaVendedor == id) > 0;
        }
    }
}