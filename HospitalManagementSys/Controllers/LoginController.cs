using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HospitalManagementSys.Models;

namespace HospitalManagementSys.Controllers
{
    public class LoginController : Controller
    {
        HMSEntities db = new HMSEntities();

        // GET: Login
        public ActionResult Index()
        {
            RolesData.RolesValue = "";
            //ViewBag.Rolesdata = new SelectList(db.Roles.ToList(), "RoleId", "RoleName");
            return View();
        }

        public ActionResult Logout()
        {
            RolesData.RolesValue = "";
            return RedirectToAction("Index");
        }

        public ActionResult Loginnew()
        {
            RolesData.RolesValue = "";
            //ViewBag.Rolesdata = new SelectList(db.Roles.ToList(), "RoleId", "RoleName");
            return View();
        }

        public ActionResult Loginpost(string username, string password)
        {
            //int roleid = Convert.ToInt32(((string[])Roles)[0]);
            var tolistlogin = db.LoginUsers.ToList();
            var rolesdata = db.Roles.ToList();
            int cnt = tolistlogin.Where(x => x.UserName == username && x.Password == password).Count();
            int temproleid = 0;
            string rolesvalue = "";
            if (cnt > 0)
            {
                temproleid = tolistlogin.Where(x => x.UserName == username && x.Password == password).First().RoleId;
                rolesvalue = rolesdata.Where(x => x.RoleId == temproleid).First().RoleName;
            }

            if (cnt > 0)
            {
                if (rolesvalue.ToLower() == "admin")
                {
                    RolesData.RolesValue = "admin";
                    return RedirectToAction("Index", "Home");
                }
                else if (rolesvalue.ToLower() == "receiptionist")
                {
                    RolesData.RolesValue = "receiptionist";
                    return RedirectToAction("Receiptionist", "Home");
                }
                else
                {
                    RolesData.RolesValue = "";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                RolesData.RolesValue = "";
                return RedirectToAction("Index");
            }

            
        }
    }
}