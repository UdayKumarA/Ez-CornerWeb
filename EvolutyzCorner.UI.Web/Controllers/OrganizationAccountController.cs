using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Evolutyz.Entities;
using Evolutyz.Business;

using System.IO;
using System.Web.Configuration;

namespace EvolutyzCorner.UI.Web.Controllers
{

    [Authorize]
    [EvolutyzCorner.UI.Web.MvcApplication.SessionExpire]
    [EvolutyzCorner.UI.Web.MvcApplication.NoDirectAccess]
    public class OrganizationAccountController : Controller
    {
        public ActionResult Index()
        {

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

            HomeController hm = new HomeController();
            var obj = hm.GetAdminMenu();
            foreach (var item in obj)
            {

                if (item.ModuleName == "Add Account")
                {
                    var mk = item.ModuleAccessType;


                    ViewBag.a = mk;

                }

            }


            return View();
        }

       


        public ActionResult showAllRecords()
        {
            List<OrganizationAccountEntity> AccDetails = null;
            OrganizationAccountEntity currentAccount = new OrganizationAccountEntity();
            try
            {
                if (ViewBag.currentRecord != null)
                {
                    currentAccount = ViewBag.currentRecord;
                    ViewBag.currentRecord = null;
                }
                else
                {

                    var objDtl = new OrganizationAccountComponent();
                    AccDetails = objDtl.GetOrganizationAccounts();
                    currentAccount = AccDetails.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return View(currentAccount);
        }

        public ActionResult BrowseAccountRecords(int id, string navigation)
        {
            OrganizationAccountEntity currentRecord = null;
            try
            {
                var objDtl = new OrganizationAccountComponent();
                currentRecord = objDtl.BrowseAccountRecords(id, navigation);
                ViewBag.currentRecord = currentRecord;
            }
            catch (Exception ex)
            {
                return null;
            }

            return View("showAllRecords");
        }
        public string CreateOrganizationAccount(OrganizationAccountEntity AccDtl)
        {
            string strResponse = string.Empty;
            var orgcomponent = new OrganizationAccountComponent();
            UserSessionInfo objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
            int _userID = objSessioninfo.UserId;
            AccDtl.Acc_CreatedBy = _userID;
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
                AccDtl.Acc_CompanyLogo = "User.png";
            }
            else
            {
                AccDtl.Acc_CompanyLogo = imagename;
            }
            
            int r = orgcomponent.AddOrganizationAccount(AccDtl);
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
                strResponse = "Please Fill Mandatory Fields";
            }
            else if (r ==2)
            {
                strResponse = "AccountName already exists";
            }
            else if (r ==3)
            {
                strResponse = "AccountCode already exists";
            }
            //else if (r == 4)
            //{
            //    strResponse = "AccountCode already exists";
            //}
            return strResponse;

        }

        public string UpdateOrganizationAccount(OrganizationAccountEntity accdtl)
        {
            var orgcomponent = new OrganizationAccountComponent();
            UserSessionInfo objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
            int _userID = objSessioninfo.UserId;

            accdtl.Acc_ModifiedBy = _userID;
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
                imagename = accdtl.Acc_CompanyLogo;
            }
            accdtl.Acc_CompanyLogo = imagename;  
            string response = orgcomponent.UpdateOrganizationAccount(accdtl);
            return response;

        }


        //public string AddOrganizationAccount(OrganizationAccountEntity AccDtl)
        //{
        //    string strResponse = string.Empty;
        //        short AccCurrentVersion = 0;
        //    try
        //    {
        //            var orgComponent = new OrganizationAccountComponent();
        //            var currentAccountDetails = orgComponent.GetOrganizationAccountByID(AccDtl.Acc_AccountID);
        //            int AccID = currentAccountDetails.Acc_AccountID;
        //            AccCurrentVersion = currentAccountDetails.Acc_Version;
        //            bool AccCurrentStatus = false;
        //            //AccDtl.Acc_Version  = ++currentAccountDetails.Acc_Version;

