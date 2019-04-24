using Evolutyz.Data;
using Evolutyz.Entities;
using System.Collections.Generic;

namespace Evolutyz.Business
{
    public class OrganizationAccountComponent
    {
        #region To call add method of Account Table from Data access layer
        public int AddOrganizationAccount(OrganizationAccountEntity orgAccount)
        {
            var organizationAccountDAC = new OrganizationAccountDAC();
            return organizationAccountDAC.AddOrganizationAccount(orgAccount);
        }
        #endregion

        #region To call update method of Account Table from Data access layer
        public string UpdateOrganizationAccount(OrganizationAccountEntity orgAccount)
        {
            var organizationAccountDAC = new OrganizationAccountDAC();
            return organizationAccountDAC.UpdateAccountDetail(orgAccount);
        }
        #endregion

        #region To call delete method of Account Table from Data access layer
        public int DeleteOrganizationAccount(int ctID)
        {
            var organizationAccountDAC = new OrganizationAccountDAC();
            return organizationAccountDAC.DeleteAccountDetail(ctID);
        }
        #endregion

        #region To get all details of Account Table from Data access layer
        public List<OrganizationAccountEntity> GetOrganizationAccounts()
        {
            var organizationAccountDAC = new OrganizationAccountDAC();
            return organizationAccountDAC.GetAccountDetail();
        }
        #endregion

        public OrganizationAccountEntity BrowseAccountRecords(int id, string navigation)
        {
            var organizationAccountDAC = new OrganizationAccountDAC();
            return organizationAccountDAC.BrowseAccountRecords(id, navigation);
        }


        #region To get all details of Account Table from Data access layer
        public OrganizationAccountEntity GetOrganizationAccountByID(int orgID)
        {
            var organizationAccountDAC = new OrganizationAccountDAC();
            return organizationAccountDAC.GetAccountDetailByID(orgID);
        }
        #endregion

        #region To get ID and name of Account Table from Data access layer
        public List<OrganizationAccountEntity> SelectOrganizationAccounts()
        {
            var organizationAccountDAC = new OrganizationAccountDAC();
            return organizationAccountDAC.SelectAccount();
        }
        #endregion
        #region To get Max AccountID details Table from Data access layer
        public Account GetMaxOrganizationAccount()
        {
            var organizationAccountDAC = new OrganizationAccountDAC();
            return organizationAccountDAC.GetMaxAccountIDDetials();
        }
        #endregion

        #region 
        public int GetMaxOrganizationAccountID()
        {
            var organizationAccountDAC = new OrganizationAccountDAC();
            return organizationAccountDAC.GetMaxAccountIDDetials().Acc_AccountID;
        }
        #endregion
        #region To get all History details of Account Table from Data access layer
        public List<HistoryOrganizationAccountEntity> GetOrganizationHistoryAccountsByID(int accid)
        {
            var organizationAccountDAC = new OrganizationAccountDAC();
            return organizationAccountDAC.GetHistoryAccountDetailsByID(accid);
        }
        #endregion


        public List<OrganizationAccountEntity> Getrolenames(string rolename)
        {
            var orgdac = new OrganizationAccountDAC();
            return orgdac.Getrolenames(rolename);
        }

        public string ChangeStatus(string id, string status)
        {
            var LeaveTypeDAC = new OrganizationAccountDAC();
            return LeaveTypeDAC.ChangeStatus(id, status);
        }


    }
}
