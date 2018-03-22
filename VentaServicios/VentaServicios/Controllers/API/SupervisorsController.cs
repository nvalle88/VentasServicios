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
    [RoutePrefix("Api/Supervisor")]
    public class SupervisorsController : ApiController
    {
        private ModelVentas db = new ModelVentas();

        // GET: api/Supervisors
        //public IQueryable<Supervisor> GetSupervisor()
        //{
        //    return db.Supervisor;
        //}

        [HttpGet]
        [Route("LitarSupervisores")]
        public async Task<List<SupervisorRequest>> LitarSupervisores()
        {
            var lista = await db.Supervisor.Select(x => new SupervisorRequest
            {
                IdSupervisor = x.IdSupervisor,
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



        // GET: api/Supervisors/5
        [ResponseType(typeof(Supervisor))]
        public async Task<IHttpActionResult> GetSupervisor(string id)
        {
            Supervisor supervisor = await db.Supervisor.FindAsync(id);
            if (supervisor == null)
            {
                return NotFound();
            }

            return Ok(supervisor);
        }

        // PUT: api/Supervisors/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSupervisor(string id, Supervisor supervisor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != supervisor.IdSupervisor)
            {
                return BadRequest();
            }

            db.Entry(supervisor).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupervisorExists(id))
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

        // POST: api/Supervisors
        [ResponseType(typeof(Supervisor))]
        public async Task<IHttpActionResult> PostSupervisor(Supervisor supervisor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Supervisor.Add(supervisor);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SupervisorExists(supervisor.IdSupervisor))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = supervisor.IdSupervisor }, supervisor);
        }

        // DELETE: api/Supervisors/5
        [ResponseType(typeof(Supervisor))]
        public async Task<IHttpActionResult> DeleteSupervisor(string id)
        {
            Supervisor supervisor = await db.Supervisor.FindAsync(id);
            if (supervisor == null)
            {
                return NotFound();
            }

            db.Supervisor.Remove(supervisor);
            await db.SaveChangesAsync();

            return Ok(supervisor);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SupervisorExists(string id)
        {
            return db.Supervisor.Count(e => e.IdSupervisor == id) > 0;
        }
    }
}