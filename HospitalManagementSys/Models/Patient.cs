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
    
    public partial class Patient
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Patient()
        {
            this.PATIENTCASETYPES = new HashSet<PATIENTCASETYPE>();
        }
    
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string EmailId { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public string Sex { get; set; }
        public int Age { get; set; }
        public string BloodGroup { get; set; }
        public Nullable<System.DateTime> RegdDate { get; set; }
        public int Doctorid { get; set; }
        public int Casetypeid { get; set; }
        public Nullable<System.DateTime> CaseDate { get; set; }
        public string CaseTime { get; set; }
        public string Percentage { get; set; }
        public Nullable<int> SubcaseTypeId { get; set; }
        public Nullable<int> TotalAmount { get; set; }
        public Nullable<int> DoctorCommission { get; set; }
        public string Remarks { get; set; }
        public string Notes { get; set; }
        public Nullable<int> Discount { get; set; }
        public Nullable<int> Balance { get; set; }
        public string SODO { get; set; }
        public string BillNo { get; set; }
    
        public virtual CaseType CaseType { get; set; }
        public virtual Doctor Doctor { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PATIENTCASETYPE> PATIENTCASETYPES { get; set; }
    }
}
