using Evolutyz.Business;
using Evolutyz.Data;
using Evolutyz.Entities;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using EvolutyzCorner.UI.Web.Models;
using System.Configuration;
using evolCorner.Models;
using System.Data.Entity.SqlServer;
using RestSharp;
using RestSharp.Authenticators;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace EvolutyzCorner.UI.Web.Controllers
{

    [Authorize]
    [EvolutyzCorner.UI.Web.MvcApplication.SessionExpire]
    [EvolutyzCorner.UI.Web.MvcApplication.NoDirectAccess]
    public class UserController : Controller
    {
        List<SelectListItem> lsttimesheet = new List<SelectListItem>();
        List<SelectListItem> lstmonth = new List<SelectListItem>();

        string str = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public string htmlStr = "";
        public int timeSheetID = 0;
        SqlConnection Conn = new SqlConnection();
        SqlDataAdapter da = new SqlDataAdapter(); DataSet ds = new DataSet(); DataTable dt = new DataTable();
        DataSet ds1 = new DataSet(); DataTable dtheadings = new DataTable(); DataTable dtData = new DataTable();
        EmailFormats objemail = new EmailFormats(); string StatusMsg = string.Empty;
        EvolutyzCornerDataEntities db = new EvolutyzCornerDataEntities();
        timesheet lstusers = new timesheet();

        public ActionResult Index(bool? pdf)
        {

            UserSessionInfo _objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
            int UserId = _objSessioninfo.UserId;

            //int AccountId = _objSessioninfo.AccountId;
            ViewBag.UserId = UserId;
            string roleID = _objSessioninfo.RoleName;
            ViewBag.Roleid = _objSessioninfo.RoleName;
            int accountID = _objSessioninfo.AccountId;
            ViewBag.Accountid = _objSessioninfo.AccountId;
            LeaveSchemeComponent compobj = new LeaveSchemeComponent();
            var Accountname = compobj.GetallAccountnames(accountID,roleID).Select(a => new SelectListItem()
            {
                Value = a.Acc_AccountID.ToString(),
                Text = a.Acc_AccountName,
            });
            ClientComponent clientcomponent = new ClientComponent();
            ViewBag.usertitle = clientcomponent.GetTitle().Select(a => new SelectListItem()
            {
                Value = a.Usr_Titleid.ToString(),
                Text = a.TitlePrefix,

            });
            HomeController hm = new HomeController();
            var obj = hm.GetAdminMenu();
            foreach (var item in obj)
            {
                //if (item.ModuleName == "Add User")
                //{
                //    var mk = item.ModuleAccessType;
                //    ViewBag.b = mk;
                //}
                if (item.ModuleName == "Add User")
                {
                    var mk = item.ModuleAccessType;


                    ViewBag.a = mk;

                }


            }

            ViewBag.Accountname = Accountname;
            return View();
            //if (!pdf.HasValue)
            //{
            //    #region to return UserList

            //    //objSessioninfo.UserId = 501;
            //    //Session["UserSessionInfo"] = objSessioninfo;

            //    List<LookupStatusDetail> objStatusList = new List<LookupStatusDetail>();
            //    objStatusList.Add(new LookupStatusDetail { StatusID = 1, Status = "Active" });
            //    objStatusList.Add(new LookupStatusDetail { StatusID = 0, Status = "InActive" });

            //    var objStList = from cl in objStatusList
            //                    orderby cl.StatusID
            //                    select new
            //                    {
            //                        value = cl.StatusID,
            //                        text = cl.Status
            //                    };
            //    ViewBag.Status = objStList;
            //    return View();
            //    #endregion
            //}
            //else
            //{
            //    string filename = "User.pdf";
            //    string filePath = Server.MapPath("~/Content/PDFs/" + filename);

            //    var objDtl = new UserComponent();
            //    IList<UserEntity> UserList = objDtl.GetUserDetail();

            //    ExportPDF(UserList, new string[] { "Usr_UserID", "AccountName", "UserType", "RoleName", "Usr_LoginId" }, filePath);

            //    return File(filePath, "application/pdf");
            //}
        }
        
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = System.Text.Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                //lower  
                byte2String += targetData[i].ToString("x2");
                //upper  
                //byte2String += targetData[i].ToString("X2");  
            }
            return byte2String;
        }
        
        public JsonResult GetUserCollection(int acntID, string RoleId)
        {
            List<UserEntity> UserDetails = null;
            try
            {

                string accid = ViewBag.Accountid;
                // ViewBag.Accountid=
                var objDtl = new UserComponent();
                UserDetails = objDtl.GetUserDetail(acntID, RoleId);
                ViewBag.UserDetails = UserDetails[0].Usr_Version;

            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(UserDetails, JsonRequestBehavior.AllowGet);
        }
       
        public JsonResult GetUserByID(int catID)
        {
            UserEntity UserDetails = null;
            try
            {
                var objDtl = new UserComponent();
                UserDetails = objDtl.GetUserDetailByID(catID);
            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(UserDetails, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetL2ManagerNames(int AccountId)
        {
            List<UserEntity> l2Manger = null;
            try
            {
                var objDtl = new UserComponent();
                l2Manger = objDtl.GetManagerNames2(AccountId);

            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(l2Manger, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetL1ManagerNames(int AccountId)
        {
            List<UserEntity> l1Manger = null;
            try
            {
                var objDtl = new UserComponent();
                l1Manger = objDtl.GetManagerNames(AccountId);

            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(l1Manger, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult ManageProfile()
        {
            return View();
        }

        public JsonResult GetUserRoles(int AccountId, string RoleId)
        {
            List<UserEntity> UserRoles = null;
            try
            {
                var objDtl = new UserComponent();
                UserRoles = objDtl.GetUserRolenames(AccountId, RoleId);

            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(UserRoles, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTaskNames(int AccountId)
        {
            List<UserEntity> TaskNames = null;
            try
            {
                var objDtl = new UserComponent();
                TaskNames = objDtl.GetTaskNames(AccountId);

            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(TaskNames, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserAccounts(int acntID, int userid)
        {
            List<UserEntity> AccountNames = null;
            try
            {
                var objDtl = new UserComponent();
                AccountNames = objDtl.GetAccounts(acntID, userid);

            }
            catch (Exception)
            {
                return null;
            }
            return Json(AccountNames, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserAccounts1(int acntID, string RoleId)
        {
            List<UserEntity> AccountNames = null;
            try
            {
                var objDtl = new UserComponent();
                AccountNames = objDtl.GetAccounts1(acntID, RoleId);

            }
            catch (Exception)
            {
                return null;
            }
            return Json(AccountNames, JsonRequestBehavior.AllowGet);
        }
        
        public string CreateUser([Bind(Exclude = "Usr_UserID")] UserEntity UserDtl)
        {
            string strResponse = string.Empty;
            UserSessionInfo _objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
            int _userID = _objSessioninfo.UserId;
            UserDtl.Usr_CreatedBy = _userID;
            var Org = new UserComponent();
            string imagename = string.Empty;
            if (Request.Files.Count > 0)
            {
                    var file = Request.Files[0];
                    var fileName = "/uploadimages/Images/" + file.FileName;
                    imagename = file.FileName;
                    var imagepath = Server.MapPath(fileName);
                    file.SaveAs(imagepath);
            }
            if (imagename == "")
            {
                UserDtl.Usrp_ProfilePicture = "User.png";
            }
            else
            {
                UserDtl.Usrp_ProfilePicture = imagename;
            }


            UserDtl.Usr_Password = GetMD5(UserDtl.Usr_Password);

            int r = Org.AddUser(UserDtl);
            if (r ==1)
            {
                strResponse = "Record created successfully";
            }
            else if (r == 0)
            {
                strResponse = "Record already exists";
            }
            else if (r ==-1)
            {
                strResponse = "Please Fill All Mandatory Fields";
            }else if (r==2)
            {
                strResponse = "UserName already Exists";
            }
            else if (r == 3)
            {
                strResponse = "Loginid already Exists";
            }

            return strResponse;
        }
        [HttpPost]
        public string UpdateUser(UserEntity UserDtl)
        {
            string strResponse = string.Empty;
            short UsTCurrentVersion = 0;
            string imagename = string.Empty;
            if (Request.Files.Count > 0)
            {
                var em = (from p in db.Users where p.Usr_UserID == UserDtl.Usr_UserID select p.Usr_Password).FirstOrDefault();

                if (UserDtl.Usr_Password == null)
                {
                    UserDtl.Usr_Password = em;
                }else if (UserDtl.Usr_Password != em)
                {

                    UserDtl.Usr_Password = GetMD5(UserDtl.Usr_Password);

                }
                else
                {
                    UserDtl.Usr_Password = em;
                }

                var file = Request.Files[0];
                var fileName = "/uploadimages/Images/" + file.FileName;
                imagename = file.FileName;
                var imagepath = Server.MapPath(fileName);
                file.SaveAs(imagepath);
                UserDtl.Usrp_ProfilePicture = imagename;
                try
                {
                    var UserComponent = new UserComponent();
                    var currentUserDetails = UserComponent.GetUserDetailByID(UserDtl.Usr_UserID);
                    int UserID = currentUserDetails.Usr_UserID;
                    UsTCurrentVersion = Convert.ToInt16(currentUserDetails.Usr_Version);
                    bool _currentStatus = false;

                    _currentStatus = UserDtl.Usr_ActiveStatus == true;
                  
                    UserSessionInfo _objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
                    int _userID = _objSessioninfo.UserId;
                    UserDtl.Usr_ModifiedBy = _userID;
                   
                    UserDtl.Usr_Version = ++UsTCurrentVersion;
                    UserDtl.Usr_ActiveStatus = _currentStatus;
                    var Org = new UserComponent();
                    int r = Org.UpdateUserDetail(UserDtl);
                    if (r > 0)
                    {
                        strResponse = "Record updated successfully";
                    }
                    else if (r == 0)
                    {
                        strResponse = "Record does not exists";
                    }
                    else if (r < 0)
                    {
                        strResponse = "Error occured in UpdateUser";
                    }
                }
                //  }
                catch (Exception ex)
                {
                    return strResponse;
                }
            }
            else
            {

                try
                {

                    var em = (from p in db.Users where p.Usr_UserID == UserDtl.Usr_UserID select p.Usr_Password).FirstOrDefault();



                    if (UserDtl.Usr_Password == null)
                    {
                        UserDtl.Usr_Password = em;
                    }
                    else if (UserDtl.Usr_Password != em)
                    {

                        UserDtl.Usr_Password = GetMD5(UserDtl.Usr_Password);

                    }
                    else
                    {
                        UserDtl.Usr_Password = em;
                    }

                    var UserComponent = new UserComponent();
                    var currentUserDetails = UserComponent.GetUserDetailByID(UserDtl.Usr_UserID);
                    int UserID = currentUserDetails.Usr_UserID;
                    UsTCurrentVersion = Convert.ToInt16(currentUserDetails.Usr_Version);
                    bool _currentStatus = false;

                    ////check for version and active status
                    //if (ModelState["Usr_ActiveStatus"].Value != null)
                    //{
                    _currentStatus = UserDtl.Usr_ActiveStatus == true;
                    //}

                    //if (ModelState.IsValid)
                    //{
                    UserSessionInfo _objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
                    int _userID = _objSessioninfo.UserId;
                    UserDtl.Usr_ModifiedBy = _userID;
                    //while udating increment version by1
                    UserDtl.Usr_Version = ++UsTCurrentVersion;
                    UserDtl.Usr_ActiveStatus = _currentStatus;
                    var Org = new UserComponent();
                   // UserDtl.Usr_Password = GetMD5(UserDtl.Usr_Password);
                    int r = Org.UpdateUserDetailByImage(UserDtl);

                    if (r > 0)
                    {
                        strResponse = "Record updated successfully";
                    }
                    else if (r == 0)
                    {
                        strResponse = "Record does not exists";
                    }
                    else if (r < 0)
                    {
                        strResponse = "Error occured in UpdateUser";
                    }
                }
                //  }
                catch (Exception ex)
                {
                    return strResponse;
                }


            }





            return strResponse;
        }
        
        public ActionResult getUserTypes(int AccountId)
        {
            //UserSessionInfo infoobj = new UserSessionInfo();
            //int accountid = infoobj.AccountId;

            List<UserEntity> UserTypes = null;
            try
            {
                var objDtl = new UserComponent();
                UserTypes = objDtl.getUserTypes(AccountId);

            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(UserTypes, JsonRequestBehavior.AllowGet);


        }

        public ActionResult GetUserTypesByAccountid()
        {
            UserSessionInfo infoobj = new UserSessionInfo();
            int accountid = infoobj.AccountId;

            List<UserEntity> UserTypes = null;
            try
            {
                var objDtl = new UserComponent();
                UserTypes = objDtl.getUserTypes(accountid);

            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(UserTypes, JsonRequestBehavior.AllowGet);


        }
        
        public string DeleteUser(int UserID)
        {
            string strResponse = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    var Org = new UserComponent();
                    int r = Org.DeleteUserDetail(UserID);
                    if (r > 0)
                    {
                        strResponse = "Record deleted successfully";
                    }
                    else if (r == 0)
                    {
                        strResponse = "Record does not exists";
                    }
                    else if (r < 0)
                    {
                        strResponse = "Error occured in DeleteUser";
                    }
                }
            }
            catch (Exception ex)
            {
                return strResponse;
            }
            return strResponse;
        }
        public string DeleteSkill(int skillid)
        {
            string strResponse = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    var Org = new UserComponent();
                    int r = Org.DeleteSkill(skillid);
                    if (r > 0)
                    {
                        strResponse = "Record deleted successfully";
                    }
                    else if (r == 0)
                    {
                        strResponse = "Record does not exists";
                    }
                    else if (r < 0)
                    {
                        strResponse = "Error occured in DeleteUser";
                    }
                }
            }
            catch (Exception ex)
            {
                return strResponse;
            }
            return strResponse;
        }

        public string SkillChangeStatus(string id, string status)
        {
            string strResponse = string.Empty;
            var objDtl = new UserComponent();
            strResponse = objDtl.SkillChangeStatus(id, status);

            return strResponse;
        }

        public JsonResult GetUserHistoryByID(int ID)
        {
            List<History_UsersEntity> HisUserDetails = null;
            try
            {
                var objDtl = new UserComponent();
                HisUserDetails = objDtl.GetHistoryUserDetailsByID(ID);
                ViewBag.UserDetails = HisUserDetails;
            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(HisUserDetails, JsonRequestBehavior.AllowGet);
        }
        
        #region Export to PDF

        //private static void ExportPDF<TSource>(IList<TSource> UserList, string[] columns, string filePath)
        //{
        //    iTextSharp.text.Font headerFont = FontFactory.GetFont("Verdana", 10, iTextSharp.text.Color.WHITE);
        //    iTextSharp.text.Font rowfont = FontFactory.GetFont("Verdana", 10, iTextSharp.text.Color.BLUE);
        //    Document document = new Document(PageSize.A4);

        //    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.OpenOrCreate));
        //    document.Open();
        //    PdfPTable table = new PdfPTable(columns.Length);
        //    foreach (var column in columns)
        //    {
        //        PdfPCell cell = new PdfPCell(new Phrase(column, headerFont));
        //        cell.BackgroundColor = iTextSharp.text.Color.BLACK;
        //        table.AddCell(cell);
        //    }

        //    foreach (var item in UserList)
        //    {
        //        foreach (var column in columns)
        //        {
        //            string value = item.GetType().GetProperty(column).GetValue(item).ToString();
        //            PdfPCell cell5 = new PdfPCell(new Phrase(value, rowfont));
        //            table.AddCell(cell5);
        //        }
        //    }

        //    document.Add(table);
        //    document.Close();
        //}
        #endregion

        public ActionResult TaskLookups()
        {
            IEnumerable<SelectListItem> itemssss = db.GenericTasks.Select(c => new SelectListItem
            {

                Value = SqlFunctions.StringConvert((double)c.tsk_TaskID),
                Text = c.tsk_TaskName

            });

            ViewBag.taskname = itemssss;

            return View();
        }

        public JsonResult Months()
        {

            ViewBag.MonthNames = Month;
            return Months();
        }

        public IEnumerable<SelectListItem> Month
        {
            get
            {
                return DateTimeFormatInfo
                       .InvariantInfo
                       .MonthNames
                       .Select((MonthNames, index) => new SelectListItem
                       {
                           Value = (index + 1).ToString(),
                           Text = MonthNames
                       });
            }
        }
        
        public JsonResult GetTaskdropdown()
        {

            EvolutyzCornerDataEntities db = new EvolutyzCornerDataEntities();

            IEnumerable<SelectListItem> itemssss = db.GenericTasks.Select(c => new SelectListItem
            {

                Value = c.tsk_TaskDescription,
                Text = c.tsk_TaskDescription

            });

            ViewBag.taskname = itemssss;

            return Json(ViewBag.itemssss, JsonRequestBehavior.AllowGet);
        }

        #region Call GetGradesByID Method from Component
        public JsonResult Gettimesheet(int userid)
        {
            var admComp = new UserComponent();
            TimesheetEntity qec = admComp.Gettimesheet(userid);
            return Json(qec, JsonRequestBehavior.AllowGet);
        }
        #endregion
        
        public ActionResult Usertimesheet1()
        {
            // ViewBag.userid = Convert.ToInt32(Session["UserId"]);
            ViewBag.username = Session["UserName"].ToString();
            return View();
        }

        public string ChangeStatus(string id, string status)
        {
            string strResponse = string.Empty;
            var objDtl = new UserComponent();
            strResponse = objDtl.ChangeStatus(id, status);

            return strResponse;
        }
    }

}
