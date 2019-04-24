using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evolutyz.Entities;
using Evolutyz.Data;

namespace Evolutyz.Business
{
    public class ProjectAssignComponent
    {
        #region To call add method of ProjectAllocation from Data access layer
        public int AddProjectAllocation(ProjectAllocationEntity _ProjectAllocation)
        {
            var ProjectAssignDAC = new ProjectAssignDAC();
            return ProjectAssignDAC.AddProjectAllocation(_ProjectAllocation);
        }
        #endregion

        #region To call update method of ProjectAllocation Table from Data access layer
        public int UpdateProjectAllocationDetail(ProjectAllocationEntity _ProjectAllocation)
        {
            var ProjectAssignDAC = new ProjectAssignDAC();
            return ProjectAssignDAC.UpdateProjectAllocationDetail(_ProjectAllocation);
        }
        #endregion

        #region To call delete method of ProjectAllocation Table from Data access layer
        public int DeleteProjectAllocationDetail(int ID)
        {
            var ProjectAssignDAC = new ProjectAssignDAC();
            return ProjectAssignDAC.DeleteProjectAllocationDetail(ID);
        }
        #endregion

        #region To get all details of ProjectAllocation Table from Data access layer
        public List<ProjectAllocationEntity> GetProjectAllocationDetail(string getGridBasedONRoles, string[] GettingCheckData, int Projectid)
        {
            var ProjectAssignDAC = new ProjectAssignDAC();
            return ProjectAssignDAC.GetProjectAllocationDetail(getGridBasedONRoles, GettingCheckData, Projectid);
        }
        #endregion

        #region To get all details of Account Table from Data access layer
        public ProjectAllocationEntity GetProjectAllocationDetailByID(int ID)
        {
            var ProjectAssignDAC = new ProjectAssignDAC();
            return ProjectAssignDAC.GetProjectAllocationDetailByID(ID);
        }
        #endregion

        #region To get all details of Role Names Table from Data access layer

        public List<UserEntity> GetUserRolenames()
        {
            var ProjectAssignDAC = new ProjectAssignDAC();
            return ProjectAssignDAC.GetRoleNames();
        }
        #endregion
        //#region To get ID and name of ProjectAllocation Table from Data access layer
        //public List<ProjectAllocationEntity> SelectProjectAllocation()
        //{
        //    var ProjectAssignDAC = new ProjectAssignDAC();
        //    return ProjectAssignDAC.SelectProjectAllocation();
        //}
        //#endregion
    }
}
