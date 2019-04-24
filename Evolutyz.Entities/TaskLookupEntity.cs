using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolutyz.Entities
{
    public class TaskLookupEntity : ResponseHeader
    {
        public int? tsk_TaskID { get; set; }
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
}
