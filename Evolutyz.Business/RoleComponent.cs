using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evolutyz.Entities;
using Evolutyz.Data;

namespace Evolutyz.Business
{
    public class RoleComponent
    {

        #region To call add method of Role from Data access layer
        public string AddRole(List<RoleEntity> moduleaccess, RoleEntity RoleDtl)
        {
            var RoleDAC = new RoleDAC();
            return RoleDAC.AddRole(moduleaccess, RoleDtl);
        }
        #endregion
        #region To call add method of Role from Data access layer
        public string UpdateRole(int id, List<RoleEntity> moduleaccess, RoleEntity RoleDtl)
        {
            var RoleDAC = new RoleDAC();
            return RoleDAC.UpdateRole(id, moduleaccess, RoleDtl);
        }
        #endregion

        #region To call update method of Role Table from Data access layer
        public int UpdateRoleDetail(RoleEntity _role)
        {
            var RoleDAC = new RoleDAC();
            return RoleDAC.UpdateRoleDetail(_role);
        }
        #endregion

        #region To call delete method of Role Table from Data access layer
        public int DeleteRoleDetail(int ID)
        {
            var RoleDAC = new RoleDAC();
            return RoleDAC.DeleteRoleDetail(ID);
        }
        #endregion
        #region To call delete method of Rolemodule Table from Data access layer
        public bool DeleteRolemodules(int ID)
        {
            var RoleDAC = new RoleDAC();
            return RoleDAC.DeleteRolemodules(ID);
        }
        #endregion

        #region To get all details of Role Table from Data access layer
        public List<RoleEntity> GetRoleDetail(int accountid,string rolename)
        {
            var RoleDAC = new RoleDAC();
            return RoleDAC.GetRoleDetail(accountid, rolename);
        }
        #endregion

        #region To get all details of Account Table from Data access layer
        public RoleEntity GetRoleDetailByID(int orgID)
        {
            var RoleDAC = new RoleDAC();
            return RoleDAC.GetRoleDetailByID(orgID);
        }
        #endregion
        #region To get all details of Account Table from Data access layer
        public List<RoleEntity> Getmodulesselected(int orgID)
        {
            var RoleDAC = new RoleDAC();
            return RoleDAC.Getmodulesselected(orgID);
        }
        #endregion

        #region To get ID and name of Role Table from Data access layer
        public List<RoleEntity> SelectRole()
        {
            var RoleDAC = new RoleDAC();
            return RoleDAC.SelectRole();
        }
        #endregion

        #region To get Max AccountID details Table from Data access layer
        public Role GetMaxRoleIDDetials()
        {
            var RoleDAC = new RoleDAC();
            return RoleDAC.GetMaxRoleIDDetials();
        }
        #endregion

        #region 
        public int GetMaxRoleID()
        {
            var RoleDAC = new RoleDAC();
            return RoleDAC.GetMaxRoleIDDetials().Rol_RoleID;
        }
        #endregion

        #region To get all History details of Role Table from Data access layer
        public List<History_RoleEntity> GetHistoryRoleDetailsByID(int ID)
        {
            var RoleDAC = new RoleDAC();
            return RoleDAC.GetHistoryRoleDetailsByID(ID);
        }
        #endregion

        public List<RoleEntity> Getallmodules()
        {
            var count = new RoleDAC();
            return count.Getallmodules();
        }
        public List<RoleEntity> Getallaccess()
        {
            var count = new RoleDAC();
            return count.Getallaccess();
        }

        public List<RoleEntity> Getsubmodules(int modid)
        {
            var count = new RoleDAC();
            return count.Getsubmodules(modid);
        }

        public List<RoleEntity> GetallsubModule()
        {
            var count = new RoleDAC();
            return count.GetallsubModule();
        }

        public string ChangeStatus(string id, string status)
        {
            var count = new RoleDAC();
            return count.ChangeStatus(id, status);
        }


    }
}
