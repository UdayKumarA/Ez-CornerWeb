using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EvolutyzCorner.UI.Web.Models
{
    public class ManagerDetails
    {
        public List<UserTimesheets> mytimesheets { get; set; }
        public List<ManagerTimesheetsforApprovals> timesheetsforapproval { get; set; }
    }


    public class UserTimesheets
    {
        public string ProjectName { get; set; }
        public int ProjectId { get; set; }
        public string Month_Year { get; set; }
        public string CompanyBillingHours { get; set; }
        public string ResourceWorkingHours { get; set; }
        public int TimesheetID { get; set; }
        public string TimesheetApprovalStatus { get; set; }
        public int Usr_UserID { get; set; }
        public string userName { get; set; }
        public string ManagerApprovalStatus { get; set; }
        public string SubmittedDate { get; set; }
        public string ApprovedDate { get; set; }
        public string ManagerName1 { get; set; }
        public string ManagerName2 { get; set; }

    }

    public class ManagerTimesheetsforApprovals
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int Userid { get; set; }
        public string Username { get; set; }
        public int TimesheetID { get; set; }
        public string Month_Year { get; set; }
        public int ResourceWorkingHours { get; set; }
        public int CompanyBillingHours { get; set; }
        public string TimesheetApprovalStatus { get; set; }
        public int Usr_UserID { get; set; }
        public string userName { get; set; }
        public string ManagerApprovalStatus { get; set; }
        public string SubmittedDate { get; set; }
        public string ApprovedDate { get; set; }
        public string ManagerName1 { get; set; }
        public string ManagerName2 { get; set; }
    }

    public class UserProfilesData
    {
        public string UsrPFirstName { get; set; }
        public string  UsrPLastName { get; set; }
        //public string UsrPFullName { get; set; }
        public string UsrP_EmployeeID { get; set; }
        public string UsrP_EmailID { get; set; }
        public string UsrP_MobNum { get; set; }
        public string UsrP_PhoneNumber { get; set; }
        public string UsrP_ProfilePicture { get; set; }
        public string UsrP_DOJ { get; set; }
        public string UsrP_DOB { get; set; }
        public int Usr_TitleId { get; set; }
        public int Usr_GenderId { get;set;}
        public string  MartialStatus { get; set; }
        public string PermanentAddress { get; set; }
        public string TemporaryAddress { get; set; }
        public string userName { get; set; }

    }

    public class UserTitle
    {
        public int Usr_Titleid { get; set; }
        public string TitlePrefix { get; set; }
        public int Acc_AccountID { get; set; }       

    }

    public class UserGender
    {
        public int Usr_GenderId { get; set; }
        public string Gender { get; set; }
        public int Acc_AccountID { get; set; }

    }
}