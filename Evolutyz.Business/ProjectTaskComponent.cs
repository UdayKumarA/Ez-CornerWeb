using Evolutyz.Data;
using Evolutyz.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolutyz.Business
{
    public class ProjectTaskComponent
    {

        ProjectSpecificDAC dacobj = new ProjectSpecificDAC();
        public List<AccountSpecifictasks> GetAllTasks()
        {
          
            return dacobj.GetAllTasks();
        }


        public List<ProjectSpecifictasks> GetTasks()
        {
            var count = new ProjectSpecificDAC();
            return count.GetTasks();
        }
        public List<AccountSpecifictasks> Getaccounts()
        {
            var count = new ProjectSpecificDAC();
            return count.Getaccounts();
        }
        
        public string SaveTasks(string Acc_AccountID, string tsk_TaskID, string Acc_SpecificTaskName, string StatusId)
        {
            return dacobj.SaveTasks(Acc_AccountID, tsk_TaskID, Acc_SpecificTaskName, StatusId);
        }
        public string UpdateTasks(int id,string ProjectId, string tsk_TaskID, string Proj_SpecificTaskName, string StatusId)
        {
            return dacobj.UpdateTasks(id,ProjectId, tsk_TaskID, Proj_SpecificTaskName, StatusId);
        }

        public AccountSpecifictasks GetTaskDetailByID(string id)
        {
            return dacobj.GetTaskDetailByID(id);
        }

        public int deletetaskbyid(int id)
        {
            return dacobj.deletetaskbyid(id);
        }
    }
}
