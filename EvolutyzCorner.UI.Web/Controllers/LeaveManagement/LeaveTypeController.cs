using Evolutyz.Business;
using Evolutyz.Entities;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace EvolutyzCorner.UI.Web.Controllers.LeaveManagement
{
    [Authorize]
    [EvolutyzCorner.UI.Web.MvcApplication.SessionExpire]
    [EvolutyzCorner.UI.Web.MvcApplication.NoDirectAccess]
    public class LeaveTypeController : Controller
    {
        //UserSessionInfo objSessioninfo = new UserSessionInfo();

        public ActionResult Index(bool? pdf)
        {
            if (!pdf.HasValue)
            {
                #region to return LeaveTypeList

                //objSessioninfo.UserId = 501;
                //Session["UserSessionInfo"] = objSessioninfo;
                UserSessionInfo info = new UserSessionInfo();
                int accountid = info.AccountId;
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
                LeaveSchemeComponent compobj = new LeaveSchemeComponent();
                var Accountname = compobj.GetallAccountnames(accountid).Select(a => new SelectListItem()
                {
                    Value = a.Acc_AccountID.ToString(),
                    Text = a.Acc_AccountName,
                });


                ViewBag.Accountname = Accountname;


            
                HomeController hm = new HomeController();
                var obj = hm.GetAdminMenu();
                foreach (var item in obj)
                {
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


                    if (item.ModuleName == "Add Leave Type")
                    {
                        var mk = item.ModuleAccessType;


                        ViewBag.a = mk;

                    }

                }

                return View();
                #endregion
            }
            else
            {
                string filename = "LeaveType.pdf";
                string filePath = Server.MapPath("~/Content/PDFs/" + filename);

                var objDtl = new LeaveTypeComponent();
                IList<LeaveTypeEntity> LeaveTypeList = objDtl.GetLeaveTypeDetail();

                ExportPDF(LeaveTypeList, new string[] { "LTyp_LeaveTypeID", "AccountName", "LTyp_LeaveType", "LTyp_LeaveTypeDescription" }, filePath);

                return File(filePath, "application/pdf");
            }
        }
        [HttpPost]
        public string CreateLeaveType(LeaveTypeEntity LeaveTypeDtl)
        {
            string strResponse = string.Empty;
            try
            {
                var LeaveTypeComponent = new LeaveTypeComponent();

                UserSessionInfo _objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
                int _userID = _objSessioninfo.UserId;
                LeaveTypeDtl.LTyp_CreatedBy = _userID;

                var Org = new LeaveTypeComponent();
                int r = Org.AddLeaveType(LeaveTypeDtl);
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
                    strResponse = "Error occured in CreateLeaveType";
                }

            }
            catch (Exception ex)
            {
                return strResponse;
            }
            return strResponse;
        }
        [HttpPost]
        public string UpdateLeaveType(LeaveTypeEntity LeaveTypeDtl)
        {
            string strResponse = string.Empty;
            short UsTCurrentVersion = 0;
            try
            {
                var LeaveTypeComponent = new LeaveTypeComponent();
                var currentLeaveTypeDetails = LeaveTypeComponent.GetLeaveTypeDetailByID(LeaveTypeDtl.LTyp_LeaveTypeID);
                int LeaveTypeID = currentLeaveTypeDetails.LTyp_LeaveTypeID;
                UsTCurrentVersion = Convert.ToInt16(currentLeaveTypeDetails.LTyp_Version);
                bool _currentStatus = false;

                //check for version and active status
                if (ModelState["LTyp_ActiveStatus"].Value != null)
                {
                    _currentStatus = LeaveTypeDtl.LTyp_ActiveStatus == true;
                }

                if (ModelState.IsValid)
                {
                    UserSessionInfo _objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
                    int _userID = _objSessioninfo.UserId;
                    LeaveTypeDtl.LTyp_ModifiedBy = _userID;
                    //while udating increment version by1
                    LeaveTypeDtl.LTyp_Version = ++UsTCurrentVersion;
                    LeaveTypeDtl.LTyp_ActiveStatus = _currentStatus;

                    var Org = new LeaveTypeComponent();
                    int r = Org.UpdateLeaveTypeDetail(LeaveTypeDtl);

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
                        strResponse = "Error occured in UpdateLeaveType";
                    }
                }
            }
            catch (Exception ex)
            {
                return strResponse;
            }
            return strResponse;
        }

        public string DeleteLeaveType(int LeaveTypeID)
        {
            string strResponse = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    var Org = new LeaveTypeComponent();
                    int r = Org.DeleteLeaveTypeDetail(LeaveTypeID);

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
                        strResponse = "Error occured in DeleteLeaveType";
                    }
                }
            }
            catch (Exception ex)
            {
                return strResponse;
            }
            return strResponse;
        }

        public JsonResult GetLeaveTypeCollection()
        {
            List<LeaveTypeEntity> LeaveTypeDetails = null;
            try
            {
                var objDtl = new LeaveTypeComponent();
                LeaveTypeDetails = objDtl.GetLeaveTypeDetail();
                ViewBag.LeaveTypeDetails = LeaveTypeDetails[0].LTyp_Version;
            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(LeaveTypeDetails, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLeaveTypeByID(int catID)
        {
            LeaveTypeEntity LeaveTypeDetails = null;
            try
            {
                var objDtl = new LeaveTypeComponent();
                LeaveTypeDetails = objDtl.GetLeaveTypeDetailByID(catID);
            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(LeaveTypeDetails, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLeaveTypeHistoryByID(int ID)
        {
            List<History_LeaveTypeEntity> HisLeaveTypeDetails = null;
            try
            {
                var objDtl = new LeaveTypeComponent();
                HisLeaveTypeDetails = objDtl.GetHistoryLeaveTypeDetailsByID(ID);
                ViewBag.LeaveTypeDetails = HisLeaveTypeDetails;
            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(HisLeaveTypeDetails, JsonRequestBehavior.AllowGet);
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
                    LeaveTypeEntity updateList = new LeaveTypeEntity
                    {
                        LTyp_AccountID = Convert.ToInt32(formCollection.Get("LTyp_AccountID")),
                        LTyp_LeaveType = formCollection.Get("LTyp_LeaveType"),
                        LTyp_LeaveTypeDescription = formCollection.Get("LTyp_LeaveTypeDescription"),
                        LTyp_Version = 1,
                        LTyp_ActiveStatus = Convert.ToBoolean(formCollection.Get("LTyp_ActiveStatus")),
                        LTyp_isDeleted = false
                    };
                    strResponse = CreateLeaveType(updateList);
                }
                else if (strOperation == "edit")
                {
                    LeaveTypeEntity updateList = new LeaveTypeEntity
                    {
                        LTyp_LeaveTypeID = Convert.ToInt32(formCollection.Get("id")),
                        LTyp_AccountID = Convert.ToInt32(formCollection.Get("LTyp_AccountID")),
                        LTyp_LeaveType = formCollection.Get("LTyp_LeaveType"),
                        LTyp_LeaveTypeDescription = formCollection.Get("LTyp_LeaveTypeDescription"),
                        LTyp_ActiveStatus = Convert.ToBoolean(formCollection.Get("LTyp_ActiveStatus")),
                        LTyp_isDeleted = Convert.ToBoolean(formCollection.Get("LTyp_isDeleted"))
                    };
                    strResponse = UpdateLeaveType(updateList);
                }
                else if (strOperation == "del")
                {
                    int LeaveTypeID = Convert.ToInt32(formCollection.Get("id"));
                    strResponse = DeleteLeaveType(LeaveTypeID);
                }
            }
            catch (Exception ex)
            {
                return strResponse;
            }
            return strResponse;
        }

        #region Export to PDF

        private static void ExportPDF<TSource>(IList<TSource> LeaveTypeList, string[] columns, string filePath)
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

            foreach (var item in LeaveTypeList)
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


        public string ChangeStatus(string id, string status)
        {
            string strResponse = string.Empty;
            var objDtl = new LeaveTypeComponent();
            strResponse = objDtl.ChangeStatus(id, status);

            return strResponse;
        }
    }
}