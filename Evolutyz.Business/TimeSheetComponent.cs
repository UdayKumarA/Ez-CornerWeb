using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evolutyz.Entities;
using Evolutyz.Data;

namespace Evolutyz.Business
{
    public class TimesheetComponent
    {
        #region To call add method of Timesheet from Data access layer
        public int AddTimesheet(TimesheetEntity _Timesheet)
        {
            var timesheetDAC = new TimesheetDetailsDAC();
            return timesheetDAC.AddTimesheet(_Timesheet);
        }
        #endregion

        #region To call update method of Timesheet Table from Data access layer
        public int UpdateTimesheet(TimesheetEntity _Timesheet)
        {
            var timesheetDAC = new TimesheetDetailsDAC();
            return timesheetDAC.UpdateTimesheet(_Timesheet);
        }
        #endregion

        #region To delete existing Timesheet
        public int DeleteTimesheet(int ID)
        {
            var timesheetDAC = new TimesheetDetailsDAC();
            return timesheetDAC.DeleteTimesheet(ID);
        }
        #endregion

        #region To get all details of Timesheet Table from Data access layer
        public List<TimesheetEntity> GetTimesheets()
        {
            var timesheetDAC = new TimesheetDetailsDAC();
            return timesheetDAC.GetTimesheets();
        }
        #endregion

        #region To get details of Timesheet by ID Table from Data access layer
        public TaskDetailEntity GetTaskDetailDetailByID(int ID)
        {
            var timesheetDAC = new TimesheetDetailsDAC();
            return timesheetDAC.GetTaskDetailDetailByID(ID);
        }
        #endregion

        #region SelectTaskDetail
        public List<TaskDetailEntity> SelectTaskDetail()
        {
            var timesheetDAC = new TimesheetDetailsDAC();
            return timesheetDAC.SelectTaskDetail();
        }
        #endregion
    }
}
