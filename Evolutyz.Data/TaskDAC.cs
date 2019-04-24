using Evolutyz.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Evolutyz.Data
{
    public class TaskDAC : DataAccessComponent
    {
        #region To add Task Detail in Database
        public int AddTask(TaskEntity _task)
        {
            int retVal = 0;
            GenericTask Task = new GenericTask();

            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    Task = db.Set<GenericTask>().Where(s => s.tsk_TaskID == _task.tsk_TaskID).FirstOrDefault<GenericTask>();
                    if (Task != null)
                    {
                        return retVal;
                    }
                    db.Set<GenericTask>().Add(new GenericTask
                    {
                        tsk_TaskName = _task.tsk_TaskName,
                        tsk_TaskDescription = _task.tsk_TaskDescription,
                        tsk_ActiveStatus = _task.tsk_ActiveStatus,
                        tsk_Version = _task.tsk_Version,
                        tsk_CreatedBy = _task.tsk_CreatedBy,
                        tsk_CreatedDate = System.DateTime.Now,
                        tsk_isDeleted = false
                    });
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
        #endregion

        #region To update existing Task Detail in Database
        public int UpdateTaskDetail(TaskEntity Task)
        {
            GenericTask _taskDtl = null;
            //History_tasks _taskHistory = new History_tasks();

            int retVal = 0;

            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    _taskDtl = db.Set<GenericTask>().Where(s => s.tsk_TaskID == Task.tsk_TaskID).FirstOrDefault<GenericTask>();

                    if (_taskDtl == null)
                    {
                        return retVal;
                    }

                    #region Saving History into History_task Table

                    ////check for  _taskDtl.tsk_ModifiedDate as created date to history table
                    //DateTime dtAccCreatedDate = _taskDtl.tsk_ModifiedDate ?? DateTime.Now;

                    //db.Set<History_tasks>().Add(new History_tasks
                    //{
                    //    History_tsk_taskID = _taskDtl.tsk_taskID,
                    //    History_tsk_TaskName = _taskDtl.tsk_TaskName,
                    //    History_tsk_TaskDescription = _taskDtl.tsk_TaskDescription,
                    //    History_tsk_ActiveStatus = _taskDtl.tsk_ActiveStatus,
                    //    History_tsk_Version = _taskDtl.tsk_Version,
                    //    History_tsk_CreatedDate = dtAccCreatedDate,
                    //    History_tsk_CreatedBy = _taskDtl.tsk_CreatedBy,
                    //    History_tsk_ModifiedDate = _taskDtl.tsk_ModifiedDate,
                    //    History_tsk_ModifiedBy = _taskDtl.tsk_ModifiedBy,
                    //    History_tsk_isDeleted = _taskDtl.tsk_isDeleted
                    //});
                    #endregion

                    #region Saving Task info Table

                    _taskDtl.tsk_TaskName = Task.tsk_TaskName;
                    _taskDtl.tsk_TaskDescription = Task.tsk_TaskDescription;
                    _taskDtl.tsk_ActiveStatus = Task.tsk_ActiveStatus;
                    _taskDtl.tsk_Version = Task.tsk_Version;
                    _taskDtl.tsk_ModifiedDate = System.DateTime.Now;
                    _taskDtl.tsk_ModifiedBy = Task.tsk_ModifiedBy;
                    _taskDtl.tsk_isDeleted = Task.tsk_isDeleted;
                    #endregion
                    db.Entry(_taskDtl).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    retVal = 1;
                }
                catch (Exception ex)
                {
                    retVal = -1;
                }
                return retVal;
            }
        }
        #endregion

        #region To delete existing Task details from Database
        public int DeleteTaskDetail(int ID)
        {
            int retVal = 0;
            GenericTask _taskDtl = null;

            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    _taskDtl = db.Set<GenericTask>().Where(s => s.tsk_TaskID == ID).FirstOrDefault<GenericTask>();
                    if (_taskDtl == null)
                    {
                        return retVal;
                    }
                    _taskDtl.tsk_isDeleted = true;
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
        #endregion

        #region To get all details of Task from Database
        public List<TaskEntity> GetTaskDetail()
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var query = (from q in db.GenericTasks
                                 select new TaskEntity
                                 {
                                     tsk_TaskID = q.tsk_TaskID,
                                     tsk_TaskName = q.tsk_TaskName,
                                     tsk_TaskDescription = q.tsk_TaskDescription,
                                     tsk_ActiveStatus = q.tsk_ActiveStatus,
                                     tsk_Version = q.tsk_Version,
                                     tsk_CreatedBy = q.tsk_CreatedBy,
                                     tsk_CreatedDate = q.tsk_CreatedDate,
                                     tsk_ModifiedBy = q.tsk_ModifiedBy,
                                     tsk_ModifiedDate = q.tsk_ModifiedDate,
                                     tsk_isDeleted = q.tsk_isDeleted,
                                 }).OrderBy(x => x.tsk_TaskName).ToList();

                    return query;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        #endregion

        #region To get particular Task details from Database
        public TaskEntity GetTaskDetailByID(int ID)
        {
            TaskEntity response = new TaskEntity();

            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    response = (from q in db.GenericTasks
                                where q.tsk_TaskID == ID
                                select new TaskEntity
                                {
                                    tsk_TaskID = q.tsk_TaskID,
                                    tsk_TaskName = q.tsk_TaskName,
                                    tsk_TaskDescription = q.tsk_TaskDescription,
                                    tsk_ActiveStatus = q.tsk_ActiveStatus,
                                    tsk_Version = q.tsk_Version,
                                    tsk_CreatedBy = q.tsk_CreatedBy,
                                    tsk_CreatedDate = q.tsk_CreatedDate,
                                    tsk_ModifiedBy = q.tsk_ModifiedBy,
                                    tsk_ModifiedDate = q.tsk_ModifiedDate,
                                    tsk_isDeleted = q.tsk_isDeleted,
                                }).FirstOrDefault();

                    response.IsSuccessful = true;
                    return response;
                }
                catch (Exception ex)
                {
                    response.IsSuccessful = false;
                    response.Message = "Error Occured in GetTaskDetailByID(ID)";
                    response.Detail = ex.Message.ToString();
                    return response;
                }
            }
        }
        #endregion

        #region To get Task details for select list
        public List<TaskEntity> SelectTask()
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var query = (from q in db.GenericTasks
                                 where q.tsk_isDeleted == false && q.tsk_ActiveStatus == true
                                 select new TaskEntity
                                 {
                                     tsk_TaskID = q.tsk_TaskID,
                                     tsk_TaskName = q.tsk_TaskName,
                                     tsk_TaskDescription = q.tsk_TaskDescription,
                                     tsk_ActiveStatus = q.tsk_ActiveStatus,
                                     tsk_Version = q.tsk_Version,
                                     tsk_CreatedBy = q.tsk_CreatedBy,
                                     tsk_CreatedDate = q.tsk_CreatedDate,
                                     tsk_ModifiedBy = q.tsk_ModifiedBy,
                                     tsk_ModifiedDate = q.tsk_ModifiedDate,
                                     tsk_isDeleted = q.tsk_isDeleted,
                                 }).OrderBy(x => x.tsk_TaskName).ToList();

                    return query;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        #endregion

        #region Get Max TaskID Details
        public GenericTask GetMaxTaskIDDetials()
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var TaskMaxEntry = db.GenericTasks.OrderByDescending(x => x.tsk_TaskID).FirstOrDefault();
                    return TaskMaxEntry;
                }

                catch (Exception Exception)
                {
                    return null;
                }
            }
        }

        #endregion

        //History

        #region To get particular Task details from Database
        //public List<History_tasksEntity> GetHistoryTaskDetailsByID(int ID)
        //{
        //    using (var db = new EvolutyzCornerDataEntities())
        //    {
        //        try
        //        {
        //            var query = (from q in db.History_tasks
        //                         where q.History_tsk_taskID == ID
        //                         select new History_tasksEntity
        //                         {
        //                             History_task_ID = q.History_task_ID,
        //                             History_tsk_taskID = q.History_tsk_taskID,
        //                             History_tsk_taskName = q.History_tsk_taskName,
        //                             History_tsk_taskDescription = q.History_tsk_taskDescription,
        //                             History_tsk_ActiveStatus = q.History_tsk_ActiveStatus,
        //                             History_tsk_Version = q.History_tsk_Version,
        //                             History_tsk_CreatedBy = q.History_tsk_CreatedBy,
        //                             History_tsk_CreatedDate = q.History_tsk_CreatedDate,
        //                             History_tsk_ModifiedBy = q.History_tsk_ModifiedBy,
        //                             History_tsk_ModifiedDate = q.History_tsk_ModifiedDate,
        //                             History_tsk_isDeleted = q.History_tsk_isDeleted,
        //                         }).OrderBy(x => x.History_tsk_taskName).ToList();

        //            return query;
        //        }
        //        catch (Exception ex)
        //        {
        //            return null;
        //        }
        //    }
        //}
        #endregion

        public string ChangeStatus(string id, string status)
        {
            string strResponse = string.Empty;
            AccountSpecificTask holidayData = null;
            bool Status = Convert.ToBoolean(status);
            int did = Convert.ToInt32(id);
            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    holidayData = db.Set<AccountSpecificTask>().Where(s => s.Acc_SpecificTaskId == did).FirstOrDefault<AccountSpecificTask>();
                    if (holidayData == null)
                    {
                        return null;
                    }
                    holidayData.StatusId = Status;
                    // holidayData.isActive = false;
                    db.Entry(holidayData).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    strResponse = "Status Changed Successfully";
                }
                catch (Exception ex)
                {
                    strResponse = ex.Message.ToString();
                }
            }
            return strResponse;
        }


    }
}

