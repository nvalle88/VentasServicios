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

namespace VentaServicios.Controllers.API
{
    [RoutePrefix("Api/Gerentes")]
    public class GerentesController : ApiController
    {
        
        private ModelVentas db = new ModelVentas();
        
        
        // GET: api/Gerentes
        //public IQueryable<Gerente> GetGerente()
        //{
                //para no traer relaciones
        //    db.Configuration.ProxyCreationEnabled = false;

        //    return db.Gerente;
        //}

        [HttpGet]
        [Route("LitarGerentes")]
        public async Task <List< GerenteRequest>> LitarGerentes()
        {
            //para no traer direcciones
            //db.Configuration.ProxyCreationEnabled = false;
         var lista=  await db.Gerente.Select(x => new GerenteRequest
            {
                Apellidos = x.Usuario.Apellidos,
                Direccion = x.Usuario.Direccion,
                Identificacion = x.Usuario.Identificacion,
                IdGerente = x.IdGerente,
                IdUsuario = x.IdUsuario,
                Nombres = x.Usuario.Nombres,
                Telefono = x.Usuario.Telefono,
            }).ToListAsync();

            return lista;
        }

        // GET: api/Gerentes/5
        [ResponseType(typeof(Gerente))]
        public async Task<IHttpActionResult> GetGerente(int id)
        {
            Gerente gerente = await db.Gerente.FindAsync(id);
            if (gerente == null)
            {
                return NotFound();
            }

            return Ok(gerente);
        }

        // PUT: api/Gerentes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutGerente(int id, Gerente gerente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != gerente.IdGerente)
            {
                return BadRequest();
            }

            db.Entry(gerente).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GerenteExists(id))
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

        // POST: api/Gerentes
        [ResponseType(typeof(Gerente))]
        public async Task<IHttpActionResult> PostGerente(Gerente gerente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Gerente.Add(gerente);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = gerente.IdGerente }, gerente);
        }

        // DELETE: api/Gerentes/5
        [ResponseType(typeof(Gerente))]
        public async Task<IHttpActionResult> DeleteGerente(int id)
        {
            Gerente gerente = await db.Gerente.FindAsync(id);
            if (gerente == null)
            {
                return NotFound();
            }

            db.Gerente.Remove(gerente);
            await db.SaveChangesAsync();

            return Ok(gerente);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GerenteExists(int id)
        {
            return db.Gerente.Count(e => e.IdGerente == id) > 0;
        }
    }
}