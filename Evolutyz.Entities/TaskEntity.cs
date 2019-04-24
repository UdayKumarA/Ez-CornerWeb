using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolutyz.Entities
{
    public class TaskEntity : ResponseHeader
    {
        public int tsk_TaskID { get; set; }
        public string tsk_TaskName { get; set; }
        public string tsk_TaskDescription { get; set; }
        public bool tsk_ActiveStatus { get; set; }
        public short tsk_Version { get; set; }
        public System.DateTime tsk_CreatedDate { get; set; }
        public int tsk_CreatedBy { get; set; }
        public Nullable<System.DateTime> tsk_ModifiedDate { get; set; }
        public Nullable<int> tsk_ModifiedBy { get; set; }
        public bool tsk_isDeleted { get; set; }
    }

    public class ProjectSpecifictasks
    {
        public int Proj_SpecificTaskId { get; set; }
        public Nullable<int> ProjectId { get; set; }
        public string Proj_SpecificTaskName { get; set; }
        public string RTMId { get; set; }
        public Nullable<System.DateTime> Actual_StartDate { get; set; }
        public Nullable<System.DateTime> Actual_EndDate { get; set; }
        public Nullable<System.DateTime> Plan_StartDate { get; set; }
        public Nullable<System.DateTime> Plan_EndDate { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<bool> isDeleted { get; set; }
        public Nullable<bool> StatusId { get; set; }
        public Nullable<int> tsk_TaskID { get; set; }

        public string Proj_ProjectName { get; set; }
        public int Proj_ProjectId { get; set; }

      
        public string tsk_TaskName { get; set; }

    }


    public class AccountSpecifictasks
    {
        public int Acc_SpecificTaskId { get; set; }
        public Nullable<int> AccountID { get; set; }
        public string Acc_SpecificTaskName { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<bool> isDeleted { get; set; }
        public Nullable<bool> StatusId { get; set; }
        public Nullable<int> tsk_TaskID { get; set; }

        public int Acc_AccountID { get; set; }
        public string Acc_AccountName { get; set; }


        public string tsk_TaskName { get; set; }

    }

}
