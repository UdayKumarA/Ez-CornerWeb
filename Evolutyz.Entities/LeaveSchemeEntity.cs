using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolutyz.Entities
{
    public class LeaveSchemeEntity : ResponseHeader
    {
        public int LSchm_LeaveSchemeID { get; set; }
        public int LSchm_LeaveTypeID { get; set; }
        public string LeaveType { get; set; }
        public double? Noofdays { get; set; }
        public int LSchm_UserTypeID { get; set; }
        public string UserType { get; set; }
        public string LSchm_LeaveFrequency { get; set; }
        public int LSchm_AccountID { get; set; }
        public string AccountName { get; set; }
        public string LSchm_LeaveSchemeCode { get; set; }
        public string LSchm_LeaveScheme { get; set; }
        public string LSchm_LeaveSchemeDescription { get; set; }
        public double? LSchm_LeaveCount { get; set; }
        public bool LSchm_ActiveStatus { get; set; }
        public short? LSchm_Version { get; set; }
        public System.DateTime LSchm_CreatedDate { get; set; }
        public int LSchm_CreatedBy { get; set; }
        public Nullable<System.DateTime> LSchm_ModifiedDate { get; set; }
        public Nullable<int> LSchm_ModifiedBy { get; set; }
        public bool LSchm_isDeleted { get; set; }
        public int? FinancialYearId { get; set; }
        public Nullable<int> StartDate { get; set; }
        public Nullable<int> EndDate { get; set; }
        public string financialyear { get; set; }
    }

    public class History_LeaveSchemeEntity
    {
        public int History_LeaveScheme_ID { get; set; }
        public int History_LSchm_LeaveSchemeID { get; set; }
        public int History_LSchm_LeaveTypeID { get; set; }
        public string LeaveType { get; set; }
        public int History_LSchm_UserTypeID { get; set; }
        public string UserType { get; set; }
        public string History_LSchm_LeaveFrequency { get; set; }
        public int History_LSchm_AccountID { get; set; }
        public string AccountName { get; set; }
        public string History_LSchm_LeaveSchemeCode { get; set; }
        public string History_LSchm_LeaveScheme { get; set; }
        public string History_LSchm_LeaveSchemeDescription { get; set; }
        public short? History_LSchm_LeaveCount { get; set; }
        public bool History_LSchm_ActiveStatus { get; set; }
        public short? History_LSchm_Version { get; set; }
        public System.DateTime History_LSchm_CreatedDate { get; set; }
        public int History_LSchm_CreatedBy { get; set; }
        public Nullable<System.DateTime> History_LSchm_ModifiedDate { get; set; }
        public Nullable<int> History_LSchm_ModifiedBy { get; set; }
        public bool History_LSchm_isDeleted { get; set; }
    }
    public class AccountEntity
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

    }

    public class LeaveSchemeModel
    {
        public int LSchm_LeaveSchemeID { get; set; }
        public int LSchm_LeaveTypeID { get; set; }

        public int LSchm_UserTypeID { get; set; }


        public int LSchm_AccountID { get; set; }

        public double? LSchm_LeaveCount { get; set; }
        public string LSchm_ActiveStatus { get; set; }
        public string FinancialYearId { get; set; }
    }


    public partial class FinancialYearEntity
    {
        public int FinancialYearId { get; set; }
        public Nullable<int> StartDate { get; set; }
        public Nullable<int> EndDate { get; set; }
        public string financialyear { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
}
