using Evolutyz.Business;
using Evolutyz.Data;
using Evolutyz.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace EvolutyzCorner.UI.Web.Controllers
{

    [Authorize]
    [EvolutyzCorner.UI.Web.MvcApplication.SessionExpire]
    [EvolutyzCorner.UI.Web.MvcApplication.NoDirectAccess]
    public class ClientController : Controller
    {
        int projectid;
        // GET: Client
        public ActionResult Index(int proid)

        {

            HomeController hm = new HomeController();
            var obj = hm.GetAdminMenu();
            foreach (var item in obj)
            {
               
                 if (item.ModuleName == "Add User(Manager/Employee)")
                {
                    var mk = item.ModuleAccessType;
                    ViewBag.b = mk;
                }
                 if (item.ModuleName == "Add Client Holidays")
                {
                    var mk = item.ModuleAccessType;
                    ViewBag.c = mk;
                }

                if (item.ModuleName == "Add Project/Client")
                {
                    var mk = item.ModuleAccessType;
                    
                    ViewBag.a = mk;

                }
                if (item.ModuleName == "Project Task(Sub tasks)")
                {
                    var mk = item.ModuleAccessType;

                    ViewBag.d = mk;
                }
                if (item.ModuleName == "Associate Employee")
                {
                    var mk = item.ModuleAccessType;

                    ViewBag.e = mk;
                }
                if (item.ModuleName == "AddHoliday Calendar")
                {
                    var mk = item.ModuleAccessType;

                    ViewBag.f = mk;
                }
            }
            if (proid != null)
            {


                UserSessionInfo _objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
                int AccountId = _objSessioninfo.AccountId;
                int UserId = _objSessioninfo.UserId;
                ViewBag.AccountID = AccountId;
                ViewBag.UserId = UserId;
                bool? usaccount = _objSessioninfo.UsAccount;
                ViewBag.usaccount = usaccount;
                //dynamic id = TempData["mydata"];
                //ViewBag.id = id;
                ViewBag.id = proid;
                dynamic procode = TempData["projcode"];
                ViewBag.procode = procode;

                ClientComponent clientcomponent = new ClientComponent();
                var Gender = clientcomponent.GetGenders().Select(a => new SelectListItem()
                {
                    Value = a.Usr_GenderId.ToString(),
                    Text = a.Gender,

                });
                ViewBag.Gender = Gender;
               
                var accspecifictasks = clientcomponent.Getaccountspecifictasks().Select(a => new SelectListItem()
                {
                    Value = a.Acc_SpecificTaskId.ToString(),
                    Text = a.Acc_SpecificTaskName,

                });

                ViewBag.accspecifictasks = accspecifictasks;
                ViewBag.usertitle = clientcomponent.GetTitle().Select(a => new SelectListItem()
                {
                    Value = a.Usr_Titleid.ToString(),
                    Text = a.TitlePrefix,

                });
                ViewBag.countrycode = clientcomponent.GetCountryCodes().Select(a => new SelectListItem()
                {
                    Value = a.CountryId.ToString(),
                    Text = a.PhoneCode,

                });
                var countries = clientcomponent.GetCountryNames().Select(a => new SelectListItem()
                {
                    Value = a.CountryId.ToString(),
                    Text = a.CountryName,

                });
                ViewBag.countries = countries;
                var Timesheetmodes = clientcomponent.GetTimeSheetModes(AccountId).Select(a => new SelectListItem()
                {
                    Value = a.TimesheetMode_id.ToString(),
                    Text = a.TimeModeName,

                });
                ViewBag.Timesheetmodes = Timesheetmodes;
                var LeaveSchemeComponent = new LeaveSchemeComponent();
                var FinancialYears = LeaveSchemeComponent.Getallfinancialyears().Select(a => new SelectListItem()
                {
                    Value = a.FinancialYearId.ToString(),
                    Text = a.financialyear,
                }).OrderByDescending(x=>x.Value);
                ViewBag.FinancialYears = FinancialYears;
                EvolutyzCornerDataEntities db = new EvolutyzCornerDataEntities();

                var client = (from em in db.Projects select em).Select(a => new SelectListItem()
                {

                    Value = a.Proj_ProjectID.ToString(),
                    Text = a.Proj_ProjectName,


                });

                ViewBag.client = client;

               
               
                return View();
            }
            else
            {
                return View();
            }
        }
        


        public JsonResult getclientforproject(int projid)
        {

            List<ClientEntity> ProjectDetails = null;

            var objDtl = new ClientComponent();
            ProjectDetails = objDtl.getclientprojectsdropdown(projid);

            return Json(ProjectDetails, JsonRequestBehavior.AllowGet);
                 
        }

        public ActionResult EditIndex(int proid)
        {
            int indexprojectid = proid;
            TempData["mydata"] = indexprojectid;
            return RedirectToAction("Index");

        }

        public string CreateClientProject(int? client,string clientprojecttitle,string clientprojdescription)
        {
            string strresponse = string.Empty;
            try
            {
                EvolutyzCornerDataEntities DB = new EvolutyzCornerDataEntities();
              
                UserSessionInfo _objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
                int? AccountId = _objSessioninfo.AccountId;
                //Role orgCheck = db.Set<Role>().Where(s => (s.Rol_RoleCode == user.Rol_RoleCode && s.Rol_AccountID == user.Rol_AccountID)).FirstOrDefault<Role>();

                ClientProject checktitle = DB.Set<ClientProject>().Where(s => (s.ClientProjTitle == clientprojecttitle && s.Proj_ProjectID== client)).FirstOrDefault<ClientProject>();
                if (checktitle != null)
                {
                    return strresponse = "Project Name Already Existed";
                }
                DB.Set<ClientProject>().Add(new ClientProject
                {

                    Proj_ProjectID = client,
                    ClientProjDesc = clientprojdescription,
                    ClientProjTitle = clientprojecttitle,
                    Accountid = AccountId,

                });

                DB.SaveChanges();
                strresponse = "Successfully Project Added";
            }

            catch(Exception ex)
            {

                throw ex;

            }

            return strresponse;
        }
        public JsonResult updatecp(int client, string clientprojecttitle, string clientprojdescription, int accid, int projid)
        {

            try
            {
                EvolutyzCornerDataEntities DB = new EvolutyzCornerDataEntities();

                UserSessionInfo _objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
                int? AccountId = _objSessioninfo.AccountId;


                //    DB.Set<ClientProject>().Add(new ClientProject
                //  {
                ClientProject obj = new ClientProject();

                //  Proj_ProjectID = client,
                obj.Accountid = accid;
                obj.Proj_ProjectID = projid;

                obj.CL_ProjectID = client;
                obj.ClientProjDesc = clientprojdescription;
                obj.ClientProjTitle = clientprojecttitle;
                //Accountid = AccountId,

                //    });
                DB.Entry(obj).State = System.Data.Entity.EntityState.Modified;

                DB.SaveChanges();
            }

            catch (Exception ex)
            {



            }

            return Json("Succesfully Updated", JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateClientProject(int id)
        {
            ProjectAllocationEntities ProjectUserDetails = null;
            string strResponse = string.Empty;
            short UsTCurrentVersion = 0;
            try
            {

                UserSessionInfo _objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
                int _userID = _objSessioninfo.UserId;

                var Org = new ClientComponent();


                EvolutyzCornerDataEntities db = new EvolutyzCornerDataEntities();

                ProjectUserDetails = Org.GetClientProjbyid(id);


            }
            catch (Exception ex)
            {

            }
            return Json(ProjectUserDetails, JsonRequestBehavior.AllowGet);
        }
        // [HttpGet]
        public JsonResult getclientprojects(int projid)
        {

            List<ClientEntity> ProjectDetails = null;
           
            var objDtl = new ClientComponent();
            ProjectDetails = objDtl.getclientprojects(projid);

            return Json(ProjectDetails, JsonRequestBehavior.AllowGet);
        }


        public string SequenceCode()
        {
            int getProjectCode;
            string mix = string.Empty;
            try
            {
                SqlConnection con = null;
                string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                con = new SqlConnection(constr);

                con.Open();
                SqlCommand cmd = new SqlCommand("select NEXT VALUE FOR ProjectCode", con);
                getProjectCode = Convert.ToInt16(cmd.ExecuteScalar());
                con.Close();
                string prefix = "PROJ";
                int i = getProjectCode;
                string s = i.ToString().PadLeft(5, '0');
                mix = prefix + " " + s;
                TempData["projcode"] = mix;
                return mix;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult GetManagerByRole(int projid, int userid)
        {
            List<UserEntity> l2Manger = null;
            UserSessionInfo objinfo = new UserSessionInfo();
            int AccountId = objinfo.AccountId;
            try
            {
                var objDtl = new ClientComponent();
                l2Manger = objDtl.GetManagerByRole(projid, userid, AccountId);

            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(l2Manger, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetManagerOnChange(int projid, int ManagerID)
        {
            List<UserEntity> l2Manger = null;
            UserSessionInfo objinfo = new UserSessionInfo();
            int AccountId = objinfo.AccountId;
            try
            {
                var objDtl = new ClientComponent();
                l2Manger = objDtl.GetManagerOnChange(projid, ManagerID);

            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(l2Manger, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetL2Manager(int projid)
        {
            List<UserEntity> l2Manger = null;
            try
            {
                var objDtl = new ClientComponent();
                l2Manger = objDtl.GetManagerNames2(projid);

            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(l2Manger, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetL1Manager(int projid)
        {
            List<UserEntity> l1Manger = null;
            try
            {
                var objDtl = new ClientComponent();
                l1Manger = objDtl.GetManagerNames(projid);

            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(l1Manger, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult GetCountries()
        {
            List<ProjectEntity> Countries = null;
            try
            {
                var objDtl = new ClientComponent();
                Countries = objDtl.GetCountryNames();

            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(Countries, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStates(int CountryId)
        {
            List<ProjectEntity> States = null;
            try
            {
                var objDtl = new ClientComponent();
                States = objDtl.GetStateNames(CountryId);

            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(States, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRoles()
        {
            List<OrganizationAccountEntity> UserRoles = null;
            UserSessionInfo sessionInfo = new UserSessionInfo();
            int accountid = sessionInfo.AccountId;
            try
            {
                var objDtl = new ClientComponent();
                UserRoles = objDtl.GetUserRolenames(accountid);

            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(UserRoles, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllRoles()
        {
            List<OrganizationAccountEntity> UserRoles = null;
            try
            {
                var objDtl = new ClientComponent();
                UserRoles = objDtl.GetAllRoles();

            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(UserRoles, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRoleNamesbyemp()
        {
            UserSessionInfo objinfo = new UserSessionInfo();
            int accountid = objinfo.AccountId;
            List<OrganizationAccountEntity> UserRoles = null;
            try
            {
                var objDtl = new ClientComponent();
                UserRoles = objDtl.GetRoleNamesbyemp(accountid);

            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(UserRoles, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult GetAlltasknames(int projid,int Roleid)
        {
            List<ProjectAllocationEntities> TaskNames = null;
            try
            {
                var objDtl = new ClientComponent();
                TaskNames = objDtl.GetAlltasknames(projid,Roleid);

            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(TaskNames, JsonRequestBehavior.AllowGet);
        }
        public JsonResult gettasknames(int projid)
        {
            List<ProjectAllocationEntities> TaskNames = null;
            try
            {
                var objDtl = new ClientComponent();
                TaskNames = objDtl.gettasknames(projid);

            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(TaskNames, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateClient(ProjectEntity ProjectDtl)
        {
            //int strResponse ;
            int ProjectId;
            try
            {
                var ProjectComponent = new ClientComponent();
                string code = SequenceCode();

                UserSessionInfo _objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;

                int AccountId = _objSessioninfo.AccountId;
                int _userID = _objSessioninfo.UserId;
                ProjectDtl.Proj_ProjectCode = code;
                ProjectDtl.Proj_CreatedBy = _userID;
                ProjectDtl.Proj_AccountID = AccountId;

                var Org = new ClientComponent();
                var r = Org.AddProject(ProjectDtl);

            }

            catch (Exception ex)
            {
                return Json(ProjectDtl, JsonRequestBehavior.AllowGet);
            }
            return Json(new { ProjectId = ProjectDtl.Proj_ProjectID }, JsonRequestBehavior.AllowGet);
        }

        public string UpdateClient(ProjectEntity project)
        {
            string strResponse = string.Empty;
            short UsTCurrentVersion = 0;
            try
            {

                UserSessionInfo _objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
                int _userID = _objSessioninfo.UserId;
                project.Proj_ModifiedBy = _userID;
                //while udating increment version by1
                project.Proj_Version = ++UsTCurrentVersion;
                // project.Proj_ActiveStatus = UsTCurrentStatus;
                var Org = new ClientComponent();
                int r = Org.UpdateclientDetails(project);
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
                    strResponse = "Please Fill All Mandatory Fields";
                }

            }
            catch (Exception ex)
            {
                return strResponse;
            }
            return strResponse;
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

        public string CreateManager([Bind(Exclude = "Ufp_UsersForProjectsID")] ProjectAllocationEntity ProjectDtl)
        {
            string strResponse = string.Empty;
            try
            {
                //var ProjectComponent = new ProjectAssignComponent();

                //if (ModelState.IsValid)
                //{
                UserSessionInfo _objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
                int _userID = _objSessioninfo.UserId;
                ProjectDtl.UProj_CreatedBy = _userID;

                var Org = new ClientComponent();
                int r = Org.AddManager(ProjectDtl);
                if (r > 0)
                {
                    strResponse = "Record created successfully";
                }
                else if (r == 0)
                {
                    strResponse = "Record already exists";
                }
                else if (r < 0)
                {
                    strResponse = "Error occured in CreateProjectAllocation";
                }
                // }
            }
            catch (Exception ex)
            {
                return strResponse;
            }
            return strResponse;
        }

        public JsonResult GetClientByID(int catID)
        {
            ProjectEntity ProjectDetails = null;
            try
            {
                var objDtl = new ClientComponent();
                ProjectDetails = objDtl.GetClientDetailByID(catID);
            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(ProjectDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string ManagerSave(ClientEntity formdata)
        {
            var clicomponent = new ClientComponent();
            UserSessionInfo objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
            int _userID = objSessioninfo.UserId;
            formdata.UProj_CreatedBy = _userID;
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
                formdata.Usrp_ProfilePicture = "User.png";
            }
            else
            {
                formdata.Usrp_ProfilePicture = imagename;
            }
            //formdata.Usrp_ProfilePicture = imagename;
            formdata.Usr_Password = GetMD5(formdata.Usr_Password);
            string response = clicomponent.Savemanager(formdata);
              return response;

            //return Json("", JsonRequestBehavior.AllowGet);
        }
        
        public string SaveEmployee(ClientEntity ProjectDtl)
        {
            var clicomponent = new ClientComponent();
            UserSessionInfo objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
            int _userID = objSessioninfo.UserId;
            ProjectDtl.UProj_CreatedBy = _userID;
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
                ProjectDtl.Usrp_ProfilePicture = "User.png";
            }
            else
            {
                ProjectDtl.Usrp_ProfilePicture = imagename;
            }
           // ProjectDtl.Usrp_ProfilePicture = imagename;
            ProjectDtl.Usr_Password = GetMD5(ProjectDtl.Usr_Password);

            string response = clicomponent.SaveEmployee(ProjectDtl);
            return response;

        }

        public JsonResult GetProjectAllocationCollection(int id)
        {
            List<ProjectAllocationEntities> ProjectDetails = null;
            try
            {
                ClientComponent objDtl = new ClientComponent();
                ProjectDetails = objDtl.GetProjectUserDetails(id);
                TempData["projectdata"] = ProjectDetails;
            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(ProjectDetails, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GEtprojectdetails(int id)
        {
            List<ProjectAllocationEntities> ProjectDetails = null;
            try
            {
                ClientComponent objDtl = new ClientComponent();
                ProjectDetails = objDtl.GetProjectUserDetails(id);
                TempData["projectdata"] = ProjectDetails;
            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(ProjectDetails, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserDetailById(int id, string proid)
        {
            ProjectAllocationEntities ProjectUserDetails = null;
            try
            {
                var objDtl = new ClientComponent();
                ProjectUserDetails = objDtl.GetUserDetailById(id, proid);
            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(ProjectUserDetails, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetassUserDetailById(int id)
        {
            ProjectAllocationEntities ProjectUserDetails = null;
            try
            {
                var objDtl = new ClientComponent();
                ProjectUserDetails = objDtl.GetassUserDetailById(id);
            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(ProjectUserDetails, JsonRequestBehavior.AllowGet);
        }

        public string updateuserprojectbyid(ClientEntity ProjectDtl)
        {
            var clicomponent = new ClientComponent();
            UserSessionInfo objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
            int _userID = objSessioninfo.UserId;
            ProjectDtl.UProj_CreatedBy = _userID;
            string imagename = string.Empty;
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                var fileName = "/uploadimages/Images/" + file.FileName;
                imagename = file.FileName;
                var imagepath = Server.MapPath(fileName);
                file.SaveAs(imagepath);
            }
            else
            {
                imagename = ProjectDtl.Usrp_ProfilePicture;
            }
            ProjectDtl.Usrp_ProfilePicture = imagename;
            if (ProjectDtl.Usr_Password== null)
            {
                ProjectDtl.Usr_Password = "";
            }
            else
            {
                ProjectDtl.Usr_Password = GetMD5(ProjectDtl.Usr_Password);
            }
           
            string response = clicomponent.updateuserdetails(ProjectDtl);
            return response;
        }

        public string DeleteUser(int userid)
        {
            string strResponse = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    var Org = new ClientComponent();
                    int r = Org.DeleteUser(userid);

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
                        strResponse = "Error occured in DeleteProject";
                    }
                }
            }
            catch (Exception ex)
            {
                return strResponse;
            }
            return strResponse;
        }
        
        public JsonResult GetHolidays(int projectid)
        {
            List<ProjectAllocationEntities> holidaylist = new List<ProjectAllocationEntities>();
            var clientcomponent = new ClientComponent();

            holidaylist = clientcomponent.GetHolidays(projectid);
            return Json(holidaylist, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Getprotasks(int projectid)
        {
            List<ProjectAllocationEntities> tasklist = new List<ProjectAllocationEntities>();
            var clientcomponent = new ClientComponent();

            tasklist = clientcomponent.Getprotasks(projectid);
            return Json(tasklist, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public string Addprotasks(string Acc_SpecificTaskName, string ProjectID, string Proj_SpecificTaskName, string RTMId, string Actual_StartDate, string Actual_EndDate,string Plan_StartDate,string Plan_EndDate,string StatusId)
        {
            string strResponse = string.Empty;
            UserSessionInfo objinfo = new UserSessionInfo();
            int accountid = objinfo.AccountId;
            var clientcomponent = new ClientComponent();

            if (ModelState.IsValid)
            {
                strResponse = clientcomponent.Addprotasks( Acc_SpecificTaskName,  ProjectID,  Proj_SpecificTaskName,  RTMId,  Actual_StartDate,  Actual_EndDate,  Plan_StartDate,  Plan_EndDate,  StatusId);
            }
            return strResponse;
        }
        
        [HttpPost]
        public string updatetasks(int id,string Acc_SpecificTaskName, string ProjectID, string Proj_SpecificTaskName, string RTMId, string Actual_StartDate, string Actual_EndDate, string Plan_StartDate, string Plan_EndDate, string StatusId)
        {
            string strResponse = string.Empty;
            UserSessionInfo objinfo = new UserSessionInfo();
            int accountid = objinfo.AccountId;
            var clientcomponent = new ClientComponent();

            if (ModelState.IsValid)
            {
                strResponse = clientcomponent.updatetasks(id,Acc_SpecificTaskName, ProjectID, Proj_SpecificTaskName, RTMId, Actual_StartDate, Actual_EndDate, Plan_StartDate, Plan_EndDate, StatusId);
            }
            return strResponse;
        }

        public JsonResult getprojecttaskbyid(int id)
        {
            List<ProjectAllocationEntities> tasklist = new List<ProjectAllocationEntities>();
            var clientcomponent = new ClientComponent();

            tasklist = clientcomponent.getprojecttaskbyid(id);
            return Json(tasklist, JsonRequestBehavior.AllowGet);
        }

        public string DeleteProjecttask(string id)
        {
            string response = string.Empty;
            ClientComponent compobj = new ClientComponent();
            response = compobj.DeleteProjecttask(id);
            return response;
        }

        public JsonResult Associatemanagers(int projectid)
        {
            UserSessionInfo info = new UserSessionInfo();
            int accountid = info.AccountId;
            ClientComponent component = new ClientComponent();
            List<ProjectAllocationEntities> managerlist = new List<ProjectAllocationEntities>();
            managerlist = component.Associatemanagers(projectid, accountid);
            return Json(managerlist, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AssociateEmployees(int projectid)
        {
            UserSessionInfo info = new UserSessionInfo();
            int accountid = info.AccountId;
            ClientComponent component = new ClientComponent();
            List<ProjectAllocationEntities> managerlist = new List<ProjectAllocationEntities>();
            managerlist = component.AssociateEmployees(projectid, accountid);
            return Json(managerlist, JsonRequestBehavior.AllowGet);
        }

        public string AssociateEmployee(ClientEntity ProjectDtl)
        {
            var clicomponent = new ClientComponent();
            UserSessionInfo objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
            int _userID = objSessioninfo.UserId;
            ProjectDtl.UProj_CreatedBy = _userID;
            string imagename = string.Empty;
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                var fileName = "/uploadimages/Images/" + file.FileName;
                imagename = file.FileName;
                var imagepath = Server.MapPath(fileName);
                file.SaveAs(imagepath);
            }
            ProjectDtl.Usrp_ProfilePicture = imagename; if (ProjectDtl.Usr_Password == null)
            {
                ProjectDtl.Usr_Password = "";
            }
            else
            {
                ProjectDtl.Usr_Password = GetMD5(ProjectDtl.Usr_Password);
            }
            ProjectDtl.Usr_Password = GetMD5(ProjectDtl.Usr_Password);

            string response = clicomponent.AssociateEmployee(ProjectDtl);
            return response;

        }

        public string AssociateManager(ClientEntity ProjectDtl)
        {
            var clicomponent = new ClientComponent();
            UserSessionInfo objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
            int _userID = objSessioninfo.UserId;
            ProjectDtl.UProj_CreatedBy = _userID;
            string imagename = string.Empty;
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                var fileName = "/uploadimages/Images/" + file.FileName;
                imagename = file.FileName;
                var imagepath = Server.MapPath(fileName);
                file.SaveAs(imagepath);
            }
            ProjectDtl.Usrp_ProfilePicture = imagename; if (ProjectDtl.Usr_Password == null)
            {
                ProjectDtl.Usr_Password = "";
            }
            else
            {
                ProjectDtl.Usr_Password = GetMD5(ProjectDtl.Usr_Password);
            }
            ProjectDtl.Usr_Password = GetMD5(ProjectDtl.Usr_Password);

            string response = clicomponent.AssociateManager(ProjectDtl);
            return response;

        }

        public string ChangeStatus(string id, string status)
        {
            string strResponse = string.Empty;
            var objDtl = new ClientComponent();
            strResponse = objDtl.ChangeStatus(id, status);

            return strResponse;
        }

    }
}