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
    
    public partial class AccountSpecificTask
    {
        public int Acc_SpecificTaskId { get; set; }
        public Nullable<int> AccountID { get; set; }
        public string Acc_SpecificTaskName { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<bool> isDeleted { get; set; }
        public Nullable<bool> StatusId { get; set; }
        public Nullable<int> tsk_TaskID { get; set; }
    }
}
