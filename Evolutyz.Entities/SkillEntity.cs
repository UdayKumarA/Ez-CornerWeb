using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolutyz.Entities
{
   
    public class SkillEntity: ResponseHeader
    {
        public int SkillId { get; set; }
        public string SkillTitle { get; set; }
        public string ShortDescription { get; set; }
        public Nullable<System.DateTime> Sk_CreatedDate { get; set; }
        public Nullable<int> Sk_CreatedBy { get; set; }
        public Nullable<System.DateTime> Sk_ModifiedDate { get; set; }
        public Nullable<int> Sk_ModifiedBy { get; set; }
        public Nullable<bool> Sk_isDeleted { get; set; }
        public Nullable<int> Acc_AccountID { get; set; }
        public Nullable<int> StatusID { get; set; }
    }
    
}
