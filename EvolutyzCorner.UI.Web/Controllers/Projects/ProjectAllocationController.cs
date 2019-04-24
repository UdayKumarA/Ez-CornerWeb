using Evolutyz.Business;
using Evolutyz.Data;
using Evolutyz.Entities;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace EvolutyzCorner.UI.Web.Controllers.Projects
{
    //[Authorize(Roles = "Admin")]
    [Authorize]
    [EvolutyzCorner.UI.Web.MvcApplication.SessionExpire]
    [EvolutyzCorner.UI.Web.MvcApplication.NoDirectAccess]
    public class ProjectAllocationController : Controller
    {
        int id;
        //DateTime startDate;
        //DateTime endDate;

        //UserSessionInfo objSessioninfo = new UserSessionInfo();
        //  int  Projectid;

        //public ActionResult Index(bool? pdf)
        //{
        //   // Projectid = ProjectID;

        //    //if (!pdf.HasValue)
        //    //{
        //    //    #region to return ProjectList

        //    //    //objSessioninfo.UserId = 501;
        //    //    //Session["UserSessionInfo"] = objSessioninfo;

        //    //    List<LookupStatusDetail> objStatusList = new List<LookupStatusDetail>();
        //    //    objStatusList.Add(new LookupStatusDetail { StatusID = 1, Status = "Active" });
        //    //    objStatusList.Add(new LookupStatusDetail { StatusID = 0, Status = "InActive" });

        //    //    var objStList = from cl in objStatusList
        //    //                    orderby cl.StatusID
        //    //                    select new
        //    //                    {
        //    //                        value = cl.StatusID,
        //    //                        text = cl.Status
        //    //                    };
        //    //    ViewBag.Status = objStList;
        //    //    return View();
        //    //    #endregion
        //    //}
        //    //else
        //    //{
        //    //    string filename = "Project.pdf";
        //    //    string filePath = Server.MapPath("~/Content/PDFs/" + filename);

        //    //    var objDtl = new ProjectComponent();
        //    //    IList<ProjectEntity> ProjectList = objDtl.GetProjectDetail();

        //    //    ExportPDF(ProjectList, new string[] { "Ufp_ProjectID", "AccountName", "Ufp_
        // ", "Ufp_ProjectName", "Ufp_ProjectDescription", "Ufp_StartDate", "Ufp_EndDate" }, filePath);

        //    //    return File(filePath, "application/pdf");
        //    //}
        //    UserSessionInfo _objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
        //    int AccountId = _objSessioninfo.AccountId;
        //    string LoginId = _objSessioninfo.LoginId;
        //    ViewBag.LoginId = LoginId;
        //    ViewBag.AccountID = AccountId;

        //    return View();
        //}


        public ActionResult Index(DateTime? Proj_StartDate, DateTime? Proj_EndDate, string projectCode, string Proj_ProjectName, int ProjectId = 0)
        {

            HomeController hm = new HomeController();
            var obj = hm.GetAdminMenu();
            foreach (var item in obj)
            {
                if (item.ModuleAccessType.ToLower() == "read/write")
                {
                    var mk = item.ModuleAccessType;


                    ViewBag.a = mk;

                }

                else
                {

                    var mk = item.ModuleAccessType;


                    ViewBag.a = mk;


                }

            }

            if (ProjectId != 0)
            {
                UserSessionInfo _objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
                int AccountId = _objSessioninfo.AccountId;
                string LoginId = _objSessioninfo.LoginId;
                ViewBag.LoginId = LoginId;
                ViewBag.AccountID = AccountId;
                id = ProjectId;
                ViewBag.id = id;
                var startDate = Proj_StartDate;

                ViewBag.startDate = string.Format("{0:dd/MM/yyyy}", startDate);
                var endDate = Proj_EndDate;
                ViewBag.endDate = string.Format("{0:dd/MM/yyyy}", endDate);
                var projcode = projectCode;
                ViewBag.projcode = projcode;
                var projName = Proj_ProjectName;
                ViewBag.projName = projName;
            }
            else
            {
                id = ProjectId;
                UserSessionInfo _objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
                int AccountId = _objSessioninfo.AccountId;
                string LoginId = _objSessioninfo.LoginId;
                ViewBag.LoginId = LoginId;
                ViewBag.AccountID = AccountId;
            }

            return View();
        }

        public string CreateProjectAllocation([Bind(Exclude = "Ufp_UsersForProjectsID")] ProjectAllocationEntity ProjectDtl)
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

                var Org = new ProjectAssignComponent();
                int r = Org.AddProjectAllocation(ProjectDtl);
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
        private DateTime getDate()
        {
            return DateTime.ParseExact("", "dd/mm/yyyy", null);
        }



        public string UpdateProjectAllocation(ProjectAllocationEntity ProjectDtl)
        {

            string strResponse = string.Empty;

            short UsTCurrentVersion = 0;
            try
            {

                var projectAllocationComponent = new ProjectAssignComponent();
                var currentRecordDetails = projectAllocationComponent.GetProjectAllocationDetailByID(ProjectDtl.UProj_UserProjectID);
                int recID = currentRecordDetails.UProj_UserProjectID;
                UsTCurrentVersion = Convert.ToInt16(currentRecordDetails.UProj_Version);
                //bool _currentStatus = false;

                if (ModelState.IsValid)
                {
                    UserSessionInfo _objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
                    int _userID = 0;
                    if (_objSessioninfo.UserId > 0)
                        _userID = _objSessioninfo.UserId;
                    ProjectDtl.UProj_ModifiedBy = _userID;
                    //while udating increment version by1
                    ProjectDtl.UProj_Version = ++UsTCurrentVersion;
                    //ProjectDtl.Ufp_ActiveStatus = _currentStatus;

                    int r = projectAllocationComponent.UpdateProjectAllocationDetail(ProjectDtl);

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
                        strResponse = "Error occured in UpdateProjectAllocation";
                    }
                }



            }

            catch (Exception ex)
            {
                return strResponse;
            }
            return strResponse;
        }

        public string DeleteProjectAllocation(int ID)
        {
            string strResponse = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    var Org = new ProjectAssignComponent();
                    int r = Org.DeleteProjectAllocationDetail(ID);

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
                        strResponse = "Error occured in DeleteProjectAllocation";
                    }
                }
            }
            catch (Exception ex)
            {
                return strResponse;
            }
            return strResponse;
        }


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

        //public string CreateProject([Bind(Exclude = "Ufp_ProjectID")] ProjectEntity ProjectDtl)
        //{
        //    string strResponse = string.Empty;
        //    try
        //    {
        //        var ProjectComponent = new ProjectComponent();

        //        if (ModelState.IsValid)
        //        {
        //            UserSessionInfo _objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
        //            int _userID = _objSessioninfo.UserId;
        //            ProjectDtl.Ufp_CreatedBy = _userID;

        //            var Org = new ProjectComponent();
        //            int r = Org.AddProject(ProjectDtl);
        //            if (r > 0)
        //            {
        //                strResponse = "Record created successfully";
        //            }
        //            else if (r == 0)
        //            {
        //                strResponse = "Record already exists";
        //            }
        //            else if (r < 0)
        //            {
        //                strResponse = "Error occured in CreateProject";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return strResponse;
        //    }
        //    return strResponse;
        //}

        //public string UpdateProject(ProjectEntity ProjectDtl)
        //{
        //    string strResponse = string.Empty;
        //    short UsTCurrentVersion = 0;
        //    try
        //    {
        //        var ProjectComponent = new ProjectComponent();
        //        var currentProjectDetails = ProjectComponent.GetProjectDetailByID(ProjectDtl.Ufp_ProjectID);
        //        int ProjectID = currentProjectDetails.Ufp_ProjectID;
        //        UsTCurrentVersion = Convert.ToInt16(currentProjectDetails.Ufp_Version);
        //        bool _currentStatus = false;

        //        //check for version and active status
        //        if (ModelState["Ufp_ActiveStatus"].Value != null)
        //        {
        //            _currentStatus = ProjectDtl.Ufp_ActiveStatus == true;
        //        }

        //        if (ModelState.IsValid)
        //        {
        //            UserSessionInfo _objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
        //            int _userID = _objSessioninfo.UserId;
        //            ProjectDtl.Ufp_ModifiedBy = _userID;
        //            //while udating increment version by1
        //            ProjectDtl.Ufp_Version = ++UsTCurrentVersion;
        //            ProjectDtl.Ufp_ActiveStatus = _currentStatus;

        //            var Org = new ProjectComponent();
        //            int r = Org.UpdateProjectDetail(ProjectDtl);

        //            if (r > 0)
        //            {
        //                strResponse = "Record updated successfully";
        //            }
        //            else if (r == 0)
        //            {
        //                strResponse = "Record does not exists";
        //            }
        //            else if (r < 0)
        //            {
        //                strResponse = "Error occured in UpdateProject";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return strResponse;
        //    }
        //    return strResponse;
        //}

        public JsonResult GetUserRoles()
        {
            List<UserEntity> UserRoles = null;
            try
            {
                var objDtl = new ProjectAssignComponent();
                UserRoles = objDtl.GetUserRolenames();

            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(UserRoles, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProjectAllocationCollection(string getGridBasedONRoles, string[] GettingCheckData, int Projectid)
        {
            List<ProjectAllocationEntity> ProjectDetails = null;
            try
            {
                var objDtl = new ProjectAssignComponent();
                ProjectDetails = objDtl.GetProjectAllocationDetail(getGridBasedONRoles, GettingCheckData, Projectid);
            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(ProjectDetails, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetUserNames()
        {
            List<ProjectAllocationEntity> UserNames = null;
            try
            {
                var objDtl = new ProjectComponent();
                UserNames = objDtl.GetUserNames();

            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(UserNames, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProjectByID(int catID)
        {
            ProjectAllocationEntity ProjectDetails = null;
            try
            {
                var objDtl = new ProjectAssignComponent();
                ProjectDetails = objDtl.GetProjectAllocationDetailByID(catID);
            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(ProjectDetails, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public string CrudOperation(FormCollection formCollection)
        //{
        //    string strResponse = string.Empty;
        //    try
        //    {
        //        var strOperation = formCollection["oper"];

        //        if (strOperation == "add")
        //        {
        //            ProjectAllocationEntity updateList = new ProjectAllocationEntity
        //            {
        //                Ufp_ProjectID = Convert.ToInt32(formCollection.Get("Ufp_ProjectID")),
        //                Ufp_UserID = Convert.ToInt32(formCollection.Get("Ufp_UserID")),
        //                Ufp_ParticipationPercentage = Convert.ToInt32(formCollection.Get("Ufp_ParticipationPercentage")),
        //                Ufp_StartDate = Convert.ToDateTime(formCollection.Get("Ufp_StartDate")),
        //                Ufp_EndDate = Convert.ToDateTime(formCollection.Get("Ufp_EndDate")),
        //                Ufp_Version = 1,
        //                Ufp_ActiveStatus = Convert.ToBoolean(formCollection.Get("Ufp_ActiveStatus")),
        //                Ufp_isDeleted = false
        //            };
        //            strResponse = CreateProjectAllocation(updateList);
        //        }
        //        else if (strOperation == "edit")
        //        {
        //            ProjectAllocationEntity updateList = new ProjectAllocationEntity
        //            {
        //                Ufp_UsersForProjectsID = Convert.ToInt32(formCollection.Get("id")),
        //                Ufp_ProjectID = Convert.ToInt32(formCollection.Get("Ufp_ProjectID")),
        //                Ufp_UserID = Convert.ToInt32(formCollection.Get("Ufp_UserID")),
        //                Ufp_ParticipationPercentage = Convert.ToInt32(formCollection.Get("Ufp_ParticipationPercentage")),
        //                Ufp_StartDate = Convert.ToDateTime(formCollection.Get("Ufp_StartDate")),
        //                Ufp_EndDate = Convert.ToDateTime(formCollection.Get("Ufp_EndDate")),
        //                Ufp_ActiveStatus = Convert.ToBoolean(formCollection.Get("Ufp_ActiveStatus")),
        //                Ufp_isDeleted = Convert.ToBoolean(formCollection.Get("Ufp_isDeleted"))
        //            };
        //            strResponse = UpdateProjectAllocation(updateList);
        //        }
        //        else if (strOperation == "del")
        //        {
        //            int ProjectID = Convert.ToInt32(formCollection.Get("id"));
        //            strResponse = DeleteProjectAllocation(ProjectID);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return strResponse;
        //    }
        //    return strResponse;
        //}

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
    }
}