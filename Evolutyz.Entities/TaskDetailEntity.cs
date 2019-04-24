using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolutyz.Entities
{
    public class TaskDetailEntity : ResponseHeader
    {
        public int TaskDetailsID { get; set; }
        public int TimesheetID { get; set; }
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public int TaskID { get; set; }
        public string TaskName { get; set; }
        public String HoursWorked { get; set; }
        public string Description { get; set; }
    }
}
