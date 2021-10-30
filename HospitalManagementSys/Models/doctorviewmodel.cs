using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementSys.Models
{
    public class doctorviewmodel
    {
        public string Doctor { get; set; }
        public string CaseType { get; set; }
        public string Subcasetype { get; set; }
        public string TotalAmount { get; set; }
        public string Percentage { get; set; }
        public string DoctorCommission { get; set; }
        public string CaseDate { get; set; }
    }
}