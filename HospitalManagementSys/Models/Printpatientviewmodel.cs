using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementSys.Models
{
    public class Printpatientviewmodel : patientviewmodel
    {
        public string TotalAmount { get; set; }
        public string Discount { get; set; }
        public string AmountToBePaid { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public string DoctorEmailId { get; set; }
        public string DoctorPhoneNo { get; set; }
        public string Age { get; set; }
        public string Sex { get; set; }
        public string billno { get; set; }
        public List<reportcharges> lstpatientcasetype { get; set;}
        public string SODO { get; set; }
        public string amountinwords { get; set; }
        

    }
}