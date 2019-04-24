using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Data;
using System.Data.SqlClient;
using Evolutyz.Business;
using EvolutyzCorner.UI.Web.Models;
using Evolutyz.Entities;
using Evolutyz.Data;
using RestSharp;
using RestSharp.Authenticators;
using System.Configuration;


namespace EvolutyzCorner.UI.Web.Controllers.LeaveManagement
{

    public class LeaveApplicationManagementController : Controller
    {
        SqlConnection Conn = new SqlConnection();
        public static string str = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public static string host = System.Web.HttpContext.Current.Request.Url.Host;
        public static string port = System.Web.HttpContext.Current.Request.Url.Port.ToString();
        public static string port1 = System.Configuration.ConfigurationManager.AppSettings["RedirectURL"];
        public static string UrlEmailAddress = string.Empty;

        public ActionResult Index()
        {
            GetHolidayDates();
            UserSessionInfo objSessioninfo = new UserSessionInfo();
            var ussacnt = objSessioninfo.UsAccount;
            if (ussacnt == true)
            {
                //USLeaveCalendar();
                return RedirectToAction("USLeaveCalendar");
            }
            else
            {
                Calendar();
                return RedirectToAction("Calendar");
            }
        }
        public ActionResult PreviewLeaves()
        {

            return View();
        }

        public ActionResult PreviewWorkFromHome()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetUSaccmail()
        {
            AdminComponent admcomp = new AdminComponent();
            UserSessionInfo info = new UserSessionInfo();
            int userid = info.UserId;
            var query = admcomp.GetUSaccmail(userid);
            return Json(query, JsonRequestBehavior.AllowGet);

        }

