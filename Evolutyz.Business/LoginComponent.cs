using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evolutyz.Entities;
using Evolutyz.Data;
using evolCorner.Models;

namespace Evolutyz.Business
{
    public  class LoginComponent
    {
        #region To call Login method of User Table from Data access layer
        public User LoginUser(LoginEntity loginentity)
        {
            LoginDAC loginDAC = new LoginDAC();
            return loginDAC.LoginMethod(loginentity);
        }
        #endregion

        #region To Validate Login method of User Table from Data access layer
        public UserEntity ValidateLogin(LoginEntity loginentity)
        {
            LoginDAC loginDAC = new LoginDAC();
            return loginDAC.ValidateLogin(loginentity);
        }
        #endregion

        #region To After Validating to get UserProject Details for  default Timesheet Details  from Data access layer
        public UserProjectdetailsEntity GetUserProjectsDetailsInfo(UserSessionInfo loginentity)
        {
            LoginDAC loginDAC = new LoginDAC();
            return loginDAC.GetUserProjectsDetails(loginentity);
        }
        #endregion
        
        #region To Get User Details  method of User Table from Data access layer
        public UserEntity GerUserDetails(string UserName)
        {
            LoginDAC loginDAC = new LoginDAC();
            return loginDAC.GerUserDetails(UserName);
        }
        #endregion

        #region To Save token  method of User Table from Data access layer
        public string SaveToken(string token, int uid)
        {
            LoginDAC loginDAC = new LoginDAC();
            return loginDAC.SaveToken(token, uid);
        }
        #endregion
        #region To UpdatePasswordandtoken method of User Table from Data access layer
        public string UpdatePasswordandtoken(string token, string NewPassword)
        {
            LoginDAC loginDAC = new LoginDAC();
            return loginDAC.UpdatePasswordandtoken(token, NewPassword);
        }
        #endregion
    }
}
