using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementSys.Models
{
    public class reportcharges
    {
        public int serialno { get; set; }
        public string Casetype { get; set; }
        public string Subcasetype { get; set; }
        public string TestCharges { get; set; } 
    }
}