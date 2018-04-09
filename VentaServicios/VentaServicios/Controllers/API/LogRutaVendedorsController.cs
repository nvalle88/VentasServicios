using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using VentaServicios.ModeloDato;

namespace VentaServicios.Controllers.API
{
    public class LogRutaVendedorsController : ApiController
    {
        private ModelVentas db = new ModelVentas();

        // GET: api/LogRutaVendedors
        public IQueryable<LogRutaVendedor> GetLogRutaVendedor()
        {
            return db.LogRutaVendedor;
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