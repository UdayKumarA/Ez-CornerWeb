using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evolutyz.Entities;
using Evolutyz.Data;

namespace Evolutyz.Business
{
    public class TaskComponent
    {
        #region To call add method of Task from Data access layer
        public int AddTask(TaskEntity _Task)
        {
            var TaskDAC = new TaskDAC();
            return TaskDAC.AddTask(_Task);
        }
        #endregion

        #region To call update method of Task Table from Data access layer
        public int UpdateTaskDetail(TaskEntity _Task)
        {
            var TaskDAC = new TaskDAC();
            return TaskDAC.UpdateTaskDetail(_Task);
        }
        #endregion

        #region To call delete method of Task Table from Data access layer
        public int DeleteTaskDetail(int ID)
        {
            var TaskDAC = new TaskDAC();
            return TaskDAC.DeleteTaskDetail(ID);
        }
        #endregion

        #region To get all details of Task Table from Data access layer
        public List<TaskEntity> GetTaskDetail()
        {
            var TaskDAC = new TaskDAC();
            return TaskDAC.GetTaskDetail();
        }
        #endregion

        #region To get all details of Account Table from Data access layer
        public TaskEntity GetTaskDetailByID(int orgID)
        {
            var TaskDAC = new TaskDAC();
            return TaskDAC.GetTaskDetailByID(orgID);
        }
        #endregion

        #region To get ID and name of Task Table from Data access layer
        public List<TaskEntity> SelectTask()
        {
            var TaskDAC = new TaskDAC();
            return TaskDAC.SelectTask();
        }
        #endregion
        
        #region To get Max AccountID details Table from Data access layer
        public GenericTask GetMaxTaskIDDetials()
        {
            var TaskDAC = new TaskDAC();
            return TaskDAC.GetMaxTaskIDDetials();
        }
        #endregion

        #region 
        public int GetMaxTaskID()
        {
            var TaskDAC = new TaskDAC();
            return TaskDAC.GetMaxTaskIDDetials().tsk_TaskID;
        }
        #endregion

        #region To get all History details of Task Table from Data access layer
        //public List<History_TaskEntity> GetHistoryTaskDetailsByID(int ID)
        //{
        //    var TaskDAC = new TaskDAC();
        //    return TaskDAC.GetHistoryTaskDetailsByID(ID);
        //}
        #endregion


        public string ChangeStatus(string id, string status)
        {
            var LeaveTypeDAC = new TaskDAC();
            return LeaveTypeDAC.ChangeStatus(id, status);
        }

    }
}
