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
    public class DoctorsController : Controller
    {
        private HMSEntities db = new HMSEntities();

        // GET: Doctors
        public ActionResult Index(int? page)
        {
            var doctors = db.Doctors.ToList().OrderBy(x => x.DoctorId);
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            int pagesize = 7;
            int pagenumber = (page ?? 1);
            return View(doctors.ToPagedList(pagenumber, pagesize));
            //return View(db.Doctors.ToList());
        }

        // GET: Doctors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        // GET: Doctors/Create
        public ActionResult Create()
        {
            var data = db.CaseTypes.Select(x => new SelectListItem() { Text = x.CaseType1, Value = x.CaseTypeId.ToString() }).ToList();
            ViewBag.Casetypelist = data;
            return View();
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DoctorId,DoctorName,EmailId,Address,PhoneNo,CASETYPEID,PERCENTANGE")] Doctor doctor, object casetypes, object percentages)
        {
            if (ModelState.IsValid)
            {
                Doctor dc = new Doctor();
                dc.DoctorId = doctor.DoctorId;
                dc.DoctorName = doctor.DoctorName;
                dc.EmailId = doctor.EmailId;
                dc.Address = doctor.Address;
                dc.PhoneNo = doctor.PhoneNo;
                dc.CASETYPEID = doctor.CASETYPEID;
                dc.PERCENTANGE = doctor.PERCENTANGE;
                db.Doctors.Add(dc);
                db.SaveChanges();
                int docid = dc.DoctorId;
                string[] casetypesdata = (string[])casetypes;
                string[] percentdata = (string[])percentages;
                DoctorCasetype dcini = new DoctorCasetype();
                dcini.CaseTypeId = doctor.CASETYPEID.HasValue ? doctor.CASETYPEID.Value : 0;
                dcini.DoctorId = docid;
                dcini.percentage = doctor.PERCENTANGE;
                db.DoctorCasetypes.Add(dcini);
                db.SaveChanges();
                for(int ij = 0; ij < casetypesdata.Length; ij++)
                {
                    DoctorCasetype dct = new DoctorCasetype();
                    dct.DoctorId = docid;
                    dct.CaseTypeId = Convert.ToInt32(casetypesdata[ij]);
                    dct.percentage = percentdata[ij].ToString();
                    db.DoctorCasetypes.Add(dct);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            return View(doctor);
        }

        // GET: Doctors/Edit/5
        public ActionResult Edit(int? id)
        {
            var data = db.CaseTypes.Select(x => new SelectListItem() { Text = x.CaseType1, Value = x.CaseTypeId.ToString() }).ToList();
            ViewBag.Casetypelist = data;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        [HttpGet]
        public JsonResult GetCasetype()
        {
            //db.Configuration.ProxyCreationEnabled = false;
            List<CaseType> lstcasetype = db.CaseTypes.ToList();

            //lstcasetype.Add(ct);
            var data = lstcasetype.Select(d => new
            {
                Value = d.CaseTypeId,
                Text = d.CaseType1
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DoctorId,DoctorName,EmailId,Address,PhoneNo,CASETYPEID,PERCENTANGE")] Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                Doctor dc = new Doctor();
                dc.DoctorId = doctor.DoctorId;
                dc.DoctorName = doctor.DoctorName;
                dc.EmailId = doctor.EmailId;
                dc.Address = doctor.Address;
                dc.PhoneNo = doctor.PhoneNo;
                dc.CASETYPEID = doctor.CASETYPEID;
                dc.PERCENTANGE = doctor.PERCENTANGE;
                db.Entry(dc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(doctor);
        }

        // GET: Doctors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Doctor doctor = db.Doctors.Find(id);
            List<DoctorCasetype> dct = db.DoctorCasetypes.ToList().Where(x => x.DoctorId == id).ToList();
            foreach(var data in dct)
            {
                db.DoctorCasetypes.Remove(data);
                db.SaveChanges();
            }
            db.Doctors.Remove(doctor);
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
