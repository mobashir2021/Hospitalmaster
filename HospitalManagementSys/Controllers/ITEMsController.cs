using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HospitalManagementSys.Models;
using PagedList;

namespace HospitalManagementSys.Controllers
{
    public class ITEMsController : Controller
    {
        private HMSEntities db = new HMSEntities();

        // GET: ITEMs
        public ActionResult Index(int? page)
        {
            var items = db.ITEMS.ToList().OrderByDescending(x => x.ITEMID);
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            int pagesize = 7;
            int pagenumber = (page ?? 1);
            return View(items.ToPagedList(pagenumber, pagesize));
        }

        // GET: ITEMs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ITEM iTEM = db.ITEMS.Find(id);
            if (iTEM == null)
            {
                return HttpNotFound();
            }
            return View(iTEM);
        }

        // GET: ITEMs/Create
        public ActionResult Create()
        {
            ViewBag.SUPPLIERID = new SelectList(db.SUPPLIERs, "SUPPLIERID", "SUPPLIERNAME");
            return View();
        }

        // POST: ITEMs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ITEMID,ITEMNAME,BASEPRICE,SUPPLIERID")] ITEM iTEM)
        {
            if (ModelState.IsValid)
            {
                db.ITEMS.Add(iTEM);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SUPPLIERID = new SelectList(db.SUPPLIERs, "SUPPLIERID", "SUPPLIERNAME");
            return View(iTEM);
        }

        // GET: ITEMs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ITEM iTEM = db.ITEMS.Find(id);
            if (iTEM == null)
            {
                return HttpNotFound();
            }
            ViewBag.SUPPLIERID = new SelectList(db.SUPPLIERs, "SUPPLIERID", "SUPPLIERNAME");
            return View(iTEM);
        }

        // POST: ITEMs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ITEMID,ITEMNAME,BASEPRICE,SUPPLIERID")] ITEM iTEM)
        {
            ViewBag.SUPPLIERID = new SelectList(db.SUPPLIERs, "SUPPLIERID", "SUPPLIERNAME");
            if (ModelState.IsValid)
            {
                db.Entry(iTEM).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(iTEM);
        }

        // GET: ITEMs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ITEM iTEM = db.ITEMS.Find(id);
            if (iTEM == null)
            {
                return HttpNotFound();
            }
            return View(iTEM);
        }

        // POST: ITEMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ITEM iTEM = db.ITEMS.Find(id);
            db.ITEMS.Remove(iTEM);
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
