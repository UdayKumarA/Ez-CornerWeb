using Evolutyz.Business;
using Evolutyz.Entities;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace EvolutyzCorner.UI.Web.Controllers.UserManagement
{
    [Authorize]
    [EvolutyzCorner.UI.Web.MvcApplication.SessionExpire]
    [EvolutyzCorner.UI.Web.MvcApplication.NoDirectAccess]
    public class UserTypeController : Controller
    {
        //UserSessionInfo objSessioninfo = new UserSessionInfo();

        public ActionResult Index(bool? pdf)
        {
            UserSessionInfo _objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
            int AccountId = _objSessioninfo.AccountId;
            ViewBag.AccountID = AccountId;
            int UserId = _objSessioninfo.UserId;
            ViewBag.Accountid = _objSessioninfo.AccountId;
            ViewBag.UserId = UserId;
            string roleID = _objSessioninfo.RoleName;

            LeaveSchemeComponent compobj = new LeaveSchemeComponent();
            var Accountname = compobj.GetallAccountnames(AccountId, roleID).Select(a => new SelectListItem()
            {
                Value = a.Acc_AccountID.ToString(),
                Text = a.Acc_AccountName,
            });
            ViewBag.accountnames = Accountname;
            ViewBag.Roleid = _objSessioninfo.RoleName;
            HomeController hm = new HomeController();
            var obj = hm.GetAdminMenu();
            var mk = "read";

            foreach (var item in obj)
            {
                if (item.ModuleName == "Add Employeement Type")
                {
                    mk = item.ModuleAccessType;


                    ViewBag.a = mk;

                }



            }

            //}

            return View();
        }


        public string CreateUserType([Bind(Exclude = "UsT_UserTypeID")] UserTypeEntity usertypeDtl)
        {
            string strResponse = string.Empty;
            try
            {
                var usertypeComponent = new UserTypeComponent();

                UserSessionInfo _objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
                int _userID = _objSessioninfo.UserId;
                usertypeDtl.UsT_CreatedBy = _userID;

                var Org = new UserTypeComponent();
                int r = Org.AddUserType(usertypeDtl);
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
                }
                else if (r ==2)
                {
                    strResponse = "Employee Code Already Existed";
                }
                else if (r==3)
                {
                    strResponse = "Employee Type Already Existed";
                }

            }
            catch (Exception ex)
            {
                return strResponse;
            }
            return strResponse;
        }

        public string UpdateUserType(UserTypeEntity usertypeDtl)
        {
            string strResponse = string.Empty;
            short UsTCurrentVersion = 0;
            try
            {
                var usertypeComponent = new UserTypeComponent();
                var currentUserTypeDetails = usertypeComponent.GetUserTypeDetailByID(usertypeDtl.UsT_UserTypeID);
                int usertypeID = currentUserTypeDetails.UsT_UserTypeID;
                UsTCurrentVersion = Convert.ToInt16(currentUserTypeDetails.UsT_Version);
                bool UsTCurrentStatus = false;

                //check for version and active status
                if (ModelState["UsT_ActiveStatus"].Value != null)
                {
                    //UsTCurrentStatus = (usertypeDtl.UsT_ActiveStatus == true) ? true : false;
                    UsTCurrentStatus = usertypeDtl.UsT_ActiveStatus == true;
                }

                if (ModelState.IsValid)
                {
                    UserSessionInfo _objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
                    int _userID = _objSessioninfo.UserId;
                    usertypeDtl.UsT_ModifiedBy = _userID;
                    //while udating increment version by1
                    usertypeDtl.UsT_Version = ++UsTCurrentVersion;
                    usertypeDtl.UsT_ActiveStatus = UsTCurrentStatus;
                    var Org = new UserTypeComponent();
                    int r = Org.UpdateUserTypeDetail(usertypeDtl);
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
                        strResponse = "Error occured in UpdateUserType";
                    }
                }
            }
            catch (Exception ex)
            {
                return strResponse;
            }
            return strResponse;
        }

        public string DeleteUserType(int usertypeID)
        {
            string strResponse = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    var Org = new UserTypeComponent();
                    int r = Org.DeleteUserTypeDetail(usertypeID);
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
                        strResponse = "Error occured in DeleteUserType";
                    }
                }
            }
            catch (Exception ex)
            {
                return strResponse;
            }
            return strResponse;
        }

        public JsonResult GetUserTypeCollection(int acntID, string RoleId)
        {
            List<UserTypeEntity> UserTypeDetails = null;
            try
            {
                var objDtl = new UserTypeComponent();
                UserTypeDetails = objDtl.GetUserTypeDetail(acntID, RoleId);
                ViewBag.UserTypeDetails = UserTypeDetails[0].UsT_Version;
            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(UserTypeDetails, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserTypeByID(int catID)
        {
            UserTypeEntity UserTypeDetails = null;
            try
            {
                var objDtl = new UserTypeComponent();
                UserTypeDetails = objDtl.GetUserTypeDetailByID(catID);
            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(UserTypeDetails, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserTypeHistoryByID(int ID)
        {
            List<History_UserTypeEntity> HisUserTypeDetails = null;
            try
            {
                var objDtl = new UserTypeComponent();
                HisUserTypeDetails = objDtl.GetHistoryUserTypeDetailsByID(ID);
                ViewBag.UserTypeDetails = HisUserTypeDetails;
            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(HisUserTypeDetails, JsonRequestBehavior.AllowGet);
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
                    UserTypeEntity updateList = new UserTypeEntity
                    {
                        UsT_AccountID = Convert.ToInt32(formCollection.Get("UsT_AccountID")),
                        UsT_UserTypeCode = formCollection.Get("UsT_UserTypeCode"),
                        UsT_UserType = formCollection.Get("UsT_UserType"),
                        UsT_UserTypeDescription = formCollection.Get("UsT_UserTypeDescription"),
                        UsT_Version = 1,
                        UsT_ActiveStatus = Convert.ToBoolean(formCollection.Get("UsT_ActiveStatus")),
                        UsT_isDeleted = false
                    };
                    strResponse = CreateUserType(updateList);
                }
                else if (strOperation == "edit")
                {
                    UserTypeEntity updateList = new UserTypeEntity
                    {
                        UsT_UserTypeID = Convert.ToInt32(formCollection.Get("id")),
                        UsT_AccountID = Convert.ToInt32(formCollection.Get("UsT_AccountID")),
                        UsT_UserTypeCode = formCollection.Get("UsT_UserTypeCode"),
                        UsT_UserType = formCollection.Get("UsT_UserType"),
                        UsT_UserTypeDescription = formCollection.Get("UsT_UserTypeDescription"),
                        //UsT_Version = Convert.ToInt16(formCollection.Get("UsT_Version")),
                        UsT_ActiveStatus = Convert.ToBoolean(formCollection.Get("UsT_ActiveStatus")),
                        UsT_isDeleted = Convert.ToBoolean(formCollection.Get("UsT_isDeleted"))
                    };
                    strResponse = UpdateUserType(updateList);
                }
                else if (strOperation == "del")
                {
                    int usertypeID = Convert.ToInt32(formCollection.Get("id"));
                    strResponse = DeleteUserType(usertypeID);
                }
            }
            catch (Exception ex)
            {
                return strResponse;
            }
            return strResponse;
        }

        #region Export to PDF

        //private static void ExportPDF<TSource>(IList<TSource> userTypeList, string[] columns, string filePath)
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

        //    foreach (var item in userTypeList)
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

        #region Export to Excel
        public ActionResult ExportToExcel(string _entity, string _sidx, string _sord, string filters)
        {
            string where = "";
            //if (!string.IsNullOrEmpty(filters))
            //{
            //    var serializer = new JavaScriptSerializer();
            //    Filters filtersList = serializer.Deserialize<Filters>(filters);
            //    where = filtersList.FilterObjectSet(entity);
            //}
            //if (string.IsNullOrEmpty(where))
            //    where = " TRUE ";
            //Response.ClearContent();
            //Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");
            //Response.ContentType = "application/excel";
            //Response.Write(GetAllData(_entity, _sidx, _sord, where));
            //Response.End();
            return View("Index");
        }
        #endregion

        public string ChangeStatus(string id, string status)
        {
            string strResponse = string.Empty;
            var objDtl = new UserTypeComponent();
            strResponse = objDtl.ChangeStatus(id, status);

            return strResponse;
        }

    }
}