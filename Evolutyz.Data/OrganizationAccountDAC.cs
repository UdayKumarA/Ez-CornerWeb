using Evolutyz.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace Evolutyz.Data
{
    public class OrganizationAccountDAC : DataAccessComponent
    {
        #region To add Organization Account Detail in Database
        public int AddOrganizationAccount(OrganizationAccountEntity orgDtl)
        {

            Account orgCheck = new Account();
            Account CodeCheck = new Account();
            int retVal = 0;
            string response = string.Empty;
            string str = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection Conn = new SqlConnection();
            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    orgCheck = db.Set<Account>().Where(s => s.Acc_AccountName == orgDtl.Acc_AccountName).FirstOrDefault<Account>();
                    CodeCheck= db.Set<Account>().Where(s => s.Acc_AccountCode == orgDtl.Acc_AccountCode).FirstOrDefault<Account>();
                    if (orgCheck != null)
                    {
                        return retVal=2;
                    }
                    if (CodeCheck != null)
                    {
                        return retVal = 3;
                    }
                    Account OrgCheck = new Account();
                    OrgCheck.Acc_AccountID = orgDtl.Acc_AccountID;
                    OrgCheck.Acc_AccountCode = orgDtl.Acc_AccountCode;
                    OrgCheck.Acc_AccountName = orgDtl.Acc_AccountName;
                    OrgCheck.Acc_AccountDescription = orgDtl.Acc_AccountDescription;
                    OrgCheck.Acc_EmailID = orgDtl.Acc_EmailID;
                    OrgCheck.Acc_MobileNumber = orgDtl.Acc_MobileNumber;
                    OrgCheck.Acc_PhoneNumber = orgDtl.Acc_PhoneNumber;
                    OrgCheck.Acc_CompanyLogo = orgDtl.Acc_CompanyLogo;
                    OrgCheck.Acc_ActiveStatus = orgDtl.Acc_ActiveStatus;
                    // orgCheck.//Acc_Version = orgDtl.Acc_Version,  
                    OrgCheck.Acc_CreatedBy = orgDtl.Acc_CreatedBy;
                    OrgCheck.Acc_CreatedDate = System.DateTime.Now;
                    OrgCheck.Acc_isDeleted = false;
                    OrgCheck.is_UsAccount = orgDtl.is_UsAccount;


                    db.Set<Account>().Add(OrgCheck);

                    db.SaveChanges();
                    int id = OrgCheck.Acc_AccountID;
                    Conn = new SqlConnection(str);

                    if (orgDtl.is_pre_requisite == true)
                    {
                        if (Conn.State != System.Data.ConnectionState.Open)
                            Conn.Open();
                        SqlCommand objCommand = new SqlCommand("[CreateDefaultRoles]", Conn);
                        objCommand.CommandType = CommandType.StoredProcedure;
                        objCommand.Parameters.AddWithValue("@AccountID", id);
                        objCommand.ExecuteNonQuery();
                   }




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

        #region To update existing Organization Account Detail in Database

        public string UpdateAccountDetail(OrganizationAccountEntity ctDtl)
        {
            Account AccountDtl = null;
            History_Account _AccountHistory = new History_Account();

            string response = string.Empty;
            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    AccountDtl = db.Set<Account>().Where(s => s.Acc_AccountID == ctDtl.Acc_AccountID).FirstOrDefault<Account>();

                    if (AccountDtl != null)
                    {
                        AccountDtl.Acc_AccountCode = ctDtl.Acc_AccountCode;
                        AccountDtl.Acc_AccountDescription = ctDtl.Acc_AccountDescription;
                        AccountDtl.Acc_AccountName = ctDtl.Acc_AccountName;
                        AccountDtl.Acc_CompanyLogo = ctDtl.Acc_CompanyLogo;
                        AccountDtl.Acc_EmailID = ctDtl.Acc_EmailID;
                        AccountDtl.Acc_MobileNumber = ctDtl.Acc_MobileNumber;
                        AccountDtl.Acc_PhoneNumber = ctDtl.Acc_PhoneNumber;
                        AccountDtl.Acc_ActiveStatus = ctDtl.Acc_ActiveStatus;
                        AccountDtl.Acc_Version = ctDtl.Acc_Version;
                        AccountDtl.Acc_ModifiedBy = ctDtl.Acc_ModifiedBy;
                        AccountDtl.Acc_ModifiedDate = System.DateTime.Now;
                        AccountDtl.Acc_isDeleted = false;
                        AccountDtl.is_UsAccount = ctDtl.is_UsAccount;
                    }

                    db.SaveChanges();
                    response = "Successfully Updated";

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return response;
            }
        }


        #endregion

        #region To delete existing Organization Account details from Database
        public int DeleteAccountDetail(int ctID)
        {
            int retVal = 0;
            Account _AccountDtl = null;

            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    _AccountDtl = db.Set<Account>().Where(s => s.Acc_AccountID == ctID).FirstOrDefault<Account>();
                    if (_AccountDtl == null)
                    {
                        return retVal;
                    }
                    _AccountDtl.Acc_isDeleted = true;

                    db.Entry(_AccountDtl).State = System.Data.Entity.EntityState.Modified;
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

        #region To get all details of Organization Account from Database
        public List<OrganizationAccountEntity> GetAccountDetail()
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var query = (from q in db.Accounts
                                 where q.Acc_isDeleted == false
                                 select new OrganizationAccountEntity
                                 {
                                     Acc_AccountID = q.Acc_AccountID,
                                     Acc_AccountCode = q.Acc_AccountCode,
                                     Acc_AccountName = q.Acc_AccountName,
                                     Acc_AccountDescription = q.Acc_AccountDescription.Substring(0,25),
                                     Acc_EmailID = q.Acc_EmailID,
                                     Acc_MobileNumber = q.Acc_MobileNumber,
                                     Acc_PhoneNumber = q.Acc_PhoneNumber,
                                     Acc_CompanyLogo = q.Acc_CompanyLogo,
                                     Acc_ActiveStatus = q.Acc_ActiveStatus,
                                     Acc_Version = q.Acc_Version,
                                     Acc_CreatedBy = q.Acc_CreatedBy,
                                     Acc_CreatedDate = q.Acc_CreatedDate,
                                     Acc_ModifiedBy = q.Acc_ModifiedBy,
                                     Acc_ModifiedDate = q.Acc_ModifiedDate,
                                     Acc_isDeleted = q.Acc_isDeleted,
                                     is_UsAccount = q.is_UsAccount
                                 }).OrderByDescending(x => x.Acc_CreatedDate).ToList();

                    return query;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        #endregion

        #region To get details of Organization Account from Database on click of First/Next/Last/Previous Buttons
        public OrganizationAccountEntity BrowseAccountRecords(int id, string navigation)
        {
            OrganizationAccountEntity record = null;

            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    if (navigation.ToUpper() == "FIRST")
                    {
                        record = (from q in db.Accounts
                                  orderby q.Acc_AccountID ascending
                                  select new OrganizationAccountEntity
                                  {
                                      Acc_AccountID = q.Acc_AccountID,
                                      Acc_AccountCode = q.Acc_AccountCode,
                                      Acc_AccountName = q.Acc_AccountName,
                                      Acc_AccountDescription = q.Acc_AccountDescription,
                                      Acc_EmailID = q.Acc_EmailID,
                                      Acc_MobileNumber = q.Acc_MobileNumber,
                                      Acc_PhoneNumber = q.Acc_PhoneNumber,
                                      Acc_CompanyLogo = q.Acc_CompanyLogo,
                                      Acc_ActiveStatus = q.Acc_ActiveStatus,
                                      Acc_Version = q.Acc_Version,
                                      Acc_CreatedBy = q.Acc_CreatedBy,
                                      Acc_CreatedDate = q.Acc_CreatedDate,
                                      Acc_ModifiedBy = q.Acc_ModifiedBy,
                                      Acc_ModifiedDate = q.Acc_ModifiedDate,
                                      Acc_isDeleted = q.Acc_isDeleted,
                                  }).FirstOrDefault();
                    }
                    else if (navigation.ToUpper() == "LAST")
                    {
                        record = (from q in db.Accounts
                                  orderby q.Acc_AccountID descending
                                  select new OrganizationAccountEntity
                                  {
                                      Acc_AccountID = q.Acc_AccountID,
                                      Acc_AccountCode = q.Acc_AccountCode,
                                      Acc_AccountName = q.Acc_AccountName,
                                      Acc_AccountDescription = q.Acc_AccountDescription,
                                      Acc_EmailID = q.Acc_EmailID,
                                      Acc_MobileNumber = q.Acc_MobileNumber,
                                      Acc_PhoneNumber = q.Acc_PhoneNumber,
                                      Acc_CompanyLogo = q.Acc_CompanyLogo,
                                      Acc_ActiveStatus = q.Acc_ActiveStatus,
                                      Acc_Version = q.Acc_Version,
                                      Acc_CreatedBy = q.Acc_CreatedBy,
                                      Acc_CreatedDate = q.Acc_CreatedDate,
                                      Acc_ModifiedBy = q.Acc_ModifiedBy,
                                      Acc_ModifiedDate = q.Acc_ModifiedDate,
                                      Acc_isDeleted = q.Acc_isDeleted,
                                  }).FirstOrDefault();
                    }
                    else if (navigation.ToUpper() == "NEXT")
                    {
                        record = (from q in db.Accounts
                                  where q.Acc_AccountID >= id
                                  orderby q.Acc_AccountID ascending
                                  select new OrganizationAccountEntity
                                  {
                                      Acc_AccountID = q.Acc_AccountID,
                                      Acc_AccountCode = q.Acc_AccountCode,
                                      Acc_AccountName = q.Acc_AccountName,
                                      Acc_AccountDescription = q.Acc_AccountDescription,
                                      Acc_EmailID = q.Acc_EmailID,
                                      Acc_MobileNumber = q.Acc_MobileNumber,
                                      Acc_PhoneNumber = q.Acc_PhoneNumber,
                                      Acc_CompanyLogo = q.Acc_CompanyLogo,
                                      Acc_ActiveStatus = q.Acc_ActiveStatus,
                                      Acc_Version = q.Acc_Version,
                                      Acc_CreatedBy = q.Acc_CreatedBy,
                                      Acc_CreatedDate = q.Acc_CreatedDate,
                                      Acc_ModifiedBy = q.Acc_ModifiedBy,
                                      Acc_ModifiedDate = q.Acc_ModifiedDate,
                                      Acc_isDeleted = q.Acc_isDeleted,
                                  }).Skip(1).FirstOrDefault();
                    }

                    else if (navigation.ToUpper() == "PREVIOUS")
                    {
                        record = (from q in db.Accounts
                                  where q.Acc_AccountID < id
                                  orderby q.Acc_AccountID ascending
                                  select new OrganizationAccountEntity
                                  {
                                      Acc_AccountID = q.Acc_AccountID,
                                      Acc_AccountCode = q.Acc_AccountCode,
                                      Acc_AccountName = q.Acc_AccountName,
                                      Acc_AccountDescription = q.Acc_AccountDescription,
                                      Acc_EmailID = q.Acc_EmailID,
                                      Acc_MobileNumber = q.Acc_MobileNumber,
                                      Acc_PhoneNumber = q.Acc_PhoneNumber,
                                      Acc_CompanyLogo = q.Acc_CompanyLogo,
                                      Acc_ActiveStatus = q.Acc_ActiveStatus,
                                      Acc_Version = q.Acc_Version,
                                      Acc_CreatedBy = q.Acc_CreatedBy,
                                      Acc_CreatedDate = q.Acc_CreatedDate,
                                      Acc_ModifiedBy = q.Acc_ModifiedBy,
                                      Acc_ModifiedDate = q.Acc_ModifiedDate,
                                      Acc_isDeleted = q.Acc_isDeleted,
                                  }).FirstOrDefault();
                    }

                    return record;
                }
                catch (Exception ex)
                {
                    return record;
                }
            }
        }
        #endregion

        #region To get particular Organization Account details from Database
        public OrganizationAccountEntity GetAccountDetailByID(int accID)
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var query = (from q in db.Accounts
                                 where q.Acc_AccountID == accID
                                 select new OrganizationAccountEntity
                                 {
                                     Acc_AccountID = q.Acc_AccountID,
                                     Acc_AccountCode = q.Acc_AccountCode,
                                     Acc_AccountName = q.Acc_AccountName,
                                     Acc_AccountDescription = q.Acc_AccountDescription,
                                     Acc_EmailID = q.Acc_EmailID,
                                     Acc_MobileNumber = q.Acc_MobileNumber,
                                     Acc_PhoneNumber = q.Acc_PhoneNumber,
                                     Acc_CompanyLogo = q.Acc_CompanyLogo,
                                     Acc_ActiveStatus = q.Acc_ActiveStatus,
                                     Acc_Version = q.Acc_Version,
                                     Acc_CreatedBy = q.Acc_CreatedBy,
                                     Acc_CreatedDate = q.Acc_CreatedDate,
                                     Acc_ModifiedBy = q.Acc_ModifiedBy,
                                     Acc_ModifiedDate = q.Acc_ModifiedDate,
                                     Acc_isDeleted = q.Acc_isDeleted,
                                     is_UsAccount = q.is_UsAccount
                                 }).FirstOrDefault();

                    return query;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        #endregion

        #region To get Organization Account details for select list
        public List<OrganizationAccountEntity> SelectAccount()
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var query = (from c in db.Accounts
                                 where c.Acc_isDeleted == false && c.Acc_ActiveStatus == true
                                 select new OrganizationAccountEntity
                                 {
                                     Acc_AccountID = c.Acc_AccountID,
                                     Acc_AccountCode = c.Acc_AccountCode,
                                     Acc_AccountName = c.Acc_AccountName,
                                     Acc_AccountDescription = c.Acc_AccountDescription,
                                     Acc_EmailID = c.Acc_EmailID,
                                     Acc_MobileNumber = c.Acc_MobileNumber,
                                     Acc_PhoneNumber = c.Acc_PhoneNumber,
                                     Acc_CompanyLogo = c.Acc_CompanyLogo,
                                     Acc_ActiveStatus = c.Acc_ActiveStatus,
                                     Acc_Version = c.Acc_Version,
                                     Acc_CreatedBy = c.Acc_CreatedBy,
                                     Acc_CreatedDate = System.DateTime.Now,
                                     is_UsAccount = c.is_UsAccount
                                 }).OrderBy(x => x.Acc_AccountName).ToList();

                    return query;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        #endregion

        #region Get Max AccountIDDetails
        public Account GetMaxAccountIDDetials()
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var orgMaxEntry = db.Accounts.OrderByDescending(x => x.Acc_AccountID).FirstOrDefault();
                    return orgMaxEntry;
                }

                catch (Exception Exception)
                {
                    return null;
                    //throw;
                }
            }
        }

        #endregion

        #region To get particular Organization History Account details from Database
        public List<HistoryOrganizationAccountEntity> GetHistoryAccountDetailsByID(int accID)
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var query = (from q in db.History_Account
                                 where q.History_Acc_AccountID == accID
                                 select new HistoryOrganizationAccountEntity
                                 {
                                     History_Account_AccountID = q.History_Account_AccountID,
                                     History_Acc_AccountCode = q.History_Acc_AccountCode,
                                     History_Acc_AccountName = q.History_Acc_AccountName,
                                     History_Acc_AccountDescription = q.History_Acc_AccountDescription,
                                     History_Acc_EmailID = q.History_Acc_EmailID,
                                     History_Acc_MobileNumber = q.History_Acc_MobileNumber,
                                     History_Acc_PhoneNumber = q.History_Acc_PhoneNumber,
                                     History_Acc_CompanyLogo = q.History_Acc_CompanyLogo,
                                     History_Acc_ActiveStatus = q.History_Acc_ActiveStatus,
                                     History_Acc_Version = q.History_Acc_Version,
                                     History_Acc_CreatedBy = q.History_Acc_CreatedBy,
                                     History_Acc_CreatedDate = q.History_Acc_CreatedDate,
                                     History_Acc_ModifiedBy = q.History_Acc_ModifiedBy,
                                     History_Acc_ModifiedDate = q.History_Acc_ModifiedDate,
                                     History_Acc_isDeleted = q.History_Acc_isDeleted,
                                 }).OrderBy(x => x.History_Acc_AccountName).ToList();

                    return query;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        #endregion

        public List<OrganizationAccountEntity> Getrolenames(string rolename)
        {
            try
            {
                using (var db = new EvolutyzCornerDataEntities())
                {

                    if (rolename == "Super Admin")
                    {
                        var query = (from UT in db.GenericRoles
                                     where UT.GenericRoleID == 1002
                                     select new OrganizationAccountEntity
                                     {
                                         GenericRoleID = UT.GenericRoleID,
                                         Title = UT.Title
                                     }).ToList();
                        return query;
                    }
                    else if (rolename == "Admin")
                    {
                        var query = (from UT in db.GenericRoles
                                     where UT.GenericRoleID != 1001
                                     select new OrganizationAccountEntity
                                     {
                                         GenericRoleID = UT.GenericRoleID,
                                         Title = UT.Title
                                     }).ToList();
                        return query;
                    }

                    else
                    {


                        var query = (from UT in db.GenericRoles
                                     where UT.GenericRoleID != 1001 && UT.GenericRoleID != 1002
                                     select new OrganizationAccountEntity
                                     {
                                         GenericRoleID = UT.GenericRoleID,
                                         Title = UT.Title
                                     }).ToList();
                        return query;
                    }

                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string ChangeStatus(string id, string status)
        {
            string strResponse = string.Empty;
            Account holidayData = null;
            bool Status = Convert.ToBoolean(status);
            int did = Convert.ToInt32(id);
            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    holidayData = db.Set<Account>().Where(s => s.Acc_AccountID == did).FirstOrDefault<Account>();
                    if (holidayData == null)
                    {
                        return null;
                    }
                    holidayData.Acc_ActiveStatus = Status;
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
