//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WorkZen.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Task
    {
        public int id { get; set; }
        public System.DateTime taskDate { get; set; }
        public Nullable<int> employeeId { get; set; }
        public string taskName { get; set; }
        public string taskDescription { get; set; }
        public Nullable<int> approverId { get; set; }
        public Nullable<int> approvedOrRejectedBy { get; set; }
        public string status { get; set; }
        public System.DateTime createdOn { get; set; }
        public System.DateTime modifiedOn { get; set; }
    
        public virtual Employee Employee { get; set; }
        public virtual Employee Employee1 { get; set; }
        public virtual Employee Employee2 { get; set; }
    }
}
