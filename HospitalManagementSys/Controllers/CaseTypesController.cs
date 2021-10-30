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
    public class CaseTypesController : Controller
    {
        private HMSEntities db = new HMSEntities();

        // GET: CaseTypes
        public ActionResult Index(int? page)
        {
            var casetypes = db.CaseTypes.ToList().OrderBy(x => x.CaseTypeId);
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            int pagesize = 7;
            int pagenumber = (page ?? 1);
            return View(casetypes.ToPagedList(pagenumber, pagesize));
            //return View(db.CaseTypes.ToList());
        }

        // GET: CaseTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CaseType caseType = db.CaseTypes.Find(id);
            if (caseType == null)
            {
                return HttpNotFound();
            }
            return View(caseType);
        }

        // GET: CaseTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CaseTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CaseTypeId,CaseType1")] CaseType caseType)
        {
            if (ModelState.IsValid)
            {
                CaseType cs = new CaseType();
                cs.CaseTypeId = caseType.CaseTypeId;
                cs.CaseType1 = caseType.CaseType1;
                db.CaseTypes.Add(cs);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(caseType);
        }

        // GET: CaseTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CaseType caseType = db.CaseTypes.Find(id);
            if (caseType == null)
            {
                return HttpNotFound();
            }
            return View(caseType);
        }

        // POST: CaseTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CaseTypeId,CaseType1")] CaseType caseType)
        {
            if (ModelState.IsValid)
            {
                CaseType cs = new CaseType();
                cs.CaseTypeId = caseType.CaseTypeId;
                cs.CaseType1 = caseType.CaseType1;
                db.Entry(cs).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(caseType);
        }

        // GET: CaseTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CaseType caseType = db.CaseTypes.Find(id);
            if (caseType == null)
            {
                return HttpNotFound();
            }
            return View(caseType);
        }

        // POST: CaseTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CaseType caseType = db.CaseTypes.Find(id);
            db.CaseTypes.Remove(caseType);
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
