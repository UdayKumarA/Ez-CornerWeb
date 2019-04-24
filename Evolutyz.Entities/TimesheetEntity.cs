using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolutyz.Entities
{

    public class AllUsersTimeSheetDetails
    {
        public string Month_Year { get; set; }
        public string CompanyBillingHours { get; set; }
        public string ResourceWorkingHours { get; set; }
        public int TimesheetID { get; set; }
        public string TimesheetStatus { get; set; }
    }

    public class ManagerTimesheet
    {
        public int Userid { get; set; }
        public string Username { get; set; }
        public int TimesheetID { get; set; }
        public string Month_Year { get; set; }
        public int CompanyBillMyWorkingHours { get; set; }
        public int CompanyBillingHours { get; set; }
        public string TimesheetStatus { get; set; }

    }

    public class TimesheetEntity
    {
        public string AccountName { get; set; }
        public string RoleName { get; set; }
        public string TaskName { get; set; }
        public string HoursWorked { get; set; }
        public string TimesheetMonth { get; set; }
        public int TimesheetID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public DateTime TaskDate { get; set; }
        public string strTaskDate { get; set; }
        public string Comments { get; set; }
        public List<TaskDetailEntity> taskDetails { get; set; }
    }


    public class ManagerDetailsEntity
    {
        public List<UserTimesheetsEntity> mytimesheets { get; set; }
        public List<ManagerTimesheetsforApprovalsEntity> timesheetsforapproval { get; set; }
    }


    public class TotalRows
    {
        public int TotalCountforApproval { get; set; }
    }
    public class TimesheetDetails
    {

        public string userid { get; set; }
        public int startposition { get; set; }
        public int endPosition { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
    }

    public class UserTimesheetsEntity
    {
        public string ProjectName { get; set; }
        public string Month_Year { get; set; }
        public string CompanyBillingHours { get; set; }
        public string ResourceWorkingHours { get; set; }
        public int TimesheetID { get; set; }
        public string TimesheetApprovalStatus { get; set; }
        public string Usr_Username { get; set; }
        public string ManagerApprovalStatus { get; set; }

    }

    public class ManagerTimesheetsforApprovalsEntity
    {
        public string ProjectName { get; set; }
        //public int Userid { get; set; }
        //public string Username { get; set; }
        public int TimesheetID { get; set; }
        public string Month_Year { get; set; }
        public int ResourceWorkingHours { get; set; }
        public int CompanyBillingHours { get; set; }
        public string TimesheetApprovalStatus { get; set; }
        public string Usr_Username { get; set; }
        public string ManagerApprovalStatus { get; set; }

    }


}
