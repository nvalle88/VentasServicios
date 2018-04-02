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
using VentaServicios.Utils;

namespace VentaServicios.Controllers.API
{
    [RoutePrefix("api/Compromisoes")]
    public class TipoCompromisoesController : ApiController
    {
        private ModelVentas db = new ModelVentas();

        // GET: api/TipoCompromisoes
        public IQueryable<TipoCompromiso> GetTipoCompromiso()
        {
            db.Configuration.ProxyCreationEnabled = false;
            return db.TipoCompromiso;
        }

        // GET: api/TipoCompromisoes/5
        [ResponseType(typeof(TipoCompromiso))]
        public IHttpActionResult GetTipoCompromiso(int id)
        {
            TipoCompromiso tipoCompromiso = db.TipoCompromiso.Find(id);
            if (tipoCompromiso == null)
            {
                return NotFound();
            }

            return Ok(tipoCompromiso);
        }

       
        // PUT: api/TipoCompromisoes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTipoCompromiso(int id, TipoCompromiso tipoCompromiso)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tipoCompromiso.IdTipoCompromiso)
            {
                return BadRequest();
            }

            db.Entry(tipoCompromiso).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoCompromisoExists(id))
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

        // POST: api/TipoCompromisoes
        [ResponseType(typeof(TipoCompromiso))]
        public IHttpActionResult PostTipoCompromiso(TipoCompromiso tipoCompromiso)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TipoCompromiso.Add(tipoCompromiso);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tipoCompromiso.IdTipoCompromiso }, tipoCompromiso);
        }

        // DELETE: api/TipoCompromisoes/5
        [ResponseType(typeof(TipoCompromiso))]
        public IHttpActionResult DeleteTipoCompromiso(int id)
        {
            TipoCompromiso tipoCompromiso = db.TipoCompromiso.Find(id);
            if (tipoCompromiso == null)
            {
                return NotFound();
            }

            db.TipoCompromiso.Remove(tipoCompromiso);
            db.SaveChanges();

            return Ok(tipoCompromiso);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TipoCompromisoExists(int id)
        {
            return db.TipoCompromiso.Count(e => e.IdTipoCompromiso == id) > 0;
        }
    }
}