        //            //check for version and active status
        //            if ((ValueProviderResult)ModelState["Acc_ActiveStatus"].Value != null)
        //            {
        //                ModelState.SetModelValue("Acc_ActiveStatus", (ValueProviderResult)ModelState["Acc_ActiveStatus"].Value);
        //                ModelState["Acc_ActiveStatus"].Errors.Clear();
        //                AccCurrentStatus = Request.Form["Acc_ActiveStatus"] == "1" ? true : false;
        //                //AccCurrentStatus =((ValueProviderResult)ModelState["Acc_ActiveStatus"].Value.AttemptedValue);
        //            }
        //            dynamic versionValue = (ValueProviderResult)ModelState["Acc_Version"].Value;
        //            if (versionValue != null)
        //            {
        //                ModelState.SetModelValue("Acc_Version", (ValueProviderResult)ModelState["Acc_Version"].Value);
        //                ModelState["Acc_Version"].Errors.Clear();
        //            }
        //        if (ModelState.IsValid)
        //        {
        //            try
        //            {
        //                // Upload Company Logo
        //                string fileExtension = string.Empty;
        //                string newFilename = string.Empty;
        //                if (Request.Files.Count > 0)
        //                {
        //                    var mediaFile = Request.Files[0];

        //                    if (mediaFile != null)
        //                    {
        //                        var fileName = Path.GetFileName(mediaFile.FileName);
        //                        var path = Path.Combine(Server.MapPath("~" + @"\Content\images\CompanyLogos\"), fileName);
        //                        mediaFile.SaveAs(path);
        //                        fileExtension = Path.GetExtension(path).ToLower();

        //                        //renaming files 
        //                        string newName = AccDtl.Acc_AccountID.ToString();
        //                        ViewBag.ImagePath ="~" + @"\Content\images\CompanyLogos\"+ newName + fileExtension;
        //                        if (System.IO.File.Exists(path))
        //                        {
        //                            AccDtl.Acc_CompanyLogo = newName + "." + fileExtension; ;
        //                            System.IO.File.Copy(path, Path.Combine(Server.MapPath("~" + @"\Content\images\CompanyLogos\"), newName + fileExtension), true);
        //                            System.IO.File.Delete(fileName);
        //                        }
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                return strResponse;
        //            }

        //                UserSessionInfo objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
        //            int _userID = objSessioninfo.UserId;
        //            AccDtl.Acc_ModifiedBy = _userID;
        //                //while udating increment version by1
        //                AccDtl.Acc_Version = ++AccCurrentVersion;
        //                AccDtl.Acc_ActiveStatus = AccCurrentStatus;
        //            var Org = new OrganizationAccountComponent();
        //            int r = Org.UpdateOrganizationAccount(AccDtl);
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
        //                strResponse = "Error occured in UpdateOrganizationAccount";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return strResponse;
        //    }
        //    return strResponse;
        //}

        public string DeleteOrganizationAccount(int accID)
        {
            string strResponse = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    var Org = new OrganizationAccountComponent();
                    int r = Org.DeleteOrganizationAccount(accID);
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
                        strResponse = "Error occured in DeleteOrganizationAccount";
                    }
                }
            }
            catch (Exception ex)
            {
                return strResponse;
            }
            return strResponse;
        }

        public JsonResult GetOrganizationAccountCollection()
        {
            List<OrganizationAccountEntity> AccDetails = null;
            try
            {
                var objDtl = new OrganizationAccountComponent();
                AccDetails = objDtl.GetOrganizationAccounts();
                ViewBag.OrgAccDetails = AccDetails[0].Acc_Version;
            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(AccDetails, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOrganizationAccountByID(int catID)
        {
            OrganizationAccountEntity AccDetails = null;
            try
            {
                var objDtl = new OrganizationAccountComponent();
                AccDetails = objDtl.GetOrganizationAccountByID(catID);
            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(AccDetails, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOrganizationHistoryAccountByID(int catID)
        {
            List<HistoryOrganizationAccountEntity> HisAccDetails = null;
            try
            {
                var objDtl = new OrganizationAccountComponent();
                HisAccDetails = objDtl.GetOrganizationHistoryAccountsByID(catID);
                //ViewBag.OrgAccDetails = HisAccDetails[0].History_Acc_Version;
                ViewBag.OrgAccDetails = HisAccDetails;
            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(HisAccDetails, JsonRequestBehavior.AllowGet);
        }


        public string ChangeStatus(string id, string status)
        {
            string strResponse = string.Empty;
            var objDtl = new OrganizationAccountComponent();
            strResponse = objDtl.ChangeStatus(id, status);

            return strResponse;
        }


    }
}