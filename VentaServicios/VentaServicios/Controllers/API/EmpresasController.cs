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
    [RoutePrefix("api/Empresa")]
    public class EmpresasController : ApiController
    {
        private ModelVentas db = new ModelVentas();

        // GET: api/Empresas
        public IQueryable<Empresa> GetEmpresa()
        {
            return db.Empresa;
        }

        [HttpPost]
        [Route("ObtenerEmpresaPorCliente")]
        public async Task<Response> ObtenerEmpresaPorCliente(UserRequest userRequest)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var empresa = await db.AspNetUsers.FindAsync(userRequest.Id.ToString());
                if (empresa == null)
                {
                    return new Response { IsSuccess = false, };
                }

                return new Response { IsSuccess = true, Resultado = empresa };
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        // GET: api/Empresas/5
        [ResponseType(typeof(Empresa))]
        public async Task<IHttpActionResult> GetEmpresa(int id)
        {
            Empresa empresa = await db.Empresa.FindAsync(id);
            if (empresa == null)
            {
                return NotFound();
            }

            return Ok(empresa);
        }

        // PUT: api/Empresas/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutEmpresa(int id, Empresa empresa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != empresa.IdEmpresa)
            {
                return BadRequest();
            }

            db.Entry(empresa).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmpresaExists(id))
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

        // POST: api/Empresas
        [ResponseType(typeof(Empresa))]
        public async Task<IHttpActionResult> PostEmpresa(Empresa empresa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Empresa.Add(empresa);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = empresa.IdEmpresa }, empresa);
        }

        // DELETE: api/Empresas/5
        [ResponseType(typeof(Empresa))]
        public async Task<IHttpActionResult> DeleteEmpresa(int id)
        {
            Empresa empresa = await db.Empresa.FindAsync(id);
            if (empresa == null)
            {
                return NotFound();
            }

            db.Empresa.Remove(empresa);
            await db.SaveChangesAsync();

            return Ok(empresa);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmpresaExists(int id)
        {
            return db.Empresa.Count(e => e.IdEmpresa == id) > 0;
        }
    }
}