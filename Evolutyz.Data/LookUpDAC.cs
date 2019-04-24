using evolCorner.Models;
using Evolutyz.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Evolutyz.Data
{
    public class LookUpDAC
    {
        public List<TaskLookupEntity> GetLookUp()
        {
            UserSessionInfo obj = new UserSessionInfo();
            int objaccountid = obj.AccountId;
            int userid = obj.UserId;
            int roleid = obj.RoleId;


            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {

                    //var Roleid = db.Roles.Where(d => d.Rol_RoleID == roleid).FirstOrDefault().Rol_RoleName;
                    // var Roleid = db.Users.Where(d => d.Usr_UserID == userid).FirstOrDefault().Usr_RoleID;
                    //var query = (from p in db.ClientProjectsTasks
                    //             join a in db.Accounts on p.Accountid equals a.Acc_AccountID
                    //             join r in db.Roles on p.rol_roleid equals r.Rol_RoleID
                    //             join at in db.AccountSpecificTasks on p.acc_specifictaskid equals at.Acc_SpecificTaskId
                    //             where p.Accountid == objaccountid && p.rol_roleid == Roleid
                    //             select new TaskLookupEntity
                    //             {
                    //                 tsk_TaskID = p.acc_specifictaskid,
                    //                 tsk_TaskName = at.Acc_SpecificTaskName
                    //             }).ToList();
                    //// }
                    var query = (from UT in db.AccountSpecificTasks
                                 join cp in db.ClientProjectsTasks on UT.Acc_SpecificTaskId equals cp.acc_specifictaskid
                                 where cp.rol_roleid == roleid && cp.Accountid == objaccountid
                                 select new TaskLookupEntity
                                 {
                                     tsk_TaskID = cp.acc_specifictaskid,
                                     tsk_TaskName = UT.Acc_SpecificTaskName
                                 }).ToList();
                    return query;

                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public List<TaskLookupEntity> GetLookUpByEmpId(int Userid)
        {
            UserSessionInfo obj = new UserSessionInfo();
            int objaccountid = obj.AccountId;
            int roleid = obj.RoleId;
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var Roleid = db.Users.Where(d => d.Usr_UserID == Userid).FirstOrDefault().Usr_RoleID;
                    var Roleid2 = db.Roles.Where(d => d.Rol_RoleID == Roleid).FirstOrDefault().Rol_RoleName;

                    var query1 = (from UT in db.AccountSpecificTasks
                                  join cp in db.ClientProjectsTasks on UT.Acc_SpecificTaskId equals cp.acc_specifictaskid
                                  where cp.rol_roleid == Roleid2 && cp.Accountid == objaccountid
                                  select new TaskLookupEntity
                                  {
                                      tsk_TaskID = cp.acc_specifictaskid,
                                      tsk_TaskName = UT.Acc_SpecificTaskName
                                  }).ToList();
                    return query1;

                }
                catch (Exception)
                {
                    return null;
                }
            }
        }


    }
}
