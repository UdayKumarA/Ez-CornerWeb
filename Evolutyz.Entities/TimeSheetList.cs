using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace evolCorner.Models
{
    //public class listtimesheetdetailsModel
    //{
    //}

    public class TotalTimeSheetTimeDetails
    {
        public timesheet timesheets { get; set; }
        public List<listtimesheetdetail> listtimesheetdetails { get; set; }
        public StatusDetails StatusDetail { get; set; }

    }

    public class timesheet
    {
        public string UserName { get; set; }
        public int TimesheetID { get; set; }
        public int UserID { get; set; }
        public int EmpUsrID { get; set; }
        public string TaskDate { get; set; }
        public string Comments { get; set; }
        public int ProjectID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string TaskId { get; set; }
        public int FlagEmailStatus { get; set; }
        public int Transoutput { get; set; }
        public string ManagerId { get; set; }
        public string ActionType { get; set; }
        public string SubmittedType { get; set; }
        public string SubmittedFlag { get; set; }
        public string ManagerEmail1 { get; set; }
        public string ManagerEmail2 { get; set; }
        public string AccManagerEmail { get; set; }
        public string AccManagerID { get; set; }
        public string UserEmailId { get; set; }
        public Nullable<System.DateTime> SubmittedDate { get; set; }
        public string L1ApproverStatus { get; set; }
        public string L2ApproverStatus { get; set; }
        public string statusmsg { get; set; }
        public Nullable<System.DateTime> L1_ApproverDate { get; set; }
        public Nullable<System.DateTime> L2_ApproverDate { get; set; }
        public Nullable<System.DateTime> L1_RejectedDate { get; set; }
        public Nullable<System.DateTime> L2_RejectedDate { get; set; }


    }
    public class listtimesheetdetail
    {

        public int projectid { get; set; }
        public int taskid { get; set; }
        public int hoursWorked { get; set; }
        public string taskDate { get; set; }
        public string Message { get; set; }
        public string MessageCode { get; set; }
        public string errormessage { get; set; }

        // public DateTime CreatedDate { get; set; }

    }


    public class StatusDetails
    {
        public string Message { get; set; }
        public string StatusCode { get; set; }
        public string errormessage { get; set; }

        // public DateTime CreatedDate { get; set; }

    }

}