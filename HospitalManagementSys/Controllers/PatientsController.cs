using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HospitalManagementSys.Models;
using System.Web.Services;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using PagedList;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

namespace HospitalManagementSys.Controllers
{
    public class PatientsController : Controller
    {
        private HMSEntities db = new HMSEntities();

        // GET: Patients
        public ActionResult Index(int? page)
        {
            var patients = db.Patients.Include(p => p.CaseType).Include(p => p.Doctor).OrderByDescending(x => x.CaseDate);
            if(Request.HttpMethod != "GET")
            {
                page = 1;
            }
            int pagesize = 7;
            int pagenumber = (page ?? 1);
            return View(patients.ToPagedList(pagenumber, pagesize));
        }

        [HttpPost]
        public ActionResult Import(HttpPostedFileBase excelfile)
        {
            if (excelfile.ContentLength == 0)
                return View("Index");
            DataTable dtExcel = ReadExcel(excelfile, "Sheet1");
            Dictionary<string, Doctor> lstdoctor = db.Doctors.ToDictionary(x => x.DoctorName, x => x);
            Dictionary<string, CaseType> lstcasetype = db.CaseTypes.ToDictionary(x => x.CaseType1, x => x);
            Dictionary<string, subcasetype> lstsubcasetype = db.subcasetypes.ToDictionary(x => x.Subcasetype1, x => x);
            List<Patient> lstpatient = new List<Patient>();
            foreach(DataRow dr in dtExcel.Rows)
            {
                string patientname = dr["Patientname"].ToString();
                string doctorname = dr["Doctorname"].ToString();
                string emailid = dr["Emailid"].ToString();
                string Address = dr["Address"].ToString();
                string Phoneno = dr["Phoneno"].ToString();
                string Mobileno = dr["Mobileno"].ToString();
                string Gender = dr["Gender"].ToString();
                string Age = dr["Age"].ToString();
                string Bloodgroup = dr["Bloodgroup"].ToString();
                string casetype = dr["Casetype"].ToString();
                string subcasetypedata = dr["Subcasetype"].ToString();
                string Percentage = dr["Percentage"].ToString();
                string Total = dr["TotalAmount"].ToString();
                string Doctorcommission = dr["Doctorcommission"].ToString();
                string Notes = dr["Notes"].ToString();
                string Remarks = dr["Remarks"].ToString();
                string Discount = dr["Discount"].ToString();
                string Balance = dr["Balance"].ToString();
                Doctor dc = new Doctor();
                CaseType ct = new CaseType();
                int subcasetypeid = 0;

                if (doctorname != null && lstdoctor.ContainsKey(doctorname))
                {
                    dc = lstdoctor[doctorname];
                }

                if (casetype != null && lstcasetype.ContainsKey(casetype))
                {
                    ct = lstcasetype[casetype];
                }

                if (subcasetypedata != null && lstsubcasetype.ContainsKey(subcasetypedata))
                {
                    subcasetypeid = lstsubcasetype[subcasetypedata].SubcasetypeId;
                }

                Patient pt = new Patient();
                pt.PatientName = patientname;
                pt.Address = Address;
                pt.Age = Age == null ? 0 : Convert.ToInt32(Age);
                pt.BloodGroup = Bloodgroup;
                pt.CaseDate = DateTime.Now.Date;
                pt.CaseTime = DateTime.Now.ToString("HH:mm");
                pt.CaseType = ct;
                pt.Doctor = dc;
                pt.Doctorid = dc.DoctorId;
                pt.Casetypeid = ct.CaseTypeId;
                pt.SubcaseTypeId = subcasetypeid;
                pt.Sex = Gender;
                pt.TotalAmount = Total == null ? 0 : Convert.ToInt32(Total);
                pt.DoctorCommission = Doctorcommission == null ? 0 : Convert.ToInt32(Doctorcommission);
                pt.Percentage = Percentage;
                pt.MobileNo = Mobileno;
                pt.PhoneNo = Phoneno;
                pt.EmailId = emailid;
                pt.Discount =  Discount == null ? 0 : Convert.ToInt32(Discount);
                pt.Balance = Balance == null ? 0 : Convert.ToInt32(Balance);
                pt.Notes = Notes;
                pt.Remarks = Remarks;
                lstpatient.Add(pt);
            }
            db.Patients.AddRange(lstpatient);
            db.SaveChanges();
            var patients = db.Patients.Include(p => p.CaseType).Include(p => p.Doctor).OrderByDescending(x => x.CaseDate);
            int? page = 1;
            int pagesize = 7;
            int pagenumber = (page ?? 1);
            return View("Index",patients.ToPagedList(pagenumber, pagesize));
        }

