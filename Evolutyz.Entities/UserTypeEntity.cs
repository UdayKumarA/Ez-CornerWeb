using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolutyz.Entities
{
    public class UserTypeEntity : ResponseHeader
    {
        public int UsT_UserTypeID { get; set; }
        public int UsT_AccountID { get; set; }
        public string AccountName { get; set; }
        public string UsT_UserTypeCode { get; set; }
        public string UsT_UserType { get; set; }
        public string UsT_UserTypeDescription { get; set; }
        public bool UsT_ActiveStatus { get; set; }
        public int UsT_Version { get; set; }
        public System.DateTime UsT_CreatedDate { get; set; }
        public int UsT_CreatedBy { get; set; }
       public Nullable<System.DateTime> UsT_ModifiedDate { get; set; }
        public Nullable<int> UsT_ModifiedBy { get; set; }
        public bool UsT_isDeleted { get; set; }
    }


    public partial class History_UserTypeEntity
    {
        public int History_UserType_ID { get; set; }
        public int History_UsT_UserTypeID { get; set; }
        public int History_UsT_AccountID { get; set; }
        public string AccountName { get; set; }
        public string History_UsT_UserTypeCode { get; set; }
        public string History_UsT_UserType { get; set; }
        public string History_UsT_UserTypeDescription { get; set; }
        public bool History_UsT_ActiveStatus { get; set; }
        public int History_UsT_Version { get; set; }
        public System.DateTime History_UsT_CreatedDate { get; set; }
        public int History_UsT_CreatedBy { get; set; }
        public Nullable<System.DateTime> History_UsT_ModifiedDate { get; set; }
        public Nullable<int> History_UsT_ModifiedBy { get; set; }
        public bool History_UsT_isDeleted { get; set; }
    }
}
