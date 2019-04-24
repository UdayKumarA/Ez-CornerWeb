using Evolutyz.Business;
using Evolutyz.Entities;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using Evolutyz.Data;
using Evolutyz.Entities;

namespace EvolutyzCorner.UI.Web.Controllers.LeaveManagement
{
    [Authorize]
    [EvolutyzCorner.UI.Web.MvcApplication.SessionExpire]
    [EvolutyzCorner.UI.Web.MvcApplication.NoDirectAccess]
    public class LeaveSchemeController : Controller
    {
       
        public ActionResult Index(bool? pdf)
        {
            var LeaveSchemeComponent = new LeaveSchemeComponent();
            if (!pdf.HasValue)
            {
                #region to return LeaveSchemeList

                //objSessioninfo.UserId = 501;
                //Session["UserSessionInfo"] = objSessioninfo;
                UserSessionInfo objinfo = new UserSessionInfo();
                var accountid = objinfo.AccountId;
                ViewBag.accid = accountid;
                var usertypeid = objinfo.Usr_UserTypeID;
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
                var Employeementtypes = LeaveSchemeComponent.GetAllEmployeementtypes().Select(a => new SelectListItem()
                {
                    Value = a.Usr_UserTypeID.ToString(),
                    Text = a.UserType,

                });
                ViewBag.Employeementtypes = Employeementtypes;
                var UserTypes = LeaveSchemeComponent.GetAllUserTypes().Select(a => new SelectListItem()
                {
                    Value = a.Usr_UserTypeID.ToString(),
                    Text = a.UserType,

                });
                ViewBag.UserTypes = UserTypes;

                var Accountname = LeaveSchemeComponent.GetallAccountnames(accountid).Select(a => new SelectListItem()
                {
                    Value = a.Acc_AccountID.ToString(),
                    Text = a.Acc_AccountName,
                });


                ViewBag.Accountname = Accountname;
                var FinancialYears = LeaveSchemeComponent.Getallfinancialyears().Select(a => new SelectListItem()
                {
                    Value = a.FinancialYearId.ToString(),
                    Text = a.financialyear,
                }).OrderByDescending(x=>x.Value);
                ViewBag.FinancialYears = FinancialYears;
                var leavetypes = LeaveSchemeComponent.Getallleavetypes();
                ViewBag.leavecount = leavetypes.Count();
                ViewBag.leavetypes = leavetypes;
                HomeController hm = new HomeController();
                var obj = hm.GetAdminMenu();
                foreach (var item in obj)
                {

                    if (item.ModuleName == "Add Leave Scheme")
                    {
                        var mk = item.ModuleAccessType;


                        ViewBag.a = mk;

                    }
                    //if (item.ModuleAccessType.ToLower() == "read/write")
                    //{
                    //    var mk = item.ModuleAccessType;


                    //    ViewBag.a = mk;

                    //}

                    //else
                    //{

                    //    var mk = item.ModuleAccessType;


                    //    ViewBag.a = mk;


                    //}

                }
                return View();
                #endregion
            }
            else
            {
                string filename = "LeaveScheme.pdf";
                string filePath = Server.MapPath("~/Content/PDFs/" + filename);

                var objDtl = new LeaveSchemeComponent();
                IList<LeaveSchemeEntity> LeaveSchemeList = objDtl.GetLeaveSchemeDetail();

                ExportPDF(LeaveSchemeList, new string[] { "LSchm_LeaveSchemeID", "AccountName", "LSchm_LeaveScheme", "LSchm_LeaveSchemeDescription" }, filePath);

                return File(filePath, "application/pdf");
            }
        }
        
