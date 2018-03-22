using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VentaServicios.ModeloDato;

namespace VentaServicios.Controllers.MVC
{
    public class GerentesController : Controller
    {
        private ModelVentas db = new ModelVentas();

        // GET: Gerentes
        public async Task<ActionResult> Index()
        {
            var gerente = db.Gerente.Include(g => g.Usuario);
            return View(await gerente.ToListAsync());
        }

        // GET: Gerentes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gerente gerente = await db.Gerente.FindAsync(id);
            if (gerente == null)
            {
                return HttpNotFound();
            }
            return View(gerente);
        }

        // GET: Gerentes/Create
        public ActionResult Create()
        {
            ViewBag.IdUsuario = new SelectList(db.Usuario, "IdUsuario", "TokenContrasena");
            return View();
        }

        // POST: Gerentes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "IdGerente,IdUsuario")] Gerente gerente)
        {
            if (ModelState.IsValid)
            {
                db.Gerente.Add(gerente);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.IdUsuario = new SelectList(db.Usuario, "IdUsuario", "TokenContrasena", gerente.IdUsuario);
            return View(gerente);
        }

        // GET: Gerentes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gerente gerente = await db.Gerente.FindAsync(id);
            if (gerente == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdUsuario = new SelectList(db.Usuario, "IdUsuario", "TokenContrasena", gerente.IdUsuario);
            return View(gerente);
        }

        // POST: Gerentes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "IdGerente,IdUsuario")] Gerente gerente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gerente).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.IdUsuario = new SelectList(db.Usuario, "IdUsuario", "TokenContrasena", gerente.IdUsuario);
            return View(gerente);
        }

        // GET: Gerentes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gerente gerente = await db.Gerente.FindAsync(id);
            if (gerente == null)
            {
                return HttpNotFound();
            }
            return View(gerente);
        }

        // POST: Gerentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Gerente gerente = await db.Gerente.FindAsync(id);
            db.Gerente.Remove(gerente);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
