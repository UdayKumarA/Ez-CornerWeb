using evolCorner.Models;
using Evolutyz.Business;
using Evolutyz.Data;
using Evolutyz.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EvolutyzCorner.UI.Web.Controllers
{

    [Authorize]
    [EvolutyzCorner.UI.Web.MvcApplication.SessionExpire]
    [EvolutyzCorner.UI.Web.MvcApplication.NoDirectAccess]
    public class SkillsController : Controller
    {
        // GET: Skills
        public ActionResult Index()
        {

            HomeController hm = new HomeController();
            var obj = hm.GetAdminMenu();
            var mk = "Read";

            foreach (var item in obj)
            {
                if (item.ModuleName == "Add Skill")
                {
                    mk = item.ModuleAccessType;


                    ViewBag.a = mk;

                }



            }
            return View();
        }
        [HttpPost]
        public ActionResult AddSkills(string skillTitle, string Description ,string status)
        {
            try
            {
                SkillEntity nt = new SkillEntity();
                UserSessionInfo sessId = new UserSessionInfo();
                //UserProjectdetailsEntity sessId = new UserProjectdetailsEntity();
                int userID = sessId.UserId;
                int accid = sessId.AccountId;
                int statusid;
                if (status=="True")
                {
                    statusid = 1;
                }
                else
                {
                    statusid = 0;
                }

               EvolutyzCornerDataEntities evolutyzData = new EvolutyzCornerDataEntities();
                
                Skill skill = new Skill();
                skill.SkillTitle = skillTitle;
                skill.ShortDescription = Description;
                skill.Sk_CreatedBy = userID;
                skill.Sk_CreatedDate = System.DateTime.Now;
                //skill.Sk_ModifiedBy = userID;
                //skill.Sk_ModifiedDate = System.DateTime.Now;
                skill.StatusID = statusid;
                skill.Acc_AccountID = accid;
                skill.Sk_isDeleted = false;

                evolutyzData.Skills.Add(skill);

                int response = evolutyzData.SaveChanges();

                if (response > 0)
                {
                    return Json("Skills Added Successfully", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Try Again!!", JsonRequestBehavior.AllowGet);
                }

                
            }
            catch (Exception)
            {

                throw;
            }
        }
        SkillComponent skillcomp = new SkillComponent();
        public JsonResult GetAccountSkills()
        {
            List<SkillEntity> skills = null;
            UserSessionInfo _objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
            int accountID = _objSessioninfo.AccountId;
         
            try
            {
                skills = skillcomp.GetAccountSkills(accountID);


            }

            catch (Exception ex)
            {
                return null;
            }
            return Json(skills, JsonRequestBehavior.AllowGet);
        }

        public string UpdateSkills(int id,string skillTitle, string Description, string status)
        {
            string skills = string.Empty;
            try
            {
                skills = skillcomp.UpdateSkills( id,  skillTitle,  Description,  status);


            }

            catch (Exception ex)
            {
                return null;
            }
            return skills;

        }

        public string DeleteSkill(int skillid)
        {
            string strResponse = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                  
                    int r = skillcomp.DeleteSkill(skillid);
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
                        strResponse = "Error occured in DeleteUser";
                    }
                }
            }
            catch (Exception ex)
            {
                return strResponse;
            }
            return strResponse;
        }


        public JsonResult Getskillbyid(int id)
        {
            SkillEntity skills = null;
           
            SkillComponent skillcomp = new SkillComponent();
            try
            {
                skills = skillcomp.Getskillbyid(id);


            }

            catch (Exception ex)
            {
                return null;
            }
            return Json(skills, JsonRequestBehavior.AllowGet);
        }
    }
}