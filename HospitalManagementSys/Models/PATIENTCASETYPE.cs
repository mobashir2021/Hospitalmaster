//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HospitalManagementSys.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PATIENTCASETYPE
    {
        public int PATIENTCASETYPE1 { get; set; }
        public int PATIENTID { get; set; }
        public int DOCTORID { get; set; }
        public Nullable<int> CASETYPEID { get; set; }
        public Nullable<int> SUBCASETYPEID { get; set; }
        public string PERCENTAGE { get; set; }
    
        public virtual Patient Patient { get; set; }
    }
}
