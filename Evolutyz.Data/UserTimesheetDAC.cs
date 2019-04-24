using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evolutyz.Entities;

namespace Evolutyz.Data
{
    public class UserTimesheetDAC : DataAccessComponent
    {
        public TimesheetEntity Gettimesheet(int userid)
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    EvolutyzCornerDataEntities entites = new EvolutyzCornerDataEntities();

                    var query = (
                     from us in db.Users
                     join ac in db.Accounts on us.Usr_AccountID equals ac.Acc_AccountID
                     join rl in db.Roles on us.Usr_RoleID equals rl.Rol_RoleID
                     join ts in db.GenericTasks on us.Usr_TaskID equals ts.tsk_TaskID
                     join tim in db.TIMESHEETs on us.Usr_UserID equals tim.UserID
                     join tsk in db.TaskDetails on tim.TimesheetID equals tsk.TimesheetID
                     join gr in db.GenericRoles on rl.Rol_RoleName equals gr.GenericRoleID
                     where us.Usr_UserID==userid

                     select new TimesheetEntity
                     //{ ac.Acc_AccountName, rl.Rol_RoleName, ts.Tsk_TaskName, tim.TimesheetMonth, tsk.TaskDate, tsk.HoursWorked };
                     {
                         AccountName = ac.Acc_AccountName,
                         RoleName = gr.Title,
                         TaskName = ts.tsk_TaskName,
                         TimesheetMonth = Convert.ToString(tim.TimesheetMonth),  
                         TaskDate = Convert.ToDateTime(tsk.TaskDate),
                         Comments = tim.Comments,
                       //  HoursWorked = tsk.HoursWorked

                     })
                    .OrderBy(x => x.AccountName).FirstOrDefault();
                    
                    return query;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