        public void ExportPDF(string Patient, string EmailId, string Address, string MobileNo, object Gender, string Age, string BloodGroup )
        {
            

        }

        public byte[] GetPDF(string pHTML)
        {
            byte[] bPDF = null;

            MemoryStream ms = new MemoryStream();
            TextReader txtReader = new StringReader(pHTML);

            // 1: create object of a itextsharp document class  
            Document doc = new Document(PageSize.A4, 25, 25, 25, 25);

            // 2: we create a itextsharp pdfwriter that listens to the document and directs a XML-stream to a file  
            PdfWriter oPdfWriter = PdfWriter.GetInstance(doc, ms);

            // 3: we create a worker parse the document  
            HTMLWorker htmlWorker = new HTMLWorker(doc);

            // 4: we open document and start the worker on the document  
            doc.Open();
            htmlWorker.StartDocument();


            // 5: parse the html into the document  
            htmlWorker.Parse(txtReader);

            // 6: close the document and the worker  
            htmlWorker.EndDocument();
            htmlWorker.Close();
            doc.Close();

            bPDF = ms.ToArray();

            return bPDF;
        }

        private DataTable ReadExcel(HttpPostedFileBase file, string sheetname)
        {
            var filename = Path.GetFileName(file.FileName);
            var data = Path.Combine(Server.MapPath("~/Content/Excel"), filename);
            file.SaveAs(data);

            System.Data.OleDb.OleDbConnection MyConnection;
            System.Data.DataSet DtSet = new DataSet();
            System.Data.OleDb.OleDbDataAdapter MyCommand;
            MyConnection = new System.Data.OleDb.OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + data + ";Extended Properties=\"Excel 12.0;HDR=YES;\"");
            MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [" + sheetname + "$]", MyConnection);
            MyCommand.TableMappings.Add("Table", "HospitalTable");
            DtSet = new System.Data.DataSet();
            MyCommand.Fill(DtSet);
            DataTable dttemp = DtSet.Tables[0];
            return dttemp;
        }