        public ActionResult LeaveApplication()
        {
            LeaveTypeComponent LeaveTypes = new LeaveTypeComponent();
            var Employeementtypes = LeaveTypes.GetAllLeaveTypes().Select(a => new SelectListItem()
            {
                Value = a.LTyp_LeaveTypeID.ToString(),
                Text = a.LTyp_LeaveType,

            });
            ViewBag.Employeementtypes = Employeementtypes;
            return View();
        }
        public JsonResult GetEmpIds()
        {
            EvolutyzCornerDataEntities db = new EvolutyzCornerDataEntities();
            var query = (from ei in db.UsersProfiles
                         select new LeaveTypeEntity
                         {
                             UsrP_UserProfileID = ei.UsrP_UserProfileID,
                             UsrP_EmployeeID = ei.UsrP_EmployeeID
                         }).ToList();

            return Json(query, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult GetLeaveTypes(string id)
        {
            AdminComponent admcmp = new AdminComponent();
            var query = admcmp.GetLeaveTypes(id);
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetLeaves(string id)
        {
            AdminComponent admcmp = new AdminComponent();
            var query = admcmp.GetLeaves(id);
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetEmpNames(string id)
        {
            EvolutyzCornerDataEntities db = new EvolutyzCornerDataEntities();
            int ID = Convert.ToInt32(id);
            var query = (from en in db.UsersProfiles
                         where en.UsrP_UserProfileID == ID
                         select new LeaveTypeEntity
                         {
                             UsrP_FirstName = en.UsrP_FirstName,
                             UsrP_LastName = en.UsrP_LastName
                         }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        DateTime newdate;
        public ActionResult Calendar()
        {
            UserSessionInfo objSessioninfo = new UserSessionInfo();
            bool? accid = objSessioninfo.UsAccount;

            ViewBag.isusacc = accid;
            var ussacnt = objSessioninfo.UsAccount;
            if (objSessioninfo.UserId != 0)
            {
                var uid = objSessioninfo.UserId;

                var Usacc = objSessioninfo.UsAccount.HasValue ? objSessioninfo.UsAccount : true;
                EvolutyzCornerDataEntities db = new EvolutyzCornerDataEntities();
                var getuserid = (from u in db.UsersProfiles where u.UsrP_UserID == uid select u.UsrP_EmployeeID).FirstOrDefault();
                var getempname = (from u in db.UsersProfiles where u.UsrP_UserID == uid select u.UsrP_FirstName).FirstOrDefault();
                var getlastname = (from u in db.UsersProfiles where u.UsrP_UserID == uid select u.UsrP_LastName).FirstOrDefault();
                var getusermailid = (from u in db.UsersProfiles where u.UsrP_UserID == uid select u.UsrP_EmailID).FirstOrDefault();
                var getusrwfhId = (from wfh in db.UserworkfromHomes where wfh.Usrl_UserId == uid select wfh.UserwfhID).FirstOrDefault();
                var getuserfullname = getempname + "  " + getlastname;

                TempData["User_id"] = uid;
                TempData["UserId"] = getuserid;
                TempData["Name"] = getuserfullname;
                TempData["FromAddress"] = getusermailid;
                TempData["UsrWrkFrmId"] = getusrwfhId;
            }
            var Daylst = new List<WeekDays>();
            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;
            int totalDays = DateTime.DaysInMonth(currentYear, currentMonth);

            LeaveTypeEntity objcal = new LeaveTypeEntity();
            WeekDays days = new WeekDays();

            AdminComponent admcmp = new AdminComponent();
            var getempdetails = admcmp.GetApprovedLeaveCount();
            for (int index = 1; index <= totalDays; index++)
            {
                var dayOfWeek = new DateTime(currentYear, currentMonth, index).DayOfWeek;
                 newdate = Convert.ToDateTime(currentYear + "-" + currentMonth + "-" + index);               
                switch (dayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        objcal.Su = Convert.ToString(index);
                        days.Su = Convert.ToString(index);
                        days.SuLeaveList = (from newdateslst in getempdetails where newdateslst.LeaveStartDate == newdate select newdateslst).ToList();
                        FinalizeDaysList(Daylst, totalDays, days, index);
                        break;
                    case DayOfWeek.Monday:
                        objcal.Mo = Convert.ToString(index);
                        days.Mo = Convert.ToString(index);
                        days.MoLeaveList = (from newdateslst in getempdetails where newdateslst.LeaveStartDate == newdate select newdateslst).ToList();
                        FinalizeDaysList(Daylst, totalDays, days, index);
                        break;
                    case DayOfWeek.Tuesday:
                        objcal.Tu = Convert.ToString(index);
                        days.Tu = Convert.ToString(index);
                        days.TuLeaveList = (from newdateslst in getempdetails where newdateslst.LeaveStartDate == newdate select newdateslst).ToList();
                        FinalizeDaysList(Daylst, totalDays, days, index);
                        break;
                    case DayOfWeek.Wednesday:
                        objcal.We = Convert.ToString(index);
                        days.We = Convert.ToString(index);
                        days.WeLeaveList = (from newdateslst in getempdetails where newdateslst.LeaveStartDate == newdate select newdateslst).ToList();
                        FinalizeDaysList(Daylst, totalDays, days, index);
                        break;
                    case DayOfWeek.Thursday:
                        days.Th = Convert.ToString(index);
                        objcal.Th = Convert.ToString(index);
                        days.ThLeaveList = (from newdateslst in getempdetails where newdateslst.LeaveStartDate == newdate select newdateslst).ToList();
                        FinalizeDaysList(Daylst, totalDays, days, index);
                        break;
                    case DayOfWeek.Friday:
                        objcal.Fr = Convert.ToString(index);
                        days.Fr = Convert.ToString(index);
                        days.FrLeaveList = (from newdateslst in getempdetails where newdateslst.LeaveStartDate == newdate select newdateslst).ToList();
                        FinalizeDaysList(Daylst, totalDays, days, index);
                        break;
                    case DayOfWeek.Saturday:
                        days.Sa = Convert.ToString(index);
                        objcal.Sa = Convert.ToString(index);
                        days.SaLeaveList = (from newdateslst in getempdetails where newdateslst.LeaveStartDate == newdate select newdateslst).ToList();
                        Daylst.Add(days);
                        objcal = new LeaveTypeEntity();
                        days = new WeekDays();
                        break;
                }
            }

            return View(Daylst);


        }



        public ActionResult USLeaveCalendar()
        {

            UserSessionInfo objSessioninfo = new UserSessionInfo();
            bool? accid = objSessioninfo.UsAccount;

            // ViewBag.isusacc = accid;
            if (objSessioninfo.UserId != 0)
            {
                var uid = objSessioninfo.UserId;

                var Usacc = objSessioninfo.UsAccount.HasValue ? objSessioninfo.UsAccount : true;
                EvolutyzCornerDataEntities db = new EvolutyzCornerDataEntities();
                var getuserid = (from u in db.UsersProfiles where u.UsrP_UserID == uid select u.UsrP_EmployeeID).FirstOrDefault();
                var getempname = (from u in db.UsersProfiles where u.UsrP_UserID == uid select u.UsrP_FirstName).FirstOrDefault();
                var getlastname = (from u in db.UsersProfiles where u.UsrP_UserID == uid select u.UsrP_LastName).FirstOrDefault();
                var getusermailid = (from u in db.UsersProfiles where u.UsrP_UserID == uid select u.UsrP_EmailID).FirstOrDefault();
                var getusrwfhId = (from wfh in db.UserworkfromHomes where wfh.Usrl_UserId == uid select wfh.UserwfhID).FirstOrDefault();
                var getuserfullname = getempname + "  " + getlastname;
                TempData["User_id"] = uid;
                TempData["UserId"] = getuserid;
                TempData["Name"] = getuserfullname;
                TempData["FromAddress"] = getusermailid;
                TempData["UsrWrkFrmId"] = getusrwfhId;
            }
            var Daylst = new List<WeekDays>();

            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;
            int totalDays = DateTime.DaysInMonth(currentYear, currentMonth);

            LeaveTypeEntity objcal = new LeaveTypeEntity();
            WeekDays days = new WeekDays();

            AdminComponent admcmp = new AdminComponent();
           // var gethldydetails = admcmp.GetHolidayDates(accountid);
            var getempdetails = admcmp.GetApprovedLeaveCount();
            for (int index = 1; index <= totalDays; index++)
            {
                var dayOfWeek = new DateTime(currentYear, currentMonth, index).DayOfWeek;
                newdate = Convert.ToDateTime(currentYear + "-" + currentMonth + "-" + index);
                switch (dayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        objcal.Su = Convert.ToString(index);
                        days.Su = Convert.ToString(index);
                        days.SuLeaveList = (from newdateslst in getempdetails where newdateslst.LeaveStartDate == newdate select newdateslst).ToList();
                        FinalizeDaysList(Daylst, totalDays, days, index);
                        break;
                    case DayOfWeek.Monday:
                        objcal.Mo = Convert.ToString(index);
                        days.Mo = Convert.ToString(index);
                        days.MoLeaveList = (from newdateslst in getempdetails where newdateslst.LeaveStartDate == newdate select newdateslst).ToList();
                        FinalizeDaysList(Daylst, totalDays, days, index);
                        break;
                    case DayOfWeek.Tuesday:
                        objcal.Tu = Convert.ToString(index);
                        days.Tu = Convert.ToString(index);
                        days.TuLeaveList = (from newdateslst in getempdetails where newdateslst.LeaveStartDate == newdate select newdateslst).ToList();
                        FinalizeDaysList(Daylst, totalDays, days, index);
                        break;
                    case DayOfWeek.Wednesday:
                        objcal.We = Convert.ToString(index);
                        days.We = Convert.ToString(index);
                        days.WeLeaveList = (from newdateslst in getempdetails where newdateslst.LeaveStartDate == newdate select newdateslst).ToList();
                        FinalizeDaysList(Daylst, totalDays, days, index);
                        break;
                    case DayOfWeek.Thursday:
                        days.Th = Convert.ToString(index);
                        objcal.Th = Convert.ToString(index);
                        days.ThLeaveList = (from newdateslst in getempdetails where newdateslst.LeaveStartDate == newdate select newdateslst).ToList();
                        FinalizeDaysList(Daylst, totalDays, days, index);
                        break;
                    case DayOfWeek.Friday:
                        objcal.Fr = Convert.ToString(index);
                        days.Fr = Convert.ToString(index);
                        days.FrLeaveList = (from newdateslst in getempdetails where newdateslst.LeaveStartDate == newdate select newdateslst).ToList();
                        FinalizeDaysList(Daylst, totalDays, days, index);
                        break;
                    case DayOfWeek.Saturday:
                        days.Sa = Convert.ToString(index);
                        objcal.Sa = Convert.ToString(index);
                        days.SaLeaveList = (from newdateslst in getempdetails where newdateslst.LeaveStartDate == newdate select newdateslst).ToList();
                        Daylst.Add(days);
                        objcal = new LeaveTypeEntity();
                        days = new WeekDays();
                        break;
                }
            }

            return View(Daylst);
        }

        private static void FinalizeDaysList(List<WeekDays> Daylst, int totalDays, WeekDays objcal, int index)
        {
            var id = index;
            int currentmonth = DateTime.Now.Month;
            int currentyear = DateTime.Now.Year;

            DateTime newdate = Convert.ToDateTime(currentyear + "-" + currentmonth + "-" + id);

            AdminComponent admcmp = new AdminComponent();

            var getempdetails = admcmp.GetApprovedLeaveCount();
            var data = (from newdateslst in getempdetails where newdateslst.LeaveStartDate == newdate select newdateslst).ToList();
            if (data.Count > 0)
            {

            }

            if (index == totalDays)
            {
                Daylst.Add(objcal);

            }


            //UserSessionInfo info = new UserSessionInfo();
            //var accountid = info.AccountId;

            //var gethldydetails = admcmp.GetHolidayDates(accountid);
            //var hdata = (from hlddateslst in gethldydetails where hlddateslst.HolidayDate == newdate select hlddateslst).ToList();
            //if (index == totalDays)
            //{
            //    Daylst.Add(objcal);
            //}
        }


        private static void ChangedFinalizeDaysList(List<WeekDays> Daylst, int totalDays, WeekDays objcal, int index, int Selmonth, int SelYear)
        {
            var id = index;

            int currentyear = SelYear;
            int selectedMonth = Selmonth;

            DateTime newdate = Convert.ToDateTime(currentyear + "-" + selectedMonth + "-" + id);

            AdminComponent admcmp = new AdminComponent();
            var getempdetails = admcmp.GetApprovedLeaveCount();
            var data = (from newdateslst in getempdetails where newdateslst.LeaveStartDate == newdate select newdateslst).ToList();
            if (data.Count > 0)
            {

            }

            if (index == totalDays)
            {
                Daylst.Add(objcal);

            }
        }


        [HttpPost]
        public JsonResult SelectedMonth(int Selmonth, int Selyear)
        {

            var Daylst = new List<WeekDays>();
            int totalDays = DateTime.DaysInMonth(Selyear, Selmonth);

            LeaveTypeEntity objcal = new LeaveTypeEntity();
            WeekDays days = new WeekDays();

            AdminComponent admcmp = new AdminComponent();
            var getempdetails = admcmp.GetApprovedLeaveCount();
            for (int index = 1; index <= totalDays; index++)
            {
                var dayOfWeek = new DateTime(Selyear, Selmonth, index).DayOfWeek;
                DateTime newdate = Convert.ToDateTime(Selyear + "-" + Selmonth + "-" + index);
                switch (dayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        objcal.Su = Convert.ToString(index);
                        days.Su = Convert.ToString(index);
                        days.SuLeaveList = (from newdateslst in getempdetails where newdateslst.LeaveStartDate == newdate select newdateslst).ToList();
                        ChangedFinalizeDaysList(Daylst, totalDays, days, index, Selmonth, Selyear);
                        break;
                    case DayOfWeek.Monday:
                        objcal.Mo = Convert.ToString(index);
                        days.Mo = Convert.ToString(index);
                        days.MoLeaveList = (from newdateslst in getempdetails where newdateslst.LeaveStartDate == newdate select newdateslst).ToList();
                        ChangedFinalizeDaysList(Daylst, totalDays, days, index, Selmonth, Selyear);
                        break;
                    case DayOfWeek.Tuesday:
                        objcal.Tu = Convert.ToString(index);
                        days.Tu = Convert.ToString(index);
                        days.TuLeaveList = (from newdateslst in getempdetails where newdateslst.LeaveStartDate == newdate select newdateslst).ToList();
                        ChangedFinalizeDaysList(Daylst, totalDays, days, index, Selmonth, Selyear);
                        break;
                    case DayOfWeek.Wednesday:
                        objcal.We = Convert.ToString(index);
                        days.We = Convert.ToString(index);
                        days.WeLeaveList = (from newdateslst in getempdetails where newdateslst.LeaveStartDate == newdate select newdateslst).ToList();
                        ChangedFinalizeDaysList(Daylst, totalDays, days, index, Selmonth, Selyear);
                        break;
                    case DayOfWeek.Thursday:
                        days.Th = Convert.ToString(index);
                        objcal.Th = Convert.ToString(index);
                        days.ThLeaveList = (from newdateslst in getempdetails where newdateslst.LeaveStartDate == newdate select newdateslst).ToList();
                        ChangedFinalizeDaysList(Daylst, totalDays, days, index, Selmonth, Selyear);
                        break;
                    case DayOfWeek.Friday:
                        objcal.Fr = Convert.ToString(index);
                        days.Fr = Convert.ToString(index);
                        days.FrLeaveList = (from newdateslst in getempdetails where newdateslst.LeaveStartDate == newdate select newdateslst).ToList();
                        ChangedFinalizeDaysList(Daylst, totalDays, days, index, Selmonth, Selyear);
                        break;
                    case DayOfWeek.Saturday:
                        days.Sa = Convert.ToString(index);
                        objcal.Sa = Convert.ToString(index);
                        days.SaLeaveList = (from newdateslst in getempdetails where newdateslst.LeaveStartDate == newdate select newdateslst).ToList();
                        Daylst.Add(days);
                        objcal = new LeaveTypeEntity();
                        days = new WeekDays();
                        break;
                }
            }

            return Json(Daylst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult selectedYear(int Selmonth, int Selyear)
        {
            var Daylst = new List<WeekDays>();
            int totalDays = DateTime.DaysInMonth(Selyear, Selmonth);

            LeaveTypeEntity objcal = new LeaveTypeEntity();
            WeekDays days = new WeekDays();

            AdminComponent admcmp = new AdminComponent();
            var getempdetails = admcmp.GetApprovedLeaveCount();
            for (int index = 1; index <= totalDays; index++)
            {
                var dayOfWeek = new DateTime(Selyear, Selmonth, index).DayOfWeek;
                DateTime newdate = Convert.ToDateTime(Selyear + "-" + Selmonth + "-" + index);
                switch (dayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        objcal.Su = Convert.ToString(index);
                        days.Su = Convert.ToString(index);
                        days.SuLeaveList = (from newdateslst in getempdetails where newdateslst.LeaveStartDate == newdate select newdateslst).ToList();
                        ChangedFinalizeDaysList(Daylst, totalDays, days, index, Selmonth, Selyear);
                        break;
                    case DayOfWeek.Monday:
                        objcal.Mo = Convert.ToString(index);
                        days.Mo = Convert.ToString(index);
                        days.MoLeaveList = (from newdateslst in getempdetails where newdateslst.LeaveStartDate == newdate select newdateslst).ToList();
                        ChangedFinalizeDaysList(Daylst, totalDays, days, index, Selmonth, Selyear);
                        break;
                    case DayOfWeek.Tuesday:
                        objcal.Tu = Convert.ToString(index);
                        days.Tu = Convert.ToString(index);
                        days.TuLeaveList = (from newdateslst in getempdetails where newdateslst.LeaveStartDate == newdate select newdateslst).ToList();
                        ChangedFinalizeDaysList(Daylst, totalDays, days, index, Selmonth, Selyear);
                        break;
                    case DayOfWeek.Wednesday:
                        objcal.We = Convert.ToString(index);
                        days.We = Convert.ToString(index);
                        days.WeLeaveList = (from newdateslst in getempdetails where newdateslst.LeaveStartDate == newdate select newdateslst).ToList();
                        ChangedFinalizeDaysList(Daylst, totalDays, days, index, Selmonth, Selyear);
                        break;
                    case DayOfWeek.Thursday:
                        days.Th = Convert.ToString(index);
                        objcal.Th = Convert.ToString(index);
                        days.ThLeaveList = (from newdateslst in getempdetails where newdateslst.LeaveStartDate == newdate select newdateslst).ToList();
                        ChangedFinalizeDaysList(Daylst, totalDays, days, index, Selmonth, Selyear);
                        break;
                    case DayOfWeek.Friday:
                        objcal.Fr = Convert.ToString(index);
                        days.Fr = Convert.ToString(index);
                        days.FrLeaveList = (from newdateslst in getempdetails where newdateslst.LeaveStartDate == newdate select newdateslst).ToList();
                        ChangedFinalizeDaysList(Daylst, totalDays, days, index, Selmonth, Selyear);
                        break;
                    case DayOfWeek.Saturday:
                        days.Sa = Convert.ToString(index);
                        objcal.Sa = Convert.ToString(index);
                        days.SaLeaveList = (from newdateslst in getempdetails where newdateslst.LeaveStartDate == newdate select newdateslst).ToList();
                        Daylst.Add(days);
                        objcal = new LeaveTypeEntity();
                        days = new WeekDays();
                        break;
                }
            }

            return Json(Daylst, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public string CheckLeave(string leaves)
        {
            string response = string.Empty;
            UserSessionInfo sessionInfo = new UserSessionInfo();
            int userid = sessionInfo.UserId;
            EvolutyzCornerDataEntities db = new EvolutyzCornerDataEntities();
            var query = (from ls in db.LeaveSchemes
                         join u in db.UserTypes on ls.LSchm_UserTypeID equals u.UsT_UserTypeID
                         join ut in db.Users on u.UsT_UserTypeID equals ut.Usr_UserTypeID
                         join a in db.Accounts on ls.LSchm_AccountID equals a.Acc_AccountID
                         join lt in db.LeaveTypes on ls.LSchm_LeaveTypeID equals lt.LTyp_LeaveTypeID

                         where ls.LSchm_UserTypeID == u.UsT_UserTypeID
                         group ls by
                         new
                         {
                             ls.LSchm_UserTypeID,
                             u.UsT_UserTypeID,
                             ls.LSchm_LeaveSchemeID,

                         } into gs

                         select new LeaveSchemeEntity
                         {
                             LSchm_LeaveSchemeID = gs.Key.LSchm_LeaveSchemeID,
                             Noofdays = gs.Sum(p => p.LSchm_LeaveCount),

                         }).FirstOrDefault();

            return response;
        }
        static string accmail = string.Empty;
        DateTime newleavestartdate;
        int userid = 0;
        [HttpPost]
        public string saveleavecount(List<AppliedLeaves> leaveupdate, string LeaveStartDate, string LeaveEndDate, string Usrl_UserId, string Acc_EmailID)
        {
            DateTime sdate = Convert.ToDateTime(LeaveStartDate);
            DateTime ldate = Convert.ToDateTime(LeaveEndDate);
            int CalDates = Convert.ToInt32(1 + (ldate - sdate).TotalDays);
            TimeSpan span = ldate - sdate;
            int businessdays = span.Days + 1;
            int weekcount = businessdays / 7;
            if (businessdays > weekcount * 7)
            {
                int FirstDayOfWeek = sdate.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)sdate.DayOfWeek;
                int LastDayOfWeek = ldate.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)ldate.DayOfWeek;
                if (LastDayOfWeek < FirstDayOfWeek)
                    LastDayOfWeek += 7;
                if (FirstDayOfWeek <= 6)
                {
                    if (LastDayOfWeek >= 7)
                        businessdays -= 2;
                    else if (LastDayOfWeek >= 6)
                        businessdays -= 1;
                }
                else if (FirstDayOfWeek <= 7 && LastDayOfWeek >= 7)
                    businessdays -= 1;
                businessdays -= weekcount + weekcount;
            }
            newleavestartdate = Convert.ToDateTime(LeaveStartDate);
            AdminComponent admcomp = new AdminComponent();
            accmail = Acc_EmailID;
            string response = string.Empty;
            int sid = admcomp.saveleavecount(leaveupdate, LeaveStartDate, LeaveEndDate, Usrl_UserId);
            int leaveid = Convert.ToInt32(sid);
            MailLeaveApplication(LeaveStartDate, LeaveEndDate, businessdays, CalDates, leaveid);
            response = "Successfully Leave Applied";


            return response;
        }

      
        [HttpPost]
        public string saveleavecountForUS(string LeaveStartDate, string LeaveEndDate, string Usrl_UserId, string Acc_EmailID)
        {
            DateTime sdate = Convert.ToDateTime(LeaveStartDate);
            DateTime ldate = Convert.ToDateTime(LeaveEndDate);
            int CalDates = Convert.ToInt32(1 + (ldate - sdate).TotalDays);
            TimeSpan span = ldate - sdate;
            int businessdays = span.Days + 1;
            int weekcount = businessdays / 7;
            if (businessdays > weekcount * 7)
            {
                int FirstDayOfWeek = sdate.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)sdate.DayOfWeek;
                int LastDayOfWeek = ldate.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)ldate.DayOfWeek;
                if (LastDayOfWeek < FirstDayOfWeek)
                    LastDayOfWeek += 7;
                if (FirstDayOfWeek <= 6)
                {
                    if (LastDayOfWeek >= 7)
                        businessdays -= 2;
                    else if (LastDayOfWeek >= 6)
                        businessdays -= 1;
                }
                else if (FirstDayOfWeek <= 7 && LastDayOfWeek >= 7)
                    businessdays -= 1;
                businessdays -= weekcount + weekcount;
            }

            newleavestartdate = Convert.ToDateTime(LeaveStartDate);
            AdminComponent admcomp = new AdminComponent();
            accmail = Acc_EmailID;
            string response = string.Empty;
            int sid = admcomp.saveleavecountForUS(LeaveStartDate, LeaveEndDate, Usrl_UserId, businessdays);
            int leaveid = Convert.ToInt32(sid);
            MailLeaveApplication(LeaveStartDate, LeaveEndDate, businessdays, CalDates, leaveid);
            response = "Successfully Leave Applied";
            return response;
        }



        [HttpPost]
        public string savewrkfrmhomeForUS(string LeaveStartDate, string LeaveEndDate, string Usrl_UserId, string Acc_EmailID)
        {
            DateTime sdate = Convert.ToDateTime(LeaveStartDate);
            DateTime ldate = Convert.ToDateTime(LeaveEndDate);
            int CalDates = Convert.ToInt32(1 + (ldate - sdate).TotalDays);
            TimeSpan span = ldate - sdate;
            int businessdays = span.Days + 1;
            int weekcount = businessdays / 7;            
            if (businessdays > weekcount * 7)
            {
                int FirstDayOfWeek = sdate.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)sdate.DayOfWeek;
                int LastDayOfWeek = ldate.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)ldate.DayOfWeek;
                if (LastDayOfWeek < FirstDayOfWeek)
                    LastDayOfWeek += 7;
                if (FirstDayOfWeek <= 6)
                {
                    if (LastDayOfWeek >= 7)
                        businessdays -= 2;
                    else if (LastDayOfWeek >= 6)
                        businessdays -= 1;
                }
                else if (FirstDayOfWeek <= 7 && LastDayOfWeek >= 7)
                    businessdays -= 1;
                businessdays -= weekcount + weekcount;
            }
            newleavestartdate = Convert.ToDateTime(LeaveStartDate);
            AdminComponent admcomp = new AdminComponent();
            accmail = Acc_EmailID;
            string response = string.Empty;
            int res = admcomp.savewrkfrmhomeForUS(LeaveStartDate, LeaveEndDate, Usrl_UserId, businessdays);
            ApplyWorkFromHome(LeaveStartDate, LeaveEndDate, businessdays, CalDates, res);
            response = "Successfully Applied";
            return response;
        }

        [HttpPost]
        public string savewrkfrmhome(string fromdate, string todate, string Usrl_UserId, string Acc_EmailID)
        {
            DateTime sdate = Convert.ToDateTime(fromdate);
            DateTime ldate = Convert.ToDateTime(todate);
            int CalDates = Convert.ToInt32(1 + (ldate - sdate).TotalDays);
            TimeSpan span = ldate - sdate;
            int businessdays = span.Days + 1;
            int weekcount = businessdays / 7;
            if (businessdays > weekcount * 7)
            {
                int FirstDayOfWeek = sdate.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)sdate.DayOfWeek;
                int LastDayOfWeek = ldate.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)ldate.DayOfWeek;
                if (LastDayOfWeek < FirstDayOfWeek)
                    LastDayOfWeek += 7;
                if (FirstDayOfWeek <= 6)
                {
                    if (LastDayOfWeek >= 7)
                        businessdays -= 2;
                    else if (LastDayOfWeek >= 6)
                        businessdays -= 1;
                }
                else if (FirstDayOfWeek <= 7 && LastDayOfWeek >= 7)
                    businessdays -= 1;
                businessdays -= weekcount + weekcount;
            }
            newleavestartdate = Convert.ToDateTime(fromdate);
            AdminComponent admcomp = new AdminComponent();
            accmail = Acc_EmailID;
            int res = admcomp.savewrkfrmhome(fromdate, todate, Usrl_UserId);
            ApplyWorkFromHome(fromdate, todate, businessdays, CalDates, res);
            string response = "Successfully  Applied";
            return response;
        }


        string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        static string mail;
        static string newleavid;
        static int uid;
        static string mgrname;
        static string mgr2name;
        static string mgr2id;
        static string mgr1id;
        static string email;
        static string id;
        static string name;
        static string newuwfhid;
        static string newemail;
        static string levelofmanager;


        public static string ApplyWorkFromHome(string fromdate, string todate, int businessdays, int CalDates, int userwfhId)
        {

            UserSessionInfo objSessioninfo = new UserSessionInfo();
            string UrlEmailAddress = string.Empty;
            if (host == "localhost")
            {
                UrlEmailAddress = host + ":" + port;
            }
            else
            {
                UrlEmailAddress = port1;
            }
            Encrypt objenc = new Encrypt();
            RestClient client = new RestClient();
            RestRequest request = new RestRequest();
            mail = objenc.Encryption(accmail);
            newuwfhid = userwfhId.ToString();
            mail = objenc.Encryption(accmail);
            var userwfhid = objenc.Encryption(userwfhId.ToString());
            uid = objSessioninfo.UserId;
            EvolutyzCornerDataEntities db = new EvolutyzCornerDataEntities();
            var getuserid = (from u in db.UsersProfiles where u.UsrP_UserID == uid select u.UsrP_EmployeeID).FirstOrDefault();
            var getempname = (from u in db.UsersProfiles where u.UsrP_UserID == uid select u.UsrP_FirstName).FirstOrDefault();
            var getlastname = (from u in db.UsersProfiles where u.UsrP_UserID == uid select u.UsrP_LastName).FirstOrDefault();
            var getuserfullname = getempname + "  " + getlastname;
            UserSessionInfo info = new UserSessionInfo();
            AdminComponent admcmp = new AdminComponent();
            string userid = (info.UserId).ToString();
            var Uid = objenc.Encryption(userid);
            var res = admcmp.GetAllManagersEmails(userid);
            var manager1 = res.Select(a => a.ManagerLevel1).FirstOrDefault();
            var manager2 = res.Select(a => a.ManagerLevel2).FirstOrDefault();
            string[] manager1emailids = manager1.Split('/');
            string[] manageremailids = manager2.Split('/');
            var manager1email = manager1emailids[0];
            var manager1id = manager1emailids[1];
            var manager1name = manager1emailids[2];
            var manager2email = manageremailids[0];
            var manager2id = manageremailids[1];
            var manager2name = manageremailids[2];
            var objectToSerialize = new managerjson();
            objectToSerialize.items = new List<manageremails>
                          {
                             new manageremails { manageremail = manager1email, managerid = manager1id, managername = manager1name },
                          };
            var emailcontent = "";
            string newemail = string.Empty;
            string newid = string.Empty;
            string newname = string.Empty;
            string newlevel = string.Empty;
            for (var i = 0; i <= objectToSerialize.items.Count - 1; i++)
            {
                email = objectToSerialize.items[i].manageremail;
                id = objectToSerialize.items[i].managerid;
                name = objectToSerialize.items[i].managername;
                newemail = objenc.Encryption(email);
                newid = objenc.Encryption(id);
                newname = objenc.Encryption(name);
                emailcontent = "<html>" +
               "<body bgcolor='#f6f6f6' style='-webkit-font-smoothing: antialiased; background:#f6f6f6; margin:0 auto; padding:0;'>" +
               "<div style='background:#f4f4f4; border: 0px solid #f4f4f4; margin:0; padding:0'>" +
               "<center>" +
               "<table bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' style='background: #ffffff; border-collapse:collapse !important; max-width:650px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:100%'>" +
               "<tbody>" +
               "<tr>" +
               "<td align='center' bgcolor='#ffffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size:18px; line-height:28px; padding-bottom:0px; padding-top:0px valign='top'>" +
               "<a href='javascript:;' style='color: #7DA33A;text-decoration: none;display: block;' target='_blank'>" +
               "<img alt='Company Logo' src='http://evolutyz.in/img/evolutyz-logo.png'/>" +
               "</a>" +
               "</td>" +
               " </tr>" +
               " <tr>" +
               "<td align='center' bgcolor='#ffffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding-bottom: 20px; padding-top: 0px' valign='top'>" +
               " </td>" +
               "</tr>" +
               "<tr>" +
               "<td align='center' bgcolor='#fffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding-bottom: 20px; padding-top: 20px' valign=top>" +
               " <h2 align='center' style='color: #707070; font-weight: 400; margin:0; text-align: center'>Work From Home</h2>" +
               "</td>" +
               " </tr>" +
               "</tbody>" +
               "</table>" +
               "<table bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' class='mobile' style='background: #ffffff; border-collapse:collapse !important; max-width:650px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:100%'>" +
               "<tbody>" +
               "<tr>" +
               "<td align='center'>" +
               "<table bgcolor='#ffffff' cellpadding='0' cellspacing='0' class='mobile' style='background: #ffffff; border-collapse:collapse !important; border: 1px solid #d1d2d1; max-width:600px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:96%'>" +
               " <tbody>" +
               " <tr>" +
               "<td align=center bgcolor='#ffffff' style='background: #ffffff; padding-bottom:0px'; padding-top:0' valign='top'>" +
               "<table align='center' border='0' cellpadding = '0' cellspacing = '0' style = 'border-collapse: collapse !important; max-width:600px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:80%'>" +
               " <tbody>" +
               "<tr>" +
               "<td align='left' bgcolor='#ffffff' style='background: #ffffff; font-family:Lato, Helevetica, Arial, sans-serif; font-size:18px; line-height:28px; padding:10px 0px 20px' valign='top'>" +
               "<table align='center' border='0' cellpadding='0' cellspacing='0' style='border-collapse:collapse !important; max-width:600px; width:100%'>" +
               "<tbody>" +
               "<tr>" +
               "<td align='left' bgcolor='#ffffff' style = 'background: #ffffff; display: none; font-family:Lato, Helevetica, Arial, sans-serif; font-size:18px; max-width:80%; padding:0px; width:40%' valign='top'>" +
               "<p align='left' style='color: #707070; font-size:14px; font-weight:400; line-height:22px; padding-bottom:0px; text-align:left'>" +
               "<strong>" +
               "Employee ID" +
               " </strong>" +
               "<br>" +
               "</p>" +
               "</td>" +
               "<td align = 'right' bgcolor = '#ffffff' style = 'background: #ffffff; display: none; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10px 0px 0px' valign='top'>" +
               "<p align = 'right' style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: 'right'>" +
               getuserid +
               "</p >" +
               "</td>" +
               "</tr>" +
               "<tr>" +
               "<td align='left' bgcolor='#ffffff' style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 0px; width: 40%' valign='top'>" +
               "<p align='left' style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left' >" +
               "<strong>" +
               " Employee Name" +
               "</strong>" +
               "<br>" +
               " </p>" +
               "</td>" +
               "<td align = 'right' bgcolor = '#ffffff' style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10px 0px 0px' valign='top' >" +
               "<p align = 'right' style='color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right' >" +
               getuserfullname +
               " </p>" +
               " </td>" +
               "</tr>" +
               "</tbody>" +
               "</table>" +
               "<hr>" +
               "<table align ='center' border = '0' cellpadding = '0' cellspacing = '0' style = 'border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 100%'>" +
               " <tbody>" +
               "<tr>" +
               "<td align = 'left' bgcolor = '#ffffff' style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = 'top'>" +
               "<p align = 'left' style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left'>" +
               "<strong>" +
               " FromDate " +
               " </strong>" +
               "<br> " +
               "</p>" +
               " </td>" +
               "<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top>" +
               "<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
               "<strong>" +
               fromdate +
               "</strong>" +
               "</p>" +
               "</td>" +
               "</tr>" +
               "<tr>" +
               "<td align = left bgcolor = #ffffff style = background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = top>" +
               "<p align = left style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left' >" +
               "<strong> " +
               " ToDate " +
               " </strong>" +
               "<br>" +
               "</ p >" +
               "</ td >" +
               "<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top>" +
               "<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
               "<strong> " +
               todate +
               "</strong>" +
               "</p>" +
               "</td>" +
               "</tr>" +
                "<tr>" +
               "<td align = left bgcolor = #ffffff style = background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = top>" +
               "<p align = left style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left' >" +
               "<strong> " +
               " Calendar Days " +
               " </strong>" +
               "<br>" +
               "</ p >" +
               "</ td >" +
               "<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top>" +
               "<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
               "<strong> " +
               CalDates +
               "</strong>" +
               "</p>" +
               "</td>" +
               "</tr>" +
                "<tr>" +
               "<td align = left bgcolor = #ffffff style = background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = top>" +
               "<p align = left style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left' >" +
               "<strong> " +
               "No. Of Days Applied " +
               " </strong>" +
               "<br>" +
               "</ p >" +
               "</ td >" +
               "<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top>" +
               "<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
               "<strong> " +
                businessdays +
               "</strong>" +
               "</p>" +
               "</td>" +
               "</tr>" +
               "</tbody>" +
               "</table>" +
               "</td>" +
               "</tr >" +
               "</tbody>" +
               "</table>" +
               "<table align = center bgcolor = #ffffff border =0 cellpadding =0 cellspacing = 0 style = 'background: #ffffff; border-collapse: collapse !important; border-top-color: #D1D2D1; border-top-style: solid; border-top-width: 1px; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 100%'>" +
               "<tbody>" +
               " <tr>" +
               "<td align = center bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding: 10x 0px 20px' valign = top >" +
               "<table align = center border = 0 cellpadding = 0 cellspacing = 0 style = 'border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 80%'>" +
               "<tbody >" +
               "<tr>" +
               " <td align = center bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = top >" +
               "<p align = left style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left' >" +
               "<strong> " +
               "Comments " +
               "</strong>" +
               "</p>" +
               "</td>" +
               "<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top >" +
               "<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
               "<strong>" +
               "----" +
               "</strong>" +
               "</p>" +
               "</td>" +
               "</tr>" +
               "</tbody>" +
               "</table>" +
               "</td>" +
               "</tr>" +
               "</tbody>" +
               "</table>" +
               " </td>" +
               "</tr>" +
               "</tbody>" +
               " </table>" +
               "<table bgcolor = #ffffff cellpadding =0 cellspacing =0 class=mobile style='background: #ffffff; border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 96%'>" +
               "<tbody>" +
               "<tr>" +
               "<td align = center style='padding-bottom: 20px' valign=top>" +
               "<table align = center border=0 cellpadding=0 cellspacing=0 style='border-collapse: collapse !important; mso-table-lspace: 0pt; mso-table-rspace: 0pt'>" +
               "<tbody>" +
               "<tr>" +
               "<td align = center style='padding-bottom: 0px; padding-top: 30px' valign=top>" +
               "<a href = 'http://" + UrlEmailAddress + "/LeaveApplicationManagement/WorkFromHomeStatus?accemail=" + mail + "&userid=" + Uid + "&status=" + 1 + "&userwfhId=" + userwfhid + "&managermail=" + newemail + "&managerid=" + newid + "&managername=" + newname + "&startDate=" + fromdate + "&endDate=" + todate + "'javascript:; style='-moz-border-radius: 3px; -webkit-border-radius: 3px; border-radius: 3px; display: inline-block; font: bold 12px Arial, Helvetica, Clean, sans-serif; margin: 0 15px 10px 0; padding: 10px 20px; position: relative; text-align: center;background: #f997b0; background: -webkit-gradient(linear, 0 0, 0 bottom, from(#cae285), to(#a3cd5a)); background: -moz-linear-gradient(#cae285, #a3cd5a); background: linear-gradient(#cae285, #a3cd5a); border: solid 1px #aad063; border-bottom: solid 3px #799545; box-shadow: inset 0 0 0 1px #e0eeb6; color: #5d7731; text-shadow: 0 1px 0 #d0e5a4; text-decoration: none' >Approve</a>" +
               "<a href = 'http://" + UrlEmailAddress + "/LeaveApplicationManagement/WorkFromHomeStatus?accemail=" + mail + "&userid=" + Uid + "&status=" + 2 + "&userwfhId=" + userwfhid + "&managermail=" + newemail + "&managerid=" + newid + "&managername=" + newname + "&startDate=" + fromdate + "&endDate=" + todate + "'javascript:; style='-moz-border-radius: 3px; -webkit-border-radius: 3px; border-radius: 3px; display: inline-block; font: bold 12px Arial, Helvetica, Clean, sans-serif; margin: 0 15px 10px 0; padding: 10px 20px; position: relative; text-align: center;background: #cae285; background: -webkit-gradient(linear, 0 0, 0 bottom, from(#f997b0), to(#f56778)); background: -moz-linear-gradient(#f997b0, #f56778); background: linear-gradient(#f997b0, #f56778); border: solid 1px #ee8090; border-bottom: solid 3px #cb5462; box-shadow: inset 0 0 0 1px #fbc1d0; color: #913944; text-shadow: 0 1px 0 #f9a0ad; text-decoration: none' >Reject</a>" +
               "<a href = 'http://" + UrlEmailAddress + "/LeaveApplicationManagement/WorkFromHomeStatus?accemail=" + mail + "&userid=" + Uid + "&status=" + 3 + "&userwfhId=" + userwfhid + "&managermail=" + newemail + "&managerid=" + newid + "&managername=" + newname + "&startDate=" + fromdate + "&endDate=" + todate + "'javascript:; style='-moz-border-radius: 3px; -webkit-border-radius: 3px; border-radius: 3px; display: inline-block; font: bold 12px Arial, Helvetica, Clean, sans-serif; margin: 0 0px 10px 0; padding: 10px 20px; position: relative; text-align: center;background: #feda71; background: -webkit-gradient(linear, 0 0, 0 bottom, from(#feda71), to(#febe4d)); background: -moz-linear-gradient(#feda71, #febe4d); background: linear-gradient(#feda71, #febe4d); border: solid 1px #eab551; border-bottom: solid 3px #b98a37; box-shadow: inset 0 0 0 1px #fee9aa; color: #996633; text-shadow: 0 1px 0 #fedd9b; text-decoration: none'>Hold</a>" +
               "</td>" +
               "</tr>" +
               "</tbody>" +
               "</table>" +
               "</td>" +
               "</tr>" +
               "</tbody>" +
               "</table>" +
               "</td>" +
               "</tr>" +
               "</tbody>" +
               "</table>" +
               "</center>" +
               "</div>" +
               "</body>" +
               "</html>";
                client = new RestClient();
                client.BaseUrl = new Uri("https://api.mailgun.net/v3");
                client.Authenticator = new HttpBasicAuthenticator("api", "key-25b8e8c23e4a09ef67b7957d77eb7413");
                request = new RestRequest();
                request.AddParameter("domain", "evolutyzstaging.com", ParameterType.UrlSegment);
                request.Resource = "{domain}/messages";
                request.AddParameter("from", "Evolutyz ITServices <postmaster@evolutyzstaging.com>");
                request.AddParameter("to", manager1email);
                request.AddParameter("subject", "Work From Home Application from " + getuserfullname);
                request.AddParameter("html", emailcontent);
                request.Method = Method.POST;
                client.Execute(request);
            }
            return null;

        }



        public string WorkFromHomeStatus(string accemail, string userid, string status, string userwfhId, string managermail, string managerid, string managername, string startDate, string endDate)
        {

            string LeavId = String.Empty, mangrmail = String.Empty, mangrname = String.Empty, mangrid = String.Empty;

            Decript objdecrypt = new Decript();
            var newemail = accmail;
            newemail = objdecrypt.Decryption(HttpUtility.UrlDecode(Request.QueryString["accemail"]));
             usrid = objdecrypt.Decryption(HttpUtility.UrlDecode(Request.QueryString["userid"]));
             leaveeid = objdecrypt.Decryption(HttpUtility.UrlDecode(Request.QueryString["userwfhId"]));
             manageremails = objdecrypt.Decryption(HttpUtility.UrlDecode(Request.QueryString["managermail"]));
             mngrid = objdecrypt.Decryption(HttpUtility.UrlDecode(Request.QueryString["managerid"]));
             mngrname = objdecrypt.Decryption(HttpUtility.UrlDecode(Request.QueryString["managername"]));

            LeavId = newuwfhid;
            mangrmail = email;
            mangrid = id;
            mangrname = name;

            EvolutyzCornerDataEntities db = new EvolutyzCornerDataEntities();
            var getusermailid = (from u in db.UsersProfiles where u.UsrP_UserID == uid select u.UsrP_EmailID).FirstOrDefault();
            var emailcontent = "";

            ManagerWFHforApproval obj = new ManagerWFHforApproval();
            obj = new ManagerWFHforApproval()
            {
                Usrl_UserId = Convert.ToInt32(usrid),
                UserwfhID = Convert.ToInt32(leaveeid),
                ManagerID1 = Convert.ToInt32(mngrid),
                ManagerName1 = mngrname,
                Leavestatus = status.ToString(),

            };
            obj = CheckMailWFHApproval(obj);

            if (obj.Message == "Already approved")
            {
                return obj.Message;
            }

            else if (obj.Message == "Already Rejected")
            {
                return obj.Message;
            }
            else if (obj.Message == "On Hold")
            {
                return obj.Message;
            }
            //else if(obj.Message == "Leave Already Approved")
            //{
            //    return obj.Message;
            //}
            //else if(obj.Message == "Leave Already Rejected")
            //{
            //    return obj.Message;
            //}
            else
            {

                if (status == "1")
                {
                    emailcontent = "<html>" +
                    "<body bgcolor='#f6f6f6' style='-webkit-font-smoothing: antialiased; background:#f6f6f6; margin:0 auto; padding:0;'>" +
                    "<div style='background:#f4f4f4; border: 0px solid #f4f4f4; margin:0; padding:0'>" +
                    "<center>" +
                    "<table bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' style='background: #ffffff; border-collapse:collapse !important; max-width:650px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:100%'>" +
                    "<tbody>" +
                    "<tr>" +
                    "<td align='center' bgcolor='#ffffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size:18px; line-height:28px; padding-bottom:0px; padding-top:0px valign='top'>" +
                    "<a href='javascript:;' style='color: #7DA33A;text-decoration: none;display: block;' target='_blank'>" +
                    "<img alt='Company Logo' src='http://evolutyz.in/img/evolutyz-logo.png'/>" +
                    "</a>" +
                    "</td>" +
                    " </tr>" +
                    " <tr>" +
                    "<td align='center' bgcolor='#ffffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding-bottom: 20px; padding-top: 0px' valign='top'>" +
                    " </td>" +
                    "</tr>" +
                    "<tr>" +
                    "<td align='center' bgcolor='#fffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding-bottom: 20px; padding-top: 20px' valign=top>" +
                    " <h2 align='center' style='color: #707070; font-weight: 400; margin:0; text-align: center'>Leave Application</h2>" +
                    "</td>" +
                    " </tr>" +
                    "</tbody>" +
                    "</table>" +
                    "<table bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' class='mobile' style='background: #ffffff; border-collapse:collapse !important; max-width:650px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:100%'>" +
                    "<tbody>" +
                    "<tr>" +
                    "<td align='center'>" +
                    "<table bgcolor='#ffffff' cellpadding='0' cellspacing='0' class='mobile' style='background: #ffffff; border-collapse:collapse !important; border: 1px solid #d1d2d1; max-width:600px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:96%'>" +
                    " <tbody>" +
                    " <tr>" +
                    "<td align=center bgcolor='#ffffff' style='background: #ffffff; padding-bottom:0px'; padding-top:0' valign='top'>" +
                    "<table align='center' border='0' cellpadding = '0' cellspacing = '0' style = 'border-collapse: collapse !important; max-width:600px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:80%'>" +
                    " <tbody>" +
                    "<tr>" +
                    "<td align='left' bgcolor='#ffffff' style='background: #ffffff; font-family:Lato, Helevetica, Arial, sans-serif; font-size:18px; line-height:28px; padding:10px 0px 20px' valign='top'>" +
                    "<table align='center' border='0' cellpadding='0' cellspacing='0' style='border-collapse:collapse !important; max-width:600px; width:100%'>" +
                    "<tbody>" +
                    "<tr>" +
                    "<td align='left' bgcolor='#ffffff' style=background:'#ffffff; font-family:Lato, Helevetica, Arial, sans-serif; font-size:18px; max-width:80%; padding:0px; width:40%' valign='top'>" +
                    "<p align='left' style='color: #707070; font-size:14px; font-weight:400; line-height:22px; padding-bottom:0px; text-align:left'>" +
                    "<strong>" +
                    "Work From Home Approved By" +
                    " </strong>" +
                    "<br>" +
                    "</p>" +
                    "</td>" +
                    "<td align = 'right' bgcolor = '#ffffff' style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10px 0px 0px' valign='top'>" +
                    "<p align = 'right' style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: 'right'>" +
                     name +
                    "</p >" +
                    "</td>" +
                    "</tr>" +
                    "</tbody>" +
                    "</table>" +
                    "<hr>" +
                    "<table align ='center' border = '0' cellpadding = '0' cellspacing = '0' style = 'border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 100%'>" +
                    " <tbody>" +
                    "<tr>" +
                    "<td align = 'left' bgcolor = '#ffffff' style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = 'top'>" +
                    "<p align = 'left' style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left'>" +
                    "<strong>" +
                    " FromDate " +
                    " </strong>" +
                    "<br> " +
                    "</p>" +
                    " </td>" +
                    "<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top>" +
                    "<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
                    "<strong>" +
                    startDate +
                    "</strong>" +
                    "</p>" +
                    "</td>" +
                    "</tr>" +
                    "<tr>" +
                    "<td align = left bgcolor = #ffffff style = background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = top>" +
                    "<p align = left style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left' >" +
                    "<strong> " +
                    " ToDate " +
                    " </strong>" +
                    "<br>" +
                    "</ p >" +
                    "</ td >" +
                    "<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top>" +
                    "<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
                    "<strong> " +
                    endDate +
                    "</strong>" +
                    "</p>" +
                    "</td>" +
                    "</tr>" +
                    "</tbody>" +
                    "</table>" +
                    "</td>" +
                    "</tr >" +
                    "</tbody>" +
                    "</table>" +
                    "<table align = center bgcolor = #ffffff border =0 cellpadding =0 cellspacing = 0 style = 'background: #ffffff; border-collapse: collapse !important; border-top-color: #D1D2D1; border-top-style: solid; border-top-width: 1px; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 100%'>" +
                    "<tbody>" +
                    " <tr>" +
                    "<td align = center bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding: 10x 0px 20px' valign = top >" +
                    "<table align = center border = 0 cellpadding = 0 cellspacing = 0 style = 'border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 80%'>" +
                    "<tbody >" +
                    //"<tr>" +
                    //" <td align = center bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = top >" +
                    //"<p align = left style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left' >" +
                    //"<strong> " +
                    //"Comments " +
                    //"</strong>" +
                    //"</p>" +
                    //"</td>" +
                    //"<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top >" +
                    //"<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
                    //"<strong>" +
                    //"----" +
                    //"</strong>" +
                    //"</p>" +
                    //"</td>" +
                    //"</tr>" +
                    "</tbody>" +
                    "</table>" +
                    "</td>" +
                    "</tr>" +
                    "</tbody>" +
                    "</table>" +
                    " </td>" +
                    "</tr>" +
                    "</tbody>" +
                    " </table>" +
                    "<table bgcolor = #ffffff cellpadding =0 cellspacing =0 class=mobile style='background: #ffffff; border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 96%'>" +
                    "<tbody>" +
                    "<tr>" +
                    "<td align = center style='padding-bottom: 20px' valign=top>" +
                    "<table align = center border=0 cellpadding=0 cellspacing=0 style='border-collapse: collapse !important; mso-table-lspace: 0pt; mso-table-rspace: 0pt'>" +
                    "<tbody>" +
                    "<tr>" +
                    "<td align = center style='padding-bottom: 0px; padding-top: 30px' valign=top>" +
                    "</td>" +
                    "</tr>" +
                    "</tbody>" +
                    "</table>" +
                    "</td>" +
                    "</tr>" +
                    "</tbody>" +
                    "</table>" +
                    "</td>" +
                    "</tr>" +
                    "</tbody>" +
                    "</table>" +
                    "</center>" +
                    "</div>" +
                    "</body>" +
                    "</html>";
                    RestClient client = new RestClient();
                    client.BaseUrl = new Uri("https://api.mailgun.net/v3");
                    client.Authenticator = new HttpBasicAuthenticator("api", "key-25b8e8c23e4a09ef67b7957d77eb7413");
                    RestRequest request = new RestRequest();
                    request.AddParameter("domain", "evolutyzstaging.com", ParameterType.UrlSegment);
                    request.Resource = "{domain}/messages";
                    request.AddParameter("from", "Evolutyz ITServices <postmaster@evolutyzstaging.com>");
                    request.AddParameter("to", getusermailid);
                    request.AddParameter("to", newemail);
                    request.AddParameter("subject", "Work From Home Approved");
                    request.AddParameter("html", emailcontent);
                    request.Method = Method.POST;
                    client.Execute(request);
                }
                else
                {
                    if (status == "2")
                    {
                        emailcontent = "<html>" +
                   "<body bgcolor='#f6f6f6' style='-webkit-font-smoothing: antialiased; background:#f6f6f6; margin:0 auto; padding:0;'>" +
                   "<div style='background:#f4f4f4; border: 0px solid #f4f4f4; margin:0; padding:0'>" +
                   "<center>" +
                   "<table bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' style='background: #ffffff; border-collapse:collapse !important; max-width:650px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:100%'>" +
                   "<tbody>" +
                   "<tr>" +
                   "<td align='center' bgcolor='#ffffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size:18px; line-height:28px; padding-bottom:0px; padding-top:0px valign='top'>" +
                   "<a href='javascript:;' style='color: #7DA33A;text-decoration: none;display: block;' target='_blank'>" +
                   "<img alt='Company Logo' src='http://evolutyz.in/img/evolutyz-logo.png'/>" +
                   "</a>" +
                   "</td>" +
                   " </tr>" +
                   " <tr>" +
                   "<td align='center' bgcolor='#ffffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding-bottom: 20px; padding-top: 0px' valign='top'>" +
                   " </td>" +
                   "</tr>" +
                   "<tr>" +
                   "<td align='center' bgcolor='#fffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding-bottom: 20px; padding-top: 20px' valign=top>" +
                   " <h2 align='center' style='color: #707070; font-weight: 400; margin:0; text-align: center'>Leave Application</h2>" +
                   "</td>" +
                   " </tr>" +
                   "</tbody>" +
                   "</table>" +
                   "<table bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' class='mobile' style='background: #ffffff; border-collapse:collapse !important; max-width:650px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:100%'>" +
                   "<tbody>" +
                   "<tr>" +
                   "<td align='center'>" +
                   "<table bgcolor='#ffffff' cellpadding='0' cellspacing='0' class='mobile' style='background: #ffffff; border-collapse:collapse !important; border: 1px solid #d1d2d1; max-width:600px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:96%'>" +
                   " <tbody>" +
                   " <tr>" +
                   "<td align=center bgcolor='#ffffff' style='background: #ffffff; padding-bottom:0px'; padding-top:0' valign='top'>" +
                   "<table align='center' border='0' cellpadding = '0' cellspacing = '0' style = 'border-collapse: collapse !important; max-width:600px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:80%'>" +
                   " <tbody>" +
                   "<tr>" +
                   "<td align='left' bgcolor='#ffffff' style='background: #ffffff; font-family:Lato, Helevetica, Arial, sans-serif; font-size:18px; line-height:28px; padding:10px 0px 20px' valign='top'>" +
                   "<table align='center' border='0' cellpadding='0' cellspacing='0' style='border-collapse:collapse !important; max-width:600px; width:100%'>" +
                   "<tbody>" +
                   "<tr>" +
                   "<td align='left' bgcolor='#ffffff' style=background:'#ffffff; font-family:Lato, Helevetica, Arial, sans-serif; font-size:18px; max-width:80%; padding:0px; width:40%' valign='top'>" +
                   "<p align='left' style='color: #707070; font-size:14px; font-weight:400; line-height:22px; padding-bottom:0px; text-align:left'>" +
                   "<strong>" +
                   "Leave Rejected By" +
                   " </strong>" +
                   "<br>" +
                   "</p>" +
                   "</td>" +
                   "<td align = 'right' bgcolor = '#ffffff' style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10px 0px 0px' valign='top'>" +
                   "<p align = 'right' style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: 'right'>" +
                    name +
                   "</p >" +
                   "</td>" +
                   "</tr>" +
                   "</tbody>" +
                   "</table>" +
                   "<hr>" +
                   "<table align ='center' border = '0' cellpadding = '0' cellspacing = '0' style = 'border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 100%'>" +
                   " <tbody>" +
                   "<tr>" +
                   "<td align = 'left' bgcolor = '#ffffff' style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = 'top'>" +
                   "<p align = 'left' style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left'>" +
                   "<strong>" +
                   " FromDate " +
                   " </strong>" +
                   "<br> " +
                   "</p>" +
                   " </td>" +
                   "<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top>" +
                   "<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
                   "<strong>" +
                   startDate +
                   "</strong>" +
                   "</p>" +
                   "</td>" +
                   "</tr>" +
                   "<tr>" +
                   "<td align = left bgcolor = #ffffff style = background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = top>" +
                   "<p align = left style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left' >" +
                   "<strong> " +
                   " ToDate " +
                   " </strong>" +
                   "<br>" +
                   "</ p >" +
                   "</ td >" +
                   "<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top>" +
                   "<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
                   "<strong> " +
                   endDate +
                   "</strong>" +
                   "</p>" +
                   "</td>" +
                   "</tr>" +
                   "</tbody>" +
                   "</table>" +
                   "</td>" +
                   "</tr >" +
                   "</tbody>" +
                   "</table>" +
                   "<table align = center bgcolor = #ffffff border =0 cellpadding =0 cellspacing = 0 style = 'background: #ffffff; border-collapse: collapse !important; border-top-color: #D1D2D1; border-top-style: solid; border-top-width: 1px; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 100%'>" +
                   "<tbody>" +
                   " <tr>" +
                   "<td align = center bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding: 10x 0px 20px' valign = top >" +
                   "<table align = center border = 0 cellpadding = 0 cellspacing = 0 style = 'border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 80%'>" +
                   "<tbody >" +
                   //"<tr>" +
                   //" <td align = center bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = top >" +
                   //"<p align = left style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left' >" +
                   //"<strong> " +
                   //"Comments " +
                   //"</strong>" +
                   //"</p>" +
                   //"</td>" +
                   //"<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top >" +
                   //"<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
                   //"<strong>" +
                   //"----" +
                   //"</strong>" +
                   //"</p>" +
                   //"</td>" +
                   //"</tr>" +
                   "</tbody>" +
                   "</table>" +
                   "</td>" +
                   "</tr>" +
                   "</tbody>" +
                   "</table>" +
                   " </td>" +
                   "</tr>" +
                   "</tbody>" +
                   " </table>" +
                   "<table bgcolor = #ffffff cellpadding =0 cellspacing =0 class=mobile style='background: #ffffff; border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 96%'>" +
                   "<tbody>" +
                   "<tr>" +
                   "<td align = center style='padding-bottom: 20px' valign=top>" +
                   "<table align = center border=0 cellpadding=0 cellspacing=0 style='border-collapse: collapse !important; mso-table-lspace: 0pt; mso-table-rspace: 0pt'>" +
                   "<tbody>" +
                   "<tr>" +
                   "<td align = center style='padding-bottom: 0px; padding-top: 30px' valign=top>" +
                   "</td>" +
                   "</tr>" +
                   "</tbody>" +
                   "</table>" +
                   "</td>" +
                   "</tr>" +
                   "</tbody>" +
                   "</table>" +
                   "</td>" +
                   "</tr>" +
                   "</tbody>" +
                   "</table>" +
                   "</center>" +
                   "</div>" +
                   "</body>" +
                   "</html>";
                        RestClient client = new RestClient();
                        client.BaseUrl = new Uri("https://api.mailgun.net/v3");
                        client.Authenticator = new HttpBasicAuthenticator("api", "key-25b8e8c23e4a09ef67b7957d77eb7413");
                        RestRequest request = new RestRequest();
                        request.AddParameter("domain", "evolutyzstaging.com", ParameterType.UrlSegment);
                        request.Resource = "{domain}/messages";
                        request.AddParameter("from", "Evolutyz ITServices <postmaster@evolutyzstaging.com>");
                        request.AddParameter("to", getusermailid);
                        request.AddParameter("to", newemail);
                        request.AddParameter("subject", "Work From Home Rejected");
                        request.AddParameter("html", emailcontent);
                        request.Method = Method.POST;
                        client.Execute(request);
                    }
                    else
                    {
                        emailcontent = "<html>" +
                "<body bgcolor='#f6f6f6' style='-webkit-font-smoothing: antialiased; background:#f6f6f6; margin:0 auto; padding:0;'>" +
                "<div style='background:#f4f4f4; border: 0px solid #f4f4f4; margin:0; padding:0'>" +
                "<center>" +
                "<table bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' style='background: #ffffff; border-collapse:collapse !important; max-width:650px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:100%'>" +
                "<tbody>" +
                "<tr>" +
                "<td align='center' bgcolor='#ffffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size:18px; line-height:28px; padding-bottom:0px; padding-top:0px valign='top'>" +
                "<a href='javascript:;' style='color: #7DA33A;text-decoration: none;display: block;' target='_blank'>" +
                "<img alt='Company Logo' src='http://evolutyz.in/img/evolutyz-logo.png'/>" +
                "</a>" +
                "</td>" +
                " </tr>" +
                " <tr>" +
                "<td align='center' bgcolor='#ffffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding-bottom: 20px; padding-top: 0px' valign='top'>" +
                " </td>" +
                "</tr>" +
                "<tr>" +
                "<td align='center' bgcolor='#fffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding-bottom: 20px; padding-top: 20px' valign=top>" +
                " <h2 align='center' style='color: #707070; font-weight: 400; margin:0; text-align: center'>Leave Application</h2>" +
                "</td>" +
                " </tr>" +
                "</tbody>" +
                "</table>" +
                "<table bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' class='mobile' style='background: #ffffff; border-collapse:collapse !important; max-width:650px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:100%'>" +
                "<tbody>" +
                "<tr>" +
                "<td align='center'>" +
                "<table bgcolor='#ffffff' cellpadding='0' cellspacing='0' class='mobile' style='background: #ffffff; border-collapse:collapse !important; border: 1px solid #d1d2d1; max-width:600px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:96%'>" +
                " <tbody>" +
                " <tr>" +
                "<td align=center bgcolor='#ffffff' style='background: #ffffff; padding-bottom:0px'; padding-top:0' valign='top'>" +
                "<table align='center' border='0' cellpadding = '0' cellspacing = '0' style = 'border-collapse: collapse !important; max-width:600px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:80%'>" +
                " <tbody>" +
                "<tr>" +
                "<td align='left' bgcolor='#ffffff' style='background: #ffffff; font-family:Lato, Helevetica, Arial, sans-serif; font-size:18px; line-height:28px; padding:10px 0px 20px' valign='top'>" +
                "<table align='center' border='0' cellpadding='0' cellspacing='0' style='border-collapse:collapse !important; max-width:600px; width:100%'>" +
                "<tbody>" +
                "<tr>" +
                "<td align='left' bgcolor='#ffffff' style=background:'#ffffff; font-family:Lato, Helevetica, Arial, sans-serif; font-size:18px; max-width:80%; padding:0px; width:40%' valign='top'>" +
                "<p align='left' style='color: #707070; font-size:14px; font-weight:400; line-height:22px; padding-bottom:0px; text-align:left'>" +
                "<strong>" +
                "Your Leave was kept on hold by" +
                " </strong>" +
                "<br>" +
                "</p>" +
                "</td>" +
                "<td align = 'right' bgcolor = '#ffffff' style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10px 0px 0px' valign='top'>" +
                "<p align = 'right' style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: 'right'>" +
                 name +
                "</p >" +
                "</td>" +
                "</tr>" +
                "</tbody>" +
                "</table>" +
                "<hr>" +
                "<table align ='center' border = '0' cellpadding = '0' cellspacing = '0' style = 'border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 100%'>" +
                " <tbody>" +
                "<tr>" +
                "<td align = 'left' bgcolor = '#ffffff' style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = 'top'>" +
                "<p align = 'left' style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left'>" +
                "<strong>" +
                " FromDate " +
                " </strong>" +
                "<br> " +
                "</p>" +
                " </td>" +
                "<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top>" +
                "<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
                "<strong>" +
                startDate +
                "</strong>" +
                "</p>" +
                "</td>" +
                "</tr>" +
                "<tr>" +
                "<td align = left bgcolor = #ffffff style = background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = top>" +
                "<p align = left style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left' >" +
                "<strong> " +
                " ToDate " +
                " </strong>" +
                "<br>" +
                "</ p >" +
                "</ td >" +
                "<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top>" +
                "<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
                "<strong> " +
                endDate +
                "</strong>" +
                "</p>" +
                "</td>" +
                "</tr>" +
                "</tbody>" +
                "</table>" +
                "</td>" +
                "</tr >" +
                "</tbody>" +
                "</table>" +
                "<table align = center bgcolor = #ffffff border =0 cellpadding =0 cellspacing = 0 style = 'background: #ffffff; border-collapse: collapse !important; border-top-color: #D1D2D1; border-top-style: solid; border-top-width: 1px; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 100%'>" +
                "<tbody>" +
                " <tr>" +
                "<td align = center bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding: 10x 0px 20px' valign = top >" +
                "<table align = center border = 0 cellpadding = 0 cellspacing = 0 style = 'border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 80%'>" +
                "<tbody >" +
                //"<tr>" +
                //" <td align = center bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = top >" +
                //"<p align = left style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left' >" +
                //"<strong> " +
                //"Comments " +
                //"</strong>" +
                //"</p>" +
                //"</td>" +
                //"<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top >" +
                //"<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
                //"<strong>" +
                //"----" +
                //"</strong>" +
                //"</p>" +
                //"</td>" +
                //"</tr>" +
                "</tbody>" +
                "</table>" +
                "</td>" +
                "</tr>" +
                "</tbody>" +
                "</table>" +
                " </td>" +
                "</tr>" +
                "</tbody>" +
                " </table>" +
                "<table bgcolor = #ffffff cellpadding =0 cellspacing =0 class=mobile style='background: #ffffff; border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 96%'>" +
                "<tbody>" +
                "<tr>" +
                "<td align = center style='padding-bottom: 20px' valign=top>" +
                "<table align = center border=0 cellpadding=0 cellspacing=0 style='border-collapse: collapse !important; mso-table-lspace: 0pt; mso-table-rspace: 0pt'>" +
                "<tbody>" +
                "<tr>" +
                "<td align = center style='padding-bottom: 0px; padding-top: 30px' valign=top>" +
                "</td>" +
                "</tr>" +
                "</tbody>" +
                "</table>" +
                "</td>" +
                "</tr>" +
                "</tbody>" +
                "</table>" +
                "</td>" +
                "</tr>" +
                "</tbody>" +
                "</table>" +
                "</center>" +
                "</div>" +
                "</body>" +
                "</html>";

                        RestClient client = new RestClient();
                        client.BaseUrl = new Uri("https://api.mailgun.net/v3");
                        client.Authenticator = new HttpBasicAuthenticator("api", "key-25b8e8c23e4a09ef67b7957d77eb7413");
                        RestRequest request = new RestRequest();
                        request.AddParameter("domain", "evolutyzstaging.com", ParameterType.UrlSegment);
                        request.Resource = "{domain}/messages";
                        request.AddParameter("from", "Evolutyz ITServices <postmaster@evolutyzstaging.com>");
                        request.AddParameter("to", getusermailid);
                        request.AddParameter("to", newemail);
                        request.AddParameter("subject", "Work from Home OnHold");
                        request.AddParameter("html", emailcontent);
                        request.Method = Method.POST;
                        client.Execute(request);
                    }
                }

                AdminComponent admc = new AdminComponent();

                var res = admc.SaveWorkFromHomeStatus(newuwfhid, status, id);
            }

            return null;

        }

        public static string MailLeaveApplication(string LeaveStartDate, string LeaveEndDate, int businessdays, int CalDates, int leaveid)
        {

            UserSessionInfo objSessioninfo = new UserSessionInfo();
            string UrlEmailAddress = string.Empty;

            if (host == "localhost")
            {
                UrlEmailAddress = host + ":" + port;
            }
            else
            {
                UrlEmailAddress = port1;
            }

            newleavid = leaveid.ToString();
            Encrypt objenc = new Encrypt();
            RestClient client = new RestClient();
            RestRequest request = new RestRequest();
            mail = objenc.Encryption(accmail);
            var leavid = objenc.Encryption(leaveid.ToString());
            uid = objSessioninfo.UserId;
            EvolutyzCornerDataEntities db = new EvolutyzCornerDataEntities();
            var getuserid = (from u in db.UsersProfiles where u.UsrP_UserID == uid select u.UsrP_EmployeeID).FirstOrDefault();
            var getempname = (from u in db.UsersProfiles where u.UsrP_UserID == uid select u.UsrP_FirstName).FirstOrDefault();
            var getlastname = (from u in db.UsersProfiles where u.UsrP_UserID == uid select u.UsrP_LastName).FirstOrDefault();
            var getuserfullname = getempname + "  " + getlastname;
            UserSessionInfo info = new UserSessionInfo();
            AdminComponent admcmp = new AdminComponent();
            string userid = (info.UserId).ToString();
            var Uid = objenc.Encryption(userid);
            var res = admcmp.GetAllManagersEmails(userid);
            var manager1 = res.Select(a => a.ManagerLevel1).FirstOrDefault();
            var manager2 = res.Select(a => a.ManagerLevel2).FirstOrDefault();
            string[] manager1emailids = manager1.Split('/');
            string[] manageremailids = manager2.Split('/');
            var manager1email = manager1emailids[0];
            var manager1id = manager1emailids[1];
            var manager1name = manager1emailids[2];
            var manager2email = manageremailids[0];
            var manager2id = manageremailids[1];
            var manager2name = manageremailids[2];
            var objectToSerialize = new managerjson();
            objectToSerialize.items = new List<manageremails>
                          {
                             new manageremails { manageremail = manager1email, managerid = manager1id, managername = manager1name },
                          };
            var emailcontent = "";
            string newemail = string.Empty;
            string newid = string.Empty;
            string newname = string.Empty;
            string newlevel = string.Empty;
            for (var i = 0; i <= objectToSerialize.items.Count - 1; i++)
            {
                email = objectToSerialize.items[i].manageremail;
                id = objectToSerialize.items[i].managerid;
                name = objectToSerialize.items[i].managername;
                newemail = objenc.Encryption(email);
                newid = objenc.Encryption(id);
                newname = objenc.Encryption(name);

                emailcontent = "<html>" +
               "<body bgcolor='#f6f6f6' style='-webkit-font-smoothing: antialiased; background:#f6f6f6; margin:0 auto; padding:0;'>" +
               "<div style='background:#f4f4f4; border: 0px solid #f4f4f4; margin:0; padding:0'>" +
               "<center>" +
               "<table bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' style='background: #ffffff; border-collapse:collapse !important; max-width:650px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:100%'>" +
               "<tbody>" +
               "<tr>" +
               "<td align='center' bgcolor='#ffffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size:18px; line-height:28px; padding-bottom:0px; padding-top:0px valign='top'>" +
               "<a href='javascript:;' style='color: #7DA33A;text-decoration: none;display: block;' target='_blank'>" +
               "<img alt='Company Logo' src='http://evolutyz.in/img/evolutyz-logo.png'/>" +
               "</a>" +
               "</td>" +
               " </tr>" +
               " <tr>" +
               "<td align='center' bgcolor='#ffffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding-bottom: 20px; padding-top: 0px' valign='top'>" +
               " </td>" +
               "</tr>" +
               "<tr>" +
               "<td align='center' bgcolor='#fffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding-bottom: 20px; padding-top: 20px' valign=top>" +
               " <h2 align='center' style='color: #707070; font-weight: 400; margin:0; text-align: center'>Leave Application</h2>" +
               "</td>" +
               " </tr>" +
               "</tbody>" +
               "</table>" +
               "<table bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' class='mobile' style='background: #ffffff; border-collapse:collapse !important; max-width:650px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:100%'>" +
               "<tbody>" +
               "<tr>" +
               "<td align='center'>" +
               "<table bgcolor='#ffffff' cellpadding='0' cellspacing='0' class='mobile' style='background: #ffffff; border-collapse:collapse !important; border: 1px solid #d1d2d1; max-width:600px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:96%'>" +
               " <tbody>" +
               " <tr>" +
               "<td align=center bgcolor='#ffffff' style='background: #ffffff; padding-bottom:0px'; padding-top:0' valign='top'>" +
               "<table align='center' border='0' cellpadding = '0' cellspacing = '0' style = 'border-collapse: collapse !important; max-width:600px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:80%'>" +
               " <tbody>" +
               "<tr>" +
               "<td align='left' bgcolor='#ffffff' style='background: #ffffff; font-family:Lato, Helevetica, Arial, sans-serif; font-size:18px; line-height:28px; padding:10px 0px 20px' valign='top'>" +
               "<table align='center' border='0' cellpadding='0' cellspacing='0' style='border-collapse:collapse !important; max-width:600px; width:100%'>" +
               "<tbody>" +
               "<tr>" +
               "<td align='left' bgcolor='#ffffff' style = 'background: #ffffff; display: none; font-family:Lato, Helevetica, Arial, sans-serif; font-size:18px; max-width:80%; padding:0px; width:40%' valign='top'>" +
               "<p align='left' style='color: #707070; font-size:14px; font-weight:400; line-height:22px; padding-bottom:0px; text-align:left'>" +
               "<strong>" +
               "Employee ID" +
               " </strong>" +
               "<br>" +
               "</p>" +
               "</td>" +
               "<td align = 'right' bgcolor = '#ffffff' style = 'background: #ffffff; display: none; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10px 0px 0px' valign='top'>" +
               "<p align = 'right' style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: 'right'>" +
               getuserid +
               "</p >" +
               "</td>" +
               "</tr>" +
               "<tr>" +
               "<td align='left' bgcolor='#ffffff' style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 0px; width: 40%' valign='top'>" +
               "<p align='left' style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left' >" +
               "<strong>" +
               " Employee Name" +
               "</strong>" +
               "<br>" +
               " </p>" +
               "</td>" +
               "<td align = 'right' bgcolor = '#ffffff' style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10px 0px 0px' valign='top' >" +
               "<p align = 'right' style='color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right' >" +
               getuserfullname +
               " </p>" +
               " </td>" +
               "</tr>" +
               "</tbody>" +
               "</table>" +
               "<hr>" +
               "<table align ='center' border = '0' cellpadding = '0' cellspacing = '0' style = 'border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 100%'>" +
               " <tbody>" +
               "<tr>" +
               "<td align = 'left' bgcolor = '#ffffff' style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = 'top'>" +
               "<p align = 'left' style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left'>" +
               "<strong>" +
               " FromDate " +
               " </strong>" +
               "<br> " +
               "</p>" +
               " </td>" +
               "<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top>" +
               "<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
               "<strong>" +
               LeaveStartDate +
               "</strong>" +
               "</p>" +
               "</td>" +
               "</tr>" +
               "<tr>" +
               "<td align = left bgcolor = #ffffff style = background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = top>" +
               "<p align = left style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left' >" +
               "<strong> " +
               " ToDate " +
               " </strong>" +
               "<br>" +
               "</ p >" +
               "</ td >" +
               "<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top>" +
               "<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
               "<strong> " +
               LeaveEndDate +
               "</strong>" +
               "</p>" +
               "</td>" +
               "</tr>" +
               "</tbody>" +
               "</table>" +
               "</td>" +
               "</tr >" +
               "<tr>" +
               "<td align = left bgcolor = #ffffff style = background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = top>" +
               "<p align = left style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left' >" +
               "<strong> " +
               " Calendar Days " +
               " </strong>" +
               "<br>" +
               "</ p >" +
               "</ td >" +
               "<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top>" +
               "<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
               "<strong> " +
               CalDates +
               "</strong>" +
               "</p>" +
               "</td>" +
               "</tr>" +
                "<tr>" +
               "<td align = left bgcolor = #ffffff style = background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = top>" +
               "<p align = left style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left' >" +
               "<strong> " +
               "No. Of Days Applied " +
               " </strong>" +
               "<br>" +
               "</ p >" +
               "</ td >" +
               "<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top>" +
               "<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
               "<strong> " +
                businessdays +
               "</strong>" +
               "</p>" +
               "</td>" +
               "</tr>" +
               "</tbody>" +
               "</table>" +
               "<table align = center bgcolor = #ffffff border =0 cellpadding =0 cellspacing = 0 style = 'background: #ffffff; border-collapse: collapse !important; border-top-color: #D1D2D1; border-top-style: solid; border-top-width: 1px; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 100%'>" +
               "<tbody>" +
               " <tr>" +
               "<td align = center bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding: 10x 0px 20px' valign = top >" +
               "<table align = center border = 0 cellpadding = 0 cellspacing = 0 style = 'border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 80%'>" +
               "<tbody >" +
               //"<tr>" +
               //" <td align = center bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = top >" +
               //"<p align = left style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left' >" +
               //"<strong> " +
               //"Comments " +
               //"</strong>" +
               //"</p>" +
               //"</td>" +
               //"<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top >" +
               //"<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
               //"<strong>" +
               //"----" +
               //"</strong>" +
               //"</p>" +
               //"</td>" +
               //"</tr>" +
               "</tbody>" +
               "</table>" +
               "</td>" +
               "</tr>" +
               "</tbody>" +
               "</table>" +
               " </td>" +
               "</tr>" +
               "</tbody>" +
               " </table>" +
               "<table bgcolor = #ffffff cellpadding =0 cellspacing =0 class=mobile style='background: #ffffff; border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 96%'>" +
               "<tbody>" +
               "<tr>" +
               "<td align = center style='padding-bottom: 20px' valign=top>" +
               "<table align = center border=0 cellpadding=0 cellspacing=0 style='border-collapse: collapse !important; mso-table-lspace: 0pt; mso-table-rspace: 0pt'>" +
               "<tbody>" +
               "<tr>" +
               "<td align = center style='padding-bottom: 0px; padding-top: 30px' valign=top>" +
               //   "<a href = '" + "http://" + UrlEmailAddress + "/LeaveStatus?accemail="+mail+"&userid="+Uid+"&status="+1+"&leaveid="+leavid+"&managermail="+newemail+"&managerid="+newid+"&managername="+newname+"&startDate="+LeaveStartDate+"&endDate="+LeaveEndDate+"'javascript:; style='-moz-border-radius:3px;-webkit-border-radius: 3px; border-radius: 3px; display: inline-block; font: bold 12px Arial, Helvetica, Clean, sans-serif; margin: 0 15px 10px 0; padding: 10px 20px; position: relative; text-align: center;background: #f997b0; background: -webkit-gradient(linear, 0 0, 0 bottom, from(#cae285), to(#a3cd5a)); background: -moz-linear-gradient(#cae285, #a3cd5a); background: linear-gradient(#cae285, #a3cd5a); border: solid 1px #aad063; border-bottom: solid 3px #799545; box-shadow: inset 0 0 0 1px #e0eeb6; color: #5d7731; text-shadow: 0 1px 0 #d0e5a4; text-decoration: none' >Approved</a>" +
               "<a href = 'http://" + UrlEmailAddress + "/LeaveApplicationManagement/LeaveStatus?accemail=" + mail + "&userid=" + Uid + "&status=" + 1 + "&leaveid=" + leavid + "&managermail=" + newemail + "&managerid=" + newid + "&managername=" + newname + "&startDate=" + LeaveStartDate + "&endDate=" + LeaveEndDate + "'javascript:; style='-moz-border-radius:3px;-webkit-border-radius: 3px; border-radius: 3px; display: inline-block; font: bold 12px Arial, Helvetica, Clean, sans-serif; margin: 0 15px 10px 0; padding: 10px 20px; position: relative; text-align: center;background: #f997b0; background: -webkit-gradient(linear, 0 0, 0 bottom, from(#cae285), to(#a3cd5a)); background: -moz-linear-gradient(#cae285, #a3cd5a); background: linear-gradient(#cae285, #a3cd5a); border: solid 1px #aad063; border-bottom: solid 3px #799545; box-shadow: inset 0 0 0 1px #e0eeb6; color: #5d7731; text-shadow: 0 1px 0 #d0e5a4; text-decoration: none' >Approve</a>" +
               "<a href = 'http://" + UrlEmailAddress + "/LeaveApplicationManagement/LeaveStatus?accemail=" + mail + "&userid=" + Uid + "&status=" + 2 + "&leaveid=" + leavid + "&managermail=" + newemail + "&managerid=" + newid + "&managername=" + newname + "&startDate=" + LeaveStartDate + "&endDate=" + LeaveEndDate + "'javascript:; style=' -moz-border-radius: 3px; -webkit-border-radius: 3px; border-radius: 3px; display: inline-block; font: bold 12px Arial, Helvetica, Clean, sans-serif; margin: 0 15px 10px 0; padding: 10px 20px; position: relative; text-align: center;background: #cae285; background: -webkit-gradient(linear, 0 0, 0 bottom, from(#f997b0), to(#f56778)); background: -moz-linear-gradient(#f997b0, #f56778); background: linear-gradient(#f997b0, #f56778); border: solid 1px #ee8090; border-bottom: solid 3px #cb5462; box-shadow: inset 0 0 0 1px #fbc1d0; color: #913944; text-shadow: 0 1px 0 #f9a0ad; text-decoration: none' >Reject</a>" +
               "<a href = 'http://" + UrlEmailAddress + "/LeaveApplicationManagement/LeaveStatus?accemail=" + mail + "&userid=" + Uid + "&status=" + 3 + "&leaveid=" + leavid + "&managermail=" + newemail + "&managerid=" + newid + "&managername=" + newname + "&startDate=" + LeaveStartDate + "&endDate=" + LeaveEndDate + "'javascript:; style='-moz-border-radius: 3px; -webkit-border-radius: 3px; border-radius: 3px; display: inline-block; font: bold 12px Arial, Helvetica, Clean, sans-serif; margin: 0 0px 10px 0; padding: 10px 20px; position: relative; text-align: center;background: #feda71; background: -webkit-gradient(linear, 0 0, 0 bottom, from(#feda71), to(#febe4d)); background: -moz-linear-gradient(#feda71, #febe4d); background: linear-gradient(#feda71, #febe4d); border: solid 1px #eab551; border-bottom: solid 3px #b98a37; box-shadow: inset 0 0 0 1px #fee9aa; color: #996633; text-shadow: 0 1px 0 #fedd9b; text-decoration: none'>Hold</a>" +
               "</td>" +
               "</tr>" +
               "</tbody>" +
               "</table>" +
               "</td>" +
               "</tr>" +
               "</tbody>" +
               "</table>" +
               "</td>" +
               "</tr>" +
               "</tbody>" +
               "</table>" +
               "</center>" +
               "</div>" +
               "</body>" +
               "</html>";
                client = new RestClient();
                client.BaseUrl = new Uri("https://api.mailgun.net/v3");
                client.Authenticator = new HttpBasicAuthenticator("api", "key-25b8e8c23e4a09ef67b7957d77eb7413");
                request = new RestRequest();
                request.AddParameter("domain", "evolutyzstaging.com", ParameterType.UrlSegment);
                request.Resource = "{domain}/messages";
                request.AddParameter("from", "Evolutyz ITServices <postmaster@evolutyzstaging.com>");
                request.AddParameter("subject", "Leave Application from " + getuserfullname);
                request.AddParameter("to", email);
                request.AddParameter("html", emailcontent);
                request.Method = Method.POST;
                client.Execute(request);
            }
            return "";
        }

        string  usrid = string.Empty, leaveeid = string.Empty, manageremails = string.Empty, mngrid = string.Empty, mngrname = string.Empty, mgrlevel = string.Empty, status = string.Empty;
        public string LeaveStatus(string accemail, string userid, string status, string leaveid, string managermail, string managerid, string managername, string managerlevel, string startDate, string endDate)

        {
                string LeavId = String.Empty, mangrmail = String.Empty, mangrname = String.Empty, mangrid = String.Empty;
                Decript objdecrypt = new Decript();
                var newemail = accmail;
                 newemail = objdecrypt.Decryption(HttpUtility.UrlDecode(Request.QueryString["accemail"]));
                 usrid = objdecrypt.Decryption(HttpUtility.UrlDecode(Request.QueryString["userid"]));
                 leaveeid = objdecrypt.Decryption(HttpUtility.UrlDecode(Request.QueryString["leaveid"]));
                 manageremails = objdecrypt.Decryption(HttpUtility.UrlDecode(Request.QueryString["managermail"]));
                 mngrid = objdecrypt.Decryption(HttpUtility.UrlDecode(Request.QueryString["managerid"]));
                 mngrname = objdecrypt.Decryption(HttpUtility.UrlDecode(Request.QueryString["managername"]));
                 mgrlevel = HttpUtility.UrlDecode(Request.QueryString["managerlevel"]);
                
                int Usrid = uid;
                EvolutyzCornerDataEntities db = new EvolutyzCornerDataEntities();
                var getusermailid = (from u in db.UsersProfiles where u.UsrP_UserID == uid select u.UsrP_EmailID).FirstOrDefault();
                var emailcontent = "";
                ManagerLeavesforApprovals obj = new ManagerLeavesforApprovals();
                 obj = new ManagerLeavesforApprovals()
                 {
                   Usrl_UserId   = Convert.ToInt32(usrid),
                   Usrl_LeaveId= Convert.ToInt32(leaveeid),
                   ManagerID1 = Convert.ToInt32(mngrid),
                    ManagerName1= mngrname,
                    Leavestatus= status.ToString(),
          
                 };
                 obj = CheckMailLeaveApproval(obj);
                
            if (obj.Message == "Already approved")
            {
                return obj.Message;
            }

            else if (obj.Message == "Already Rejected")
            {
                return obj.Message;
            }
            else if (obj.Message == "On Hold")
            {
                return obj.Message;
            }
            else 
            {

                if (status == "1")
                {

                    emailcontent = "<html>" +
                    "<body bgcolor='#f6f6f6' style='-webkit-font-smoothing: antialiased; background:#f6f6f6; margin:0 auto; padding:0;'>" +
                    "<div style='background:#f4f4f4; border: 0px solid #f4f4f4; margin:0; padding:0'>" +
                    "<center>" +
                    "<table bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' style='background: #ffffff; border-collapse:collapse !important; max-width:650px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:100%'>" +
                    "<tbody>" +
                    "<tr>" +
                    "<td align='center' bgcolor='#ffffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size:18px; line-height:28px; padding-bottom:0px; padding-top:0px valign='top'>" +
                    "<a href='javascript:;' style='color: #7DA33A;text-decoration: none;display: block;' target='_blank'>" +
                    "<img alt='Company Logo' src='http://evolutyz.in/img/evolutyz-logo.png'/>" +
                    "</a>" +
                    "</td>" +
                    " </tr>" +
                    " <tr>" +
                    "<td align='center' bgcolor='#ffffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding-bottom: 20px; padding-top: 0px' valign='top'>" +
                    " </td>" +
                    "</tr>" +
                    "<tr>" +
                    "<td align='center' bgcolor='#fffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding-bottom: 20px; padding-top: 20px' valign=top>" +
                    " <h2 align='center' style='color: #707070; font-weight: 400; margin:0; text-align: center'>Leave Application</h2>" +
                    "</td>" +
                    " </tr>" +
                    "</tbody>" +
                    "</table>" +
                    "<table bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' class='mobile' style='background: #ffffff; border-collapse:collapse !important; max-width:650px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:100%'>" +
                    "<tbody>" +
                    "<tr>" +
                    "<td align='center'>" +
                    "<table bgcolor='#ffffff' cellpadding='0' cellspacing='0' class='mobile' style='background: #ffffff; border-collapse:collapse !important; border: 1px solid #d1d2d1; max-width:600px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:96%'>" +
                    " <tbody>" +
                    " <tr>" +
                    "<td align=center bgcolor='#ffffff' style='background: #ffffff; padding-bottom:0px'; padding-top:0' valign='top'>" +
                    "<table align='center' border='0' cellpadding = '0' cellspacing = '0' style = 'border-collapse: collapse !important; max-width:600px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:80%'>" +
                    " <tbody>" +
                    "<tr>" +
                    "<td align='left' bgcolor='#ffffff' style='background: #ffffff; font-family:Lato, Helevetica, Arial, sans-serif; font-size:18px; line-height:28px; padding:10px 0px 20px' valign='top'>" +
                    "<table align='center' border='0' cellpadding='0' cellspacing='0' style='border-collapse:collapse !important; max-width:600px; width:100%'>" +
                    "<tbody>" +
                    "<tr>" +
                    "<td align='left' bgcolor='#ffffff' style=background:'#ffffff; font-family:Lato, Helevetica, Arial, sans-serif; font-size:18px; max-width:80%; padding:0px; width:40%' valign='top'>" +
                    "<p align='left' style='color: #707070; font-size:14px; font-weight:400; line-height:22px; padding-bottom:0px; text-align:left'>" +
                    "<strong>" +
                    "Leave Approved By" +
                    " </strong>" +
                    "<br>" +
                    "</p>" +
                    "</td>" +
                    "<td align = 'right' bgcolor = '#ffffff' style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10px 0px 0px' valign='top'>" +
                    "<p align = 'right' style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: 'right'>" +
                     name +
                    "</p >" +
                    "</td>" +
                    "</tr>" +
                    "</tbody>" +
                    "</table>" +
                    "<hr>" +
                    "<table align ='center' border = '0' cellpadding = '0' cellspacing = '0' style = 'border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 100%'>" +
                    " <tbody>" +
                    "<tr>" +
                    "<td align = 'left' bgcolor = '#ffffff' style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = 'top'>" +
                    "<p align = 'left' style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left'>" +
                    "<strong>" +
                    " FromDate " +
                    " </strong>" +
                    "<br> " +
                    "</p>" +
                    " </td>" +
                    "<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top>" +
                    "<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
                    "<strong>" +
                    startDate +
                    "</strong>" +
                    "</p>" +
                    "</td>" +
                    "</tr>" +
                    "<tr>" +
                    "<td align = left bgcolor = #ffffff style = background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = top>" +
                    "<p align = left style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left' >" +
                    "<strong> " +
                    " ToDate " +
                    " </strong>" +
                    "<br>" +
                    "</ p >" +
                    "</ td >" +
                    "<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top>" +
                    "<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
                    "<strong> " +
                    endDate +
                    "</strong>" +
                    "</p>" +
                    "</td>" +
                    "</tr>" +
                    "</tbody>" +
                    "</table>" +
                    "</td>" +
                    "</tr >" +
                    "</tbody>" +
                    "</table>" +
                    "<table align = center bgcolor = #ffffff border =0 cellpadding =0 cellspacing = 0 style = 'background: #ffffff; border-collapse: collapse !important; border-top-color: #D1D2D1; border-top-style: solid; border-top-width: 1px; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 100%'>" +
                    "<tbody>" +
                    " <tr>" +
                    "<td align = center bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding: 10x 0px 20px' valign = top >" +
                    "<table align = center border = 0 cellpadding = 0 cellspacing = 0 style = 'border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 80%'>" +
                    "<tbody >" +
                    //"<tr>" +
                    //" <td align = center bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = top >" +
                    //"<p align = left style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left' >" +
                    //"<strong> " +
                    //"Comments " +
                    //"</strong>" +
                    //"</p>" +
                    //"</td>" +
                    //"<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top >" +
                    //"<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
                    //"<strong>" +
                    //"----" +
                    //"</strong>" +
                    //"</p>" +
                    //"</td>" +
                    //"</tr>" +
                    "</tbody>" +
                    "</table>" +
                    "</td>" +
                    "</tr>" +
                    "</tbody>" +
                    "</table>" +
                    " </td>" +
                    "</tr>" +
                    "</tbody>" +
                    " </table>" +
                    "<table bgcolor = #ffffff cellpadding =0 cellspacing =0 class=mobile style='background: #ffffff; border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 96%'>" +
                    "<tbody>" +
                    "<tr>" +
                    "<td align = center style='padding-bottom: 20px' valign=top>" +
                    "<table align = center border=0 cellpadding=0 cellspacing=0 style='border-collapse: collapse !important; mso-table-lspace: 0pt; mso-table-rspace: 0pt'>" +
                    "<tbody>" +
                    "<tr>" +
                    "<td align = center style='padding-bottom: 0px; padding-top: 30px' valign=top>" +
                    "</td>" +
                    "</tr>" +
                    "</tbody>" +
                    "</table>" +
                    "</td>" +
                    "</tr>" +
                    "</tbody>" +
                    "</table>" +
                    "</td>" +
                    "</tr>" +
                    "</tbody>" +
                    "</table>" +
                    "</center>" +
                    "</div>" +
                    "</body>" +
                    "</html>";
                    RestClient client = new RestClient();
                    client.BaseUrl = new Uri("https://api.mailgun.net/v3");
                    client.Authenticator = new HttpBasicAuthenticator("api", "key-25b8e8c23e4a09ef67b7957d77eb7413");
                    RestRequest request = new RestRequest();
                    request.AddParameter("domain", "evolutyzstaging.com", ParameterType.UrlSegment);
                    request.Resource = "{domain}/messages";
                    request.AddParameter("from", "Evolutyz ITServices <postmaster@evolutyzstaging.com>");
                    request.AddParameter("to", getusermailid);
                    request.AddParameter("to", accmail);
                    request.AddParameter("subject", "Leave Approved");
                    request.AddParameter("html", emailcontent);
                    request.Method = Method.POST;
                    client.Execute(request);
                }
                else
                {
                    if (status == "2")
                    {
                        emailcontent = "<html>" +
                        "<body bgcolor='#f6f6f6' style='-webkit-font-smoothing: antialiased; background:#f6f6f6; margin:0 auto; padding:0;'>" +
                        "<div style='background:#f4f4f4; border: 0px solid #f4f4f4; margin:0; padding:0'>" +
                        "<center>" +
                        "<table bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' style='background: #ffffff; border-collapse:collapse !important; max-width:650px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:100%'>" +
                        "<tbody>" +
                        "<tr>" +
                        "<td align='center' bgcolor='#ffffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size:18px; line-height:28px; padding-bottom:0px; padding-top:0px valign='top'>" +
                        "<a href='javascript:;' style='color: #7DA33A;text-decoration: none;display: block;' target='_blank'>" +
                        "<img alt='Company Logo' src='http://evolutyz.in/img/evolutyz-logo.png'/>" +
                        "</a>" +
                        "</td>" +
                        " </tr>" +
                        " <tr>" +
                        "<td align='center' bgcolor='#ffffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding-bottom: 20px; padding-top: 0px' valign='top'>" +
                        " </td>" +
                        "</tr>" +
                        "<tr>" +
                        "<td align='center' bgcolor='#fffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding-bottom: 20px; padding-top: 20px' valign=top>" +
                        " <h2 align='center' style='color: #707070; font-weight: 400; margin:0; text-align: center'>Leave Application</h2>" +
                        "</td>" +
                        " </tr>" +
                        "</tbody>" +
                        "</table>" +
                        "<table bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' class='mobile' style='background: #ffffff; border-collapse:collapse !important; max-width:650px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:100%'>" +
                        "<tbody>" +
                        "<tr>" +
                        "<td align='center'>" +
                        "<table bgcolor='#ffffff' cellpadding='0' cellspacing='0' class='mobile' style='background: #ffffff; border-collapse:collapse !important; border: 1px solid #d1d2d1; max-width:600px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:96%'>" +
                        " <tbody>" +
                        " <tr>" +
                        "<td align=center bgcolor='#ffffff' style='background: #ffffff; padding-bottom:0px'; padding-top:0' valign='top'>" +
                        "<table align='center' border='0' cellpadding = '0' cellspacing = '0' style = 'border-collapse: collapse !important; max-width:600px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:80%'>" +
                        " <tbody>" +
                        "<tr>" +
                        "<td align='left' bgcolor='#ffffff' style='background: #ffffff; font-family:Lato, Helevetica, Arial, sans-serif; font-size:18px; line-height:28px; padding:10px 0px 20px' valign='top'>" +
                        "<table align='center' border='0' cellpadding='0' cellspacing='0' style='border-collapse:collapse !important; max-width:600px; width:100%'>" +
                        "<tbody>" +
                        "<tr>" +
                        "<td align='left' bgcolor='#ffffff' style=background:'#ffffff; font-family:Lato, Helevetica, Arial, sans-serif; font-size:18px; max-width:80%; padding:0px; width:40%' valign='top'>" +
                        "<p align='left' style='color: #707070; font-size:14px; font-weight:400; line-height:22px; padding-bottom:0px; text-align:left'>" +
                        "<strong>" +
                        "Leave Rejected By" +
                        " </strong>" +
                        "<br>" +
                        "</p>" +
                        "</td>" +
                        "<td align = 'right' bgcolor = '#ffffff' style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10px 0px 0px' valign='top'>" +
                        "<p align = 'right' style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: 'right'>" +
                         name +
                        "</p >" +
                        "</td>" +
                        "</tr>" +
                        "</tbody>" +
                        "</table>" +
                        "<hr>" +
                        "<table align ='center' border = '0' cellpadding = '0' cellspacing = '0' style = 'border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 100%'>" +
                        " <tbody>" +
                        "<tr>" +
                        "<td align = 'left' bgcolor = '#ffffff' style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = 'top'>" +
                        "<p align = 'left' style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left'>" +
                        "<strong>" +
                        " FromDate " +
                        " </strong>" +
                        "<br> " +
                        "</p>" +
                        " </td>" +
                        "<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top>" +
                        "<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
                        "<strong>" +
                        startDate +
                        "</strong>" +
                        "</p>" +
                        "</td>" +
                        "</tr>" +
                        "<tr>" +
                        "<td align = left bgcolor = #ffffff style = background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = top>" +
                        "<p align = left style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left' >" +
                        "<strong> " +
                        " ToDate " +
                        " </strong>" +
                        "<br>" +
                        "</ p >" +
                        "</ td >" +
                        "<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top>" +
                        "<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
                        "<strong> " +
                        endDate +
                        "</strong>" +
                        "</p>" +
                        "</td>" +
                        "</tr>" +
                        "</tbody>" +
                        "</table>" +
                        "</td>" +
                        "</tr >" +
                        "</tbody>" +
                        "</table>" +
                        "<table align = center bgcolor = #ffffff border =0 cellpadding =0 cellspacing = 0 style = 'background: #ffffff; border-collapse: collapse !important; border-top-color: #D1D2D1; border-top-style: solid; border-top-width: 1px; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 100%'>" +
                        "<tbody>" +
                        " <tr>" +
                        "<td align = center bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding: 10x 0px 20px' valign = top >" +
                        "<table align = center border = 0 cellpadding = 0 cellspacing = 0 style = 'border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 80%'>" +
                        "<tbody >" +
                        //"<tr>" +
                        //" <td align = center bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = top >" +
                        //"<p align = left style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left' >" +
                        //"<strong> " +
                        //"Comments " +
                        //"</strong>" +
                        //"</p>" +
                        //"</td>" +
                        //"<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top >" +
                        //"<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
                        //"<strong>" +
                        //"----" +
                        //"</strong>" +
                        //"</p>" +
                        //"</td>" +
                        //"</tr>" +
                        "</tbody>" +
                        "</table>" +
                        "</td>" +
                        "</tr>" +
                        "</tbody>" +
                        "</table>" +
                        " </td>" +
                        "</tr>" +
                        "</tbody>" +
                        " </table>" +
                        "<table bgcolor = #ffffff cellpadding =0 cellspacing =0 class=mobile style='background: #ffffff; border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 96%'>" +
                        "<tbody>" +
                        "<tr>" +
                        "<td align = center style='padding-bottom: 20px' valign=top>" +
                        "<table align = center border=0 cellpadding=0 cellspacing=0 style='border-collapse: collapse !important; mso-table-lspace: 0pt; mso-table-rspace: 0pt'>" +
                        "<tbody>" +
                        "<tr>" +
                        "<td align = center style='padding-bottom: 0px; padding-top: 30px' valign=top>" +
                        "</td>" +
                        "</tr>" +
                        "</tbody>" +
                        "</table>" +
                        "</td>" +
                        "</tr>" +
                        "</tbody>" +
                        "</table>" +
                        "</td>" +
                        "</tr>" +
                        "</tbody>" +
                        "</table>" +
                        "</center>" +
                        "</div>" +
                        "</body>" +
                        "</html>";

                        RestClient client = new RestClient();
                        client.BaseUrl = new Uri("https://api.mailgun.net/v3");
                        client.Authenticator = new HttpBasicAuthenticator("api", "key-25b8e8c23e4a09ef67b7957d77eb7413");
                        RestRequest request = new RestRequest();
                        request.AddParameter("domain", "evolutyzstaging.com", ParameterType.UrlSegment);
                        request.Resource = "{domain}/messages";
                        request.AddParameter("from", "Evolutyz ITServices <postmaster@evolutyzstaging.com>");
                        request.AddParameter("to", getusermailid);
                        request.AddParameter("to", newemail);
                        request.AddParameter("subject", "LeaveRejected");
                        request.AddParameter("html", emailcontent);
                        request.Method = Method.POST;
                        client.Execute(request);
                    }
                    else
                    {
                        emailcontent = "<html>" +
                        "<body bgcolor='#f6f6f6' style='-webkit-font-smoothing: antialiased; background:#f6f6f6; margin:0 auto; padding:0;'>" +
                        "<div style='background:#f4f4f4; border: 0px solid #f4f4f4; margin:0; padding:0'>" +
                        "<center>" +
                        "<table bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' style='background: #ffffff; border-collapse:collapse !important; max-width:650px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:100%'>" +
                        "<tbody>" +
                        "<tr>" +
                        "<td align='center' bgcolor='#ffffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size:18px; line-height:28px; padding-bottom:0px; padding-top:0px valign='top'>" +
                        "<a href='javascript:;' style='color: #7DA33A;text-decoration: none;display: block;' target='_blank'>" +
                        "<img alt='Company Logo' src='http://evolutyz.in/img/evolutyz-logo.png'/>" +
                        "</a>" +
                        "</td>" +
                        " </tr>" +
                        " <tr>" +
                        "<td align='center' bgcolor='#ffffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding-bottom: 20px; padding-top: 0px' valign='top'>" +
                        " </td>" +
                        "</tr>" +
                        "<tr>" +
                        "<td align='center' bgcolor='#fffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding-bottom: 20px; padding-top: 20px' valign=top>" +
                        " <h2 align='center' style='color: #707070; font-weight: 400; margin:0; text-align: center'>Leave Application</h2>" +
                        "</td>" +
                        " </tr>" +
                        "</tbody>" +
                        "</table>" +
                        "<table bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' class='mobile' style='background: #ffffff; border-collapse:collapse !important; max-width:650px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:100%'>" +
                        "<tbody>" +
                        "<tr>" +
                        "<td align='center'>" +
                        "<table bgcolor='#ffffff' cellpadding='0' cellspacing='0' class='mobile' style='background: #ffffff; border-collapse:collapse !important; border: 1px solid #d1d2d1; max-width:600px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:96%'>" +
                        " <tbody>" +
                        " <tr>" +
                        "<td align=center bgcolor='#ffffff' style='background: #ffffff; padding-bottom:0px'; padding-top:0' valign='top'>" +
                        "<table align='center' border='0' cellpadding = '0' cellspacing = '0' style = 'border-collapse: collapse !important; max-width:600px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:80%'>" +
                        " <tbody>" +
                        "<tr>" +
                        "<td align='left' bgcolor='#ffffff' style='background: #ffffff; font-family:Lato, Helevetica, Arial, sans-serif; font-size:18px; line-height:28px; padding:10px 0px 20px' valign='top'>" +
                        "<table align='center' border='0' cellpadding='0' cellspacing='0' style='border-collapse:collapse !important; max-width:600px; width:100%'>" +
                        "<tbody>" +
                        "<tr>" +
                        "<td align='left' bgcolor='#ffffff' style=background:'#ffffff; font-family:Lato, Helevetica, Arial, sans-serif; font-size:18px; max-width:80%; padding:0px; width:40%' valign='top'>" +
                        "<p align='left' style='color: #707070; font-size:14px; font-weight:400; line-height:22px; padding-bottom:0px; text-align:left'>" +
                        "<strong>" +
                        "Your Leave was kept on hold by" +
                        " </strong>" +
                        "<br>" +
                        "</p>" +
                        "</td>" +
                        "<td align = 'right' bgcolor = '#ffffff' style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10px 0px 0px' valign='top'>" +
                        "<p align = 'right' style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: 'right'>" +
                         name +
                        "</p >" +
                        "</td>" +
                        "</tr>" +
                        "</tbody>" +
                        "</table>" +
                        "<hr>" +
                        "<table align ='center' border = '0' cellpadding = '0' cellspacing = '0' style = 'border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 100%'>" +
                        " <tbody>" +
                        "<tr>" +
                        "<td align = 'left' bgcolor = '#ffffff' style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = 'top'>" +
                        "<p align = 'left' style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left'>" +
                        "<strong>" +
                        " FromDate " +
                        " </strong>" +
                        "<br> " +
                        "</p>" +
                        " </td>" +
                        "<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top>" +
                        "<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
                        "<strong>" +
                        startDate +
                        "</strong>" +
                        "</p>" +
                        "</td>" +
                        "</tr>" +
                        "<tr>" +
                        "<td align = left bgcolor = #ffffff style = background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = top>" +
                        "<p align = left style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left' >" +
                        "<strong> " +
                        " ToDate " +
                        " </strong>" +
                        "<br>" +
                        "</ p >" +
                        "</ td >" +
                        "<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top>" +
                        "<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
                        "<strong> " +
                        endDate +
                        "</strong>" +
                        "</p>" +
                        "</td>" +
                        "</tr>" +
                        "</tbody>" +
                        "</table>" +
                        "</td>" +
                        "</tr >" +
                        "</tbody>" +
                        "</table>" +
                        "<table align = center bgcolor = #ffffff border =0 cellpadding =0 cellspacing = 0 style = 'background: #ffffff; border-collapse: collapse !important; border-top-color: #D1D2D1; border-top-style: solid; border-top-width: 1px; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 100%'>" +
                        "<tbody>" +
                        " <tr>" +
                        "<td align = center bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding: 10x 0px 20px' valign = top >" +
                        "<table align = center border = 0 cellpadding = 0 cellspacing = 0 style = 'border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 80%'>" +
                        "<tbody >" +
                        //"<tr>" +
                        //" <td align = center bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = top >" +
                        //"<p align = left style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left' >" +
                        //"<strong> " +
                        //"Comments " +
                        //"</strong>" +
                        //"</p>" +
                        //"</td>" +
                        //"<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top >" +
                        //"<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
                        //"<strong>" +
                        //"----" +
                        //"</strong>" +
                        //"</p>" +
                        //"</td>" +
                        //"</tr>" +
                        "</tbody>" +
                        "</table>" +
                        "</td>" +
                        "</tr>" +
                        "</tbody>" +
                        "</table>" +
                        " </td>" +
                        "</tr>" +
                        "</tbody>" +
                        " </table>" +
                        "<table bgcolor = #ffffff cellpadding =0 cellspacing =0 class=mobile style='background: #ffffff; border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 96%'>" +
                        "<tbody>" +
                        "<tr>" +
                        "<td align = center style='padding-bottom: 20px' valign=top>" +
                        "<table align = center border=0 cellpadding=0 cellspacing=0 style='border-collapse: collapse !important; mso-table-lspace: 0pt; mso-table-rspace: 0pt'>" +
                        "<tbody>" +
                        "<tr>" +
                        "<td align = center style='padding-bottom: 0px; padding-top: 30px' valign=top>" +
                        "</td>" +
                        "</tr>" +
                        "</tbody>" +
                        "</table>" +
                        "</td>" +
                        "</tr>" +
                        "</tbody>" +
                        "</table>" +
                        "</td>" +
                        "</tr>" +
                        "</tbody>" +
                        "</table>" +
                        "</center>" +
                        "</div>" +
                        "</body>" +
                        "</html>";

                        RestClient client = new RestClient();
                        client.BaseUrl = new Uri("https://api.mailgun.net/v3");
                        client.Authenticator = new HttpBasicAuthenticator("api", "key-25b8e8c23e4a09ef67b7957d77eb7413");
                        RestRequest request = new RestRequest();
                        request.AddParameter("domain", "evolutyzstaging.com", ParameterType.UrlSegment);
                        request.Resource = "{domain}/messages";
                        request.AddParameter("from", "Evolutyz ITServices <postmaster@evolutyzstaging.com>");
                        request.AddParameter("to", getusermailid);
                        request.AddParameter("to", newemail);
                        request.AddParameter("subject", "Leave OnHold");
                        request.AddParameter("html", emailcontent);
                        request.Method = Method.POST;
                        client.Execute(request);
                    }
                }

                AdminComponent admc = new AdminComponent();

                var res = admc.saveUserLeaveStatus(leaveeid, status, mngrid, managerlevel);
            }

            return null;
        }

        public ManagerLeavesforApprovals CheckMailLeaveApproval(ManagerLeavesforApprovals objLeave)
        {
            ManagerLeavesforApprovals objLeaves = new ManagerLeavesforApprovals();
            int Trans_Output = 0;
            UserSessionInfo sessId = Session["UserSessionInfo"] as UserSessionInfo;         
            try
            {
                Conn = new SqlConnection(str);
                if (Conn.State != System.Data.ConnectionState.Open)
                    Conn.Open();
                SqlCommand objCommand = new SqlCommand("[UserLeaveManagerActionsfromEmail]", Conn);
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.Parameters.AddWithValue("@userid", objLeave.Usrl_UserId);
                objCommand.Parameters.AddWithValue("@LeaveID", objLeave.Usrl_LeaveId);
                objCommand.Parameters.AddWithValue("@ManagerId", objLeave.ManagerID1); 
                objCommand.Parameters.AddWithValue("@StatusType", objLeave.Leavestatus);
                objCommand.Parameters.AddWithValue("@Trans_Output", SqlDbType.Int);
                objCommand.Parameters["@Trans_Output"].Direction = ParameterDirection.Output;
                objCommand.ExecuteNonQuery();
                Trans_Output = int.Parse(objCommand.Parameters["@Trans_Output"].Value.ToString());
                if (Trans_Output == 0)
                {
                    objLeaves = new ManagerLeavesforApprovals()
                    {
                        Message = "Waiting for Approval",
                    };
                }

                if (Trans_Output == 111)
                {
                    objLeaves = new ManagerLeavesforApprovals()
                    {
                        Message = "Already approved",
                    };
                }
                if (Trans_Output == 112)
                {
                    objLeaves = new ManagerLeavesforApprovals()
                    {
                        Message = "Already Rejected",
                    };
                }
                if (Trans_Output == 113)
                {
                    objLeaves = new ManagerLeavesforApprovals()
                    {
                        Message = "On Hold",
                    };
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Conn != null)
                {
                    if (Conn.State == ConnectionState.Open)
                    {
                        Conn.Close();
                        Conn.Dispose();
                    }
                }
            }

            return objLeaves;
        }

        public ManagerWFHforApproval CheckMailWFHApproval(ManagerWFHforApproval objWFH)
        {
            ManagerWFHforApproval objLeaves = new ManagerWFHforApproval();
            int Trans_Output = 0;
            UserSessionInfo sessId = Session["UserSessionInfo"] as UserSessionInfo;
            try
            {
                Conn = new SqlConnection(str);
                if (Conn.State != System.Data.ConnectionState.Open)
                    Conn.Open();
                SqlCommand objCommand = new SqlCommand("[UserWFHManagerActionsfromEmail]", Conn);
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.Parameters.AddWithValue("@userid", objWFH.Usrl_UserId);
                objCommand.Parameters.AddWithValue("@WFHID", objWFH.UserwfhID);
                objCommand.Parameters.AddWithValue("@ManagerId", objWFH.ManagerID1);
                objCommand.Parameters.AddWithValue("@StatusType", objWFH.Leavestatus);
                objCommand.Parameters.AddWithValue("@Trans_Output", SqlDbType.Int);
                objCommand.Parameters["@Trans_Output"].Direction = ParameterDirection.Output;
                objCommand.ExecuteNonQuery();
                Trans_Output = int.Parse(objCommand.Parameters["@Trans_Output"].Value.ToString());
                if (Trans_Output == 0)
                {
                    objWFH = new ManagerWFHforApproval()
                    {
                        Message = "Waiting for Approval",
                    };
                }

                if (Trans_Output == 111)
                {
                    objWFH = new ManagerWFHforApproval()
                    {
                        Message = "Already approved",
                    };
                }
                if (Trans_Output == 112)
                {
                    objWFH = new ManagerWFHforApproval()
                    {
                        Message = "Already Rejected",
                    };
                }
                if (Trans_Output == 113)
                {
                    objWFH = new ManagerWFHforApproval()
                    {
                        Message = "On Hold",
                    };
                }
                //if (Trans_Output == 114)
                //{
                //    objWFH = new ManagerWFHforApproval()
                //    {
                //        Message = "Leave Already Approved"
                //    };
                //}
                //if (Trans_Output == 115)
                //{
                //    objWFH = new ManagerWFHforApproval()
                //    {
                //        Message = "Leave Already Rejected"
                //    };
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Conn != null)
                {
                    if (Conn.State == ConnectionState.Open)
                    {
                        Conn.Close();
                        Conn.Dispose();
                    }
                }
            }
            return objWFH;
        }




        [HttpPost]
        public string WebLeaveApproval(string Userid, string LeaveStartDate, string LeaveEndDate, int leaveid, string accntmail, string Leavestatus, int ManagerId, string ManagerName, string ManagerMail, string UserMail)
        {
            newleavid = leaveid.ToString();
            id = ManagerId.ToString();
            string Lstatus = Leavestatus;
            var emailcontent = "";
            if (Leavestatus == "4")
            {

                emailcontent = "<html>" +
                "<body bgcolor='#f6f6f6' style='-webkit-font-smoothing: antialiased; background:#f6f6f6; margin:0 auto; padding:0;'>" +
                "<div style='background:#f4f4f4; border: 0px solid #f4f4f4; margin:0; padding:0'>" +
                "<center>" +
                "<table bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' style='background: #ffffff; border-collapse:collapse !important; max-width:650px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:100%'>" +
                "<tbody>" +
                "<tr>" +
                "<td align='center' bgcolor='#ffffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size:18px; line-height:28px; padding-bottom:0px; padding-top:0px valign='top'>" +
                "<a href='javascript:;' style='color: #7DA33A;text-decoration: none;display: block;' target='_blank'>" +
                "<img alt='Company Logo' src='http://evolutyz.in/img/evolutyz-logo.png'/>" +
                "</a>" +
                "</td>" +
                " </tr>" +
                " <tr>" +
                "<td align='center' bgcolor='#ffffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding-bottom: 20px; padding-top: 0px' valign='top'>" +
                " </td>" +
                "</tr>" +
                "<tr>" +
                "<td align='center' bgcolor='#fffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding-bottom: 20px; padding-top: 20px' valign=top>" +
                " <h2 align='center' style='color: #707070; font-weight: 400; margin:0; text-align: center'>Leave Application</h2>" +
                "</td>" +
                " </tr>" +
                "</tbody>" +
                "</table>" +
                "<table bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' class='mobile' style='background: #ffffff; border-collapse:collapse !important; max-width:650px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:100%'>" +
                "<tbody>" +
                "<tr>" +
                "<td align='center'>" +
                "<table bgcolor='#ffffff' cellpadding='0' cellspacing='0' class='mobile' style='background: #ffffff; border-collapse:collapse !important; border: 1px solid #d1d2d1; max-width:600px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:96%'>" +
                " <tbody>" +
                " <tr>" +
                "<td align=center bgcolor='#ffffff' style='background: #ffffff; padding-bottom:0px'; padding-top:0' valign='top'>" +
                "<table align='center' border='0' cellpadding = '0' cellspacing = '0' style = 'border-collapse: collapse !important; max-width:600px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:80%'>" +
                " <tbody>" +
                "<tr>" +
                "<td align='left' bgcolor='#ffffff' style='background: #ffffff; font-family:Lato, Helevetica, Arial, sans-serif; font-size:18px; line-height:28px; padding:10px 0px 20px' valign='top'>" +
                "<table align='center' border='0' cellpadding='0' cellspacing='0' style='border-collapse:collapse !important; max-width:600px; width:100%'>" +
                "<tbody>" +
                "<tr>" +
                "<td align='left' bgcolor='#ffffff' style=background:'#ffffff; font-family:Lato, Helevetica, Arial, sans-serif; font-size:18px; max-width:80%; padding:0px; width:40%' valign='top'>" +
                "<p align='left' style='color: #707070; font-size:14px; font-weight:400; line-height:22px; padding-bottom:0px; text-align:left'>" +
                "<strong>" +
                "Leave Approved By" +
                " </strong>" +
                "<br>" +
                "</p>" +
                "</td>" +
                "<td align = 'right' bgcolor = '#ffffff' style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10px 0px 0px' valign='top'>" +
                "<p align = 'right' style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: 'right'>" +
                 ManagerName +
                "</p >" +
                "</td>" +
                "</tr>" +
                "</tbody>" +
                "</table>" +
                "<hr>" +
                "<table align ='center' border = '0' cellpadding = '0' cellspacing = '0' style = 'border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 100%'>" +
                " <tbody>" +
                "<tr>" +
                "<td align = 'left' bgcolor = '#ffffff' style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = 'top'>" +
                "<p align = 'left' style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left'>" +
                "<strong>" +
                " FromDate " +
                " </strong>" +
                "<br> " +
                "</p>" +
                " </td>" +
                "<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top>" +
                "<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
                "<strong>" +
                LeaveStartDate +
                "</strong>" +
                "</p>" +
                "</td>" +
                "</tr>" +
                "<tr>" +
                "<td align = left bgcolor = #ffffff style = background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = top>" +
                "<p align = left style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left' >" +
                "<strong> " +
                " ToDate " +
                " </strong>" +
                "<br>" +
                "</ p >" +
                "</ td >" +
                "<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top>" +
                "<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
                "<strong> " +
                LeaveEndDate +
                "</strong>" +
                "</p>" +
                "</td>" +
                "</tr>" +
                "</tbody>" +
                "</table>" +
                "</td>" +
                "</tr >" +
                "</tbody>" +
                "</table>" +
                "<table align = center bgcolor = #ffffff border =0 cellpadding =0 cellspacing = 0 style = 'background: #ffffff; border-collapse: collapse !important; border-top-color: #D1D2D1; border-top-style: solid; border-top-width: 1px; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 100%'>" +
                "<tbody>" +
                " <tr>" +
                "<td align = center bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding: 10x 0px 20px' valign = top >" +
                "<table align = center border = 0 cellpadding = 0 cellspacing = 0 style = 'border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 80%'>" +
                "<tbody >" +
                //"<tr>" +
                //" <td align = center bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = top >" +
                //"<p align = left style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left' >" +
                //"<strong> " +
                //"Comments " +
                //"</strong>" +
                //"</p>" +
                //"</td>" +
                //"<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top >" +
                //"<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
                //"<strong>" +
                //"----" +
                //"</strong>" +
                //"</p>" +
                //"</td>" +
                //"</tr>" +
                "</tbody>" +
                "</table>" +
                "</td>" +
                "</tr>" +
                "</tbody>" +
                "</table>" +
                " </td>" +
                "</tr>" +
                "</tbody>" +
                " </table>" +
                "<table bgcolor = #ffffff cellpadding =0 cellspacing =0 class=mobile style='background: #ffffff; border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 96%'>" +
                "<tbody>" +
                "<tr>" +
                "<td align = center style='padding-bottom: 20px' valign=top>" +
                "<table align = center border=0 cellpadding=0 cellspacing=0 style='border-collapse: collapse !important; mso-table-lspace: 0pt; mso-table-rspace: 0pt'>" +
                "<tbody>" +
                "<tr>" +
                "<td align = center style='padding-bottom: 0px; padding-top: 30px' valign=top>" +
                "</td>" +
                "</tr>" +
                "</tbody>" +
                "</table>" +
                "</td>" +
                "</tr>" +
                "</tbody>" +
                "</table>" +
                "</td>" +
                "</tr>" +
                "</tbody>" +
                "</table>" +
                "</center>" +
                "</div>" +
                "</body>" +
                "</html>";
                RestClient client = new RestClient();
                client.BaseUrl = new Uri("https://api.mailgun.net/v3");
                client.Authenticator = new HttpBasicAuthenticator("api", "key-25b8e8c23e4a09ef67b7957d77eb7413");
                RestRequest request = new RestRequest();
                request.AddParameter("domain", "evolutyzstaging.com", ParameterType.UrlSegment);
                request.Resource = "{domain}/messages";
                request.AddParameter("from", "Evolutyz ITServices <postmaster@evolutyzstaging.com>");
                request.AddParameter("to", UserMail);
                request.AddParameter("to", accntmail);
                request.AddParameter("subject", "Leave Approved");
                request.AddParameter("html", emailcontent);
                request.Method = Method.POST;
                client.Execute(request);
            }
            else
            {
                if (Leavestatus == "5")
                {

                    emailcontent = "<html>" +
                    "<body bgcolor='#f6f6f6' style='-webkit-font-smoothing: antialiased; background:#f6f6f6; margin:0 auto; padding:0;'>" +
                    "<div style='background:#f4f4f4; border: 0px solid #f4f4f4; margin:0; padding:0'>" +
                    "<center>" +
                    "<table bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' style='background: #ffffff; border-collapse:collapse !important; max-width:650px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:100%'>" +
                    "<tbody>" +
                    "<tr>" +
                    "<td align='center' bgcolor='#ffffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size:18px; line-height:28px; padding-bottom:0px; padding-top:0px valign='top'>" +
                    "<a href='javascript:;' style='color: #7DA33A;text-decoration: none;display: block;' target='_blank'>" +
                    "<img alt='Company Logo' src='http://evolutyz.in/img/evolutyz-logo.png'/>" +
                    "</a>" +
                    "</td>" +
                    " </tr>" +
                    " <tr>" +
                    "<td align='center' bgcolor='#ffffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding-bottom: 20px; padding-top: 0px' valign='top'>" +
                    " </td>" +
                    "</tr>" +
                    "<tr>" +
                    "<td align='center' bgcolor='#fffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding-bottom: 20px; padding-top: 20px' valign=top>" +
                    " <h2 align='center' style='color: #707070; font-weight: 400; margin:0; text-align: center'>Leave Application</h2>" +
                    "</td>" +
                    " </tr>" +
                    "</tbody>" +
                    "</table>" +
                    "<table bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' class='mobile' style='background: #ffffff; border-collapse:collapse !important; max-width:650px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:100%'>" +
                    "<tbody>" +
                    "<tr>" +
                    "<td align='center'>" +
                    "<table bgcolor='#ffffff' cellpadding='0' cellspacing='0' class='mobile' style='background: #ffffff; border-collapse:collapse !important; border: 1px solid #d1d2d1; max-width:600px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:96%'>" +
                    " <tbody>" +
                    " <tr>" +
                    "<td align=center bgcolor='#ffffff' style='background: #ffffff; padding-bottom:0px'; padding-top:0' valign='top'>" +
                    "<table align='center' border='0' cellpadding = '0' cellspacing = '0' style = 'border-collapse: collapse !important; max-width:600px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:80%'>" +
                    " <tbody>" +
                    "<tr>" +
                    "<td align='left' bgcolor='#ffffff' style='background: #ffffff; font-family:Lato, Helevetica, Arial, sans-serif; font-size:18px; line-height:28px; padding:10px 0px 20px' valign='top'>" +
                    "<table align='center' border='0' cellpadding='0' cellspacing='0' style='border-collapse:collapse !important; max-width:600px; width:100%'>" +
                    "<tbody>" +
                    "<tr>" +
                    "<td align='left' bgcolor='#ffffff' style=background:'#ffffff; font-family:Lato, Helevetica, Arial, sans-serif; font-size:18px; max-width:80%; padding:0px; width:40%' valign='top'>" +
                    "<p align='left' style='color: #707070; font-size:14px; font-weight:400; line-height:22px; padding-bottom:0px; text-align:left'>" +
                    "<strong>" +
                    "Leave Approved By" +
                    " </strong>" +
                    "<br>" +
                    "</p>" +
                    "</td>" +
                    "<td align = 'right' bgcolor = '#ffffff' style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10px 0px 0px' valign='top'>" +
                    "<p align = 'right' style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: 'right'>" +
                     ManagerName +
                    "</p >" +
                    "</td>" +
                    "</tr>" +
                    "</tbody>" +
                    "</table>" +
                    "<hr>" +
                    "<table align ='center' border = '0' cellpadding = '0' cellspacing = '0' style = 'border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 100%'>" +
                    " <tbody>" +
                    "<tr>" +
                    "<td align = 'left' bgcolor = '#ffffff' style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = 'top'>" +
                    "<p align = 'left' style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left'>" +
                    "<strong>" +
                    " FromDate " +
                    " </strong>" +
                    "<br> " +
                    "</p>" +
                    " </td>" +
                    "<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top>" +
                    "<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
                    "<strong>" +
                    LeaveStartDate +
                    "</strong>" +
                    "</p>" +
                    "</td>" +
                    "</tr>" +
                    "<tr>" +
                    "<td align = left bgcolor = #ffffff style = background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = top>" +
                    "<p align = left style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left' >" +
                    "<strong> " +
                    " ToDate " +
                    " </strong>" +
                    "<br>" +
                    "</ p >" +
                    "</ td >" +
                    "<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top>" +
                    "<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
                    "<strong> " +
                    LeaveEndDate +
                    "</strong>" +
                    "</p>" +
                    "</td>" +
                    "</tr>" +
                    "</tbody>" +
                    "</table>" +
                    "</td>" +
                    "</tr >" +
                    "</tbody>" +
                    "</table>" +
                    "<table align = center bgcolor = #ffffff border =0 cellpadding =0 cellspacing = 0 style = 'background: #ffffff; border-collapse: collapse !important; border-top-color: #D1D2D1; border-top-style: solid; border-top-width: 1px; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 100%'>" +
                    "<tbody>" +
                    " <tr>" +
                    "<td align = center bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding: 10x 0px 20px' valign = top >" +
                    "<table align = center border = 0 cellpadding = 0 cellspacing = 0 style = 'border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 80%'>" +
                    "<tbody >" +
                    //"<tr>" +
                    //" <td align = center bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = top >" +
                    //"<p align = left style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left' >" +
                    //"<strong> " +
                    //"Comments " +
                    //"</strong>" +
                    //"</p>" +
                    //"</td>" +
                    //"<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top >" +
                    //"<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
                    //"<strong>" +
                    //"----" +
                    //"</strong>" +
                    //"</p>" +
                    //"</td>" +
                    //"</tr>" +
                    "</tbody>" +
                    "</table>" +
                    "</td>" +
                    "</tr>" +
                    "</tbody>" +
                    "</table>" +
                    " </td>" +
                    "</tr>" +
                    "</tbody>" +
                    " </table>" +
                    "<table bgcolor = #ffffff cellpadding =0 cellspacing =0 class=mobile style='background: #ffffff; border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 96%'>" +
                    "<tbody>" +
                    "<tr>" +
                    "<td align = center style='padding-bottom: 20px' valign=top>" +
                    "<table align = center border=0 cellpadding=0 cellspacing=0 style='border-collapse: collapse !important; mso-table-lspace: 0pt; mso-table-rspace: 0pt'>" +
                    "<tbody>" +
                    "<tr>" +
                    "<td align = center style='padding-bottom: 0px; padding-top: 30px' valign=top>" +
                    "</td>" +
                    "</tr>" +
                    "</tbody>" +
                    "</table>" +
                    "</td>" +
                    "</tr>" +
                    "</tbody>" +
                    "</table>" +
                    "</td>" +
                    "</tr>" +
                    "</tbody>" +
                    "</table>" +
                    "</center>" +
                    "</div>" +
                    "</body>" +
                    "</html>";
                    RestClient client = new RestClient();
                    client.BaseUrl = new Uri("https://api.mailgun.net/v3");
                    client.Authenticator = new HttpBasicAuthenticator("api", "key-25b8e8c23e4a09ef67b7957d77eb7413");
                    RestRequest request = new RestRequest();
                    request.AddParameter("domain", "evolutyzstaging.com", ParameterType.UrlSegment);
                    request.Resource = "{domain}/messages";
                    request.AddParameter("from", "Evolutyz ITServices <postmaster@evolutyzstaging.com>");
                    request.AddParameter("to", UserMail);
                    request.AddParameter("to", accntmail);
                    request.AddParameter("subject", "Leave Approved");
                    request.AddParameter("html", emailcontent);
                    request.Method = Method.POST;
                    client.Execute(request);
                }
                else

                {
                    emailcontent = "<html>" +
                    "<body bgcolor='#f6f6f6' style='-webkit-font-smoothing: antialiased; background:#f6f6f6; margin:0 auto; padding:0;'>" +
                    "<div style='background:#f4f4f4; border: 0px solid #f4f4f4; margin:0; padding:0'>" +
                    "<center>" +
                    "<table bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' style='background: #ffffff; border-collapse:collapse !important; max-width:650px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:100%'>" +
                    "<tbody>" +
                    "<tr>" +
                    "<td align='center' bgcolor='#ffffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size:18px; line-height:28px; padding-bottom:0px; padding-top:0px valign='top'>" +
                    "<a href='javascript:;' style='color: #7DA33A;text-decoration: none;display: block;' target='_blank'>" +
                    "<img alt='Company Logo' src='http://evolutyz.in/img/evolutyz-logo.png'/>" +
                    "</a>" +
                    "</td>" +
                    " </tr>" +
                    " <tr>" +
                    "<td align='center' bgcolor='#ffffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding-bottom: 20px; padding-top: 0px' valign='top'>" +
                    " </td>" +
                    "</tr>" +
                    "<tr>" +
                    "<td align='center' bgcolor='#fffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding-bottom: 20px; padding-top: 20px' valign=top>" +
                    " <h2 align='center' style='color: #707070; font-weight: 400; margin:0; text-align: center'>Leave Application</h2>" +
                    "</td>" +
                    " </tr>" +
                    "</tbody>" +
                    "</table>" +
                    "<table bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' class='mobile' style='background: #ffffff; border-collapse:collapse !important; max-width:650px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:100%'>" +
                    "<tbody>" +
                    "<tr>" +
                    "<td align='center'>" +
                    "<table bgcolor='#ffffff' cellpadding='0' cellspacing='0' class='mobile' style='background: #ffffff; border-collapse:collapse !important; border: 1px solid #d1d2d1; max-width:600px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:96%'>" +
                    " <tbody>" +
                    " <tr>" +
                    "<td align=center bgcolor='#ffffff' style='background: #ffffff; padding-bottom:0px'; padding-top:0' valign='top'>" +
                    "<table align='center' border='0' cellpadding = '0' cellspacing = '0' style = 'border-collapse: collapse !important; max-width:600px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:80%'>" +
                    " <tbody>" +
                    "<tr>" +
                    "<td align='left' bgcolor='#ffffff' style='background: #ffffff; font-family:Lato, Helevetica, Arial, sans-serif; font-size:18px; line-height:28px; padding:10px 0px 20px' valign='top'>" +
                    "<table align='center' border='0' cellpadding='0' cellspacing='0' style='border-collapse:collapse !important; max-width:600px; width:100%'>" +
                    "<tbody>" +
                    "<tr>" +
                    "<td align='left' bgcolor='#ffffff' style=background:'#ffffff; font-family:Lato, Helevetica, Arial, sans-serif; font-size:18px; max-width:80%; padding:0px; width:40%' valign='top'>" +
                    "<p align='left' style='color: #707070; font-size:14px; font-weight:400; line-height:22px; padding-bottom:0px; text-align:left'>" +
                    "<strong>" +
                    "Leave Approved By" +
                    " </strong>" +
                    "<br>" +
                    "</p>" +
                    "</td>" +
                    "<td align = 'right' bgcolor = '#ffffff' style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10px 0px 0px' valign='top'>" +
                    "<p align = 'right' style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: 'right'>" +
                     ManagerName +
                    "</p >" +
                    "</td>" +
                    "</tr>" +
                    "</tbody>" +
                    "</table>" +
                    "<hr>" +
                    "<table align ='center' border = '0' cellpadding = '0' cellspacing = '0' style = 'border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 100%'>" +
                    " <tbody>" +
                    "<tr>" +
                    "<td align = 'left' bgcolor = '#ffffff' style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = 'top'>" +
                    "<p align = 'left' style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left'>" +
                    "<strong>" +
                    " FromDate " +
                    " </strong>" +
                    "<br> " +
                    "</p>" +
                    " </td>" +
                    "<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top>" +
                    "<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
                    "<strong>" +
                    LeaveStartDate +
                    "</strong>" +
                    "</p>" +
                    "</td>" +
                    "</tr>" +
                    "<tr>" +
                    "<td align = left bgcolor = #ffffff style = background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = top>" +
                    "<p align = left style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left' >" +
                    "<strong> " +
                    " ToDate " +
                    " </strong>" +
                    "<br>" +
                    "</ p >" +
                    "</ td >" +
                    "<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top>" +
                    "<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
                    "<strong> " +
                    LeaveEndDate +
                    "</strong>" +
                    "</p>" +
                    "</td>" +
                    "</tr>" +
                    "</tbody>" +
                    "</table>" +
                    "</td>" +
                    "</tr >" +
                    "</tbody>" +
                    "</table>" +
                    "<table align = center bgcolor = #ffffff border =0 cellpadding =0 cellspacing = 0 style = 'background: #ffffff; border-collapse: collapse !important; border-top-color: #D1D2D1; border-top-style: solid; border-top-width: 1px; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 100%'>" +
                    "<tbody>" +
                    " <tr>" +
                    "<td align = center bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding: 10x 0px 20px' valign = top >" +
                    "<table align = center border = 0 cellpadding = 0 cellspacing = 0 style = 'border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 80%'>" +
                    "<tbody >" +
                    //"<tr>" +
                    //" <td align = center bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = top >" +
                    //"<p align = left style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left' >" +
                    //"<strong> " +
                    //"Comments " +
                    //"</strong>" +
                    //"</p>" +
                    //"</td>" +
                    //"<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top >" +
                    //"<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
                    //"<strong>" +
                    //"----" +
                    //"</strong>" +
                    //"</p>" +
                    //"</td>" +
                    //"</tr>" +
                    "</tbody>" +
                    "</table>" +
                    "</td>" +
                    "</tr>" +
                    "</tbody>" +
                    "</table>" +
                    " </td>" +
                    "</tr>" +
                    "</tbody>" +
                    " </table>" +
                    "<table bgcolor = #ffffff cellpadding =0 cellspacing =0 class=mobile style='background: #ffffff; border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 96%'>" +
                    "<tbody>" +
                    "<tr>" +
                    "<td align = center style='padding-bottom: 20px' valign=top>" +
                    "<table align = center border=0 cellpadding=0 cellspacing=0 style='border-collapse: collapse !important; mso-table-lspace: 0pt; mso-table-rspace: 0pt'>" +
                    "<tbody>" +
                    "<tr>" +
                    "<td align = center style='padding-bottom: 0px; padding-top: 30px' valign=top>" +
                    "</td>" +
                    "</tr>" +
                    "</tbody>" +
                    "</table>" +
                    "</td>" +
                    "</tr>" +
                    "</tbody>" +
                    "</table>" +
                    "</td>" +
                    "</tr>" +
                    "</tbody>" +
                    "</table>" +
                    "</center>" +
                    "</div>" +
                    "</body>" +
                    "</html>";
                    RestClient client = new RestClient();
                    client.BaseUrl = new Uri("https://api.mailgun.net/v3");
                    client.Authenticator = new HttpBasicAuthenticator("api", "key-25b8e8c23e4a09ef67b7957d77eb7413");
                    RestRequest request = new RestRequest();
                    request.AddParameter("domain", "evolutyzstaging.com", ParameterType.UrlSegment);
                    request.Resource = "{domain}/messages";
                    request.AddParameter("from", "Evolutyz ITServices <postmaster@evolutyzstaging.com>");
                    request.AddParameter("to", UserMail);
                    request.AddParameter("to", accntmail);
                    request.AddParameter("subject", "Leave Approved");
                    request.AddParameter("html", emailcontent);
                    request.Method = Method.POST;
                    client.Execute(request);
                }
            }
            AdminComponent admc = new AdminComponent();
            var res = admc.saveWebApprovalStatus(newleavid, Leavestatus, id);
            string response = string.Empty;

            if (Lstatus == "4")
            {
                return response = "Leave Approved Successfully";
            }
            else if (Lstatus == "5")
            {
                return response = "Leave Rejected";
            }
            else
            {
                return response = "Leave On Hold";
            }

        }
        [HttpPost]
        public string WebWrkFrmHmeApproval(string Userid, string LeaveStartDate, string LeaveEndDate, int UWFHID, string accntmail, string Leavestatus, int ManagerId, string ManagerName, string ManagerMail, string UserMail)
        {
            newuwfhid = UWFHID.ToString();
            id = ManagerId.ToString();
            string Lstatus = Leavestatus;

            var emailcontent = "";



            if (Leavestatus == "4")
            {

                emailcontent = "<html>" +
                "<body bgcolor='#f6f6f6' style='-webkit-font-smoothing: antialiased; background:#f6f6f6; margin:0 auto; padding:0;'>" +
                "<div style='background:#f4f4f4; border: 0px solid #f4f4f4; margin:0; padding:0'>" +
                "<center>" +
                "<table bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' style='background: #ffffff; border-collapse:collapse !important; max-width:650px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:100%'>" +
                "<tbody>" +
                "<tr>" +
                "<td align='center' bgcolor='#ffffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size:18px; line-height:28px; padding-bottom:0px; padding-top:0px valign='top'>" +
                "<a href='javascript:;' style='color: #7DA33A;text-decoration: none;display: block;' target='_blank'>" +
                "<img alt='Company Logo' src='http://evolutyz.in/img/evolutyz-logo.png'/>" +
                "</a>" +
                "</td>" +
                " </tr>" +
                " <tr>" +
                "<td align='center' bgcolor='#ffffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding-bottom: 20px; padding-top: 0px' valign='top'>" +
                " </td>" +
                "</tr>" +
                "<tr>" +
                "<td align='center' bgcolor='#fffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding-bottom: 20px; padding-top: 20px' valign=top>" +
                " <h2 align='center' style='color: #707070; font-weight: 400; margin:0; text-align: center'>Leave Application</h2>" +
                "</td>" +
                " </tr>" +
                "</tbody>" +
                "</table>" +
                "<table bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' class='mobile' style='background: #ffffff; border-collapse:collapse !important; max-width:650px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:100%'>" +
                "<tbody>" +
                "<tr>" +
                "<td align='center'>" +
                "<table bgcolor='#ffffff' cellpadding='0' cellspacing='0' class='mobile' style='background: #ffffff; border-collapse:collapse !important; border: 1px solid #d1d2d1; max-width:600px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:96%'>" +
                " <tbody>" +
                " <tr>" +
                "<td align=center bgcolor='#ffffff' style='background: #ffffff; padding-bottom:0px'; padding-top:0' valign='top'>" +
                "<table align='center' border='0' cellpadding = '0' cellspacing = '0' style = 'border-collapse: collapse !important; max-width:600px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:80%'>" +
                " <tbody>" +
                "<tr>" +
                "<td align='left' bgcolor='#ffffff' style='background: #ffffff; font-family:Lato, Helevetica, Arial, sans-serif; font-size:18px; line-height:28px; padding:10px 0px 20px' valign='top'>" +
                "<table align='center' border='0' cellpadding='0' cellspacing='0' style='border-collapse:collapse !important; max-width:600px; width:100%'>" +
                "<tbody>" +
                "<tr>" +
                "<td align='left' bgcolor='#ffffff' style=background:'#ffffff; font-family:Lato, Helevetica, Arial, sans-serif; font-size:18px; max-width:80%; padding:0px; width:40%' valign='top'>" +
                "<p align='left' style='color: #707070; font-size:14px; font-weight:400; line-height:22px; padding-bottom:0px; text-align:left'>" +
                "<strong>" +
                "Leave Approved By" +
                " </strong>" +
                "<br>" +
                "</p>" +
                "</td>" +
                "<td align = 'right' bgcolor = '#ffffff' style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10px 0px 0px' valign='top'>" +
                "<p align = 'right' style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: 'right'>" +
                 ManagerName +
                "</p >" +
                "</td>" +
                "</tr>" +
                "</tbody>" +
                "</table>" +
                "<hr>" +
                "<table align ='center' border = '0' cellpadding = '0' cellspacing = '0' style = 'border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 100%'>" +
                " <tbody>" +
                "<tr>" +
                "<td align = 'left' bgcolor = '#ffffff' style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = 'top'>" +
                "<p align = 'left' style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left'>" +
                "<strong>" +
                " FromDate " +
                " </strong>" +
                "<br> " +
                "</p>" +
                " </td>" +
                "<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top>" +
                "<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
                "<strong>" +
                LeaveStartDate +
                "</strong>" +
                "</p>" +
                "</td>" +
                "</tr>" +
                "<tr>" +
                "<td align = left bgcolor = #ffffff style = background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = top>" +
                "<p align = left style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left' >" +
                "<strong> " +
                " ToDate " +
                " </strong>" +
                "<br>" +
                "</ p >" +
                "</ td >" +
                "<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top>" +
                "<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
                "<strong> " +
                LeaveEndDate +
                "</strong>" +
                "</p>" +
                "</td>" +
                "</tr>" +
                "</tbody>" +
                "</table>" +
                "</td>" +
                "</tr >" +
                "</tbody>" +
                "</table>" +
                "<table align = center bgcolor = #ffffff border =0 cellpadding =0 cellspacing = 0 style = 'background: #ffffff; border-collapse: collapse !important; border-top-color: #D1D2D1; border-top-style: solid; border-top-width: 1px; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 100%'>" +
                "<tbody>" +
                " <tr>" +
                "<td align = center bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding: 10x 0px 20px' valign = top >" +
                "<table align = center border = 0 cellpadding = 0 cellspacing = 0 style = 'border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 80%'>" +
                "<tbody >" +
                //"<tr>" +
                //" <td align = center bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = top >" +
                //"<p align = left style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left' >" +
                //"<strong> " +
                //"Comments " +
                //"</strong>" +
                //"</p>" +
                //"</td>" +
                //"<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top >" +
                //"<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
                //"<strong>" +
                //"----" +
                //"</strong>" +
                //"</p>" +
                //"</td>" +
                //"</tr>" +
                "</tbody>" +
                "</table>" +
                "</td>" +
                "</tr>" +
                "</tbody>" +
                "</table>" +
                " </td>" +
                "</tr>" +
                "</tbody>" +
                " </table>" +
                "<table bgcolor = #ffffff cellpadding =0 cellspacing =0 class=mobile style='background: #ffffff; border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 96%'>" +
                "<tbody>" +
                "<tr>" +
                "<td align = center style='padding-bottom: 20px' valign=top>" +
                "<table align = center border=0 cellpadding=0 cellspacing=0 style='border-collapse: collapse !important; mso-table-lspace: 0pt; mso-table-rspace: 0pt'>" +
                "<tbody>" +
                "<tr>" +
                "<td align = center style='padding-bottom: 0px; padding-top: 30px' valign=top>" +
                "</td>" +
                "</tr>" +
                "</tbody>" +
                "</table>" +
                "</td>" +
                "</tr>" +
                "</tbody>" +
                "</table>" +
                "</td>" +
                "</tr>" +
                "</tbody>" +
                "</table>" +
                "</center>" +
                "</div>" +
                "</body>" +
                "</html>";
                RestClient client = new RestClient();
                client.BaseUrl = new Uri("https://api.mailgun.net/v3");
                client.Authenticator = new HttpBasicAuthenticator("api", "key-25b8e8c23e4a09ef67b7957d77eb7413");
                RestRequest request = new RestRequest();
                request.AddParameter("domain", "evolutyzstaging.com", ParameterType.UrlSegment);
                request.Resource = "{domain}/messages";
                request.AddParameter("from", "Evolutyz ITServices <postmaster@evolutyzstaging.com>");
                request.AddParameter("to", UserMail);
                request.AddParameter("to", accntmail);
                request.AddParameter("subject", "Leave Approved");
                request.AddParameter("html", emailcontent);
                request.Method = Method.POST;
                client.Execute(request);
            }
            else
            {
                if (Leavestatus == "5")
                {

                    emailcontent = "<html>" +
                    "<body bgcolor='#f6f6f6' style='-webkit-font-smoothing: antialiased; background:#f6f6f6; margin:0 auto; padding:0;'>" +
                    "<div style='background:#f4f4f4; border: 0px solid #f4f4f4; margin:0; padding:0'>" +
                    "<center>" +
                    "<table bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' style='background: #ffffff; border-collapse:collapse !important; max-width:650px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:100%'>" +
                    "<tbody>" +
                    "<tr>" +
                    "<td align='center' bgcolor='#ffffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size:18px; line-height:28px; padding-bottom:0px; padding-top:0px valign='top'>" +
                    "<a href='javascript:;' style='color: #7DA33A;text-decoration: none;display: block;' target='_blank'>" +
                    "<img alt='Company Logo' src='http://evolutyz.in/img/evolutyz-logo.png'/>" +
                    "</a>" +
                    "</td>" +
                    " </tr>" +
                    " <tr>" +
                    "<td align='center' bgcolor='#ffffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding-bottom: 20px; padding-top: 0px' valign='top'>" +
                    " </td>" +
                    "</tr>" +
                    "<tr>" +
                    "<td align='center' bgcolor='#fffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding-bottom: 20px; padding-top: 20px' valign=top>" +
                    " <h2 align='center' style='color: #707070; font-weight: 400; margin:0; text-align: center'>Leave Application</h2>" +
                    "</td>" +
                    " </tr>" +
                    "</tbody>" +
                    "</table>" +
                    "<table bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' class='mobile' style='background: #ffffff; border-collapse:collapse !important; max-width:650px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:100%'>" +
                    "<tbody>" +
                    "<tr>" +
                    "<td align='center'>" +
                    "<table bgcolor='#ffffff' cellpadding='0' cellspacing='0' class='mobile' style='background: #ffffff; border-collapse:collapse !important; border: 1px solid #d1d2d1; max-width:600px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:96%'>" +
                    " <tbody>" +
                    " <tr>" +
                    "<td align=center bgcolor='#ffffff' style='background: #ffffff; padding-bottom:0px'; padding-top:0' valign='top'>" +
                    "<table align='center' border='0' cellpadding = '0' cellspacing = '0' style = 'border-collapse: collapse !important; max-width:600px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:80%'>" +
                    " <tbody>" +
                    "<tr>" +
                    "<td align='left' bgcolor='#ffffff' style='background: #ffffff; font-family:Lato, Helevetica, Arial, sans-serif; font-size:18px; line-height:28px; padding:10px 0px 20px' valign='top'>" +
                    "<table align='center' border='0' cellpadding='0' cellspacing='0' style='border-collapse:collapse !important; max-width:600px; width:100%'>" +
                    "<tbody>" +
                    "<tr>" +
                    "<td align='left' bgcolor='#ffffff' style=background:'#ffffff; font-family:Lato, Helevetica, Arial, sans-serif; font-size:18px; max-width:80%; padding:0px; width:40%' valign='top'>" +
                    "<p align='left' style='color: #707070; font-size:14px; font-weight:400; line-height:22px; padding-bottom:0px; text-align:left'>" +
                    "<strong>" +
                    "Leave Approved By" +
                    " </strong>" +
                    "<br>" +
                    "</p>" +
                    "</td>" +
                    "<td align = 'right' bgcolor = '#ffffff' style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10px 0px 0px' valign='top'>" +
                    "<p align = 'right' style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: 'right'>" +
                     ManagerName +
                    "</p >" +
                    "</td>" +
                    "</tr>" +
                    "</tbody>" +
                    "</table>" +
                    "<hr>" +
                    "<table align ='center' border = '0' cellpadding = '0' cellspacing = '0' style = 'border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 100%'>" +
                    " <tbody>" +
                    "<tr>" +
                    "<td align = 'left' bgcolor = '#ffffff' style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = 'top'>" +
                    "<p align = 'left' style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left'>" +
                    "<strong>" +
                    " FromDate " +
                    " </strong>" +
                    "<br> " +
                    "</p>" +
                    " </td>" +
                    "<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top>" +
                    "<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
                    "<strong>" +
                    LeaveStartDate +
                    "</strong>" +
                    "</p>" +
                    "</td>" +
                    "</tr>" +
                    "<tr>" +
                    "<td align = left bgcolor = #ffffff style = background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = top>" +
                    "<p align = left style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left' >" +
                    "<strong> " +
                    " ToDate " +
                    " </strong>" +
                    "<br>" +
                    "</ p >" +
                    "</ td >" +
                    "<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top>" +
                    "<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
                    "<strong> " +
                    LeaveEndDate +
                    "</strong>" +
                    "</p>" +
                    "</td>" +
                    "</tr>" +
                    "</tbody>" +
                    "</table>" +
                    "</td>" +
                    "</tr >" +
                    "</tbody>" +
                    "</table>" +
                    "<table align = center bgcolor = #ffffff border =0 cellpadding =0 cellspacing = 0 style = 'background: #ffffff; border-collapse: collapse !important; border-top-color: #D1D2D1; border-top-style: solid; border-top-width: 1px; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 100%'>" +
                    "<tbody>" +
                    " <tr>" +
                    "<td align = center bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding: 10x 0px 20px' valign = top >" +
                    "<table align = center border = 0 cellpadding = 0 cellspacing = 0 style = 'border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 80%'>" +
                    "<tbody >" +
                    //"<tr>" +
                    //" <td align = center bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = top >" +
                    //"<p align = left style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left' >" +
                    //"<strong> " +
                    //"Comments " +
                    //"</strong>" +
                    //"</p>" +
                    //"</td>" +
                    //"<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top >" +
                    //"<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
                    //"<strong>" +
                    //"----" +
                    //"</strong>" +
                    //"</p>" +
                    //"</td>" +
                    //"</tr>" +
                    "</tbody>" +
                    "</table>" +
                    "</td>" +
                    "</tr>" +
                    "</tbody>" +
                    "</table>" +
                    " </td>" +
                    "</tr>" +
                    "</tbody>" +
                    " </table>" +
                    "<table bgcolor = #ffffff cellpadding =0 cellspacing =0 class=mobile style='background: #ffffff; border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 96%'>" +
                    "<tbody>" +
                    "<tr>" +
                    "<td align = center style='padding-bottom: 20px' valign=top>" +
                    "<table align = center border=0 cellpadding=0 cellspacing=0 style='border-collapse: collapse !important; mso-table-lspace: 0pt; mso-table-rspace: 0pt'>" +
                    "<tbody>" +
                    "<tr>" +
                    "<td align = center style='padding-bottom: 0px; padding-top: 30px' valign=top>" +
                    "</td>" +
                    "</tr>" +
                    "</tbody>" +
                    "</table>" +
                    "</td>" +
                    "</tr>" +
                    "</tbody>" +
                    "</table>" +
                    "</td>" +
                    "</tr>" +
                    "</tbody>" +
                    "</table>" +
                    "</center>" +
                    "</div>" +
                    "</body>" +
                    "</html>";
                    RestClient client = new RestClient();
                    client.BaseUrl = new Uri("https://api.mailgun.net/v3");
                    client.Authenticator = new HttpBasicAuthenticator("api", "key-25b8e8c23e4a09ef67b7957d77eb7413");
                    RestRequest request = new RestRequest();
                    request.AddParameter("domain", "evolutyzstaging.com", ParameterType.UrlSegment);
                    request.Resource = "{domain}/messages";
                    request.AddParameter("from", "Evolutyz ITServices <postmaster@evolutyzstaging.com>");
                    request.AddParameter("to", UserMail);
                    request.AddParameter("to", accntmail);
                    request.AddParameter("subject", "Leave Approved");
                    request.AddParameter("html", emailcontent);
                    request.Method = Method.POST;
                    client.Execute(request);
                }
                else

                {
                    emailcontent = "<html>" +
                    "<body bgcolor='#f6f6f6' style='-webkit-font-smoothing: antialiased; background:#f6f6f6; margin:0 auto; padding:0;'>" +
                    "<div style='background:#f4f4f4; border: 0px solid #f4f4f4; margin:0; padding:0'>" +
                    "<center>" +
                    "<table bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' style='background: #ffffff; border-collapse:collapse !important; max-width:650px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:100%'>" +
                    "<tbody>" +
                    "<tr>" +
                    "<td align='center' bgcolor='#ffffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size:18px; line-height:28px; padding-bottom:0px; padding-top:0px valign='top'>" +
                    "<a href='javascript:;' style='color: #7DA33A;text-decoration: none;display: block;' target='_blank'>" +
                    "<img alt='Company Logo' src='http://evolutyz.in/img/evolutyz-logo.png'/>" +
                    "</a>" +
                    "</td>" +
                    " </tr>" +
                    " <tr>" +
                    "<td align='center' bgcolor='#ffffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding-bottom: 20px; padding-top: 0px' valign='top'>" +
                    " </td>" +
                    "</tr>" +
                    "<tr>" +
                    "<td align='center' bgcolor='#fffff' style='background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding-bottom: 20px; padding-top: 20px' valign=top>" +
                    " <h2 align='center' style='color: #707070; font-weight: 400; margin:0; text-align: center'>Leave Application</h2>" +
                    "</td>" +
                    " </tr>" +
                    "</tbody>" +
                    "</table>" +
                    "<table bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' class='mobile' style='background: #ffffff; border-collapse:collapse !important; max-width:650px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:100%'>" +
                    "<tbody>" +
                    "<tr>" +
                    "<td align='center'>" +
                    "<table bgcolor='#ffffff' cellpadding='0' cellspacing='0' class='mobile' style='background: #ffffff; border-collapse:collapse !important; border: 1px solid #d1d2d1; max-width:600px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:96%'>" +
                    " <tbody>" +
                    " <tr>" +
                    "<td align=center bgcolor='#ffffff' style='background: #ffffff; padding-bottom:0px'; padding-top:0' valign='top'>" +
                    "<table align='center' border='0' cellpadding = '0' cellspacing = '0' style = 'border-collapse: collapse !important; max-width:600px; mso-table-lspace:0pt; mso-table-rspace:0pt; width:80%'>" +
                    " <tbody>" +
                    "<tr>" +
                    "<td align='left' bgcolor='#ffffff' style='background: #ffffff; font-family:Lato, Helevetica, Arial, sans-serif; font-size:18px; line-height:28px; padding:10px 0px 20px' valign='top'>" +
                    "<table align='center' border='0' cellpadding='0' cellspacing='0' style='border-collapse:collapse !important; max-width:600px; width:100%'>" +
                    "<tbody>" +
                    "<tr>" +
                    "<td align='left' bgcolor='#ffffff' style=background:'#ffffff; font-family:Lato, Helevetica, Arial, sans-serif; font-size:18px; max-width:80%; padding:0px; width:40%' valign='top'>" +
                    "<p align='left' style='color: #707070; font-size:14px; font-weight:400; line-height:22px; padding-bottom:0px; text-align:left'>" +
                    "<strong>" +
                    "Leave Approved By" +
                    " </strong>" +
                    "<br>" +
                    "</p>" +
                    "</td>" +
                    "<td align = 'right' bgcolor = '#ffffff' style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10px 0px 0px' valign='top'>" +
                    "<p align = 'right' style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: 'right'>" +
                     ManagerName +
                    "</p >" +
                    "</td>" +
                    "</tr>" +
                    "</tbody>" +
                    "</table>" +
                    "<hr>" +
                    "<table align ='center' border = '0' cellpadding = '0' cellspacing = '0' style = 'border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 100%'>" +
                    " <tbody>" +
                    "<tr>" +
                    "<td align = 'left' bgcolor = '#ffffff' style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = 'top'>" +
                    "<p align = 'left' style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left'>" +
                    "<strong>" +
                    " FromDate " +
                    " </strong>" +
                    "<br> " +
                    "</p>" +
                    " </td>" +
                    "<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top>" +
                    "<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
                    "<strong>" +
                    LeaveStartDate +
                    "</strong>" +
                    "</p>" +
                    "</td>" +
                    "</tr>" +
                    "<tr>" +
                    "<td align = left bgcolor = #ffffff style = background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = top>" +
                    "<p align = left style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left' >" +
                    "<strong> " +
                    " ToDate " +
                    " </strong>" +
                    "<br>" +
                    "</ p >" +
                    "</ td >" +
                    "<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top>" +
                    "<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
                    "<strong> " +
                    LeaveEndDate +
                    "</strong>" +
                    "</p>" +
                    "</td>" +
                    "</tr>" +
                    "</tbody>" +
                    "</table>" +
                    "</td>" +
                    "</tr >" +
                    "</tbody>" +
                    "</table>" +
                    "<table align = center bgcolor = #ffffff border =0 cellpadding =0 cellspacing = 0 style = 'background: #ffffff; border-collapse: collapse !important; border-top-color: #D1D2D1; border-top-style: solid; border-top-width: 1px; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 100%'>" +
                    "<tbody>" +
                    " <tr>" +
                    "<td align = center bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; line-height: 28px; padding: 10x 0px 20px' valign = top >" +
                    "<table align = center border = 0 cellpadding = 0 cellspacing = 0 style = 'border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 80%'>" +
                    "<tbody >" +
                    //"<tr>" +
                    //" <td align = center bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; max-width: 80%; padding: 10px 0px 0px; width: 40%' valign = top >" +
                    //"<p align = left style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: left' >" +
                    //"<strong> " +
                    //"Comments " +
                    //"</strong>" +
                    //"</p>" +
                    //"</td>" +
                    //"<td align = right bgcolor = #ffffff style = 'background: #ffffff; font-family: Lato, Helevetica, Arial, sans-serif; font-size: 18px; padding: 10x 0px 0px' valign = top >" +
                    //"<p align = right style = 'color: #707070; font-size: 14px; font-weight: 400; line-height: 22px; padding-bottom: 0px; text-align: right'>" +
                    //"<strong>" +
                    //"----" +
                    //"</strong>" +
                    //"</p>" +
                    //"</td>" +
                    //"</tr>" +
                    "</tbody>" +
                    "</table>" +
                    "</td>" +
                    "</tr>" +
                    "</tbody>" +
                    "</table>" +
                    " </td>" +
                    "</tr>" +
                    "</tbody>" +
                    " </table>" +
                    "<table bgcolor = #ffffff cellpadding =0 cellspacing =0 class=mobile style='background: #ffffff; border-collapse: collapse !important; max-width: 600px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 96%'>" +
                    "<tbody>" +
                    "<tr>" +
                    "<td align = center style='padding-bottom: 20px' valign=top>" +
                    "<table align = center border=0 cellpadding=0 cellspacing=0 style='border-collapse: collapse !important; mso-table-lspace: 0pt; mso-table-rspace: 0pt'>" +
                    "<tbody>" +
                    "<tr>" +
                    "<td align = center style='padding-bottom: 0px; padding-top: 30px' valign=top>" +
                    "</td>" +
                    "</tr>" +
                    "</tbody>" +
                    "</table>" +
                    "</td>" +
                    "</tr>" +
                    "</tbody>" +
                    "</table>" +
                    "</td>" +
                    "</tr>" +
                    "</tbody>" +
                    "</table>" +
                    "</center>" +
                    "</div>" +
                    "</body>" +
                    "</html>";
                    RestClient client = new RestClient();
                    client.BaseUrl = new Uri("https://api.mailgun.net/v3");
                    client.Authenticator = new HttpBasicAuthenticator("api", "key-25b8e8c23e4a09ef67b7957d77eb7413");
                    RestRequest request = new RestRequest();
                    request.AddParameter("domain", "evolutyzstaging.com", ParameterType.UrlSegment);
                    request.Resource = "{domain}/messages";
                    request.AddParameter("from", "Evolutyz ITServices <postmaster@evolutyzstaging.com>");
                    request.AddParameter("to", UserMail);
                    request.AddParameter("to", accntmail);
                    request.AddParameter("subject", "Leave Approved");
                    request.AddParameter("html", emailcontent);
                    request.Method = Method.POST;
                    client.Execute(request);
                }
            }
            AdminComponent admc = new AdminComponent();
            var res = admc.saveWebWFHApprovalStatus(newuwfhid, Leavestatus, id);

            string response = string.Empty;

            if (Lstatus == "4")
            {
                return response = "WorkFromHome Approved Successfully";
            }
            else if (Lstatus == "5")
            {
                return response = "WorkFromHome Rejected";
            }
            else
            {
                return response = "Leave On Hold";
            }
        }



        [HttpGet]
        public JsonResult GetLeaveApprovalsDetails()
        {
            AdminComponent admcmp = new AdminComponent();
            var getempdetails = admcmp.GetApprovedLeaveCount();
            return Json(getempdetails, JsonRequestBehavior.AllowGet);

        }


        public ActionResult GetLeavePreview()
        {

            UserSessionInfo sessId = Session["UserSessionInfo"] as UserSessionInfo;
            int UserID = sessId.UserId;
            string RoleName = sessId.RoleName;
            LeavePreviewDetails objmanagerdetails = new LeavePreviewDetails();
            Conn = new SqlConnection(str);
            try
            {
                objmanagerdetails.myleaves = new List<UserApprovedLeaves>();
                objmanagerdetails.leavesforapproval = new List<ManagerLeavesforApprovals>();

                Conn.Open();
                SqlCommand cmd = new SqlCommand("[GetUserLeavesforApproval]", Conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userid", UserID);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sda.Fill(ds);

                if ((RoleName == "Super Admin") || (RoleName == "Admin") || (RoleName == "Manager"))
                {

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {

                            objmanagerdetails.myleaves.Add(new UserApprovedLeaves
                            {
                                Usrl_LeaveId = Convert.ToInt32(dr["Usrl_LeaveId"]),
                                Usrl_UserId = Convert.ToInt32(dr["Usrl_UserId"]),
                                accntmail = dr["Acc_EmailID"].ToString(),
                                ProjectName = dr["Proj_ProjectName"].ToString(),
                                userName = dr["Usr_Username"].ToString(),
                                LeaveStartDate = dr["LeaveStartDate"].ToString(),
                                LeaveEndDate = dr["LeaveEndDate"].ToString(),
                                No_Of_Days = Convert.ToInt32(dr["No_Of_Days"]),
                                Leavestatus = dr["ResultSubmitStatus"].ToString(),

                            });

                        }
                    }

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow druser in ds.Tables[1].Rows)
                        {

                            objmanagerdetails.leavesforapproval.Add(new ManagerLeavesforApprovals
                            {
                                Usrl_LeaveId = Convert.ToInt32(druser["Usrl_LeaveId"]),
                                Usrl_UserId = Convert.ToInt32(druser["Usrl_UserId"]),
                                accntmail = druser["Acc_EmailID"].ToString(),
                                ProjectName = druser["Proj_ProjectName"].ToString(),
                                userName = druser["Usr_Username"].ToString(),
                                LeaveStartDate = druser["LeaveStartDate"].ToString(),
                                LeaveEndDate = druser["LeaveEndDate"].ToString(),
                                No_Of_Days = Convert.ToInt32(druser["No_Of_Days"]),
                                Leavestatus = druser["ResultSubmitStatus"].ToString(),
                                ManagerID1 = Convert.ToInt32(druser["L1_ManagerId"]),
                                ManagerName1 = druser["L1_ManagerName"].ToString(),
                                ManagerEmail1 = druser["L1_ManagerMail"].ToString(),
                                UserEmail = druser["UsrP_EmailID"].ToString()
                            });

                        }
                    }


                }
                else
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {

                            objmanagerdetails.myleaves.Add(new UserApprovedLeaves
                            {
                                Usrl_LeaveId = Convert.ToInt32(dr["Usrl_LeaveId"]),
                                Usrl_UserId = Convert.ToInt32(dr["Usrl_UserId"]),
                                accntmail = dr["Acc_EmailID"].ToString(),
                                ProjectName = dr["Proj_ProjectName"].ToString(),
                                userName = dr["Usr_Username"].ToString(),
                                LeaveStartDate = dr["LeaveStartDate"].ToString(),
                                LeaveEndDate = dr["LeaveEndDate"].ToString(),
                                No_Of_Days = Convert.ToInt32(dr["No_Of_Days"]),
                                Leavestatus = dr["ResultSubmitStatus"].ToString(),
                            });

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Conn != null)
                {
                    if (Conn.State == ConnectionState.Open)
                    {
                        Conn.Close();
                        Conn.Dispose();
                    }
                }
            }
            var result = new { objmanagerdetails.myleaves, objmanagerdetails.leavesforapproval, RoleName };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetWrkfrmHomePreview()
        {

            UserSessionInfo sessId = Session["UserSessionInfo"] as UserSessionInfo;
            int UserID = sessId.UserId;
            string RoleName = sessId.RoleName;
            WFHPreviewDetails objmanagerdetails = new WFHPreviewDetails();
            Conn = new SqlConnection(str);
            try
            {
                objmanagerdetails.UserWrkfrmhome = new List<UserWFHDetails>();
                objmanagerdetails.wrkfrmhmeforapproval = new List<ManagerWFHforApproval>();

                Conn.Open();
                SqlCommand cmd = new SqlCommand("[GetUserWorkFrmHomeApproval]", Conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userid", UserID);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sda.Fill(ds);

                if ((RoleName == "Super Admin") || (RoleName == "Admin") || (RoleName == "Manager"))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            objmanagerdetails.UserWrkfrmhome.Add(new UserWFHDetails
                            {
                                Usrl_UserId = Convert.ToInt32(dr["Usrl_UserId"]),
                                UserwfhID = Convert.ToInt32(dr["UserwfhID"]),
                                userName = dr["Usr_Username"].ToString(),
                                usrEmailID = dr["UsrP_EmailID"].ToString(),
                                LeaveStartDate = dr["LeaveStartDate"].ToString(),
                                LeaveEndDate = dr["LeaveEndDate"].ToString(),
                                Leavestatus = dr["ResultSubmitStatus"].ToString(),
                                ProjectName = dr["Proj_ProjectName"].ToString(),
                                accntmail = dr["Acc_EmailID"].ToString(),
                                Tot_No_Days = Convert.ToInt32(dr["Tot_No_Days"]),

                            });

                        }
                    }

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow druser in ds.Tables[1].Rows)
                        {
                            objmanagerdetails.wrkfrmhmeforapproval.Add(new ManagerWFHforApproval
                            {
                                Usrl_UserId = Convert.ToInt32(druser["Usrl_UserId"]),
                                UserwfhID = Convert.ToInt32(druser["UserwfhID"]),
                                userName = druser["Usr_Username"].ToString(),
                                UserEmail = druser["UsrP_EmailID"].ToString(),
                                LeaveStartDate = druser["LeaveStartDate"].ToString(),
                                LeaveEndDate = druser["LeaveEndDate"].ToString(),
                                Leavestatus = druser["ResultSubmitStatus"].ToString(),
                                ProjectName = druser["Proj_ProjectName"].ToString(),
                                accntmail = druser["Acc_EmailID"].ToString(),
                                Tot_No_Days = Convert.ToInt32(druser["Tot_No_Days"]),
                                ManagerID1 = Convert.ToInt32(druser["L1_ManagerId"]),
                                ManagerName1 = druser["L1_ManagerName"].ToString(),
                                ManagerEmail1 = druser["L1_ManagerMail"].ToString(),
                            });

                        }
                    }
                }
                else
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            objmanagerdetails.UserWrkfrmhome.Add(new UserWFHDetails
                            {
                                Usrl_UserId = Convert.ToInt32(dr["Usrl_UserId"]),
                                UserwfhID = Convert.ToInt32(dr["UserwfhID"]),
                                userName = dr["Usr_Username"].ToString(),
                                usrEmailID = dr["UsrP_EmailID"].ToString(),
                                LeaveStartDate = dr["LeaveStartDate"].ToString(),
                                LeaveEndDate = dr["LeaveEndDate"].ToString(),
                                Leavestatus = dr["ResultSubmitStatus"].ToString(),
                                ProjectName = dr["Proj_ProjectName"].ToString(),
                                accntmail = dr["Acc_EmailID"].ToString(),
                                Tot_No_Days = Convert.ToInt32(dr["Tot_No_Days"]),

                            });

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Conn != null)
                {
                    if (Conn.State == ConnectionState.Open)
                    {
                        Conn.Close();
                        Conn.Dispose();
                    }
                }
            }
            var result = new { objmanagerdetails.wrkfrmhmeforapproval, objmanagerdetails.UserWrkfrmhome, RoleName };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        int accountid;


        public JsonResult GetHolidayDates()
        {
            AdminComponent adcmp = new AdminComponent();
            UserSessionInfo info = new UserSessionInfo();
            accountid = info.AccountId;
            var gethldydetails = adcmp.GetHolidayDates(accountid);
            return Json(gethldydetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetUSApprovedLeaves(int Userid, string monthYear)
        {
            UserSessionInfo sessId = Session["UserSessionInfo"] as UserSessionInfo;
            int UserID = sessId.UserId;
            //Leavelist objlist = new Leavelist();
            List<USLeaveDates> usLeavesdate = new List<USLeaveDates>();
            Conn = new SqlConnection(str);
            try
            {
                Conn.Open();
                SqlCommand cmd = new SqlCommand("Get_USUserLeavesforApproval", Conn);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", Userid);
                cmd.Parameters.AddWithValue("@MonthYear", monthYear);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                dataAdapter.Fill(ds);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    usLeavesdate.Add(new USLeaveDates
                    {
                        leavedates = Convert.ToDateTime(dr["LeaveDates"]),
                        dates = Convert.ToInt32(dr["dates"]),

                    });

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Conn != null)
                {
                    if (Conn.State == ConnectionState.Open)
                    {
                        Conn.Close();
                        Conn.Dispose();
                    }
                }
            }
            return Json(usLeavesdate, JsonRequestBehavior.AllowGet);
        }
    }

}