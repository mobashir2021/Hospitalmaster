using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HospitalManagementSys.Models;

namespace HospitalManagementSys.Controllers
{
    public class ReportsController : Controller
    {
        HMSEntities db = new HMSEntities();
        // GET: Reports
        public ActionResult Index()
        {
            List<patientviewmodel> lst = new List<patientviewmodel>();
            List<Patient> pt = db.Patients.ToList();

            foreach(var patientd in pt)
            {
                Doctor d = db.Doctors.ToList().Where(x => x.DoctorId == patientd.Doctorid).First();
                CaseType c = db.CaseTypes.ToList().Where(x => x.CaseTypeId == patientd.Casetypeid).First();
                subcasetype sc = db.subcasetypes.ToList().Where(x => x.SubcasetypeId == patientd.SubcaseTypeId).First();
                patientviewmodel pv = new patientviewmodel();
                //pv.ct = c;
                //pv.dc = d;
                //pv.pt = patientd;
                //pv.sct = sc;
                lst.Add(pv);
            }
            if (HospitalManagementSys.Models.RolesData.RolesValue == "receiptionist")
            {
                return View("IndexRecep");
            }
            else
                return View("Index");

            
        }

        public ActionResult DoctorReport()
        {
            List<Doctor> lstdoctor = db.Doctors.ToList();
            ViewBag.Doctorid = new SelectList(lstdoctor, "DoctorId", "DoctorName");
            if(HospitalManagementSys.Models.RolesData.RolesValue == "receiptionist")
            {
                return View("DoctorReportRecep");
            }
            else
                return View("DoctorReport");
        }

        public JsonResult GetPatientReport()
        {
            List<patientviewmodel> lst = new List<patientviewmodel>();
            List<Patient> pt = db.Patients.ToList();

            foreach (var patientd in pt)
            {
                Doctor d = db.Doctors.ToList().Where(x => x.DoctorId == patientd.Doctorid).First();
                CaseType c = db.CaseTypes.ToList().Where(x => x.CaseTypeId == patientd.Casetypeid).First();
                subcasetype sc = db.subcasetypes.ToList().Where(x => x.SubcasetypeId == patientd.SubcaseTypeId).First();
                patientviewmodel pv = new patientviewmodel();
                pv.Patient = patientd.PatientName;
                pv.Doctor = d.DoctorName;
                pv.CaseDate = patientd.CaseDate.Value.ToString("dd-MMM-yyyy");
                pv.DoctorCommission = patientd.DoctorCommission.ToString();
                pv.CaseType = c.CaseType1;
                pv.SubCaseType = sc.Subcasetype1;

                lst.Add(pv);
            }
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPatientReportRecep()
        {
            List<patientviewmodel> lst = new List<patientviewmodel>();
            List<Patient> pt = db.Patients.ToList();

            foreach (var patientd in pt)
            {
                Doctor d = db.Doctors.ToList().Where(x => x.DoctorId == patientd.Doctorid).First();
                CaseType c = db.CaseTypes.ToList().Where(x => x.CaseTypeId == patientd.Casetypeid).First();
                subcasetype sc = db.subcasetypes.ToList().Where(x => x.SubcasetypeId == patientd.SubcaseTypeId).First();
                patientviewmodel pv = new patientviewmodel();
                pv.Patient = patientd.PatientName;
                pv.Doctor = d.DoctorName;
                pv.CaseDate = patientd.CaseDate.Value.ToString("dd-MMM-yyyy");
                
                pv.CaseType = c.CaseType1;
                pv.SubCaseType = sc.Subcasetype1;

                lst.Add(pv);
            }
            return Json(lst, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetDoctorReport(int doctorid)
        {
            List<doctorviewmodel> lst = new List<doctorviewmodel>();
            List<Patient> pt = db.Patients.ToList().Where(x => x.Doctorid == doctorid).ToList();

            foreach (var patientd in pt)
            {
                Doctor d = db.Doctors.ToList().Where(x => x.DoctorId == patientd.Doctorid).First();
                CaseType c = db.CaseTypes.ToList().Where(x => x.CaseTypeId == patientd.Casetypeid).First();
                subcasetype sc = db.subcasetypes.ToList().Where(x => x.SubcasetypeId == patientd.SubcaseTypeId).First();
                doctorviewmodel pv = new doctorviewmodel();
                pv.Percentage = patientd.Percentage;
                pv.Doctor = d.DoctorName;
                pv.CaseDate = patientd.CaseDate.Value.ToString("dd-MMM-yyyy");
                pv.DoctorCommission = patientd.DoctorCommission.ToString();
                pv.CaseType = c.CaseType1;
                pv.Subcasetype = sc.Subcasetype1;
                pv.TotalAmount = patientd.TotalAmount.Value.ToString();
                
                lst.Add(pv);
            }
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDoctorReportRecep(int doctorid)
        {
            List<doctorviewmodel> lst = new List<doctorviewmodel>();
            List<Patient> pt = db.Patients.ToList().Where(x => x.Doctorid == doctorid).ToList();

            foreach (var patientd in pt)
            {
                Doctor d = db.Doctors.ToList().Where(x => x.DoctorId == patientd.Doctorid).First();
                CaseType c = db.CaseTypes.ToList().Where(x => x.CaseTypeId == patientd.Casetypeid).First();
                subcasetype sc = db.subcasetypes.ToList().Where(x => x.SubcasetypeId == patientd.SubcaseTypeId).First();
                doctorviewmodel pv = new doctorviewmodel();
                //pv.Percentage = patientd.Percentage;
                pv.Doctor = d.DoctorName;
                pv.CaseDate = patientd.CaseDate.Value.ToString("dd-MMM-yyyy");
                //pv.DoctorCommission = patientd.DoctorCommission.ToString();
                pv.CaseType = c.CaseType1;
                pv.Subcasetype = sc.Subcasetype1;
                pv.TotalAmount = patientd.TotalAmount.Value.ToString();

                lst.Add(pv);
            }
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
    }
}