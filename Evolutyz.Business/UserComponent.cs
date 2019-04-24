using Evolutyz.Data;
using Evolutyz.Entities;
using System.Collections.Generic;

namespace Evolutyz.Business
{
    public class UserComponent
    {
        #region To call add method of User from Data access layer
        public int AddUser(UserEntity _uesrtype)
        {
            var UserDAC = new UserDAC();
            return UserDAC.AddUser(_uesrtype);
        }
        #endregion

        #region To call update method of User Table from Data access layer
        public int UpdateUserDetail(UserEntity _uesrtype)
        {
            var UserDAC = new UserDAC();
            return UserDAC.UpdateUserDetail(_uesrtype);
        }
        #endregion
        
        #region To call delete method of User Table from Data access layer
        public int DeleteUserDetail(int ID)
        {
            var UserDAC = new UserDAC();
            return UserDAC.DeleteUserDetail(ID);
        }
        public int DeleteSkill(int skillid)
        {
            var UserDAC = new UserDAC();
            return UserDAC.DeleteSkill(skillid);
        }
        #endregion

        #region To call update method of User Table from Data access layer
        public int UpdateUserDetailByImage(UserEntity _uesrtype)
        {
            var UserDAC = new UserDAC();
            return UserDAC.UpdateUserDetailByImage(_uesrtype);
        }
        #endregion

        #region To get all details of User Table from Data access layer
        public List<UserEntity> GetUserDetail(int acntID, string RoleId)
        {
            var UserDAC = new UserDAC();
            return UserDAC.GetUserDetail(acntID, RoleId);
        }
        #endregion

        #region To get all details of Account Table from Data access layer
        public UserEntity GetUserDetailByID(int orgID)
        {
            var UserDAC = new UserDAC();
            return UserDAC.GetUserDetailByID(orgID);
        }
        #endregion

        #region To get all details of Role Names Table from Data access layer

        public List<UserEntity> GetUserRolenames(int AccountId, string RoleId)
        {
            var UserDAC = new UserDAC();
            return UserDAC.GetRoleNames(AccountId, RoleId);
        }
        #endregion

        public List<UserEntity> GetManagerNames2(int AccountId)
        {
            var UserDAC = new UserDAC();
            return UserDAC.GetManagerNames2(AccountId);
        }

        public List<UserEntity> GetManagerNames(int AccountId)
        {
            var UserDAC = new UserDAC();
            return UserDAC.GetManagerNames(AccountId);
        }

        #region To get all tasknames from data access layer

        public List<UserEntity> GetTaskNames(int AccountId)
        {
            var UserDAC = new UserDAC();
            return UserDAC.GetTaskNames(AccountId);

        }

        #endregion


        #region To get all UserTypes from data access layer

        public List<UserEntity> getUserTypes(int AccountId)
        {
            var UserDAC = new UserDAC();
            return UserDAC.getUserTypes(AccountId);

        }

        #endregion

        #region to get all account names from data access layer

        public List<UserEntity> GetAccounts(int acntID, int userid)
        {
            var UserDAC = new UserDAC();
            return UserDAC.GetAccountNames(acntID, userid);

        }
        public List<UserEntity> GetAccounts1(int accountId, string RoleId)
        {
            var UserDAC = new UserDAC();
            return UserDAC.GetAccountNames1(accountId, RoleId);

        }

        #endregion

        #region To get ID and name of User Table from Data access layer
        public List<UserEntity> SelectUser()
        {
            var UserDAC = new UserDAC();
            return UserDAC.SelectUser();
        }
        #endregion

        #region To get Max AccountID details Table from Data access layer
        public User GetMaxUserIDDetials()
        {
            var UserDAC = new UserDAC();
            return UserDAC.GetMaxUserIDDetials();
        }
        #endregion

        #region 
        public int GetMaxUserID()
        {
            var UserDAC = new UserDAC();
            return UserDAC.GetMaxUserIDDetials().Usr_UserID;
        }
        #endregion

        #region To get all History details of Account Table from Data access layer
        public List<History_UsersEntity> GetHistoryUserDetailsByID(int ID)
        {
            var UserDAC = new UserDAC();
            return UserDAC.GetHistoryUserDetailsByID(ID);
        }
        #endregion

        #region To call  GetAllById method of Grades Table from Data access layer
        public TimesheetEntity Gettimesheet(int userid)
        {
            var userTimesheetDAC = new UserTimesheetDAC();
            return userTimesheetDAC.Gettimesheet(userid);
        }
        #endregion

        public List<UserEntity> GetAccounts(int userid)
        {
            var UserDAC = new UserDAC();
            return UserDAC.GetAccountNamesbyid(userid);

        }

        public string ChangeStatus(string id, string status)
        {
            var LeaveTypeDAC = new UserDAC();
            return LeaveTypeDAC.ChangeStatus(id, status);
        }
        public string SkillChangeStatus(string id, string status)
        {
            var LeaveTypeDAC = new UserDAC();
            return LeaveTypeDAC.SkillChangeStatus(id, status);
        }


    }
}
