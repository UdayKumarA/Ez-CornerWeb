using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evolutyz.Entities;
using Evolutyz.Data;

namespace Evolutyz.Business
{
    public class LeaveTypeComponent
    {
        #region To call add method of LeaveType from Data access layer
        public int AddLeaveType(LeaveTypeEntity _LeaveType)
        {
            var LeaveTypeDAC = new LeaveTypeDAC();
            return LeaveTypeDAC.AddLeaveType(_LeaveType);
        }
        #endregion

        #region To call update method of LeaveType Table from Data access layer
        public int UpdateLeaveTypeDetail(LeaveTypeEntity _LeaveType)
        {
            var LeaveTypeDAC = new LeaveTypeDAC();
            return LeaveTypeDAC.UpdateLeaveTypeDetail(_LeaveType);
        }
        #endregion

        #region To call delete method of LeaveType Table from Data access layer
        public int DeleteLeaveTypeDetail(int ID)
        {
            var LeaveTypeDAC = new LeaveTypeDAC();
            return LeaveTypeDAC.DeleteLeaveTypeDetail(ID);
        }
        #endregion

        #region To get all details of LeaveType Table from Data access layer
        public List<LeaveTypeEntity> GetLeaveTypeDetail()
        {
            var LeaveTypeDAC = new LeaveTypeDAC();
            return LeaveTypeDAC.GetLeaveTypeDetail();
        }
        #endregion

        #region To get all details of Account Table from Data access layer
        public LeaveTypeEntity GetLeaveTypeDetailByID(int orgID)
        {
            var LeaveTypeDAC = new LeaveTypeDAC();
            return LeaveTypeDAC.GetLeaveTypeDetailByID(orgID);
        }
        #endregion

        #region To get ID and name of LeaveType Table from Data access layer
        public List<LeaveTypeEntity> SelectLeaveType()
        {
            var LeaveTypeDAC = new LeaveTypeDAC();
            return LeaveTypeDAC.SelectLeaveType();
        }
        #endregion
        
        #region To get Max AccountID details Table from Data access layer
        public LeaveType GetMaxLeaveTypeIDDetials()
        {
            var LeaveTypeDAC = new LeaveTypeDAC();
            return LeaveTypeDAC.GetMaxLeaveTypeIDDetials();
        }
        #endregion

        #region 
        public int GetMaxLeaveTypeID()
        {
            var LeaveTypeDAC = new LeaveTypeDAC();
            return LeaveTypeDAC.GetMaxLeaveTypeIDDetials().LTyp_LeaveTypeID;
        }
        #endregion
        
        #region To get all History details of LeaveType Table from Data access layer
        public List<History_LeaveTypeEntity> GetHistoryLeaveTypeDetailsByID(int ID)
        {
            var LeaveTypeDAC = new LeaveTypeDAC();
            return LeaveTypeDAC.GetHistoryLeaveTypeDetailsByID(ID);
        }
        #endregion


        public List<LeaveTypeEntity> GetAllLeaveTypes()
        {
            var LeaveTypeDAC = new LeaveTypeDAC();
            return LeaveTypeDAC.GetAllLeaveTypes();
        }

        public List<LeaveTypeEntity> GetAllEmpIds()
        {
            var LeaveTypeDAC = new LeaveTypeDAC();
            return LeaveTypeDAC.GetAllEmpIds();
        }
        public List<DashboardMails> GetAllProjects()
        {
            var LeaveTypeDAC = new LeaveTypeDAC();
            return LeaveTypeDAC.GetAllProjects();
        }

        public string ChangeStatus(string id, string status)
        {
            var LeaveTypeDAC = new LeaveTypeDAC();
            return LeaveTypeDAC.ChangeStatus(id, status);
        }

    }
}
