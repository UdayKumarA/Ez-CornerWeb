using Evolutyz.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Evolutyz.Data
{
   public class SkillDAC : DataAccessComponent
    {
        
        #region To get all details of HolidayCalendar from Database
        public List<SkillEntity> GetAccountSkills(int accountID)
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var query = (from h in db.Skills
                                 join a in db.Accounts on h.Acc_AccountID equals a.Acc_AccountID
                                
                                 where h.Acc_AccountID == accountID && h.Sk_isDeleted == false
                                 select new SkillEntity
                                 {
                                   SkillId= h.SkillId,
                                   SkillTitle= h.SkillTitle,
                                   ShortDescription= h.ShortDescription,
                                     StatusID = h.StatusID
                                 }).ToList();

                  
                    return query;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        #endregion

        public SkillEntity Getskillbyid(int id)
        {
            SkillEntity response = new SkillEntity();
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                     response = (from h in db.Skills
                                where h.SkillId==id
                                 select new SkillEntity
                                 {
                                     SkillId = h.SkillId,
                                     SkillTitle = h.SkillTitle,
                                     ShortDescription = h.ShortDescription,
                                     StatusID = h.StatusID
                                 }).FirstOrDefault();


                    return response;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public string UpdateSkills(int id, string skillTitle, string Description, string status)
        {
            Skill skilldetails = null;
            UserSessionInfo info = new UserSessionInfo();
            int userid = info.UserId;
            string strResponse = string.Empty;
            int statusid;
            if (status == "True")
            {
                statusid = 1;
            }
            else
            {
                statusid = 0;
            }
            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    skilldetails = db.Set<Skill>().Where(s => s.SkillId == id).FirstOrDefault<Skill>();


                    if (skilldetails == null)
                    {
                        return null;
                    }
                    //holidayDetails.AccountID = holiday.AccountID;
                    skilldetails.SkillTitle = skillTitle;
                    skilldetails.ShortDescription = Description;
                    skilldetails.StatusID = statusid;
                  
                    skilldetails.Sk_ModifiedBy = userid;
                    skilldetails.Sk_ModifiedDate = System.DateTime.Now;

                    db.Entry(skilldetails).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    strResponse = "Record successfully updated";
                }
                catch (Exception ex)
                {
                    strResponse = ex.Message.ToString();
                }
                return strResponse;
            }

        }

        public int DeleteSkill(int skillId)
        {
            int retVal = 0;
            Skill _UserDtl = null;

            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    _UserDtl = db.Set<Skill>().Where(s => s.SkillId == skillId).FirstOrDefault<Skill>();
                    if (_UserDtl == null)
                    {
                        return retVal;
                    }
                    _UserDtl.Sk_isDeleted = true;
                    db.Entry(_UserDtl).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    retVal = 1;
                }
                catch (Exception ex)
                {
                    retVal = -1;
                }
            }
            return retVal;
        }
    }
}
