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
    
    public partial class getTimeSheetEmailDetails_Result
    {
        public int TimesheetId { get; set; }
        public string TimesheetMonth { get; set; }
        public string TimesheetMonthforMail { get; set; }
        public int Usr_UserID { get; set; }
        public string Employee_Name { get; set; }
        public int ProjectID { get; set; }
        public string Proj_ClientName { get; set; }
        public string ProjectName { get; set; }
        public Nullable<System.DateTime> SubmittedDate { get; set; }
        public Nullable<decimal> TotalWorkingHours { get; set; }
    }
}
