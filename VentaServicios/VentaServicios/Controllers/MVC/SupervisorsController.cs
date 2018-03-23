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
    public class SupervisorsController : Controller
    {
        private ModelVentas db = new ModelVentas();

        // GET: Supervisors
        public async Task<ActionResult> Index()
        {
            var supervisor = db.Supervisor.Include(s => s.AspNetUsers).Include(s => s.Gerente);
            return View(await supervisor.ToListAsync());
        }

        // GET: Supervisors/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supervisor supervisor = await db.Supervisor.FindAsync(id);
            if (supervisor == null)
            {
                return HttpNotFound();
            }
            return View(supervisor);
        }

        // GET: Supervisors/Create
        public ActionResult Create()
        {
            ViewBag.IdUsuario = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.IdGerente = new SelectList(db.Gerente, "IdGerente", "IdUsuario");
            return View();
        }

        // POST: Supervisors/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "IdSupervisor,IdUsuario,IdGerente")] Supervisor supervisor)
        {
            if (ModelState.IsValid)
            {
                db.Supervisor.Add(supervisor);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.IdUsuario = new SelectList(db.AspNetUsers, "Id", "Email", supervisor.IdUsuario);
            ViewBag.IdGerente = new SelectList(db.Gerente, "IdGerente", "IdUsuario", supervisor.IdGerente);
            return View(supervisor);
        }

        // GET: Supervisors/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supervisor supervisor = await db.Supervisor.FindAsync(id);
            if (supervisor == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdUsuario = new SelectList(db.AspNetUsers, "Id", "Email", supervisor.IdUsuario);
            ViewBag.IdGerente = new SelectList(db.Gerente, "IdGerente", "IdUsuario", supervisor.IdGerente);
            return View(supervisor);
        }

        // POST: Supervisors/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "IdSupervisor,IdUsuario,IdGerente")] Supervisor supervisor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supervisor).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.IdUsuario = new SelectList(db.AspNetUsers, "Id", "Email", supervisor.IdUsuario);
            ViewBag.IdGerente = new SelectList(db.Gerente, "IdGerente", "IdUsuario", supervisor.IdGerente);
            return View(supervisor);
        }

        // GET: Supervisors/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supervisor supervisor = await db.Supervisor.FindAsync(id);
            if (supervisor == null)
            {
                return HttpNotFound();
            }
            return View(supervisor);
        }

        // POST: Supervisors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Supervisor supervisor = await db.Supervisor.FindAsync(id);
            db.Supervisor.Remove(supervisor);
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
