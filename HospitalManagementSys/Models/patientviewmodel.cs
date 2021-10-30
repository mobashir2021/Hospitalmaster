using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementSys.Models
{
    public class patientviewmodel
    {
        public string Patient { get; set; }
        public string Doctor { get; set; }
        public string CaseDate { get; set; }
        public string DoctorCommission { get; set; }
        public string CaseType { get; set; }
        public string SubCaseType { get; set; } 
    }
}