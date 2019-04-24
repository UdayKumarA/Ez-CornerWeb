using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace Evolutyz.Entities
{
    public class LeaveTypeEntity : ResponseHeader
    {
        private static HttpSessionState session { get { return HttpContext.Current.Session; } }
        public int LTyp_LeaveTypeID { get; set; }
        public int LTyp_AccountID { get; set; }
        public string UserType { get; set; }
        public string AccountName { get; set; }
        public string LTyp_LeaveType { get; set; }
        public string LTyp_LeaveTypeDescription { get; set; }
        public bool LTyp_ActiveStatus { get; set; }
        public short LTyp_Version { get; set; }
        public System.DateTime LTyp_CreatedDate { get; set; }
        public int LTyp_CreatedBy { get; set; }
        public Nullable<System.DateTime> LTyp_ModifiedDate { get; set; }
        public Nullable<int> LTyp_ModifiedBy { get; set; }
        public bool LTyp_isDeleted { get; set; }
        public double? LSchm_LeaveCount { get; set; }
        public string Acc_EmailID { get; set; }



        public int HolidayCalendarID { get; set; }
        public int AccountID { get; set; }
        // public string AccountName { get; set; }
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


        public string Su { get; set; }
        public string Mo { get; set; }
        public string Tu { get; set; }
        public string We { get; set; }
        public string Th { get; set; }
        public string Fr { get; set; }
        public string Sa { get; set; }


        public int UsrP_UserProfileID { get; set; }
        public string UsrP_EmployeeID { get; set; }
        public string UsrP_FirstName { get; set; }
        public string UsrP_LastName { get; set; }

        public int Usrlt_TypeConsumeId { get; set; }
        public Nullable<int> Usrl_LeaveId { get; set; }
        public Nullable<int> LSchm_LeaveSchemeID { get; set; }
        public Nullable<int> No_Of_Days { get; set; }

        // public int Usrl_LeaveId { get; set; }
        public Nullable<int> Usrl_UserId { get; set; }
        public Nullable<System.DateTime> LeaveStartDate { get; set; }
        public Nullable<System.DateTime> LeaveEndDate { get; set; }
        public Nullable<int> Tot_No_Days { get; set; }
        public string Comments { get; set; }
        public Nullable<int> L_StatusId { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        public Nullable<System.DateTime> RejectedDate { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
        public Nullable<int> RejectedBy { get; set; }

        public int UsT_UserTypeID { get; set; }
        public int UsT_AccountID { get; set; }
        public string UsT_UserTypeCode { get; set; }
        public string UsT_UserType { get; set; }
        public string UsT_UserTypeDescription { get; set; }
        public bool UsT_ActiveStatus { get; set; }
        public int UsT_Version { get; set; }
        public System.DateTime UsT_CreatedDate { get; set; }
        public int UsT_CreatedBy { get; set; }
        public Nullable<System.DateTime> UsT_ModifiedDate { get; set; }
        public Nullable<int> UsT_ModifiedBy { get; set; }
        public bool UsT_isDeleted { get; set; }


        public int disablePreviousDates { get; set; }

        public bool? UsAccount
        {
            get
            {
                return (bool)session["UsAccount"];
            }
            set
            {
                session["UsAccount"] = value;
            }
        }
    }
    public class manageremails
    {
        public string manageremail { get; set; }
        public string managerid { get; set; }
        public string managername { get; set; }
    }
    public class WeekDays
    {

        public string Su { get; set; }
        public List<LeaveTypeEntity> SuLeaveList { get; set; }
        public string Mo { get; set; }
        public List<LeaveTypeEntity> MoLeaveList { get; set; }
        public string Tu { get; set; }
        public List<LeaveTypeEntity> TuLeaveList { get; set; }
        public string We { get; set; }
        public List<LeaveTypeEntity> WeLeaveList { get; set; }
        public string Th { get; set; }
        public List<LeaveTypeEntity> ThLeaveList { get; set; }
        public string Fr { get; set; }
        public List<LeaveTypeEntity> FrLeaveList { get; set; }
        public string Sa { get; set; }
        public List<LeaveTypeEntity> SaLeaveList { get; set; }

        public int disablePreviousDates { get; set; }
    }
    public class managerjson
    {
        public List<manageremails> items { get; set; }
    }

    public class CalModelEntity
    {
        public int LTyp_LeaveTypeID { get; set; }
        public int LTyp_AccountID { get; set; }
        public string UserType { get; set; }
        public string AccountName { get; set; }
        public string LTyp_LeaveType { get; set; }
        public string LTyp_LeaveTypeDescription { get; set; }
        public bool LTyp_ActiveStatus { get; set; }
        public short LTyp_Version { get; set; }
        public System.DateTime LTyp_CreatedDate { get; set; }
        public int LTyp_CreatedBy { get; set; }
        public Nullable<System.DateTime> LTyp_ModifiedDate { get; set; }
        public Nullable<int> LTyp_ModifiedBy { get; set; }
        public bool LTyp_isDeleted { get; set; }


        public string Su { get; set; }
        public string Mo { get; set; }
        public string Tu { get; set; }
        public string We { get; set; }
        public string Th { get; set; }
        public string Fr { get; set; }
        public string Sa { get; set; }

    }

    public class WorkfromHome
    {
        public int Usrl_UserId { get; set; }
        public System.DateTime UserwfhStartDate { get; set; }
        public System.DateTime UserwfhEndDate { get; set; }
        public int Tot_No_Days { get; set; }
        public string Comments { get; set; }
        public int L_StatusId { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        public Nullable<System.DateTime> RejectedDate { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
        public Nullable<int> RejectedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public short Is_WorkFromHome { get; set; }
        public string UsrP_EmployeeID { get; set; }
        public string UsrP_FirstName { get; set; }
        public string UsrP_LastName { get; set; }


    }
    public class History_LeaveTypeEntity
    {
        public int History_LeaveType_ID { get; set; }
        public int History_LTyp_LeaveTypeID { get; set; }
        public int History_LTyp_AccountID { get; set; }
        public string AccountName { get; set; }
        public string History_LTyp_LeaveType { get; set; }
        public string History_LTyp_LeaveTypeDescription { get; set; }
        public bool History_LTyp_ActiveStatus { get; set; }
        public short History_LTyp_Version { get; set; }
        public System.DateTime History_LTyp_CreatedDate { get; set; }
        public int History_LTyp_CreatedBy { get; set; }
        public Nullable<System.DateTime> History_LTyp_ModifiedDate { get; set; }
        public Nullable<int> History_LTyp_ModifiedBy { get; set; }
        public bool History_LTyp_isDeleted { get; set; }
    }

    public class DashboardMails
    {
        public string Proj_ProjectCode { get; set; }
        public string Proj_ProjectName { get; set; }
        public int Proj_ProjectID { get; set; }
        public string UsrP_EmailID { get; set; }
        public int UsrP_EmployeeID { get; set; }

        public int accountid { get; set; }
        public int users { get; set; } 
        public int SkillId { get; set; }



    }
    public class AppliedLeaves
    {
        public int LTyp_LeaveTypeID { get; set; }
        public string ltyp_leavetype { get; set; }
        public int lschm_leavecount { get; set; }
        public int? AppliedLeave { get; set; }
        public string Acc_EmailID { get; set; }
        public int lschm_leaveschemeid { get; set; }

    }

    public class LeavePreviewDetails
    {
        public List<UserApprovedLeaves> myleaves { get; set; }
        public List<ManagerLeavesforApprovals> leavesforapproval { get; set; }
    }

    public class UserApprovedLeaves
    {
        public string ProjectName { get; set; }
        public string UsrP_FirstName { get; set; }
        public string UsrP_LastName { get; set; }
        public string LeaveStartDate { get; set; }
        public string LeaveEndDate { get; set; }
        public int Usrl_LeaveId { get; set; }
        public int Usrl_UserId { get; set; }
        public string userName { get; set; }
        public int No_Of_Days { get; set; }
        public string Leavestatus { get; set; }
        public string accntmail { get; set; }
    }

    public class ManagerLeavesforApprovals
    {
        public string ProjectName { get; set; }
        public string UsrP_FirstName { get; set; }
        public string UsrP_LastName { get; set; }
        public string LeaveStartDate { get; set; }
        public string LeaveEndDate { get; set; }
        public int Usrl_LeaveId { get; set; }
        public int Usrl_UserId { get; set; }
        public string userName { get; set; }
        public int No_Of_Days { get; set; }
        public string Leavestatus { get; set; }
        public string accntmail { get; set; }
        public int ManagerID1 { get; set; }
        public string ManagerEmail1 { get; set; }
        public string ManagerName1 { get; set; }
        public int ManagerID2 { get; set; }
        public string ManagerEmail2 { get; set; }
        public string ManagerName2 { get; set; }
        public string UserEmail { get; set; }
        public string Message { get; set; }
    }
    public class WFHPreviewDetails
    {
        public List<UserWFHDetails> UserWrkfrmhome { get; set; }
        public List<ManagerWFHforApproval> wrkfrmhmeforapproval { get; set; }
    }
    public class UserWFHDetails
    {
        public int UserwfhID { get; set; }
        public string userName { get; set; }
        public string usrEmailID { get; set; }
        public string ProjectName { get; set; }
        public string LeaveStartDate { get; set; }
        public string LeaveEndDate { get; set; }
        public string Leavestatus { get; set; }
        public string accntmail { get; set; }
        public int ManagerID1 { get; set; }
        public string ManagerEmail1 { get; set; }
        public string ManagerName1 { get; set; }
        public int ManagerID2 { get; set; }
        public string ManagerEmail2 { get; set; }
        public string ManagerName2 { get; set; }
        public string UserEmail { get; set; }
        public string userid { get; set; }
        public int Tot_No_Days { get; set; }
        public int Usrl_UserId { get; set; }

    }
    public class ManagerWFHforApproval
    {
        public int UserwfhID { get; set; }
        public string userName { get; set; }
        public string usrEmailID { get; set; }
        public string ProjectName { get; set; }
        public string LeaveStartDate { get; set; }
        public string LeaveEndDate { get; set; }
        public string Leavestatus { get; set; }
        public string accntmail { get; set; }
        public int ManagerID1 { get; set; }
        public string ManagerEmail1 { get; set; }
        public string ManagerName1 { get; set; }
        public int ManagerID2 { get; set; }
        public string ManagerEmail2 { get; set; }
        public string ManagerName2 { get; set; }
        public string UserEmail { get; set; }
        public string userid { get; set; }
        public int Tot_No_Days { get; set; }
        public int Usrl_UserId { get; set; }
        public string Message { get; set; }
    }

    public class USLeaveDates
    {
        public int userid { get; set; }
        public string monthyear { get; set; }
        public DateTime leavedates { get; set; }
        public int dates { get; set; }
    }


}