        // GET: Patients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }
        [HttpGet]
        public JsonResult GetCasetype(int Doctorid)
        {
            //db.Configuration.ProxyCreationEnabled = false;
            int tempcnt = -1;
            List<DoctorCasetype> temp = new List<DoctorCasetype>();
            if (db.DoctorCasetypes.ToList().Where(x => x.DoctorId == Doctorid).Count() > 0)
                temp = db.DoctorCasetypes.ToList().Where(x => x.DoctorId == Doctorid).ToList();
            else
                tempcnt = 0;
            List<CaseType> lstcasetype = new List<CaseType>();
            CaseType ct = new CaseType();
            if (tempcnt != 0)
            {
                lstcasetype = (from ctt in db.CaseTypes.ToList()
                               join dct in temp
                               on ctt.CaseTypeId equals dct.CaseTypeId
                               select ctt).ToList();
            }
            else
            {
                ct.CaseTypeId = 0;
                ct.CaseType1 = "Select Case Type";
                lstcasetype.Add(ct);
            }

            
            
            var data = lstcasetype.Select(d => new
            {
                Value = d.CaseTypeId,
                Text = d.CaseType1
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSubCasetype(int CaseTypeId)
        {
            //db.Configuration.ProxyCreationEnabled = false;
            CaseType temp = new CaseType();
            if (db.CaseTypes.ToList().Where(x => x.CaseTypeId == CaseTypeId).Count() > 0)
                temp = db.CaseTypes.ToList().Where(x => x.CaseTypeId == CaseTypeId).First();
            else
                temp.CaseTypeId = 0;
            subcasetype ct = new subcasetype();
            List<subcasetype> lstcasetype = new List<subcasetype>();
            if (temp.CaseTypeId != 0)
                lstcasetype = db.subcasetypes.Where(x => x.CaseTypeId == temp.CaseTypeId).ToList();
            else
            {
                ct.SubcasetypeId = 0;
                ct.Subcasetype1 = "Select Subcasetype";
                lstcasetype.Add(ct);
            }

            
            //lstcasetype.Add(ct);
            var data = lstcasetype.Select(d => new
            {
                Value = d.SubcasetypeId,
                Text = d.Subcasetype1
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPercentage(int Doctorid)
        {
            db.Configuration.ProxyCreationEnabled = false;
            Doctor temp = new Doctor();
            if (db.Doctors.ToList().Where(x => x.DoctorId == Doctorid).Count() > 0)
                temp = db.Doctors.ToList().Where(x => x.DoctorId == Doctorid).First();
            else
                temp.PERCENTANGE = "0";
           
            return Json(temp.PERCENTANGE, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPrice(int subcaseid)
        {
            db.Configuration.ProxyCreationEnabled = false;
            subcasetype temp = new subcasetype();
            if (db.subcasetypes.ToList().Where(x => x.SubcasetypeId == subcaseid).Count() > 0)
                temp = db.subcasetypes.ToList().Where(x => x.SubcasetypeId == subcaseid).First();
            else
                temp.Price = 0;

            return Json(temp.Price.ToString(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCasetypedata(int Doctorid)
        {
            var docs = db.DoctorCasetypes.ToList().Where(x => x.DoctorId == Doctorid).ToList();
            var cs = db.CaseTypes.ToList();
            //db.Configuration.ProxyCreationEnabled = false;
            List<CaseType> lstcasetype = (from dd in docs
                                          join cc in cs
                                          on dd.CaseTypeId equals cc.CaseTypeId
                                          select cc).ToList();

            //lstcasetype.Add(ct);
            var data = lstcasetype.Select(d => new
            {
                Value = d.CaseTypeId,
                Text = d.CaseType1
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSubCasetypedata(int Doctorid)
        {
            var docs = db.DoctorCasetypes.ToList().Where(x => x.DoctorId == Doctorid).ToList();
            var cs = db.CaseTypes.ToList();
            //db.Configuration.ProxyCreationEnabled = false;
            CaseType lstcasetype = (from dd in docs
                                    join cc in cs
                                    on dd.CaseTypeId equals cc.CaseTypeId
                                    select cc).First();
            var cst = db.subcasetypes.ToList();
            List<subcasetype> lstsubcasetype = cst.Where(x => x.CaseTypeId == lstcasetype.CaseTypeId).ToList();

            //lstcasetype.Add(ct);
            var data = lstsubcasetype.Select(d => new
            {
                Value = d.SubcasetypeId,
                Text = d.Subcasetype1
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        // GET: Patients/Create
        public ActionResult Create()
        {
            CaseType cs = new CaseType() { CaseTypeId = 0, CaseType1 = "Select Casetype" };
            Doctor dc = new Doctor() { DoctorId = 0, DoctorName = "Select Doctor" };
            subcasetype sc = new subcasetype() { Subcasetype1 = "", SubcasetypeId = 0 };
            List<subcasetype> lstsubcasetype = new List<subcasetype>();
            lstsubcasetype.Add(sc);
            List<CaseType> lstcasetype = new List<CaseType>();
            List<Doctor> lstdoctor = new List<Doctor>();
            lstcasetype.Add(cs);
            lstdoctor.Add(dc);
            //lstcasetype.AddRange(db.CaseTypes.ToList());
            lstdoctor.AddRange(db.Doctors.ToList());
            ViewBag.Casetypeid = new SelectList(lstcasetype, "CaseTypeId", "CaseType1");
            ViewBag.Doctorid = new SelectList(lstdoctor, "DoctorId", "DoctorName");
            SelectListItem sl3 = new SelectListItem();
            sl3.Value = "Select Gender"; sl3.Text = "Select Gender";
            SelectListItem sl1 = new SelectListItem();
            sl1.Value = "Male"; sl1.Text = "Male";
            SelectListItem sl2 = new SelectListItem();
            sl2.Value = "Female"; sl2.Text = "Female";
            List<SelectListItem> lstselectlistitemsex = new List<SelectListItem>();
            //lstselectlistitemsex.Add(sl3);
            lstselectlistitemsex.Add(sl1); lstselectlistitemsex.Add(sl2);
            ViewBag.Sex = new SelectList(lstselectlistitemsex, "Value", "Text");
            ViewBag.SubCasetypeid = new SelectList(lstsubcasetype, "SubcasetypeId", "Subcasetype1");
            return View();
        }
        

        // POST: Patients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PatientId,PatientName,EmailId,Address,PhoneNo,MobileNo,Sex,Age,BloodGroup,RegdDate,Doctorid,Casetypeid,SubcaseTypeId,TotalAmount,Remarks,Notes,Balance,Percentage,DoctorCommission,Discount,SODO,BillNo")] Patient patient,
            string Print, object casetypesdata, object subcasetypesdata)
        {


            int totalamount = 0;
            Doctor dd = db.Doctors.ToList().Where(x => x.DoctorId == patient.Doctorid).First();
            List<reportcharges> lstreportcharges = new List<reportcharges>();
            patient.Percentage = dd.PERCENTANGE;
            patient.CaseDate = DateTime.Now.Date;
            patient.CaseTime = DateTime.Now.ToString("HH:mm");
            patient.SubcaseTypeId = patient.SubcaseTypeId.Value;
            int disvalue = patient.Discount.HasValue ? patient.Discount.Value : 0;
            patient.Balance = patient.Balance.HasValue ? (patient.TotalAmount.HasValue ? (patient.TotalAmount.Value - disvalue) : 0) : 0;
            patient.Discount = disvalue;
            if (!string.IsNullOrEmpty(patient.Percentage) && patient.Percentage != "0" && patient.Percentage != "")
                patient.DoctorCommission = patient.TotalAmount.HasValue ? (patient.TotalAmount.Value / Convert.ToInt32(patient.Percentage)) : 0;
            else
                patient.DoctorCommission = 0;
            patient.RegdDate = DateTime.Now.Date;
            patient.TotalAmount = patient.TotalAmount.HasValue ? patient.TotalAmount.Value : 0;
            db.Patients.Add(patient);
            db.SaveChanges();
            int patid = patient.PatientId;

            PATIENTCASETYPE pcd = new PATIENTCASETYPE();
            pcd.DOCTORID = dd.DoctorId;
            pcd.PATIENTID = patid;
            pcd.CASETYPEID = patient.Casetypeid;
            pcd.SUBCASETYPEID = patient.SubcaseTypeId.HasValue ? patient.SubcaseTypeId.Value : 0;
            string percentdataold = "0";
            var newddd = db.DoctorCasetypes.ToList().Where(x => x.DoctorId == dd.DoctorId && x.CaseTypeId == patient.Casetypeid);
            foreach(var neddata in newddd)
            {
                percentdataold = neddata.percentage;
            }
            pcd.PERCENTAGE = percentdataold;
            db.PATIENTCASETYPES.Add(pcd);
            db.SaveChanges();

            reportcharges rc = new reportcharges();
            rc.serialno = 1;
            foreach (var dd1 in db.CaseTypes.ToList().Where(x => x.CaseTypeId == pcd.CASETYPEID))
                rc.Casetype = dd1.CaseType1;
            foreach (var dd1 in db.subcasetypes.ToList().Where(x => x.SubcasetypeId == pcd.SUBCASETYPEID.Value))
            {
                rc.Subcasetype = dd1.Subcasetype1;
                rc.TestCharges = dd1.Price.HasValue ? dd1.Price.Value.ToString() : "0";
                totalamount = Convert.ToInt32(rc.TestCharges);
            }
            lstreportcharges.Add(rc);


            if (casetypesdata.ToString() != "System.Object" && subcasetypesdata.ToString() != "System.Object")
            {
                string[] csdt = (string[])casetypesdata;
                string[] scsdt = (string[])subcasetypesdata;
                for (int ij = 0; ij < csdt.Length; ij++)
                {
                    var dct = db.DoctorCasetypes.ToList().Where(x => x.DoctorId == dd.DoctorId
                    && x.CaseTypeId == Convert.ToInt32(csdt[ij]));
                    string percent = "0";
                    foreach (var newdata in dct)
                    {
                        percent = newdata.percentage;
                    }
                    PATIENTCASETYPE pc = new PATIENTCASETYPE();
                    pc.DOCTORID = dd.DoctorId;
                    pc.CASETYPEID = (csdt[ij] == "0" || string.IsNullOrEmpty(csdt[ij])) ? 0 : Convert.ToInt32(csdt[ij]);
                    pc.SUBCASETYPEID = (scsdt[ij] == "0" || string.IsNullOrEmpty(scsdt[ij])) ? 0 : Convert.ToInt32(scsdt[ij]);
                    pc.PERCENTAGE = percent;
                    pc.PATIENTID = patid;
                    db.PATIENTCASETYPES.Add(pc);
                    db.SaveChanges();

                    reportcharges rc1 = new reportcharges();
                    rc1.serialno = ij + 1 + rc.serialno;
                    foreach (var dd1 in db.CaseTypes.ToList().Where(x => x.CaseTypeId == pc.CASETYPEID.Value))
                        rc1.Casetype = dd1.CaseType1;
                    foreach (var dd1 in db.subcasetypes.ToList().Where(x => x.SubcasetypeId == pc.SUBCASETYPEID.Value))
                    {
                        rc1.Subcasetype = dd1.Subcasetype1;
                        rc1.TestCharges = dd1.Price.HasValue ? dd1.Price.Value.ToString() : "0";
                        totalamount = totalamount + Convert.ToInt32(rc1.TestCharges);
                    }
                    lstreportcharges.Add(rc1);
                }
            }

            //int i = 0;
            //int length = lstreportcharges.Count;
            //if(length == 1)
            //{

            //}
            //foreach(var value in lstreportcharges)
            //{
            //    if(i == 0)
            //        value.TestCharges = "Total Amount : " + totalamount.ToString();
            //    if(i )
            //    i++;
            //}

            if (Print == null || Print != "Create and Print")
                return RedirectToAction("Index");
        

            ViewBag.Casetypeid = new SelectList(db.CaseTypes, "CaseTypeId", "CaseType1", patient.Casetypeid);
            ViewBag.Doctorid = new SelectList(db.Doctors, "DoctorId", "DoctorName", patient.Doctorid);
            SelectListItem sl3 = new SelectListItem();
            sl3.Value = "Select Gender"; sl3.Text = "Select Gender";
            SelectListItem sl1 = new SelectListItem();
            sl1.Value = "Male"; sl1.Text = "Male";
            SelectListItem sl2 = new SelectListItem();
            sl2.Value = "Female"; sl2.Text = "Female";
            List<SelectListItem> lstselectlistitemsex = new List<SelectListItem>();
            //lstselectlistitemsex.Add(sl3);
            lstselectlistitemsex.Add(sl1); lstselectlistitemsex.Add(sl2);
            ViewBag.Sex = new SelectList(lstselectlistitemsex, "Value", "Text");
            ViewBag.SubCasetypeid = new SelectList(db.subcasetypes, "SubcasetypeId", "Subcasetype1");
            if (Print != null && Print == "Create and Print")
            {
                Printpatientviewmodel pv = new Printpatientviewmodel();
                pv.Age = patient.Age.ToString();
                pv.Sex = patient.Sex.ToString();
                pv.SODO = string.IsNullOrEmpty(patient.SODO) ? "" : patient.SODO;
                pv.Patient = patient.PatientName;
                pv.CaseType = db.CaseTypes.ToList().Where(x => x.CaseTypeId == patient.Casetypeid).First().CaseType1;
                pv.SubCaseType = db.subcasetypes.ToList().Where(x => x.SubcasetypeId == patient.SubcaseTypeId.Value).First().Subcasetype1;
                Doctor tempdoc = db.Doctors.ToList().Where(x => x.DoctorId == patient.Doctorid).First();
                pv.Doctor = tempdoc.DoctorName;
                pv.DoctorEmailId = tempdoc.EmailId;
                pv.DoctorPhoneNo = tempdoc.PhoneNo;
                pv.CaseDate = DateTime.Now.Date.ToString("dd-MMM-yyyy");
                pv.Discount = patient.Discount.HasValue ? patient.Discount.Value.ToString() : "";
                pv.TotalAmount = patient.TotalAmount.HasValue ? patient.TotalAmount.Value.ToString() : "";
                pv.AmountToBePaid = patient.Balance.HasValue ? patient.Balance.Value.ToString() : "";
                pv.Address = patient.Address;
                pv.MobileNo = patient.MobileNo;
                pv.lstpatientcasetype = lstreportcharges;
                //pv.billno = "BL-" + patid.ToString();
                pv.AmountToBePaid = (totalamount - (patient.Discount.HasValue ? patient.Discount.Value : 0)).ToString();
                pv.amountinwords = ConvertNumbertoWords(Convert.ToInt64(pv.AmountToBePaid));
                pv.billno = patient.BillNo;
                return View("PrintPatient", pv);
            }
            else
                return View(patient);
        }

        public string ConvertNumbertoWords(long number)
        {
            if (number == 0) return "ZERO";
            if (number < 0) return "minus " + ConvertNumbertoWords(Math.Abs(number));
            string words = "";
            if ((number / 1000000) > 0)
            {
                words += ConvertNumbertoWords(number / 100000) + " LAKHS ";
                number %= 1000000;
            }
            if ((number / 1000) > 0)
            {
                words += ConvertNumbertoWords(number / 1000) + " THOUSAND ";
                number %= 1000;
            }
            if ((number / 100) > 0)
            {
                words += ConvertNumbertoWords(number / 100) + " HUNDRED ";
                number %= 100;
            }
            //if ((number / 10) > 0)  
            //{  
            // words += ConvertNumbertoWords(number / 10) + " RUPEES ";  
            // number %= 10;  
            //}  
            if (number > 0)
            {
                if (words != "") words += "AND ";
                var unitsMap = new[]
                {
                    "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN"
                };
                        var tensMap = new[]
                        {
                    "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY"
                };
                if (number < 20) words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0) words += " " + unitsMap[number % 10];
                }
            }
            return words;
        }

        public ActionResult PrintdataPatient(int? id)
        {
            int totalamount = 0;
            int patcurrid = id.HasValue ? id.Value : 0;
            Patient patient = db.Patients.ToList().Where(x => x.PatientId == patcurrid).First();
            Doctor dd = db.Doctors.ToList().Where(x => x.DoctorId == patient.Doctorid).First();
            List<reportcharges> lstreportcharges = new List<reportcharges>();

            var reportdata = db.PATIENTCASETYPES.ToList().Where(x => x.PATIENTID == patcurrid).ToList();
            int ij = 0;
            foreach (var data in reportdata)
            {
                ij++;
                reportcharges rc1 = new reportcharges();
                rc1.serialno = ij;
                foreach (var dd1 in db.CaseTypes.ToList().Where(x => x.CaseTypeId == data.CASETYPEID.Value))
                    rc1.Casetype = dd1.CaseType1;
                foreach (var dd1 in db.subcasetypes.ToList().Where(x => x.SubcasetypeId == data.SUBCASETYPEID.Value))
                {
                    rc1.Subcasetype = dd1.Subcasetype1;
                    rc1.TestCharges = dd1.Price.HasValue ? dd1.Price.Value.ToString() : "0";
                    totalamount = totalamount + Convert.ToInt32(rc1.TestCharges);
                }
                lstreportcharges.Add(rc1);
            }
            

            
            Printpatientviewmodel pv = new Printpatientviewmodel();
            pv.Age = patient.Age.ToString();
            pv.Sex = patient.Sex.ToString();
            pv.SODO = string.IsNullOrEmpty(patient.SODO) ? "" : patient.SODO;
            pv.Patient = patient.PatientName;
            pv.CaseType = db.CaseTypes.ToList().Where(x => x.CaseTypeId == patient.Casetypeid).First().CaseType1;
            pv.SubCaseType = db.subcasetypes.ToList().Where(x => x.SubcasetypeId == patient.SubcaseTypeId.Value).First().Subcasetype1;
            Doctor tempdoc = db.Doctors.ToList().Where(x => x.DoctorId == patient.Doctorid).First();
            pv.Doctor = tempdoc.DoctorName;
            pv.DoctorEmailId = tempdoc.EmailId;
            pv.DoctorPhoneNo = tempdoc.PhoneNo;
            pv.CaseDate = patient.CaseDate.Value.Date.ToString("dd-MMM-yyyy");
            pv.Discount = patient.Discount.HasValue ? patient.Discount.Value.ToString() : "";
            pv.TotalAmount = patient.TotalAmount.HasValue ? patient.TotalAmount.Value.ToString() : "";
            pv.AmountToBePaid = patient.Balance.HasValue ? patient.Balance.Value.ToString() : "";
            pv.Address = patient.Address;
            pv.MobileNo = patient.MobileNo;
            pv.lstpatientcasetype = lstreportcharges;
            //pv.billno = "BL-" + patcurrid.ToString();
            pv.AmountToBePaid = (totalamount - (patient.Discount.HasValue ? patient.Discount.Value : 0)).ToString();
            pv.amountinwords = ConvertNumbertoWords(Convert.ToInt64(pv.AmountToBePaid));
            pv.billno = patient.BillNo;
            return View("PrintPatient", pv);
            
        }

        // GET: Patients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            ViewBag.Casetypeid = new SelectList(db.CaseTypes, "CaseTypeId", "CaseType1", patient.Casetypeid);
            ViewBag.Doctorid = new SelectList(db.Doctors, "DoctorId", "DoctorName", patient.Doctorid);
            SelectListItem sl3 = new SelectListItem();
            sl3.Value = "Select Gender"; sl3.Text = "Select Gender";
            SelectListItem sl1 = new SelectListItem();
            sl1.Value = "Male"; sl1.Text = "Male";
            SelectListItem sl2 = new SelectListItem();
            sl2.Value = "Female"; sl2.Text = "Female";
            List<SelectListItem> lstselectlistitemsex = new List<SelectListItem>();
            //lstselectlistitemsex.Add(sl3);
            lstselectlistitemsex.Add(sl1); lstselectlistitemsex.Add(sl2);
            ViewBag.Sex = new SelectList(lstselectlistitemsex, "Value", "Text");
            ViewBag.SubCasetypeid = new SelectList(db.subcasetypes, "SubcasetypeId", "Subcasetype1");
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PatientId,PatientName,EmailId,Address,PhoneNo,MobileNo,Sex,Age,BloodGroup,RegdDate,Doctorid,Casetypeid,SubcaseTypeId,TotalAmount,Remarks,Notes,Balance,Percentage,DoctorCommission,Discount,SODO")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Casetypeid = new SelectList(db.CaseTypes, "CaseTypeId", "CaseType1", patient.Casetypeid);
            ViewBag.Doctorid = new SelectList(db.Doctors, "DoctorId", "DoctorName", patient.Doctorid);
            return View(patient);
        }

        // GET: Patients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Patient patient = db.Patients.Find(id);
            var patientcs = db.PATIENTCASETYPES.ToList().Where(x => x.PATIENTID == id).ToList();
            foreach(var pat in patientcs)
            {
                db.PATIENTCASETYPES.Remove(pat);
                db.SaveChanges();
            }

            db.Patients.Remove(patient);
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
