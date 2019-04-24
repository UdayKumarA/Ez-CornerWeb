using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Evolutyz.Business;
using Evolutyz.Entities;
using Evolutyz.Data;
using evolCorner.Models;

namespace EvolutyzCorner.UI.Web.Controllers.UserSkillsController
{

    [Authorize]
    [EvolutyzCorner.UI.Web.MvcApplication.SessionExpire]
    [EvolutyzCorner.UI.Web.MvcApplication.NoDirectAccess]
    public class UserSkillsController : Controller
    {
        EvolutyzCornerDataEntities entities = new EvolutyzCornerDataEntities();

        public ActionResult Index()
        {

            HomeController hm = new HomeController();
            var obj = hm.GetAdminMenu();
            //foreach (var item in obj)
            //{
            //    if (item.ModuleAccessType.ToLower() == "read/write")
            //    {
            //        var mk = item.ModuleAccessType;


            //        ViewBag.a = mk;

            //    }

            //    else
            //    {

            //        var mk = item.ModuleAccessType;


            //        ViewBag.a = mk;


            //    }

            //}
            return View();
        }


        #region Select Query to display all User Details

        public ActionResult getAllUsersDetails()
        {
            UserProjectdetailsEntity sessId = new UserProjectdetailsEntity();
            string userName = sessId.Usr_Username;

            var details = (from p in entities.UserSkills
                           join c in entities.Skills
                            on p.SkillId equals c.SkillId
                           orderby p.SkillId
                           select new
                           {
                               Usr_UserId = p.Usr_UserId,
                               Usk_CreatedDate = p.Usk_CreatedDate,
                               Usk_ModifiedDate = p.Usk_ModifiedDate,
                               Experience = p.Experience,
                               SkillId=p.SkillId,
                               SkillTitle = c.SkillTitle,
                               //Description = c.exp
                           }).ToList();
            
            return Json(details, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Binding UserSkills from DataBase to Dropdown

        public ActionResult getUsers()
        {
            var SkillData = entities.Skills.ToList();
            List<SkillEntity> SkillList = new List<SkillEntity>();
            foreach (Skill item in SkillData)
            {
                SkillList.Add(new SkillEntity
                {
                    SkillId = item.SkillId,
                    SkillTitle = item.SkillTitle.ToString()
                });
            }
            //ViewBag.Skill = SkillList;
            return Json(SkillList, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region Adding skills & Experience of user

        public ActionResult AddSkills(string skill, string yearExp, string monthExp)
        {
            try
            {
                int response=0;
                UserProjectdetailsEntity sessId = new UserProjectdetailsEntity();
                int userID = sessId.User_ID;
                string username = sessId.Usr_Username;

                if (entities.UserSkills.Count((a)=>a.Usr_UserId == userID) == 0)
                {
                    string yearMonth = yearExp + "." + monthExp;
                    EvolutyzCornerDataEntities evolutyzData = new EvolutyzCornerDataEntities();

                    UserSkill skills = new UserSkill();
                    skills.SkillId = Convert.ToInt32(skill);
                    skills.Usr_UserId = userID;
                    //skills.Usr_SkillId = skill;
                 
                    skills.Experience = yearMonth;
                    skills.Usk_CreatedDate = System.DateTime.Now;
                    skills.Usk_ModifiedDate = System.DateTime.Now;


                    evolutyzData.UserSkills.Add(skills);

                    response = evolutyzData.SaveChanges();


                }
                

                if (response > 0)
                {
                    return Json("Data Inserted Successfully", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Data already Exits!!Update Skill for Existing Data", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        #endregion


        public ActionResult updateSkills(int userID, string SkillsTitle, string yearExp, string monthExp)
        {
            try
            {
                UserProjectdetailsEntity sessId = new UserProjectdetailsEntity();
               // int userID = sessId.User_ID;
              

                    string yearMonth = yearExp + "." + monthExp;
                    EvolutyzCornerDataEntities evolutyzData = new EvolutyzCornerDataEntities();

                    UserSkill skills = evolutyzData.UserSkills.SingleOrDefault(u=>u.Usr_UserId == userID);
                    skills.SkillId = Convert.ToInt32(SkillsTitle);
                    skills.Experience = yearMonth;

                    int response = evolutyzData.SaveChanges();

                    if (response > 0)
                    {
                        return Json("Data Updated Successfully", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("Updation Failed and Please Try Again!!", JsonRequestBehavior.AllowGet);
                    }
                
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ActionResult DeleteSkills(int id)
        {
            try
            {
                UserProjectdetailsEntity sessId = new UserProjectdetailsEntity();
                int userID = sessId.User_ID;

               
                EvolutyzCornerDataEntities evolutyzData = new EvolutyzCornerDataEntities();

                UserSkill skills = evolutyzData.UserSkills.SingleOrDefault(u => u.Usr_UserId == id);
                if(userID == id)
                {
                    evolutyzData.UserSkills.Remove(skills);
                    
                    evolutyzData.SaveChanges();
                    return Json("Removed Successfully", JsonRequestBehavior.AllowGet);
                }
               
                else
                {
                    return Json("Deletion Failed and Please Try Again", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}



