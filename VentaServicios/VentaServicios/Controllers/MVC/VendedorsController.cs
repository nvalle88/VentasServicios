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
    public class VendedorsController : Controller
    {
        private ModelVentas db = new ModelVentas();

        // GET: Vendedors
        public async Task<ActionResult> Index()
        {
            var vendedor = db.Vendedor.Include(v => v.AspNetUsers).Include(v => v.Supervisor);
            return View(await vendedor.ToListAsync());
        }

        // GET: Vendedors/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendedor vendedor = await db.Vendedor.FindAsync(id);
            if (vendedor == null)
            {
                return HttpNotFound();
            }
            return View(vendedor);
        }

        // GET: Vendedors/Create
        public ActionResult Create()
        {
            ViewBag.IdUsuario = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.IdSupervisor = new SelectList(db.Supervisor, "IdSupervisor", "IdUsuario");
            return View();
        }

        // POST: Vendedors/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "IdVendedor,TiempoSeguimiento,IdSupervisor,IdUsuario")] Vendedor vendedor)
        {
            if (ModelState.IsValid)
            {
                db.Vendedor.Add(vendedor);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.IdUsuario = new SelectList(db.AspNetUsers, "Id", "Email", vendedor.IdUsuario);
            ViewBag.IdSupervisor = new SelectList(db.Supervisor, "IdSupervisor", "IdUsuario", vendedor.IdSupervisor);
            return View(vendedor);
        }

        // GET: Vendedors/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendedor vendedor = await db.Vendedor.FindAsync(id);
            if (vendedor == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdUsuario = new SelectList(db.AspNetUsers, "Id", "Email", vendedor.IdUsuario);
            ViewBag.IdSupervisor = new SelectList(db.Supervisor, "IdSupervisor", "IdUsuario", vendedor.IdSupervisor);
            return View(vendedor);
        }

        // POST: Vendedors/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "IdVendedor,TiempoSeguimiento,IdSupervisor,IdUsuario")] Vendedor vendedor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vendedor).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.IdUsuario = new SelectList(db.AspNetUsers, "Id", "Email", vendedor.IdUsuario);
            ViewBag.IdSupervisor = new SelectList(db.Supervisor, "IdSupervisor", "IdUsuario", vendedor.IdSupervisor);
            return View(vendedor);
        }

        // GET: Vendedors/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendedor vendedor = await db.Vendedor.FindAsync(id);
            if (vendedor == null)
            {
                return HttpNotFound();
            }
            return View(vendedor);
        }

        // POST: Vendedors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Vendedor vendedor = await db.Vendedor.FindAsync(id);
            db.Vendedor.Remove(vendedor);
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
