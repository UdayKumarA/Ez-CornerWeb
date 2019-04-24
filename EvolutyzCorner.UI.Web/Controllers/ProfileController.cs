using evolCorner.Models;
using Evolutyz.Business;
using Evolutyz.Data;
using Evolutyz.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EvolutyzCorner.UI.Web.Controllers
{

    [Authorize]
    [EvolutyzCorner.UI.Web.MvcApplication.SessionExpire]
    [EvolutyzCorner.UI.Web.MvcApplication.NoDirectAccess]
    public class ProfileController : Controller
    {
        EvolutyzCornerDataEntities entities = new EvolutyzCornerDataEntities();
        // GET: Profile
        public ActionResult Index()
        {
            UserSessionInfo _objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
            int UserId = _objSessioninfo.UserId;
            ViewBag.UserId = UserId;
            int accountid = _objSessioninfo.AccountId;

            HomeController hm = new HomeController();
            var obj = hm.GetAdminMenu();
            ProfileComponent compobj = new ProfileComponent();
            var SocialMediaIcons = compobj.GetAllSocialMediaIcons(accountid);
            var UserSocialMediaurls = compobj.GetUserSocialMediaurls(UserId);
            ViewBag.SocialMediaIcons = SocialMediaIcons;
            ViewBag.UserSocialMediaurls = UserSocialMediaurls;
           
            return View();

        }

        public JsonResult GetUserSocialMediaurls()
        {
            UserSessionInfo _objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
            int UserId = _objSessioninfo.UserId;
            ProfileComponent compobj = new ProfileComponent();
            var UserSocialMediaurls = compobj.GetUserSocialMediaurls(UserId);
            return Json(UserSocialMediaurls,JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        public string CreateProfile([Bind(Exclude = "UsrP_UserID")] UserProfileEntity ProfileDtl)
        {
            string strResponse = string.Empty;
            try
            {
                var ProfileComponent = new ProfileComponent();

                if (ModelState.IsValid)
                {
                    UserSessionInfo _objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
                    int _userID = _objSessioninfo.UserId;
                    ProfileDtl.UsrP_CreatedBy = _userID;

                    var Org = new ProfileComponent();
                    int r = Org.AddProfile(ProfileDtl);
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
                        strResponse = "Error occured in CreateRole";
                    }
                }
            }
            catch (Exception ex)
            {
                return strResponse;
            }
            return strResponse;
        }

        [HttpPost]
        public string UpdateProfile(UserProfileEntity user)
        {
            UsersProfile userData = null;
            //string ImageName = System.IO.Path.GetFileName(file.FileName);
            UserSessionInfo objSessioninfo = new UserSessionInfo();

            string strResponse = string.Empty;
            short UsTCurrentVersion = 0;
            string imagename = string.Empty;
            if (Request.Files.Count > 0)
            {

                var file = Request.Files[0];
                var fileName = "/uploadimages/Images/" + file.FileName;
                imagename = file.FileName;
                var imagepath = Server.MapPath(fileName);
                file.SaveAs(imagepath);
                user.Usrp_ProfilePicture = imagename;
                try
                {
                    var ProjectComponent = new ProfileComponent();
                    var currentProjectDetails = ProjectComponent.GetProfileDetailid(user.UsrP_UserID);
                    int ProjectID = currentProjectDetails.UsrP_UserProfileID;
                    UsTCurrentVersion = Convert.ToInt16(currentProjectDetails.UsrP_Version);
                    // bool _currentStatus = false;

                    //check for version and active status
                    //if (ModelState["UsrP_ActiveStatus"].Value == null)
                    //{
                    //    _currentStatus = user.UsrP_ActiveStatus == true;
                    //}

                    if (ModelState.IsValid)
                    {
                        UserSessionInfo _objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
                        int _userID = _objSessioninfo.UserId;
                        user.UsrP_ModifiedBy = _userID;
                        //while udating increment version by1
                        user.UsrP_Version = ++UsTCurrentVersion;
                        user.UsrP_ActiveStatus = true;
                        user.UsrP_UserProfileID = ProjectID;
                        var Org = new ProfileComponent();
                        int r = Org.UpdateProfileDetail(user);

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
                    ex.Message.ToString();
                }
            }
            else
            {
                using (var db = new EvolutyzCornerDataEntities())
                {

                    try
                    {
                        userData = db.Set<UsersProfile>().Where(s => s.UsrP_UserID == user.UsrP_UserID).FirstOrDefault<UsersProfile>();

                        if (userData != null)
                        {

                            var ProjectComponent = new ProfileComponent();
                            var currentProjectDetails = ProjectComponent.GetProfileDetailByID(user.UsrP_UserID);
                            int ProjectID = currentProjectDetails.UsrP_UserProfileID;
                            UsTCurrentVersion = Convert.ToInt16(currentProjectDetails.UsrP_Version);
                            // bool _currentStatus = false;

                            //check for version and active status
                            //if (ModelState["UsrP_ActiveStatus"].Value == null)
                            //{
                            //    _currentStatus = user.UsrP_ActiveStatus == true;
                            //}

                            if (ModelState.IsValid)
                            {
                                UserSessionInfo _objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
                                int _userID = _objSessioninfo.UserId;
                                user.UsrP_ModifiedBy = _userID;
                                //while udating increment version by1
                                user.UsrP_Version = ++UsTCurrentVersion;
                                user.UsrP_ActiveStatus = true;

                                var Org = new ProfileComponent();
                                int r = Org.UpdateProfileDetailByImage(user);

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
                        else
                        {
                            //int retVal = 0;
                            UsersProfile profilesave = new UsersProfile();
                            try
                            {
                                //Project = db.Set<Project>().Where(s => s.Proj_ProjectID == _project.Proj_ProjectID).FirstOrDefault<Project>();

                                #region Saving ProjectAllocation info Table
                                profilesave.UsrP_UserID = user.UsrP_UserID;
                                profilesave.UsrP_FirstName = user.UsrP_FirstName;
                                profilesave.UsrP_LastName = user.UsrP_LastName;
                                profilesave.Usrp_ProfilePicture = user.Usrp_ProfilePicture;
                                profilesave.Usrp_DOJ = user.Usrp_DOJ;
                                profilesave.UsrP_DOB = user.UsrP_DOB;
                                profilesave.Usrp_MobileNumber = user.Usrp_MobileNumber;
                                profilesave.UsrP_EmailID = user.UsrP_EmailID;
                                profilesave.TemporaryAddress = user.TemporaryAddress;
                                profilesave.PermanentAddress = user.PermanentAddress;
                                profilesave.UsrP_ActiveStatus = user.UsrP_ActiveStatus;
                                profilesave.UsrP_Version = user.UsrP_Version;
                                profilesave.UsrP_CreatedDate = System.DateTime.Now;
                                profilesave.UsrP_CreatedBy = user.UsrP_CreatedBy;
                                profilesave.UsrP_ModifiedDate = System.DateTime.Now;
                                profilesave.UsrP_ModifiedBy = user.UsrP_ModifiedBy;
                                profilesave.UsrP_isDeleted = user.UsrP_isDeleted;
                                profilesave.Marital_Status = user.Marital_Status;
                                profilesave.Usr_GenderId = user.Usr_GenderId;
                                profilesave.Usr_Titleid = user.Usr_Titleid;
                                profilesave.UsrP_EmployeeID = user.UsrP_EmployeeID;
                                profilesave.Usrp_MobileNumber = user.Usrp_PhoneNumber;
                                #endregion

                                //   db.UsersProfiles.up(profilesave);

                                db.Entry(profilesave).State = EntityState.Added;
                                //  db.Entry(profilesave).State = System.Data.Entity.EntityState.Modified;

                                db.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                //retVal = -1;
                            }

                        }

                    }

                    catch (Exception ex)
                    {
                        ex.Message.ToString();
                    }
                }
            }

            return strResponse;


        }
        #region Select Query to display all User Details

        public ActionResult getAllUsersDetails()
        {
            UserSessionInfo sessId = new UserSessionInfo();
            //string userName = sessId.Usr_Username;
            int userID = sessId.UserId;
            var details = (from p in entities.UserSkills
                           join c in entities.Skills
                           on p.SkillId equals c.SkillId
                           where p.Usr_UserId == userID && p.Is_Deleted==false
                         
                           select new UserProfileEntity
                           {
                               Usr_UserId = p.Usr_UserId,
                               Usk_CreatedDate = p.Usk_CreatedDate,
                               Usk_ModifiedDate = p.Usk_ModifiedDate,
                               Experience = p.Experience,
                               SkillTitle = c.SkillTitle,
                               SkillId = c.SkillId,
                               Usr_SkillId = p.Usr_SkillId,
                               //Description = c.exp
                           }).ToList();

            return Json(details, JsonRequestBehavior.AllowGet);
        }

        #endregion
        
        public JsonResult GetSocialMediaByID(int catID)
        {
            List<UserProfileEntity> ProjectDetails = null;
            try
            {
                var objDtl = new ProfileComponent();
                ProjectDetails = objDtl.GetSocialMediaByID(catID);
            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(ProjectDetails, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProfiletByID(int catID)
        {
            UserProfileEntity ProjectDetails = null;
            try
            {
                var objDtl = new ProfileComponent();
                ProjectDetails = objDtl.GetProfileDetailByID(catID);
            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(ProjectDetails, JsonRequestBehavior.AllowGet);
        }
        #region Adding skills & Experience of user

        public string AddSkills(string skill, string yearExp, string monthExp)
        {
            try
            {
                int response = 0;
                UserProjectdetailsEntity sessId = new UserProjectdetailsEntity();
                int userID = sessId.User_ID;

                if (entities.UserSkills.Count((a) => a.Usr_UserId == userID) >= 0)
                {
                    string yearMonth = yearExp + "." + monthExp;
                    EvolutyzCornerDataEntities evolutyzData = new EvolutyzCornerDataEntities();

                    UserSkill skills = new UserSkill();
                    skills.SkillId = Convert.ToInt32(skill);
                    skills.Usr_UserId = userID;
                    //skills.Usr_SkillId = skill;
                    skills.Experience = yearMonth;
                    skills.Usk_CreatedDate = System.DateTime.Now;
                   
                    // skills.Usk_ModifiedDate = System.DateTime.Now;
                    skills.Is_Deleted = false;

                    evolutyzData.UserSkills.Add(skills);

                    response = evolutyzData.SaveChanges();


                }


                if (response > 0)
                {
                    return "Data Inserted Successfully";
                }
                else
                {
                    return"Try Again!!";
                }

            }
            catch (Exception)
            {

                throw;
            }
        }


        #endregion


        #region To get particular profile details from Database
        public ActionResult GetProfileDetailforAdminLayout(UserProfileEntity user)
        {
            UserProfileEntity response = new UserProfileEntity();

            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {

                    response = (from q in db.UsersProfiles
                                    //join a in db.Accounts on q.Proj_AccountID equals a.Acc_AccountID
                                where q.UsrP_UserID == user.UsrP_UserID
                                select new UserProfileEntity
                                {
                                    UsrP_UserProfileID = q.UsrP_UserProfileID,
                                    //Proj_AccountID = q.Proj_AccountID,
                                    // AccountName = a.Acc_AccountName,
                                    //Proj_ProjectCode = q.Proj_ProjectCode,
                                    UsrP_EmailID = q.UsrP_EmailID,
                                    UsrP_FirstName = q.UsrP_FirstName,
                                    UsrP_LastName = q.UsrP_LastName,
                                    Usrp_ProfilePicture = q.Usrp_ProfilePicture,

                                }).FirstOrDefault();

                    response.IsSuccessful = true;
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    response.IsSuccessful = false;
                    response.Message = "Error Occured in GetProjectDetailByID(ID)";
                    response.Detail = ex.Message.ToString();
                    return Json("Please check Properly", JsonRequestBehavior.AllowGet);
                }
            }
        }
        #endregion

        public string DeleteSkills(int id)
        {
            string response = string.Empty;
            ProfileComponent pcomp = new ProfileComponent();
            response = pcomp.DeleteSkills(id);
            return response;
        }

        public string updateSkills(string SkillsTitle, string yearExp, string monthExp, int userskillid)
        {
            try
            {
                UserProjectdetailsEntity sessId = new UserProjectdetailsEntity();
                int userID = sessId.User_ID;

                string yearMonth = yearExp + "." + monthExp;
                EvolutyzCornerDataEntities evolutyzData = new EvolutyzCornerDataEntities();

                UserSkill skills = evolutyzData.UserSkills.SingleOrDefault(u => u.Usr_SkillId == userskillid);
                skills.SkillId = Convert.ToInt32(SkillsTitle);
                skills.Experience = yearMonth;


                int response = evolutyzData.SaveChanges();

                if (response > 0)
                {
                    return "Data Updated Successfully";
                }
                else
                {
                    return "Try Again!!";
                }

            }
            catch (Exception)
            {

                throw;
            }
        }


        #region Binding UserSkills from DataBase to Dropdown

        public ActionResult getUsers()
        {
            UserSessionInfo info = new UserSessionInfo();
            int accid = info.AccountId;
            List<SkillEntity> SkillList = new List<SkillEntity>();
            try
            {
                var objDtl = new ProfileComponent();
                SkillList = objDtl.GetSkillList(accid);
            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(SkillList, JsonRequestBehavior.AllowGet);
        }

        #endregion

        public string CheckPassword(string oldpassword, string newpassowrd)
        {
            string Response = string.Empty;
            ProfileComponent pcomp = new ProfileComponent();
            Response = pcomp.CheckPassword(oldpassword, newpassowrd);
            return Response;
        }
        
        [HttpPost]
        public string AddUserSocialMedia(List<UserProfileEntity> jsonobj)
        {
            string Response = string.Empty;
            ProfileComponent compobj = new ProfileComponent();
            Response = compobj.AddUserSocialMedia(jsonobj);
            return Response;
        }
        [HttpPost]
        public string UpdateUserSocialMedia(List<UserProfileEntity> jsonobj)
        {
            string Response = string.Empty;
            ProfileComponent compobj = new ProfileComponent();
            Response = compobj.UpdateUserSocialMedia(jsonobj);
            return Response;
        }
    }
}