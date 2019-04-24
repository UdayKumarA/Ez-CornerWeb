using Evolutyz.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using evolCorner.Models;
using Evolutyz.Data;
using RestSharp.Authenticators;
using Evolutyz.Business;
using RestSharp;

namespace EvolutyzCorner.UI.Web.Controllers
{
    [Authorize]
    [EvolutyzCorner.UI.Web.MvcApplication.SessionExpire]
    [EvolutyzCorner.UI.Web.MvcApplication.NoDirectAccess]
    public class DashBoardController : Controller
    {

        EvolutyzCornerDataEntities db = new EvolutyzCornerDataEntities();
        string str = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public ActionResult DashBoard1()

        {
            UserSessionInfo info = new UserSessionInfo();
           string rolename = info.RoleName;
            int accid = info.AccountId;
            ViewBag.userid = Convert.ToInt32(Session["UserId"]);
            LeaveTypeComponent LeaveTypeComponent = new LeaveTypeComponent();
            var projectslist = LeaveTypeComponent.GetAllProjects().Select(a => new SelectListItem()
            {
                Value = a.Proj_ProjectID.ToString(),
                Text = a.Proj_ProjectName,
            });

            var newsboardcomp = new NewBoardComponent();
            var news = newsboardcomp.GetNewsCollection();

            ViewBag.news = news;

            ViewBag.projectslist = projectslist;
            EvolutyzCornerDataEntities db = new EvolutyzCornerDataEntities();
           int accountcount  = (from a in db.Accounts
                            where a.Acc_AccountID != 502 && a.Acc_isDeleted==false
                            select new DashboardMails
                            {
                                accountid =    a.Acc_AccountID
                            }).Count();
            
            int userscount = (from a in db.Users
                                where a.Usr_UserID != 501 && a.Usr_isDeleted==false && a.Usr_AccountID== accid
                              select new DashboardMails
                                {
                                    users = a.Usr_UserID
                                }).Count();
            int allusers = (from a in db.Users
                              where a.Usr_UserID != 501 && a.Usr_isDeleted == false 
                              select new DashboardMails
                              {
                                  users = a.Usr_UserID
                              }).Count();
            int projects = (from a in db.Projects
                            where a.Proj_isDeleted == false && a.Proj_AccountID== accid
                            select new DashboardMails
                              {
                                  Proj_ProjectID = a.Proj_ProjectID
                              }).Count();
            int skillslist = (from a in db.Skills
                              where a.Acc_AccountID== accid && a.Sk_isDeleted==false
                              select new DashboardMails
                            {
                                  SkillId = a.SkillId
                            }).Count();
            ViewBag.skillslist = skillslist;
            ViewBag.projects = projects;
            ViewBag.users = userscount;
            ViewBag.allusers = allusers;
            ViewBag.acountcount = accountcount;
            ViewBag.username = rolename;
            return View();
        }
        public IRestResponse DashboardMail(string Subject, string Message, int? ProjectId, string Emailid)
        {
            EvolutyzCornerDataEntities db = new EvolutyzCornerDataEntities();
            RestClient client = new RestClient();
            RestRequest request = new RestRequest();
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                if (Emailid != "" && ProjectId.ToString() == "")
                {
                    try
                    {
                        client.BaseUrl = new Uri("https://api.mailgun.net/v3");
                        client.Authenticator = new HttpBasicAuthenticator("api", "key-25b8e8c23e4a09ef67b7957d77eb7413");
                        request.AddParameter("domain", "evolutyzstaging.com", ParameterType.UrlSegment);
                        request.Resource = "{domain}/messages";
                        request.AddParameter("from", "Evolutyz ITServices <postmaster@evolutyzstaging.com>");
                        request.AddParameter("to", Emailid);
                        request.AddParameter("subject", Subject);
                        request.AddParameter("text", Message);
                        request.Method = Method.POST;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }

                else if (ProjectId != 0)
                {
                    try
                    {
                        var projectlist = db.Get_EmailsByProjectId(ProjectId).ToList();
                        client.BaseUrl = new Uri("https://api.mailgun.net/v3");
                        client.Authenticator = new HttpBasicAuthenticator("api", "key-25b8e8c23e4a09ef67b7957d77eb7413");
                        request.AddParameter("domain", "evolutyzstaging.com", ParameterType.UrlSegment);
                        request.Resource = "{domain}/messages";
                        request.AddParameter("from", "Evolutyz ITServices <postmaster@evolutyzstaging.com>");
                        foreach (var items in projectlist)
                        {
                            request.AddParameter("to", items.UsrP_EmailID);

                        }
                        request.AddParameter("subject", Subject);
                        request.AddParameter("text", Message);
                        request.Method = Method.POST;
                        //client.Execute(request);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else
                {
                    using (SqlCommand cmd = new SqlCommand("Select UsrP_EmailID from UsersProfile", conn))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        try
                        {
                            client.BaseUrl = new Uri("https://api.mailgun.net/v3");
                            client.Authenticator = new HttpBasicAuthenticator("api", "key-25b8e8c23e4a09ef67b7957d77eb7413");
                            request.AddParameter("domain", "evolutyzstaging.com", ParameterType.UrlSegment);
                            request.Resource = "{domain}/messages";
                            request.AddParameter("from", "Evolutyz ITServices <postmaster@evolutyzstaging.com>");
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                request.AddParameter("to", dt.Rows[i]["emails"]);
                            }
                            request.AddParameter("subject", Subject);
                            request.AddParameter("text", Message);
                            request.Method = Method.POST;
                            //client.Execute(request);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }

                }


            }
            return client.Execute(request);
        }
        public JsonResult getprofiles(string Prefix)
        {

            EvolutyzCornerDataEntities db = new EvolutyzCornerDataEntities();
            var Usermail = (from c in db.UsersProfiles
                            where c.UsrP_EmailID.Contains(Prefix.Trim())
                            select new { c.UsrP_EmailID, c.UsrP_EmployeeID }).ToList();

            return Json(Usermail, JsonRequestBehavior.AllowGet);
        }


        public ActionResult DashBoard2()
        {
            return View();
        }

        // GET: DashBoard/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DashBoard/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DashBoard/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: DashBoard/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DashBoard/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: DashBoard/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DashBoard/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult GetDashboardTimeSheet()
        {
            UserSessionInfo sessId = Session["UserSessionInfo"] as UserSessionInfo;
            string str = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection Conn = new SqlConnection();
            int UserID = sessId.UserId;
            string RoleName = sessId.RoleName;
            ManagerDetailsEntity objmanagerdetails = new ManagerDetailsEntity();
            Conn = new SqlConnection(str);
            try
            {
                objmanagerdetails.mytimesheets = new List<UserTimesheetsEntity>();
                objmanagerdetails.timesheetsforapproval = new List<ManagerTimesheetsforApprovalsEntity>();

                Conn.Open();
                SqlCommand cmd = new SqlCommand("GetDashBoardTimeSheet", Conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userid", UserID);
                cmd.Parameters.AddWithValue("@StartPosition", 1);
                cmd.Parameters.AddWithValue("@EndPosition", 5);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sda.Fill(ds);

                if ((RoleName == "Super Admin") || (RoleName == "Admin") || (RoleName == "Manager"))
                {

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {

                            objmanagerdetails.mytimesheets.Add(new UserTimesheetsEntity
                            {

                                TimesheetID = Convert.ToInt16(dr["Timesheetid"]),
                                ProjectName = dr["Proj_ProjectName"].ToString(),
                                Month_Year = dr["MonthYearName"].ToString(),
                                CompanyBillingHours = dr["Companyworkinghours"].ToString(),
                                ResourceWorkingHours = dr["WorkedHours"].ToString(),
                                TimesheetApprovalStatus = dr["ResultSubmitStatus"].ToString(),
                                Usr_Username = dr["Usr_Username"].ToString(),
                                ManagerApprovalStatus = dr["FinalStatus"].ToString(),
                            });

                        }
                    }

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow druser in ds.Tables[1].Rows)
                        {

                            objmanagerdetails.timesheetsforapproval.Add(new ManagerTimesheetsforApprovalsEntity
                            {

                                TimesheetID = Convert.ToInt16(druser["Timesheetid"]),
                                ProjectName = druser["Proj_ProjectName"].ToString(),
                                Month_Year = druser["MonthYearName"].ToString(),
                                ResourceWorkingHours = Convert.ToInt16(druser["WorkedHours"]),
                                CompanyBillingHours = Convert.ToInt16(druser["Companyworkinghours"]),
                                TimesheetApprovalStatus = druser["ResultSubmitStatus"].ToString(),
                                Usr_Username = druser["Usr_Username"].ToString(),
                                ManagerApprovalStatus = druser["FinalStatus"].ToString(),

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

                            objmanagerdetails.mytimesheets.Add(new UserTimesheetsEntity
                            {
                                TimesheetID = Convert.ToInt16(dr["Timesheetid"]),
                                ProjectName = dr["Proj_ProjectName"].ToString(),
                                Month_Year = dr["MonthYearName"].ToString(),
                                CompanyBillingHours = dr["Companyworkinghours"].ToString(),
                                ResourceWorkingHours = dr["WorkedHours"].ToString(),
                                TimesheetApprovalStatus = dr["ResultSubmitStatus"].ToString(),
                                Usr_Username = dr["Usr_Username"].ToString(),
                                ManagerApprovalStatus = dr["FinalStatus"].ToString(),
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


            var result = new { objmanagerdetails.mytimesheets, objmanagerdetails.timesheetsforapproval, RoleName };


            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public void GetModules()
        {
            (from a in db.Accounts
             join u in db.Users on a.Acc_AccountID equals u.Usr_AccountID
             join r in db.Roles on u.Usr_RoleID equals r.Rol_RoleID
             join gr in db.GenericRoles on r.Rol_RoleName equals gr.GenericRoleID
             join rm in db.RoleModules on r.Rol_RoleID equals rm.RMod_RoleID
             join mm in db.Master_Sub_Module on rm.Sub_ModuleID equals mm.Sub_ModuleID
             join mat in db.ModuleAccessTypes on rm.ModuleAccessTypeID equals mat.ModuleAccessTypeID
             where u.Usr_UserID == Convert.ToInt32(Session["userid"])
             select new

             {
                 UserID = u.Usr_UserID,
                 AccounId = a.Acc_AccountID,
                 RoleName = gr.Title,
                 ModuleAccessType = mat.ModuleAccessType1,
                 ModuleName = mm.Sub_ModuleName
             }).ToList();


        }


        public PartialViewResult GetAdminMenu()
        {

            var model = (from a in db.Accounts
                         join u in db.Users on a.Acc_AccountID equals u.Usr_AccountID
                         join r in db.Roles on u.Usr_RoleID equals r.Rol_RoleID
                         join gr in db.GenericRoles on r.Rol_RoleName equals gr.GenericRoleID
                         join rm in db.RoleModules on r.Rol_RoleID equals rm.RMod_RoleID
                         join mm in db.Master_Sub_Module on rm.Sub_ModuleID equals mm.Sub_ModuleID
                         join mat in db.ModuleAccessTypes on rm.ModuleAccessTypeID equals mat.ModuleAccessTypeID
                         where u.Usr_UserID == Convert.ToInt32(Session["userid"])
                         select new

                         {
                             UserID = u.Usr_UserID,
                             AccounId = a.Acc_AccountID,
                             RoleName = gr.Title,
                             ModuleAccessType = mat.ModuleAccessType1,
                             ModuleName = mm.Sub_ModuleName
                         }).ToList();

            //  var model = new AdminMenuViewModel();

            return PartialView(model);
        }
    }
}
