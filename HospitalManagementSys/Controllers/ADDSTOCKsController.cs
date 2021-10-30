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
    public class ADDSTOCKsController : Controller
    {
        private HMSEntities db = new HMSEntities();

        // GET: ADDSTOCKs
        public ActionResult Index(int? page)
        {
            var aDDSTOCKS = db.ADDSTOCKS.Include(a => a.ITEM).ToList().OrderByDescending(x => x.STOCKID).ToList();
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            int pagesize = 7;
            int pagenumber = (page ?? 1);
            return View(aDDSTOCKS.ToPagedList(pagenumber, pagesize));
        }

        // GET: ADDSTOCKs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ADDSTOCK aDDSTOCK = db.ADDSTOCKS.Find(id);
            if (aDDSTOCK == null)
            {
                return HttpNotFound();
            }
            return View(aDDSTOCK);
        }

        // GET: ADDSTOCKs/Create
        public ActionResult Create()
        {
            ViewBag.ITEMID = new SelectList(db.ITEMS, "ITEMID", "ITEMNAME");
            return View();
        }

        // POST: ADDSTOCKs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "STOCKID,ITEMID,QUANTITY,TOTALAMOUNT,STOCKDATE,AMOUNTPAID,BALANCE")] ADDSTOCK aDDSTOCK)
        {
            if (ModelState.IsValid)
            {
                db.ADDSTOCKS.Add(aDDSTOCK);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ITEMID = new SelectList(db.ITEMS, "ITEMID", "ITEMNAME", aDDSTOCK.ITEMID);
            return View(aDDSTOCK);
        }

        public ActionResult AmountDue()
        {
            ViewBag.SUPPLIERID = new SelectList(db.SUPPLIERs, "SUPPLIERID", "SUPPLIERNAME");
            return View();
        }

        public JsonResult GetSupplier(int supplierid)
        {
            List<supplierreport> lst = new List<supplierreport>();
            var pt = (from ads in db.ADDSTOCKS.ToList()
                      join itd in db.ITEMS.ToList()
                      on ads.ITEMID equals itd.ITEMID
                      join spi in db.SUPPLIERs.ToList()
                      on itd.SUPPLIERID equals spi.SUPPLIERID
                      select new { itd, spi, ads }).ToList();
                     

            foreach (var pi in pt)
            {
                supplierreport sr = new supplierreport();
                sr.Supplier = pi.spi.SUPPLIERNAME;
                sr.MobileNo = pi.spi.MOBILENO;
                sr.Items = pi.itd.ITEMNAME;
                sr.TotalAmount = pi.ads.TOTALAMOUNT.HasValue ? pi.ads.TOTALAMOUNT.Value.ToString() : "0";
                sr.AmountPaid = pi.ads.AMOUNTPAID.HasValue ? pi.ads.AMOUNTPAID.Value.ToString() : "0";
                sr.AmountDue = pi.ads.BALANCE.HasValue ? pi.ads.BALANCE.Value.ToString() : "0";
                sr.StockDate = pi.ads.STOCKDATE.HasValue ? pi.ads.STOCKDATE.Value.ToString("dd-MMM-yyyy") : "--";

                lst.Add(sr);
            }
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        // GET: ADDSTOCKs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ADDSTOCK aDDSTOCK = db.ADDSTOCKS.Find(id);
            if (aDDSTOCK == null)
            {
                return HttpNotFound();
            }
            ViewBag.ITEMID = new SelectList(db.ITEMS, "ITEMID", "ITEMNAME", aDDSTOCK.ITEMID);
            return View(aDDSTOCK);
        }

        // POST: ADDSTOCKs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "STOCKID,ITEMID,QUANTITY,TOTALAMOUNT,STOCKDATE,AMOUNTPAID,BALANCE")] ADDSTOCK aDDSTOCK)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aDDSTOCK).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ITEMID = new SelectList(db.ITEMS, "ITEMID", "ITEMNAME", aDDSTOCK.ITEMID);
            return View(aDDSTOCK);
        }

        // GET: ADDSTOCKs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ADDSTOCK aDDSTOCK = db.ADDSTOCKS.Find(id);
            if (aDDSTOCK == null)
            {
                return HttpNotFound();
            }
            return View(aDDSTOCK);
        }

        // POST: ADDSTOCKs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ADDSTOCK aDDSTOCK = db.ADDSTOCKS.Find(id);
            db.ADDSTOCKS.Remove(aDDSTOCK);
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
