using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolutyz.Entities
{
    public enum StatusEnum { Inactive = 0, Active = 1, Delete = 2, Hold = 3, YetToAccept = 4, Accepted = 5, Rejected = 6, Pending = 7, Block = 8, Blocked = 9 };

    public partial class LookupStatusDetail
    {
        public short StatusID { get; set; }
        public string Status { get; set; }
        public StatusEnum StatusProperty { get; set; }
    }
}
