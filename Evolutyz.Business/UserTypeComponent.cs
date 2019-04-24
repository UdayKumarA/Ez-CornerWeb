using Evolutyz.Data;
using Evolutyz.Entities;
using System.Collections.Generic;

namespace Evolutyz.Business
{
    public class UserTypeComponent
    {
        #region To call add method of UserType from Data access layer
        public int AddUserType(UserTypeEntity _uesrtype)
        {
            var UserTypeDAC = new UserTypeDAC();
            return UserTypeDAC.AddUserType(_uesrtype);
        }
        #endregion

        #region To call update method of UserType Table from Data access layer
        public int UpdateUserTypeDetail(UserTypeEntity _uesrtype)
        {
            var UserTypeDAC = new UserTypeDAC();
            return UserTypeDAC.UpdateUserTypeDetail(_uesrtype);
        }
        #endregion

        #region To call delete method of UserType Table from Data access layer
        public int DeleteUserTypeDetail(int ID)
        {
            var UserTypeDAC = new UserTypeDAC();
            return UserTypeDAC.DeleteUserTypeDetail(ID);
        }
        #endregion

        #region To get all details of UserType Table from Data access layer
        public List<UserTypeEntity> GetUserTypeDetail(int acntID, string RoleId)
        {
            var UserTypeDAC = new UserTypeDAC();
            return UserTypeDAC.GetUserTypeDetail(acntID, RoleId);
        }
        #endregion

        #region To get all details of Account Table from Data access layer
        public UserTypeEntity GetUserTypeDetailByID(int orgID)
        {
            var UserTypeDAC = new UserTypeDAC();
            return UserTypeDAC.GetUserTypeDetailByID(orgID);
        }
        #endregion

        #region To get ID and name of UserType Table from Data access layer
        public List<UserTypeEntity> SelectUserType()
        {
            var UserTypeDAC = new UserTypeDAC();
            return UserTypeDAC.SelectUserType();
        }
        #endregion
        #region To get Max AccountID details Table from Data access layer
        public UserType GetMaxUserTypeIDDetials()
        {
            var UserTypeDAC = new UserTypeDAC();
            return UserTypeDAC.GetMaxUserTypeIDDetials();
        }
        #endregion

        #region 
        public int GetMaxUserTypeID()
        {
            var UserTypeDAC = new UserTypeDAC();
            return UserTypeDAC.GetMaxUserTypeIDDetials().UsT_UserTypeID;
        }
        #endregion
        #region To get all History details of Account Table from Data access layer
        public List<History_UserTypeEntity> GetHistoryUserTypeDetailsByID(int ID)
        {
            var UserTypeDAC = new UserTypeDAC();
            return UserTypeDAC.GetHistoryUserTypeDetailsByID(ID);
        }
        #endregion

        public string ChangeStatus(string id, string status)
        {
            var LeaveTypeDAC = new UserTypeDAC();
            return LeaveTypeDAC.ChangeStatus(id, status);
        }


    }
}
