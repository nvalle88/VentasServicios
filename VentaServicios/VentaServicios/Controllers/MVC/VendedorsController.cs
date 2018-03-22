using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
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
        public ActionResult Index()
        {
            var vendedor = db.Vendedor.Include(v => v.Supervisor).Include(v => v.Usuario);
            return View(vendedor.ToList());
        }

        // GET: Vendedors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendedor vendedor = db.Vendedor.Find(id);
            if (vendedor == null)
            {
                return HttpNotFound();
            }
            return View(vendedor);
        }

        // GET: Vendedors/Create
        public ActionResult Create()
        {
            ViewBag.IdSupervisor = new SelectList(db.Supervisor, "IdSupervisor", "IdSupervisor");
            ViewBag.IdUsuario = new SelectList(db.Usuario, "IdUsuario", "TokenContrasena");
            return View();
        }

        // POST: Vendedors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdVendedor,TiempoSeguimiento,IdSupervisor,IdUsuario")] Vendedor vendedor)
        {
            if (ModelState.IsValid)
            {
                db.Vendedor.Add(vendedor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdSupervisor = new SelectList(db.Supervisor, "IdSupervisor", "IdSupervisor", vendedor.IdSupervisor);
            ViewBag.IdUsuario = new SelectList(db.Usuario, "IdUsuario", "TokenContrasena", vendedor.IdUsuario);
            return View(vendedor);
        }

        // GET: Vendedors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendedor vendedor = db.Vendedor.Find(id);
            if (vendedor == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdSupervisor = new SelectList(db.Supervisor, "IdSupervisor", "IdSupervisor", vendedor.IdSupervisor);
            ViewBag.IdUsuario = new SelectList(db.Usuario, "IdUsuario", "TokenContrasena", vendedor.IdUsuario);
            return View(vendedor);
        }

        // POST: Vendedors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdVendedor,TiempoSeguimiento,IdSupervisor,IdUsuario")] Vendedor vendedor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vendedor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdSupervisor = new SelectList(db.Supervisor, "IdSupervisor", "IdSupervisor", vendedor.IdSupervisor);
            ViewBag.IdUsuario = new SelectList(db.Usuario, "IdUsuario", "TokenContrasena", vendedor.IdUsuario);
            return View(vendedor);
        }

        // GET: Vendedors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendedor vendedor = db.Vendedor.Find(id);
            if (vendedor == null)
            {
                return HttpNotFound();
            }
            return View(vendedor);
        }

        // POST: Vendedors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vendedor vendedor = db.Vendedor.Find(id);
            db.Vendedor.Remove(vendedor);
            db.SaveChanges();
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
