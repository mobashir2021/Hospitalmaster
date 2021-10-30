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
    public class EXPENSController : Controller
    {
        private HMSEntities db = new HMSEntities();

        // GET: EXPENS
        public ActionResult Index(int? page)
        {
            var eXPENSES = db.EXPENSES.Include(e => e.ITEM).OrderByDescending(x => x.EXPENSEDATE).ToList();
            
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            int pagesize = 7;
            int pagenumber = (page ?? 1);
            return View(eXPENSES.ToPagedList(pagenumber, pagesize));
            //return View(eXPENSES.ToList());
        }

        // GET: EXPENS/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EXPENS eXPENS = db.EXPENSES.Find(id);
            if (eXPENS == null)
            {
                return HttpNotFound();
            }
            return View(eXPENS);
        }

        // GET: EXPENS/Create
        public ActionResult Create()
        {
            ViewBag.ITEMID = new SelectList(db.ITEMS, "ITEMID", "ITEMNAME");
            return View();
        }

        // POST: EXPENS/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EXPENSEID,ITEMID,QUANTITY,TOTALAMOUNT")] EXPENS eXPENS)
        {
            if (ModelState.IsValid)
            {
                eXPENS.EXPENSEDATE = DateTime.Now.Date;
                db.EXPENSES.Add(eXPENS);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ITEMID = new SelectList(db.ITEMS, "ITEMID", "ITEMNAME", eXPENS.ITEMID);
            return View(eXPENS);
        }

        // GET: EXPENS/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EXPENS eXPENS = db.EXPENSES.Find(id);
            if (eXPENS == null)
            {
                return HttpNotFound();
            }
            ViewBag.ITEMID = new SelectList(db.ITEMS, "ITEMID", "ITEMNAME", eXPENS.ITEMID);
            return View(eXPENS);
        }

        // POST: EXPENS/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EXPENSEID,ITEMID,QUANTITY,TOTALAMOUNT")] EXPENS eXPENS)
        {
            if (ModelState.IsValid)
            {
                eXPENS.EXPENSEDATE = DateTime.Now.Date;
                db.Entry(eXPENS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ITEMID = new SelectList(db.ITEMS, "ITEMID", "ITEMNAME", eXPENS.ITEMID);
            return View(eXPENS);
        }

        // GET: EXPENS/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EXPENS eXPENS = db.EXPENSES.Find(id);
            if (eXPENS == null)
            {
                return HttpNotFound();
            }
            return View(eXPENS);
        }

        // POST: EXPENS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EXPENS eXPENS = db.EXPENSES.Find(id);
            db.EXPENSES.Remove(eXPENS);
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
