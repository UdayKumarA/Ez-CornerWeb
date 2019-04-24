using Evolutyz.Business;
using Evolutyz.Data;
using Evolutyz.Entities;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace EvolutyzCorner.UI.Web.Controllers.Projects
{
    //[Authorize(Roles = "Admin")]
    [Authorize]
    [EvolutyzCorner.UI.Web.MvcApplication.SessionExpire]
    [EvolutyzCorner.UI.Web.MvcApplication.NoDirectAccess]
    public class ProjectController : Controller
    {
        //UserSessionInfo objSessioninfo = new UserSessionInfo();

        public ActionResult Index(bool? pdf)
        {

            UserSessionInfo _objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
            int AccountId = _objSessioninfo.AccountId;
            ViewBag.AccountID = AccountId;
            HomeController hm = new HomeController();
            var obj = hm.GetAdminMenu();
            foreach (var item in obj)
            {
                if (item.ModuleName== "Add Project/Client")
                {
                    var mk = item.ModuleAccessType;


                    ViewBag.a = mk;

                }

                

            }
            return View();
          
        }

      
        public ActionResult AssignUsersToProjects()
        {
            return View();
        }

        public ActionResult SequenceCode()
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

                return Json(mix, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string CreateProject([Bind(Exclude = "Proj_ProjectID")] ProjectEntity ProjectDtl)
        {
            string strResponse = string.Empty;

            try
            {
                var ProjectComponent = new ProjectComponent();

                if (ModelState.IsValid)
                {
                    UserSessionInfo _objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;

                    int _userID = _objSessioninfo.UserId;
                    ProjectDtl.Proj_CreatedBy = _userID;

                    var Org = new ProjectComponent();
                    int r = Org.AddProject(ProjectDtl);
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
                        strResponse = "Error occured in CreateProject";
                    }
                }
            }
            catch (Exception ex)
            {
                return strResponse;
            }
            return strResponse;
        }

        public string UpdateProject(ProjectEntity ProjectDtl)
        {
            string strResponse = string.Empty;
            short UsTCurrentVersion = 0;
            try
            {
                var ProjectComponent = new ProjectComponent();
                var currentProjectDetails = ProjectComponent.GetProjectDetailByID(ProjectDtl.Proj_ProjectID);
                int ProjectID = currentProjectDetails.Proj_ProjectID;
                UsTCurrentVersion = Convert.ToInt16(currentProjectDetails.Proj_Version);
                bool _currentStatus = false;

                //check for version and active status
                if (ModelState["Proj_ActiveStatus"].Value != null)
                {
                    _currentStatus = ProjectDtl.Proj_ActiveStatus == true;
                }

                if (ModelState.IsValid)
                {
                    UserSessionInfo _objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
                    int _userID = _objSessioninfo.UserId;
                    ProjectDtl.Proj_ModifiedBy = _userID;
                    //while udating increment version by1
                    ProjectDtl.Proj_Version = ++UsTCurrentVersion;
                    ProjectDtl.Proj_ActiveStatus = _currentStatus;

                    var Org = new ProjectComponent();
                    int r = Org.UpdateProjectDetail(ProjectDtl);

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
                        strResponse = "Error occured in UpdateProject";
                    }
                }
            }
            catch (Exception ex)
            {
                return strResponse;
            }
            return strResponse;
        }




        #region ProjectAssign - Abhilash Method
        //public ActionResult ProjectAssign()
        //{
        //    EvolutyzCornerDataEntities db = new EvolutyzCornerDataEntities();

        //    IEnumerable<SelectListItem> items = db.Projects.Select(c => new SelectListItem
        //    {
        //        Value = c.Proj_ProjectName,
        //        Text = c.Proj_ProjectName
        //    });
        //    ViewBag.ProjName = items;

        //    IEnumerable<SelectListItem> item = db.Users.Select(c => new SelectListItem
        //    {
        //        Value = c.Usr_LoginId,
        //        Text = c.Usr_LoginId
        //    });
        //    ViewBag.UserNames = item;

        //    return View();
        //}
        #endregion

        //#region ProjectAssign
        //public ActionResult ProjectAssign()
        //{
        //    try
        //    {
        //        List<ProjectEntity> ProjectDetails = null;
        //        List<UserEntity> UserDetails = null;

        //        var objDtl = new ProjectComponent();
        //        var _usersList = new UserComponent();

        //        ProjectDetails = objDtl.GetProjectDetail();

        //        IEnumerable<SelectListItem> items = ProjectDetails.Select(c => new SelectListItem
        //        {
        //            Value = c.Proj_ProjectID.ToString(),
        //            Text = c.Proj_ProjectName
        //        });
        //        ViewBag.ProjName = items;

        //        UserDetails = _usersList.GetUserDetail();
        //        IEnumerable<SelectListItem> item = UserDetails.Select(c => new SelectListItem
        //        {
        //            Value = c.Usr_LoginId.ToString(),
        //            Text = c.Usr_LoginId
        //        });
        //        ViewBag.UserNames = item;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }

        //    return View();
        //}
        //#endregion


        [HttpPost]
        public ActionResult ProjectAssign(string dropdown1, string dropdown2, string from, string To, string per)
        {

            EvolutyzCornerDataEntities DB = new EvolutyzCornerDataEntities();

            Project_workstatus OBJ = new Project_workstatus();


            OBJ.ProjectName = dropdown1;

            OBJ.UserName = dropdown2;

            OBJ.FromDT = Convert.ToDateTime(from);

            OBJ.ToDT = Convert.ToDateTime(To);

            OBJ.PercentageCompleted = per;


            DB.Project_workstatus.Add(OBJ);

            DB.SaveChanges();


            return Json(OBJ, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowWorkStatus()
        {



            return View();
        }

        public ActionResult getproject()
        {

            EvolutyzCornerDataEntities DB = new EvolutyzCornerDataEntities();
            Project_workstatus obj = new Project_workstatus();

            Convert.ToDateTime(obj.FromDT);


            var result = DB.Project_workstatus.ToList();




            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public string DeleteProject(int ProjectID)
        {
            string strResponse = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    var Org = new ProjectComponent();
                    int r = Org.DeleteProjectDetail(ProjectID);

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

        public JsonResult GetProjectNames()
        {

            List<ProjectEntity> ProjectNames = null;
            try
            {
                var objDtl = new ProjectComponent();
                ProjectNames = objDtl.GetProjectNames();

            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(ProjectNames, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRoleNames()
        {
            List<RoleEntity> RoleNames = null;
            try
            {
                var objDtl = new ProjectComponent();
                RoleNames = objDtl.GetUserRoles();

            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(RoleNames, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProjectCollection()
        {
            List<ProjectEntity> ProjectDetails = null;
            try
            {
                UserSessionInfo _objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
                int AccountId = _objSessioninfo.AccountId;
                var objDtl = new ProjectComponent();
                ProjectDetails = objDtl.GetProjectDetail(AccountId);
                ViewBag.ProjectDetails = ProjectDetails[0].Proj_Version;
                ViewBag.AccountId = AccountId;
            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(ProjectDetails, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProjectByID(int catID)
        {
            ProjectEntity ProjectDetails = null;
            try
            {
                var objDtl = new ProjectComponent();
                ProjectDetails = objDtl.GetProjectDetailByID(catID);
            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(ProjectDetails, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProjectHistoryByID(int ID)
        {
            List<History_ProjectsEntity> HisProjectDetails = null;
            try
            {
                var objDtl = new ProjectComponent();
                HisProjectDetails = objDtl.GetHistoryProjectDetailsByID(ID);
                ViewBag.ProjectDetails = HisProjectDetails;
            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(HisProjectDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string CrudOperation(FormCollection formCollection)
        {
            string strResponse = string.Empty;
            try
            {
                var strOperation = formCollection["oper"];

                if (strOperation == "add")
                {
                    ProjectEntity updateList = new ProjectEntity
                    {
                        Proj_AccountID = Convert.ToInt32(formCollection.Get("Proj_AccountID")),
                        Proj_ProjectCode = formCollection.Get("Proj_ProjectCode"),
                        Proj_ProjectName = formCollection.Get("Proj_ProjectName"),
                        Proj_ProjectDescription = formCollection.Get("Proj_ProjectDescription"),
                        Proj_StartDate = Convert.ToDateTime(formCollection.Get("Proj_StartDate")),
                        Proj_EndDate = Convert.ToDateTime(formCollection.Get("Proj_EndDate")),
                        Proj_Version = 1,
                        Proj_ActiveStatus = Convert.ToBoolean(formCollection.Get("Proj_ActiveStatus")),
                        Proj_isDeleted = false
                    };
                    strResponse = CreateProject(updateList);
                }
                else if (strOperation == "edit")
                {
                    ProjectEntity updateList = new ProjectEntity
                    {
                        Proj_ProjectID = Convert.ToInt32(formCollection.Get("id")),
                        Proj_AccountID = Convert.ToInt32(formCollection.Get("Proj_AccountID")),
                        Proj_ProjectCode = formCollection.Get("Proj_ProjectCode"),
                        Proj_ProjectName = formCollection.Get("Proj_ProjectName"),
                        Proj_ProjectDescription = formCollection.Get("Proj_ProjectDescription"),
                        Proj_StartDate = Convert.ToDateTime(formCollection.Get("Proj_StartDate")),
                        Proj_EndDate = Convert.ToDateTime(formCollection.Get("Proj_EndDate")),
                        Proj_ActiveStatus = Convert.ToBoolean(formCollection.Get("Proj_ActiveStatus")),
                        Proj_isDeleted = Convert.ToBoolean(formCollection.Get("Proj_isDeleted"))
                    };
                    strResponse = UpdateProject(updateList);
                }
                else if (strOperation == "del")
                {
                    int ProjectID = Convert.ToInt32(formCollection.Get("id"));
                    strResponse = DeleteProject(ProjectID);
                }
            }
            catch (Exception ex)
            {
                return strResponse;
            }
            return strResponse;
        }

        #region Export to PDF

        //private static void ExportPDF<TSource>(IList<TSource> ProjectList, string[] columns, string filePath)
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

        //    foreach (var item in ProjectList)
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

        public string ChangeStatus(string id, string status)
        {
            string strResponse = string.Empty;
            var objDtl = new ProjectComponent();
            strResponse = objDtl.ChangeStatus(id, status);

            return strResponse;
        }

    }
}