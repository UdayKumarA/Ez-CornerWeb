using Evolutyz.Business;
using Evolutyz.Entities;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace EvolutyzCorner.UI.Web.Controllers.RoleManagement
{
    [Authorize]
    [EvolutyzCorner.UI.Web.MvcApplication.SessionExpire]
    [EvolutyzCorner.UI.Web.MvcApplication.NoDirectAccess]
    public class ManageRoleController : Controller
    {
        //UserSessionInfo objSessioninfo = new UserSessionInfo();

        public ActionResult Index(bool? pdf)
        {
            var RoleComponent = new RoleComponent();

            HomeController hm = new HomeController();
            var obj = hm.GetAdminMenu();

            var mk = "read";
            foreach (var item in obj)
            {
                //if (item.ModuleAccessType == "Read")
                //{
                //    var mk = item.ModuleAccessType;


                //    ViewBag.a = mk;

                //}
                if (item.ModuleName == "Add Role")
                {
                    mk = item.ModuleAccessType;


                    ViewBag.a = mk;

                }


            }
            if (!pdf.HasValue)
            {
                #region to return RoleList

                //objSessioninfo.UserId = 501;
                //Session["UserSessionInfo"] = objSessioninfo;
                UserSessionInfo info = new UserSessionInfo();
                var rolename = info.RoleName;
                int userid = info.UserId;
                int accountId = info.AccountId;
                string RoleId = info.RoleName;
                var orgcomponent = new OrganizationAccountComponent();
                List<LookupStatusDetail> objStatusList = new List<LookupStatusDetail>();
                objStatusList.Add(new LookupStatusDetail { StatusID = 1, Status = "Active" });
                objStatusList.Add(new LookupStatusDetail { StatusID = 0, Status = "InActive" });

                var objStList = from cl in objStatusList
                                orderby cl.StatusID
                                select new
                                {
                                    value = cl.StatusID,
                                    text = cl.Status
                                };
                ViewBag.Status = objStList;

                var modules = RoleComponent.Getallmodules();
                ViewBag.modules = modules;
                var access = RoleComponent.Getallaccess();
                ViewBag.access = access;
                var objDtl = new UserComponent();
                LeaveSchemeComponent compobj = new LeaveSchemeComponent();
                var AccountNames = compobj.GetallAccountnames(accountId, RoleId).Select(a => new SelectListItem()
                {
                    Value = a.Acc_AccountID.ToString(),
                    Text = a.Acc_AccountName,
                });
               
                ViewBag.accountnames = AccountNames;
                var rolenames = orgcomponent.Getrolenames(rolename).Select(a => new SelectListItem()
                {
                    Value = a.GenericRoleID.ToString(),
                    Text = a.Title,

                });
                ViewBag.rolenames = rolenames;
                return View();
                #endregion
            }
            else
            {
                UserSessionInfo info = new UserSessionInfo();
                var rolename = info.RoleName;

                int accountid = info.AccountId;
                string filename = "Role.pdf";
                string filePath = Server.MapPath("~/Content/PDFs/" + filename);

                var objDtl = new RoleComponent();
                IList<RoleEntity> RoleList = objDtl.GetRoleDetail(accountid,rolename);

                //ExportPDF(RoleList, new string[] { "Rol_RoleID", "AccountName", "Rol_RoleCode", "Rol_RoleName", "Rol_RoleDescription" }, filePath);

                return File(filePath, "application/pdf");
            }
        }

        

        public string CreateRole(List<RoleEntity> moduleaccess, RoleEntity RoleDtl)
        {
            var rolecomponent = new RoleComponent();
            string response = rolecomponent.AddRole(moduleaccess, RoleDtl);
            return response;
        }

        public string UpdateRole(int id, List<RoleEntity> moduleaccess, RoleEntity RoleDtl)
        {
            var rolecomponent = new RoleComponent();
            var Response = rolecomponent.DeleteRolemodules(id);
            string response = string.Empty;
            if (Response == true)
            {
                response = rolecomponent.UpdateRole(id, moduleaccess, RoleDtl);
                response = "Successfully updated";
            }
            else
            {
                response = "Not  updated";
            }
            return response;
        }

        public string DeleteRole(int RoleID)
        {
            string strResponse = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    var Org = new RoleComponent();
                    int r = Org.DeleteRoleDetail(RoleID);

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
                        strResponse = "Error occured in DeleteRole";
                    }
                }
            }
            catch (Exception ex)
            {
                return strResponse;
            }
            return strResponse;
        }

        public JsonResult GetRoleCollection()
        {


            UserSessionInfo info = new UserSessionInfo();
            var rolename = info.RoleName;

            int accountid = info.AccountId;
            List<RoleEntity> RoleDetails = null;
            try
            {
                var objDtl = new RoleComponent();
                RoleDetails = objDtl.GetRoleDetail(accountid,rolename);
                ViewBag.RoleDetails = RoleDetails[0].Rol_Version;
                ViewBag.sessionname = info.RoleName;
            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(RoleDetails, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRoleByID(int catID)
        {
            RoleEntity RoleDetails = null;
            try
            {
                var objDtl = new RoleComponent();
                RoleDetails = objDtl.GetRoleDetailByID(catID);
            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(RoleDetails, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Getmodulesselected(int catID)
        {
            List<RoleEntity> RoleDetails = null;
            try
            {
                var objDtl = new RoleComponent();
                RoleDetails = objDtl.Getmodulesselected(catID);
            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(RoleDetails, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRoleHistoryByID(int ID)
        {
            List<History_RoleEntity> HisRoleDetails = null;
            try
            {
                var objDtl = new RoleComponent();
                HisRoleDetails = objDtl.GetHistoryRoleDetailsByID(ID);
                ViewBag.RoleDetails = HisRoleDetails;
            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(HisRoleDetails, JsonRequestBehavior.AllowGet);
        }


        #region Export to PDF

        //private static void ExportPDF<TSource>(IList<TSource> RoleList, string[] columns, string filePath)
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

        //    foreach (var item in RoleList)
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

        public JsonResult CheckModule(int modid)
        {
            var objDtl = new RoleComponent();
            var modules = objDtl.Getsubmodules(modid);
            return Json(modules, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetallsubModule()
        {
            var objDtl = new RoleComponent();
            var modules = objDtl.GetallsubModule();
            return Json(modules, JsonRequestBehavior.AllowGet);
        }

        public string ChangeStatus(string id, string status)
        {
            string strResponse = string.Empty;
            var objDtl = new RoleComponent();
            strResponse = objDtl.ChangeStatus(id, status);

            return strResponse;
        }

    }
}