using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolutyz.Entities
{
    public class OrganizationAccountEntity : ResponseHeader
    {
        public int Acc_AccountID { get; set; }
        public string Acc_AccountCode { get; set; }
        public string Acc_AccountName { get; set; }
        public string Acc_AccountDescription { get; set; }
        public string Acc_EmailID { get; set; }
        public string Acc_MobileNumber { get; set; }
        public string Acc_PhoneNumber { get; set; }
        public string Acc_CompanyLogo { get; set; }
        public bool Acc_ActiveStatus { get; set; }
        public short Acc_Version { get; set; }
        public System.DateTime Acc_CreatedDate { get; set; }
        public int Acc_CreatedBy { get; set; }
        public Nullable<System.DateTime> Acc_ModifiedDate { get; set; }
        public Nullable<int> Acc_ModifiedBy { get; set; }
        public bool Acc_isDeleted { get; set; }
        public bool is_UsAccount { get; set; }

        public int GenericRoleID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

      
        public bool is_pre_requisite { get; set; }
    }
    

    public class HistoryOrganizationAccountEntity
    {
        public int History_Account_AccountID { get; set; }
        public int History_Acc_AccountID { get; set; }
        public string History_Acc_AccountCode { get; set; }
        public string History_Acc_AccountName { get; set; }
        public string History_Acc_AccountDescription { get; set; }
        public string History_Acc_EmailID { get; set; }
        public string History_Acc_MobileNumber { get; set; }
        public string History_Acc_PhoneNumber { get; set; }
        public string History_Acc_CompanyLogo { get; set; }
        public bool History_Acc_ActiveStatus { get; set; }
        public short History_Acc_Version { get; set; }
        public System.DateTime History_Acc_CreatedDate { get; set; }
        public int History_Acc_CreatedBy { get; set; }
        public Nullable<System.DateTime> History_Acc_ModifiedDate { get; set; }
        public Nullable<int> History_Acc_ModifiedBy { get; set; }
        public bool History_Acc_isDeleted { get; set; }
    }
}
