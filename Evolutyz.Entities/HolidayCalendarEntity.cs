using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolutyz.Entities
{
    public class HolidayCalendarEntity : ResponseHeader
    {
        public int HolidayCalendarID { get; set; }
        public int AccountID { get; set; }
        public string AccountName { get; set; }
        public string HolidayName { get; set; }
        public int Year { get; set; }
        public System.DateTime HolidayDate { get; set; }
        public string HolidayWeek { get; set; }
        public bool isOptionalHoliday { get; set; }
        public bool isActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool isDeleted { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public int? FinancialYearId { get; set; }
        public Nullable<int> StartDate { get; set; }
        public Nullable<int> EndDate { get; set; }
        public string financialyear { get; set; }
    }

    public class timesheetEntity
    {
        public string UserName { get; set; }
        public int TimesheetID { get; set; }
        public int UserID { get; set; }

        public String TimeSheetMonth { get; set; }
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
        public string ManagerID1 { get; set; }
        public string ManagerEmail1 { get; set; }
        public string ManagerID2 { get; set; }
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
        public string EmailAppOrRejStatus { get; set; }
        public string Position { get; set; }
        public string Message { get; set; }
        public string SubmittedState { get; set; }
        public string Proj_ProjectName { get; set; }
        public string ManagerLevel1 { get; set; }
        public string ManagerLevel2 { get; set; }


    }

}
