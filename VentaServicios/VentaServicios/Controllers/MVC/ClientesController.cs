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
    public class ClientesController : Controller
    {
        private ModelVentas db = new ModelVentas();

        // GET: Clientes
        public ActionResult Index()
        {
            var cliente = db.Cliente.Include(c => c.TipoCliente).Include(c => c.Vendedor);
            return View(cliente.ToList());
        }

        // GET: Clientes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // GET: Clientes/Create
        public ActionResult Create()
        {
            ViewBag.idTipoCliente = new SelectList(db.TipoCliente, "idTipoCliente", "Tipo");
            ViewBag.IdVendedor = new SelectList(db.Vendedor, "IdVendedor", "IdSupervisor");
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idCliente,Foto,Firma,Latitud,Longitud,Nombre,Apellido,Telefono,Email,idTipoCliente,IdVendedor")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Cliente.Add(cliente);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idTipoCliente = new SelectList(db.TipoCliente, "idTipoCliente", "Tipo", cliente.idTipoCliente);
            ViewBag.IdVendedor = new SelectList(db.Vendedor, "IdVendedor", "IdSupervisor", cliente.IdVendedor);
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            ViewBag.idTipoCliente = new SelectList(db.TipoCliente, "idTipoCliente", "Tipo", cliente.idTipoCliente);
            ViewBag.IdVendedor = new SelectList(db.Vendedor, "IdVendedor", "IdSupervisor", cliente.IdVendedor);
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idCliente,Foto,Firma,Latitud,Longitud,Nombre,Apellido,Telefono,Email,idTipoCliente,IdVendedor")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cliente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idTipoCliente = new SelectList(db.TipoCliente, "idTipoCliente", "Tipo", cliente.idTipoCliente);
            ViewBag.IdVendedor = new SelectList(db.Vendedor, "IdVendedor", "IdSupervisor", cliente.IdVendedor);
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Cliente cliente = db.Cliente.Find(id);
            db.Cliente.Remove(cliente);
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
