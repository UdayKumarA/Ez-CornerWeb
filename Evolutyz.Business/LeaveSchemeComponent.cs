using Evolutyz.Data;
using Evolutyz.Entities;
using System.Collections.Generic;
using System.Data;



namespace Evolutyz.Business
{
    public class LeaveSchemeComponent
    {
      
        #region To get all details of LeaveScheme Table from Data access layer
        public List<LeaveSchemeEntity> GetLeaveSchemeDetail()
        {
            var LeaveSchemeDAC = new LeaveSchemeDAC();
            return LeaveSchemeDAC.GetLeaveSchemeDetail();
        }
        #endregion
        
        #region employeetype dropdown  
        public List<UserEntity> GetAllEmployeementtypes()
        {
            var count = new LeaveTypeDAC();
            return count.GetAllEmployeementtypes();
        }
        public List<UserEntity> GetAllUserTypes()
        {
            var count = new LeaveTypeDAC();
            return count.GetAllUserTypes();
        }
        #endregion

        #region accountnames dropdown  
        public List<AccountEntity> GetallAccountnames(int accountid)
        {
            var count = new LeaveTypeDAC();
            return count.GetallAccountnames(accountid);
        }
        public List<AccountEntity> GetallAccountnames(int accountid,string roleid)
        {
            var count = new LeaveTypeDAC();
            return count.GetallAccountnames(accountid,roleid);
        }
        #endregion

        #region accountnames dropdown  
        public List<FinancialYearEntity> Getallfinancialyears()
        {
            var count = new LeaveTypeDAC();
            return count.Getallfinancialyears();
        }

        #endregion
        
        #region Getallleavetypes dropdown
        public List<LeaveTypeEntity> Getallleavetypes()
        {
            var count = new LeaveTypeDAC();
            return count.Getallleavetypes();
        }
        #endregion

        public string SaveLeaveScheme(List<LeaveSchemeModel> jsonobj)
        {
            var LeaveSchemeDAC = new LeaveSchemeDAC();
            return LeaveSchemeDAC.SaveLeaveScheme(jsonobj);
        }

        public List<LeaveSchemeEntity> GetLeaveTypes(string id,int yearid)
        {
            var LeaveSchemeDAC = new LeaveSchemeDAC();
            return LeaveSchemeDAC.GetLeaveTypes(id, yearid);
        }

        public string updateLeavecount(List<LeaveSchemeModel> leaveupdate)
        {
            var LeaveSchemeDAC = new LeaveSchemeDAC();
            return LeaveSchemeDAC.updateLeavecount(leaveupdate);
        }

        public string UpdateLeavescheme(string id, string userid, string accountid)
        {
            var LeaveSchemeDAC = new LeaveSchemeDAC();
            return LeaveSchemeDAC.UpdateLeavescheme(id, userid, accountid);
        }

        public List<LeaveSchemeEntity> Getleaveschemebyid(string id)
        {
            var LeaveSchemeDAC = new LeaveSchemeDAC();
            return LeaveSchemeDAC.Getleaveschemebyid(id);
        }

        public List<FinancialYearEntity> GetFinanacialYears()
        {
            LeaveSchemeDAC dacobj = new LeaveSchemeDAC();
            return dacobj.GetFinanacialYears();
        }

        public string SaveFinancialyears(string startyear, string endyear, string status)
        {
            LeaveSchemeDAC dacobj = new LeaveSchemeDAC();
            return dacobj.SaveFinancialyears(startyear, endyear, status);
        }

        public bool checkyear(int usertypeid, int yearvalue)
        {
            LeaveSchemeDAC dacobj = new LeaveSchemeDAC();
            return dacobj.checkyear(usertypeid, yearvalue);
        }

        public string ChangeStatus(string id, string status)
        {
            var LeaveTypeDAC = new LeaveSchemeDAC();
            return LeaveTypeDAC.ChangeStatus(id, status);
        }
    }
}

