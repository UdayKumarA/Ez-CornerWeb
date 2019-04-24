using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evolutyz.Entities;
using Evolutyz.Data;

namespace Evolutyz.Business
{
    public class ProjectComponent
    {
        #region To call add method of Project from Data access layer
        public int AddProject(ProjectEntity _Project)
        {
            var ProjectDAC = new ProjectDAC();
            return ProjectDAC.AddProject(_Project);
        }
        #endregion

        #region To call update method of Project Table from Data access layer
        public int UpdateProjectDetail(ProjectEntity _Project)
        {
            var ProjectDAC = new ProjectDAC();
            return ProjectDAC.UpdateProjectDetail(_Project);
        }
        #endregion

        #region To call delete method of Project Table from Data access layer
        public int DeleteProjectDetail(int ID)
        {
            var ProjectDAC = new ProjectDAC();
            return ProjectDAC.DeleteProjectDetail(ID);
        }
        #endregion

        #region To get all details of Project Table from Data access layer
        //public List<ProjectEntity> GetProjectDetail(int AccountId)
        //{
        //    var ProjectDAC = new ProjectDAC();
        //    return ProjectDAC.GetProjectDetail(AccountId);
        //}
        #endregion

        #region To get all details of Account Table from Data access layer
        public ProjectEntity GetProjectDetailByID(int orgID)
        {
            var ProjectDAC = new ProjectDAC();
            return ProjectDAC.GetProjectDetailByID(orgID);
        }
        #endregion

        #region To get all UserNames from Data access layer
        public List<ProjectAllocationEntity> GetUserNames()
        {
            var ProjectDAC = new ProjectDAC();
            return ProjectDAC.GetUserNames();
        }
        #endregion

        #region To get all RoleNames from Data access layer
        public List<RoleEntity> GetUserRoles()
        {
            var ProjectDAC = new ProjectDAC();
            return ProjectDAC.GetUserRoles();
        }
        #endregion

        #region To get all ProjectNames from Data access layer
        public List<ProjectEntity> GetProjectNames()
        {
            var ProjectDAC = new ProjectDAC();
            return ProjectDAC.GetProjectNames();
        }
        #endregion

        #region To get ID and name of Project Table from Data access layer
        public List<ProjectEntity> SelectProject()
        {
            var ProjectDAC = new ProjectDAC();
            return ProjectDAC.SelectProject();
        }
        #endregion

        #region To get Max AccountID details Table from Data access layer
        public Project GetMaxProjectIDDetials()
        {
            var ProjectDAC = new ProjectDAC();
            return ProjectDAC.GetMaxProjectIDDetials();
        }
        #endregion

        #region 
        public int GetMaxProjectID()
        {
            var ProjectDAC = new ProjectDAC();
            return ProjectDAC.GetMaxProjectIDDetials().Proj_ProjectID;
        }
        #endregion


        #region To get all details of ProjectAllocation Table from Data access layer
        public List<ProjectEntity> GetProjectDetail(int AccountId)
        {
            var ProjectDAC = new ProjectDAC();
            return ProjectDAC.GetProjectDetail(AccountId);
        }
        #endregion

        #region To get all History details of Project Table from Data access layer
        public List<History_ProjectsEntity> GetHistoryProjectDetailsByID(int ID)
        {
            var ProjectDAC = new ProjectDAC();
            return ProjectDAC.GetHistoryProjectDetailsByID(ID);
        }
        #endregion


        public string ChangeStatus(string id, string status)
        {
            var LeaveTypeDAC = new ProjectDAC();
            return LeaveTypeDAC.ChangeStatus(id, status);
        }

    }
}
