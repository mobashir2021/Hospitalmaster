﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class HMSEntities : DbContext
    {
        public HMSEntities()
            : base("name=HMSEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ADDSTOCK> ADDSTOCKS { get; set; }
        public virtual DbSet<Case> Cases { get; set; }
        public virtual DbSet<CaseType> CaseTypes { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<DoctorCasetype> DoctorCasetypes { get; set; }
        public virtual DbSet<EXPENS> EXPENSES { get; set; }
        public virtual DbSet<ITEM> ITEMS { get; set; }
        public virtual DbSet<LoginUser> LoginUsers { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<PATIENTCASETYPE> PATIENTCASETYPES { get; set; }
        public virtual DbSet<Price> Prices { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<subcasetype> subcasetypes { get; set; }
        public virtual DbSet<SUPPLIER> SUPPLIERs { get; set; }
    }
}
