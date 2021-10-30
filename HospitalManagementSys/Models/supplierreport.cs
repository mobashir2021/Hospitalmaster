using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementSys.Models
{
    public class supplierreport
    {
        public string Supplier { get; set; }
        public string MobileNo { get; set; }
        public string Items { get; set; }
        public string TotalAmount { get; set; }
        public string AmountPaid { get; set; }
        public string AmountDue { get; set; }
        public string StockDate { get; set; }
    }
}