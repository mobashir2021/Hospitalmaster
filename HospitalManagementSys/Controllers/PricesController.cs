using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HospitalManagementSys.Models;

namespace HospitalManagementSys.Controllers
{
    public class PricesController : Controller
    {
        private HMSEntities db = new HMSEntities();

        // GET: Prices
        public ActionResult Index()
        {
            var prices = db.Prices.Include(p => p.CaseType).Include(p => p.Doctor);
            return View(prices.ToList());
        }

        // GET: Prices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Price price = db.Prices.Find(id);
            if (price == null)
            {
                return HttpNotFound();
            }
            return View(price);
        }

        // GET: Prices/Create
        public ActionResult Create()
        {
            ViewBag.CaseTypeId = new SelectList(db.CaseTypes, "CaseTypeId", "CaseType1");
            ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorId", "DoctorName");
            return View();
        }

        // POST: Prices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PriceId,DoctorId,CaseTypeId,price1")] Price price)
        {
            if (ModelState.IsValid)
            {
                db.Prices.Add(price);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CaseTypeId = new SelectList(db.CaseTypes, "CaseTypeId", "CaseType1", price.CaseTypeId);
            ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorId", "DoctorName", price.DoctorId);
            return View(price);
        }

        // GET: Prices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Price price = db.Prices.Find(id);
            if (price == null)
            {
                return HttpNotFound();
            }
            ViewBag.CaseTypeId = new SelectList(db.CaseTypes, "CaseTypeId", "CaseType1", price.CaseTypeId);
            ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorId", "DoctorName", price.DoctorId);
            return View(price);
        }

        // POST: Prices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PriceId,DoctorId,CaseTypeId,price1")] Price price)
        {
            if (ModelState.IsValid)
            {
                db.Entry(price).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CaseTypeId = new SelectList(db.CaseTypes, "CaseTypeId", "CaseType1", price.CaseTypeId);
            ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorId", "DoctorName", price.DoctorId);
            return View(price);
        }

        // GET: Prices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Price price = db.Prices.Find(id);
            if (price == null)
            {
                return HttpNotFound();
            }
            return View(price);
        }

        // POST: Prices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Price price = db.Prices.Find(id);
            db.Prices.Remove(price);
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
