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
    public class subcasetypesController : Controller
    {
        private HMSEntities db = new HMSEntities();

        // GET: subcasetypes
        public ActionResult Index(int? page)
        {
            var subcasetypes = db.subcasetypes.Include(s => s.CaseType).OrderByDescending(x => x.SubcasetypeId);
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            int pagesize = 7;
            int pagenumber = (page ?? 1);
            return View(subcasetypes.ToPagedList(pagenumber, pagesize));
        }

        // GET: subcasetypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subcasetype subcasetype = db.subcasetypes.Find(id);
            if (subcasetype == null)
            {
                return HttpNotFound();
            }
            return View(subcasetype);
        }

        // GET: subcasetypes/Create
        public ActionResult Create()
        {
            ViewBag.CaseTypeId = new SelectList(db.CaseTypes, "CaseTypeId", "CaseType1");
            return View();
        }

        // POST: subcasetypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SubcasetypeId,Subcasetype1,CaseTypeId,Price")] subcasetype subcasetype)
        {
            if (ModelState.IsValid)
            {
                db.subcasetypes.Add(subcasetype);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CaseTypeId = new SelectList(db.CaseTypes, "CaseTypeId", "CaseType1", subcasetype.CaseTypeId);
            return View(subcasetype);
        }

        // GET: subcasetypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subcasetype subcasetype = db.subcasetypes.Find(id);
            if (subcasetype == null)
            {
                return HttpNotFound();
            }
            ViewBag.CaseTypeId = new SelectList(db.CaseTypes, "CaseTypeId", "CaseType1", subcasetype.CaseTypeId);
            return View(subcasetype);
        }

        // POST: subcasetypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SubcasetypeId,Subcasetype1,CaseTypeId,Price")] subcasetype subcasetype)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subcasetype).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CaseTypeId = new SelectList(db.CaseTypes, "CaseTypeId", "CaseType1", subcasetype.CaseTypeId);
            return View(subcasetype);
        }

        // GET: subcasetypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subcasetype subcasetype = db.subcasetypes.Find(id);
            if (subcasetype == null)
            {
                return HttpNotFound();
            }
            return View(subcasetype);
        }

        // POST: subcasetypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            subcasetype subcasetype = db.subcasetypes.Find(id);
            db.subcasetypes.Remove(subcasetype);
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
