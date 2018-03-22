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

namespace VentaServicios.Controllers.API
{
    [RoutePrefix("api/TiposClientes")]
    public class TiposClientesController : ApiController
    {
        private ModelVentas db = new ModelVentas();

        // GET: api/TiposClientes
        [Route("ListarTiposClientes")]
        public async Task<List<TipoCliente>> ListarTiposClientes(Empresa empresa)
        {
            db.Configuration.ProxyCreationEnabled = false;
            return await db.TipoCliente.Where(x=>x.IdEmpresa==empresa.IdEmpresa).ToListAsync();
        }

        // GET: api/TiposClientes/5
        [ResponseType(typeof(TipoCliente))]
        public async Task<IHttpActionResult> GetTipoCliente(int id)
        {
            TipoCliente tipoCliente = await db.TipoCliente.FindAsync(id);
            if (tipoCliente == null)
            {
                return NotFound();
            }

            return Ok(tipoCliente);
        }

        // PUT: api/TiposClientes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTipoCliente(int id, TipoCliente tipoCliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tipoCliente.idTipoCliente)
            {
                return BadRequest();
            }

            db.Entry(tipoCliente).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoClienteExists(id))
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

        // POST: api/TiposClientes
        [ResponseType(typeof(TipoCliente))]
        public async Task<IHttpActionResult> PostTipoCliente(TipoCliente tipoCliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TipoCliente.Add(tipoCliente);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TipoClienteExists(tipoCliente.idTipoCliente))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tipoCliente.idTipoCliente }, tipoCliente);
        }

        // DELETE: api/TiposClientes/5
        [ResponseType(typeof(TipoCliente))]
        public async Task<IHttpActionResult> DeleteTipoCliente(int id)
        {
            TipoCliente tipoCliente = await db.TipoCliente.FindAsync(id);
            if (tipoCliente == null)
            {
                return NotFound();
            }

            db.TipoCliente.Remove(tipoCliente);
            await db.SaveChangesAsync();

            return Ok(tipoCliente);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TipoClienteExists(int id)
        {
            return db.TipoCliente.Count(e => e.idTipoCliente == id) > 0;
        }
    }
}