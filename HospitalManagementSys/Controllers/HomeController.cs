using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HospitalManagementSys.Models;

namespace HospitalManagementSys.Controllers
{
    public class HomeController : Controller
    {
        HospitalManagementSys.Models.HMSEntities db = new HMSEntities();
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.TotalPatients = db.Patients.ToList().Count.ToString();
            ViewBag.Doctors = db.Doctors.ToList().Count.ToString();
            var temp = db.Patients.ToList();
            ViewBag.TotalPatientsToday = temp.Where(x => x.CaseDate.Value.Date.Year == DateTime.Now.Date.Year &&
                x.CaseDate.Value.Date.Month == DateTime.Now.Date.Month && x.CaseDate.Value.Day == DateTime.Now.Day).Count();
            var newtemp = temp.Where(x => x.CaseDate.Value.Date.Year == DateTime.Now.Date.Year &&
                x.CaseDate.Value.Date.Month == DateTime.Now.Date.Month && x.CaseDate.Value.Day == DateTime.Now.Day).
                ToList();
            int allamount = 0;
            foreach(var dd in newtemp)
            {
                if(dd.Balance.HasValue)
                    allamount = allamount + dd.Balance.Value;
            }
            ViewBag.TotalAmountToday = allamount;
            int fullamount = 0;
            foreach(var dd in temp)
            {
                if(dd.Balance.HasValue)
                    fullamount = fullamount + dd.Balance.Value;
            }
            ViewBag.FullAmount = fullamount;
            return View();
        }

        public ActionResult Receiptionist()
        {
            ViewBag.TotalPatients = db.Patients.ToList().Count.ToString();
            ViewBag.Doctors = db.Doctors.ToList().Count.ToString();
            var temp = db.Patients.ToList();
            ViewBag.TotalPatientsToday = temp.Where(x => x.CaseDate.Value.Date.Year == DateTime.Now.Date.Year &&
                x.CaseDate.Value.Date.Month == DateTime.Now.Date.Month && x.CaseDate.Value.Day == DateTime.Now.Day).Count();
            var newtemp = temp.Where(x => x.CaseDate.Value.Date.Year == DateTime.Now.Date.Year &&
                x.CaseDate.Value.Date.Month == DateTime.Now.Date.Month && x.CaseDate.Value.Day == DateTime.Now.Day).
                ToList();
            int allamount = 0;
            foreach (var dd in newtemp)
            {
                if (dd.Balance.HasValue)
                    allamount = allamount + dd.Balance.Value;
            }
            ViewBag.TotalAmountToday = allamount;
            int fullamount = 0;
            foreach (var dd in temp)
            {
                if (dd.Balance.HasValue)
                    fullamount = fullamount + dd.Balance.Value;
            }
            ViewBag.FullAmount = fullamount;
            return View();
        }
    }
}