        public JsonResult GetLeaveSchemeCollection()
        {
            List<LeaveSchemeEntity> LeaveSchemeDetails = null;
            try
            {
                var objDtl = new LeaveSchemeComponent();
                LeaveSchemeDetails = objDtl.GetLeaveSchemeDetail();
                ViewBag.LeaveSchemeDetails = LeaveSchemeDetails[0].LSchm_Version;
            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(LeaveSchemeDetails, JsonRequestBehavior.AllowGet);
        }
        
        #region Export to PDF
        private static void ExportPDF<TSource>(IList<TSource> LeaveSchemeList, string[] columns, string filePath)
        {
            iTextSharp.text.Font headerFont = FontFactory.GetFont("Verdana", 10, iTextSharp.text.Color.WHITE);
            iTextSharp.text.Font rowfont = FontFactory.GetFont("Verdana", 10, iTextSharp.text.Color.BLUE);
            Document document = new Document(PageSize.A4);

            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.OpenOrCreate));
            document.Open();
            PdfPTable table = new PdfPTable(columns.Length);
            foreach (var column in columns)
            {
                PdfPCell cell = new PdfPCell(new Phrase(column, headerFont));
                cell.BackgroundColor = iTextSharp.text.Color.BLACK;
                table.AddCell(cell);
            }

            foreach (var item in LeaveSchemeList)
            {
                foreach (var column in columns)
                {
                    string value = item.GetType().GetProperty(column).GetValue(item).ToString();
                    PdfPCell cell5 = new PdfPCell(new Phrase(value, rowfont));
                    table.AddCell(cell5);
                }
            }

            document.Add(table);
            document.Close();
        }
        #endregion

        [HttpPost]
        public string SaveLeavescheme(List<LeaveSchemeModel> jsonobj)
        {

            LeaveSchemeComponent objDtl = new LeaveSchemeComponent();
            var strresponse = objDtl.SaveLeaveScheme(jsonobj);

            return strresponse;

        }

        public JsonResult GetLeaveTypes(string id,int yearid)
        {
            // LeaveSchemeEntity leavetypes = null;
            LeaveSchemeComponent objDtl = new LeaveSchemeComponent();
            List<LeaveSchemeEntity> lstleavetype = objDtl.GetLeaveTypes(id, yearid);
            ViewBag.getleavetypes = lstleavetype;

            return Json(lstleavetype, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string updateLeavecount(List<LeaveSchemeModel> leaveupdate)
        {
            LeaveSchemeComponent objDtl = new LeaveSchemeComponent();
            var strresponse = objDtl.updateLeavecount(leaveupdate);

            return strresponse;
        }

        [HttpPost]
        public JsonResult Getleaveschemebyid(string id)
        {
            LeaveSchemeComponent objDtl = new LeaveSchemeComponent();
            List<LeaveSchemeEntity> lstleavetype = objDtl.Getleaveschemebyid(id);
            return Json(lstleavetype, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string UpdateLeavescheme(string id, string userid, string accountid)
        {
            LeaveSchemeComponent objDtl = new LeaveSchemeComponent();
            var strresponse = objDtl.UpdateLeavescheme(id, userid, accountid);

            return strresponse;
        }

        public ActionResult Financialyear()
        {
            HomeController hm = new HomeController();
            var obj = hm.GetAdminMenu();
            foreach (var item in obj)
            {

                if (item.ModuleName == "Add Leave Scheme")
                {
                    var mk = item.ModuleAccessType;


                    ViewBag.a = mk;

                }
            }

            
            return View();
        }

        public JsonResult GetFinanacialYears()
        {
            LeaveSchemeComponent newobj = new LeaveSchemeComponent();
            List<FinancialYearEntity> yearlist = newobj.GetFinanacialYears();
            return Json(yearlist, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string SaveFinancialyears(string startyear, string endyear, string status)
        {
            LeaveSchemeComponent newobj = new LeaveSchemeComponent();
            string response = newobj.SaveFinancialyears(startyear, endyear, status);

            return response;
        }

        [HttpPost]
        public bool checkyear(int usertypeid, int yearvalue)
        {

            LeaveSchemeComponent compobj = new LeaveSchemeComponent();
            var response = compobj.checkyear(usertypeid, yearvalue);
            return response;
        }

        public string ChangeStatus(string id, string status)
        {
            string strResponse = string.Empty;
            var objDtl = new LeaveSchemeComponent();
            strResponse = objDtl.ChangeStatus(id, status);
            return strResponse;
        }
        
    }
}