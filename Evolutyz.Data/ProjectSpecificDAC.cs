using Evolutyz.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Evolutyz.Data;
namespace Evolutyz.Data
{
    public class ProjectSpecificDAC
    {
       
        public List<AccountSpecifictasks> GetAllTasks()
        {
            UserSessionInfo info = new UserSessionInfo();
            int accountId = info.AccountId;
            string RoleId = info.RoleName;

            try
            {
                if (RoleId == "Super Admin")
                {
                    using (var db = new EvolutyzCornerDataEntities())
                    {

                        var query = (from UT in db.AccountSpecificTasks
                                     join r in db.Accounts on UT.AccountID equals r.Acc_AccountID
                                     join GT in db.GenericTasks on UT.tsk_TaskID equals GT.tsk_TaskID
                                     where UT.isDeleted == false

                                     select new AccountSpecifictasks
                                     {
                                         Acc_SpecificTaskId = UT.Acc_SpecificTaskId,
                                         AccountID = UT.AccountID,
                                         Acc_AccountName = r.Acc_AccountName,
                                         Acc_SpecificTaskName = UT.Acc_SpecificTaskName,
                                         tsk_TaskID = UT.tsk_TaskID,
                                         tsk_TaskName = GT.tsk_TaskName,
                                         StatusId = UT.StatusId,
                                         isDeleted = UT.isDeleted,

                                     }).ToList();
                        return query;
                    }
                }
                else
                {
                    using (var db = new EvolutyzCornerDataEntities())
                    {

                        var query = (from UT in db.AccountSpecificTasks
                                     join r in db.Accounts on UT.AccountID equals r.Acc_AccountID
                                     join GT in db.GenericTasks on UT.tsk_TaskID equals GT.tsk_TaskID
                                     where UT.isDeleted == false && r.Acc_AccountID == accountId

                                     select new AccountSpecifictasks
                                     {
                                         Acc_SpecificTaskId = UT.Acc_SpecificTaskId,
                                         AccountID = UT.AccountID,
                                         Acc_AccountName = r.Acc_AccountName,
                                         Acc_SpecificTaskName = UT.Acc_SpecificTaskName,
                                         tsk_TaskID = UT.tsk_TaskID,
                                         tsk_TaskName = GT.tsk_TaskName,
                                         StatusId = UT.StatusId,
                                         isDeleted = UT.isDeleted,

                                     }).ToList();
                        return query;
                    }
                }
               
                   
                
               
                
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<ProjectSpecifictasks> GetTasks()
        {
            try
            {
                using (var db = new EvolutyzCornerDataEntities())
                {
                    var query = (from c in db.GenericTasks

                                 select new ProjectSpecifictasks
                                 {
                                     tsk_TaskID = c.tsk_TaskID,
                                     tsk_TaskName =c.tsk_TaskName,

                                 }).ToList();
                    return query;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<AccountSpecifictasks> Getaccounts()
        {
            UserSessionInfo info = new UserSessionInfo();
            int accountId = info.AccountId;
            string RoleId = info.RoleName;
            try
            {
                if (RoleId == "Super Admin")
                {
                    using (var db = new EvolutyzCornerDataEntities())
                    {
                        var query = (from c in db.Accounts

                                     select new AccountSpecifictasks
                                     {
                                         Acc_AccountID = c.Acc_AccountID,
                                         Acc_AccountName = c.Acc_AccountName,

                                     }).ToList();
                        return query;
                    }
                }
                else
                {
                    using (var db = new EvolutyzCornerDataEntities())
                    {
                        var query = (from c in db.Accounts
                                     where c.Acc_AccountID== accountId && c.Acc_isDeleted== false

                                     select new AccountSpecifictasks
                                     {
                                         Acc_AccountID = c.Acc_AccountID,
                                         Acc_AccountName = c.Acc_AccountName,

                                     }).ToList();
                        return query;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }


        public string SaveTasks(string Acc_AccountID, string tsk_TaskID, string Acc_SpecificTaskName, string StatusId)
        {
            string strresponse = "";
            int accid = Convert.ToInt32(Acc_AccountID);
            if (tsk_TaskID==""|| Acc_SpecificTaskName==""|| StatusId=="")
            {
               return strresponse = "Please Fill All Mandatory Fields";
            }
            int taskid = Convert.ToInt32(tsk_TaskID);
            int sttus = Convert.ToInt32(StatusId);
            bool b = Convert.ToBoolean(sttus);
            UserSessionInfo infoobj = new UserSessionInfo();
            int userid = infoobj.UserId;
           try
            {
                using (var db = new EvolutyzCornerDataEntities())
                {
                    AccountSpecificTask orgCheck = db.Set<AccountSpecificTask>().Where(s => (s.Acc_SpecificTaskName == Acc_SpecificTaskName && s.AccountID == accid)).FirstOrDefault<AccountSpecificTask>();
                    if (orgCheck !=null)
                    {
                        return strresponse = "Already Task Existed";
                    }
                    db.Set<AccountSpecificTask>().Add(new AccountSpecificTask
                    {
                      AccountID= accid,
                      Acc_SpecificTaskName = Acc_SpecificTaskName,
                      tsk_TaskID = taskid,
                      StatusId = b,
                      isDeleted= false,
                      CreatedBy = userid,
                      CreatedDate= DateTime.Now

                    });
                    db.SaveChanges();
                    strresponse = "Successfully Added";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strresponse;
        }


        public AccountSpecifictasks GetTaskDetailByID(string id)
        {
            int pro_spe_id = Convert.ToInt32(id);
            try
            {
                using (var db = new EvolutyzCornerDataEntities())
                {
                    var query = (from pt in db.AccountSpecificTasks
                                 join gt in db.GenericTasks on pt.tsk_TaskID equals gt.tsk_TaskID
                                 join p in db.Accounts on pt.AccountID equals p.Acc_AccountID
                                 where pt.Acc_SpecificTaskId== pro_spe_id
                                 select new AccountSpecifictasks
                                 {
                                     Acc_SpecificTaskId = pt.Acc_SpecificTaskId,
                                     AccountID = pt.AccountID,
                                     Acc_AccountName = p.Acc_AccountName,
                                     Acc_SpecificTaskName = pt.Acc_SpecificTaskName,
                                     tsk_TaskID = pt.tsk_TaskID,
                                     tsk_TaskName = gt.tsk_TaskName,
                                     StatusId = pt.StatusId,
                                     isDeleted = pt.isDeleted,

                                 }).FirstOrDefault();
                    return query;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string UpdateTasks(int id ,string ProjectId, string tsk_TaskID, string Proj_SpecificTaskName, string StatusId)
        {
            string strresponse = "";

            try
            {
                int projectid = Convert.ToInt32(ProjectId);
                int taskid = Convert.ToInt32(tsk_TaskID);
                int sttus = Convert.ToInt32(StatusId);
                bool b = Convert.ToBoolean(sttus);
                UserSessionInfo infoobj = new UserSessionInfo();
                int userid = infoobj.UserId;

                using (var db = new EvolutyzCornerDataEntities())
                {
                    AccountSpecificTask taskdetails = db.Set<AccountSpecificTask>().Where(s => s.Acc_SpecificTaskId == id).FirstOrDefault<AccountSpecificTask>();

                    if (taskdetails == null)
                    {
                        return null;
                    }

                   
                    taskdetails.AccountID = projectid;
                    taskdetails.tsk_TaskID = taskid;
                    taskdetails.StatusId = b;
                    taskdetails.Acc_SpecificTaskName = Proj_SpecificTaskName;
                    
                    taskdetails.ModifiedBy = userid;
                    taskdetails.ModifiedDate = System.DateTime.Now;

                    db.Entry(taskdetails).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    strresponse = "Record successfully updated";



                    

                }


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return strresponse;
        }


        public int deletetaskbyid(int id)
        {
            int retVal = 0;
            AccountSpecificTask _taskDtl = null;
            User tasks = new User();
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    tasks = db.Set<User>().Where(s => s.Usr_TaskID == id).FirstOrDefault<User>();
                    _taskDtl = db.Set<AccountSpecificTask>().Where(s => s.Acc_SpecificTaskId == id).FirstOrDefault<AccountSpecificTask>();
                    if (tasks != null)
                    {
                        return retVal = 2;
                    }
                    if (_taskDtl == null)
                    {
                        return retVal;
                    }
                    _taskDtl.isDeleted = true;
                    db.Entry(_taskDtl).State = System.Data.Entity.EntityState.Modified;
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
