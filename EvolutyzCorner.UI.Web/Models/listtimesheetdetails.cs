using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace EvolutyzCorner.UI.Web.Models
{


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
        [Column(TypeName = "datetime2")]
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
        public string ManagerName1 { get; set; }
        public string ManagerID2 { get; set; }
        public string ManagerEmail2 { get; set; }
        public string ManagerName2 { get; set; }
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
        public string TotalMonthName { get; set; }


    }
    public class listtimesheetdetail
    {

        public int projectid { get; set; }
        public int taskid { get; set; }
        public string hoursWorked { get; set; }
        public string taskDate { get; set; }
        public string Message { get; set; }
        public string MessageCode { get; set; }
        public string errormessage { get; set; }
        public int acctaskid { get; set; }
        public int ProSpecificTsksid { get; set; }
        // public DateTime CreatedDate { get; set; }

    }
    public class StatusDetails
    {
        public string Message { get; set; }
        public string StatusCode { get; set; }
        public string errormessage { get; set; }

        // public DateTime CreatedDate { get; set; }

    }
    public class Timesheetlist
    {

        public List<TimeSheetDetails> timeSheetDetails { get; set; }

    }
    public class TimeSheetDetails
    {
        public int TimesheetID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int Taskid { get; set; }
        public string Taskname { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectClientName { get; set; }
        public Double NoofHoursWorked { get; set; }
        public string Month_Year { get; set; }
        public string Comments { get; set; }
        public string TaskDate { get; set; }
        public string Submitted_Type { get; set; }
        public string MonthYearName { get; set; }       
        public string SubmittedDate { get; set; }
        public string ApprovedDate { get; set; }
        public string ManagerName1 { get; set; }
        public string ManagerName2 { get; set; }
        public string colour { get; set; }
        public string TotalMonthName { get; set; }
    }
    public class WeeklyDatesandDays
    {
        public List<weeknames> weekDates { get; set; }
        public List<weekNamesandDates> weeknamesDates { get; set; }
        public List<week1> Week1 { get; set; }
        public List<week2> Week2 { get; set; }
        public List<week3> Week3 { get; set; }
        public List<week4> Week4 { get; set; }
        public List<week5> Week5 { get; set; }
        public List<week6> Week6 { get; set; }

    }
    public class weeknames
    {
        public string weekStartDate { get; set; }

    }

    public class weekNamesandDates
    {
     public string weekName { get;set;}
     public string weekDate { get; set; }
    }


    public class week1
    {
        public int Taskid { get; set; }
        public string AccSpecificTaskName { get; set; }
        public int projectId { get; set; }
        public string ProjectName { get; set; }
        public int Proj_SpecificTaskId { get; set; }
        public string Proj_SpecificTaskName { get; set; }
        public string TaskDate { get; set; }
        public int hoursworked { get; set; }
    }
    public class week2
    {
        public int Taskid { get; set; }
        public string AccSpecificTaskName { get; set; }
        public int projectId { get; set; }
        public string ProjectName { get; set; }
        public int Proj_SpecificTaskId { get; set; }
        public string Proj_SpecificTaskName { get; set; }
        public string TaskDate { get; set; }
        public int hoursworked { get; set; }
    }
    public class week3
    {
        public int Taskid { get; set; }
        public string AccSpecificTaskName { get; set; }
        public int projectId { get; set; }
        public string ProjectName { get; set; }
        public int Proj_SpecificTaskId { get; set; }
        public string Proj_SpecificTaskName { get; set; }
        public string TaskDate { get; set; }
        public int hoursworked { get; set; }
    }
    public class week4
    {
        public int Taskid { get; set; }
        public string AccSpecificTaskName { get; set; }
        public int projectId { get; set; }
        public string ProjectName { get; set; }
        public int Proj_SpecificTaskId { get; set; }
        public string Proj_SpecificTaskName { get; set; }
        public string TaskDate { get; set; }
        public int hoursworked { get; set; }
    }
    public class week5
    {
        public int Taskid { get; set; }
        public string AccSpecificTaskName { get; set; }
        public int projectId { get; set; }
        public string ProjectName { get; set; }
        public int Proj_SpecificTaskId { get; set; }
        public string Proj_SpecificTaskName { get; set; }
        public string TaskDate { get; set; }
        public int hoursworked { get; set; }
    }
    public class week6
    {
        public int Taskid { get; set; }
        public string AccSpecificTaskName { get; set; }
        public int projectId { get; set; }
        public string ProjectName { get; set; }
        public int Proj_SpecificTaskId { get; set; }
        public string Proj_SpecificTaskName { get; set; }
        public string TaskDate { get; set; }
        public int hoursworked { get; set; }
    }
   

}
