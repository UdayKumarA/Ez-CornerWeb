//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Evolutyz.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class TIMESHEET
    {
        public int TimesheetID { get; set; }
        public int UserID { get; set; }
        public Nullable<System.DateTime> TaskDate { get; set; }
        public System.DateTime TimesheetMonth { get; set; }
        public string Comments { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string TaskId { get; set; }
        public Nullable<System.DateTime> SubmittedDate { get; set; }
        public Nullable<bool> SubmittedType { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string L1_Manager { get; set; }
        public Nullable<int> L1_ApproverStatus { get; set; }
        public Nullable<System.DateTime> L1_ApproverDate { get; set; }
        public Nullable<System.DateTime> L1_RejectedDate { get; set; }
        public string L2_Manager { get; set; }
        public Nullable<int> L2_ApproverStatus { get; set; }
        public Nullable<System.DateTime> L2_ApproverDate { get; set; }
        public Nullable<System.DateTime> L2_RejectedDate { get; set; }
        public Nullable<int> DeviceTypeId { get; set; }
    }
}
