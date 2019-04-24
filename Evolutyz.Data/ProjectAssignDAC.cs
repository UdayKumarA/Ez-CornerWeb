using Evolutyz.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Data;


namespace Evolutyz.Data
{
    public class ProjectAssignDAC : DataAccessComponent
    {


        #region To add ProjectAllocation Detail in Database
        public int AddProjectAllocation(ProjectAllocationEntity _ProjectAllocation)
        {
            int retVal = 0;
            UserProject ProjectAllocation = new UserProject();

            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {

                    ProjectAllocation = db.Set<UserProject>().Where(s => s.UProj_UserProjectID == ProjectAllocation.UProj_UserProjectID).FirstOrDefault<UserProject>();

                    if (ProjectAllocation != null)
                    {
                        return retVal;
                    }

                    #region Saving ProjectAllocation info Table

                    db.Set<UserProject>().Add(new UserProject
                    {

                        UProj_ProjectID = _ProjectAllocation.UProj_ProjectID,
                        UProj_UserID = _ProjectAllocation.UProj_UserID,
                        UProj_ParticipationPercentage = Convert.ToByte(_ProjectAllocation.UProj_ParticipationPercentage),
                        UProj_StartDate = Convert.ToDateTime(_ProjectAllocation.UProj_StartDate),
                        UProj_EndDate = _ProjectAllocation.UProj_EndDate,
                        UProj_ActiveStatus = _ProjectAllocation.UProj_ActiveStatus,
                        UProj_Version = _ProjectAllocation.UProj_Version,
                        UProj_CreatedDate = System.DateTime.Now,
                        UProj_CreatedBy = _ProjectAllocation.UProj_CreatedBy,
                        UProj_ModifiedDate = System.DateTime.Now,
                        UProj_ModifiedBy = _ProjectAllocation.UProj_ModifiedBy,
                        UProj_isDeleted = _ProjectAllocation.UProj_isDeleted,
                        Is_L1_Manager = _ProjectAllocation.Is_L1_Manager,
                        Is_L2_Manager = _ProjectAllocation.Is_L2_Manager,
                        UProj_L1_ManagerId = _ProjectAllocation.UProj_L1_ManagerId,
                        UProj_L2_ManagerId = _ProjectAllocation.UProj_L2_ManagerId

                        #endregion

                    });

                    retVal = db.SaveChanges();
                    //retVal = 1;

                }
                catch (Exception ex)
                {
                    retVal = -1;
                }
                return retVal;
            }

        }
        #endregion

        #region To update existing ProjectAllocation Detail in Database
        public int UpdateProjectAllocationDetail(ProjectAllocationEntity ProjectAllocation)
        {

            UserProject _ProjectAllocationDtl = new UserProject();
            ProjectAllocationEntity response = new ProjectAllocationEntity();

            int retVal = 0;

            //using (var db = new EvolutyzCornerDataEntities())
            //{
            //    try
            //    {
            //        response = (from ufp in db.UserProjects
            //                    join p in db.Projects on ufp.UProj_ProjectID equals p.Proj_ProjectID
            //                    join u in db.Users on ufp.UProj_UserID equals u.Usr_UserID
            //                    where ufp.UProj_isDeleted == false && ufp.UProj_ActiveStatus == true
            //                    && ufp.UProj_UserProjectID == ProjectAllocation.UProj_UserProjectID
            //                    select new ProjectAllocationEntity
            //                    {
            //                        UProj_UserProjectID = ufp.UProj_UserProjectID,
            //                        UProj_ProjectID = ufp.UProj_ProjectID,
            //                        //Proj_ProjectName = p.Proj_ProjectName,
            //                        UProj_UserID = ufp.UProj_UserID,
            //                        //us = u.Usr_Username,
            //                        UProj_StartDate = ufp.UProj_StartDate,
            //                        UProj_EndDate = ufp.UProj_EndDate,
            //                        UProj_ParticipationPercentage = ufp.UProj_ParticipationPercentage,
            //                        UProj_ActiveStatus = ufp.UProj_ActiveStatus,
            //                        UProj_Version = ufp.UProj_Version,
            //                        UProj_CreatedBy = ufp.UProj_CreatedBy,
            //                        UProj_CreatedDate = ufp.UProj_CreatedDate,
            //                        UProj_ModifiedBy = ufp.UProj_ModifiedBy,
            //                        UProj_ModifiedDate = ufp.UProj_ModifiedDate,
            //                        UProj_isDeleted = ufp.UProj_isDeleted,
            //                        UProj_L1_ManagerId=ufp.UProj_L1_ManagerId,
            //                        UProj_L2_ManagerId=ufp.UProj_L2_ManagerId
            //                    }).FirstOrDefault();

            //        response.IsSuccessful = true;

            //        History_UserProjects _ProjectAllocationMoveTohistory = new History_UserProjects();

            //        if (_ProjectAllocationMoveTohistory == null)
            //        {
            //            return retVal;

            //        }
            //        _ProjectAllocationMoveTohistory.HUProj_ProjectID = response.UProj_ProjectID;
            //        _ProjectAllocationMoveTohistory.HUProj_UserID = response.UProj_UserID;
            //        _ProjectAllocationMoveTohistory.HUProj_ParticipationPercentage = Convert.ToByte(response.UProj_ParticipationPercentage);
            //        _ProjectAllocationMoveTohistory.HUProj_StartDate = response.UProj_StartDate;
            //        _ProjectAllocationMoveTohistory.HUProj_EndDate = response.UProj_EndDate;
            //        _ProjectAllocationMoveTohistory.HUProj_ActiveStatus = response.UProj_ActiveStatus;
            //        _ProjectAllocationMoveTohistory.HUProj_Version = response.UProj_Version;
            //        _ProjectAllocationMoveTohistory.HUProj_ModifiedDate = System.DateTime.Now;
            //        _ProjectAllocationMoveTohistory.HUProj_CreatedDate = System.DateTime.Now;
            //        _ProjectAllocationMoveTohistory.HUProj_ModifiedBy = response.UProj_ModifiedBy;
            //        _ProjectAllocationMoveTohistory.HUProj_isDeleted = response.UProj_isDeleted;

            //        db.History_UserProjects.Add(_ProjectAllocationMoveTohistory);
            //        retVal = db.SaveChanges();

            //        //db.Entry(_ProjectAllocationMoveTohistory).State = System.Data.Entity.EntityState.Modified;
            //        //retVal = db.History_UserProjects.SaveChanges();
            //    }
            //    catch (Exception ex)
            //    {
            //        string excep = ex.InnerException.Message.ToString();
            //        //response.IsSuccessful = false;
            //        //response.Message = "Error Occured in GetProjectAllocationDetailByID(ID)";
            //        //response.Detail = ex.Message.ToString();
            //        //return response;
            //    }
            //}



            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {

                    _ProjectAllocationDtl = db.Set<UserProject>().Where(s => s.UProj_UserProjectID == ProjectAllocation.UProj_UserProjectID).FirstOrDefault<UserProject>();

                    if (_ProjectAllocationDtl == null)
                    {
                        return retVal;
                    }

                    #region Saving ProjectAllocation info Table

                    _ProjectAllocationDtl.UProj_ProjectID = ProjectAllocation.UProj_ProjectID;
                    _ProjectAllocationDtl.UProj_UserID = ProjectAllocation.UProj_UserID;
                    _ProjectAllocationDtl.UProj_ParticipationPercentage = Convert.ToByte(ProjectAllocation.UProj_ParticipationPercentage);
                    _ProjectAllocationDtl.UProj_StartDate = Convert.ToDateTime(ProjectAllocation.UProj_StartDate);
                    _ProjectAllocationDtl.UProj_EndDate = ProjectAllocation.UProj_EndDate;
                    _ProjectAllocationDtl.UProj_ActiveStatus = ProjectAllocation.UProj_ActiveStatus;
                    _ProjectAllocationDtl.UProj_Version = ProjectAllocation.UProj_Version;
                    _ProjectAllocationDtl.UProj_CreatedDate = System.DateTime.Now;
                    _ProjectAllocationDtl.UProj_CreatedBy = ProjectAllocation.UProj_CreatedBy;
                    _ProjectAllocationDtl.UProj_ModifiedDate = System.DateTime.Now;
                    _ProjectAllocationDtl.UProj_ModifiedBy = ProjectAllocation.UProj_ModifiedBy;
                    _ProjectAllocationDtl.UProj_isDeleted = ProjectAllocation.UProj_isDeleted;
                    _ProjectAllocationDtl.Is_L1_Manager = ProjectAllocation.Is_L1_Manager;
                    _ProjectAllocationDtl.Is_L2_Manager = ProjectAllocation.Is_L2_Manager;
                    _ProjectAllocationDtl.UProj_L1_ManagerId = ProjectAllocation.UProj_L1_ManagerId;
                    _ProjectAllocationDtl.UProj_L2_ManagerId = ProjectAllocation.UProj_L2_ManagerId;
                    #endregion
                    db.Entry(_ProjectAllocationDtl).State = System.Data.Entity.EntityState.Modified;

                    retVal = db.SaveChanges();
                    //retVal = 1;

                }
                catch (Exception ex)
                {
                    retVal = -1;
                }
                return retVal;
            }




        }
        #endregion

        #region To delete existing ProjectAllocation details from Database
        public int DeleteProjectAllocationDetail(int ctID)
        {
            int retVal = 0;
            UserProject _ProjectAllocationDtl = null;

            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {

                    _ProjectAllocationDtl = db.Set<UserProject>().Where(s => s.UProj_UserProjectID == ctID).FirstOrDefault<UserProject>();
                    if (_ProjectAllocationDtl == null)
                    {
                        return retVal;
                    }
                    _ProjectAllocationDtl.UProj_isDeleted = true;
                    db.Entry(_ProjectAllocationDtl).State = System.Data.Entity.EntityState.Modified;
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

        #region GetUserRoleNames

        public List<UserEntity> GetRoleNames()
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var query = (from r in db.Roles
                                 join gr in db.GenericRoles on r.Rol_RoleName equals gr.GenericRoleID
                                 where r.IsManagerRole == true
                                 select new UserEntity
                                 {
                                     Usr_RoleID = r.Rol_RoleID,
                                     RoleName = gr.Title

                                 }).ToList();


                    return query;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        #endregion





        #region To get all details of ProjectAllocation from Database
        [System.Runtime.InteropServices.ComVisible(true)]

        public List<ProjectAllocationEntity> GetProjectAllocationDetail(string getGridBasedONRoles, string[] GettingCheckData, int Projectid)
        {
            //var checkedData = GettingCheckData[0].Split(',');

            List<decimal> checkedData = GettingCheckData[0].Split(',').Select(decimal.Parse).ToList();

            List<ProjectAllocationEntity> lstResponse = new List<ProjectAllocationEntity>();

            if (getGridBasedONRoles == "1")
            {
                using (var db = new EvolutyzCornerDataEntities())
                {
                    try
                    {
                        var temp = (from ufp in db.UserProjects
                                    join p in db.Projects on ufp.UProj_ProjectID equals p.Proj_ProjectID
                                    join u in db.Users on ufp.UProj_UserID equals u.Usr_UserID
                                    join R in db.Roles on u.Usr_RoleID equals R.Rol_RoleID
                                    join gr in db.GenericRoles on R.Rol_RoleName equals gr.GenericRoleID
                                    where ufp.UProj_isDeleted == false && ufp.UProj_ActiveStatus == true &&
                                     checkedData.Contains(u.Usr_RoleID)
                                    // checkedData.Contains(System.Data.Entity.SqlServer.SqlFunctions.StringConvert((double)u.Usr_RoleID))
                                    select new ProjectAllocationEntity
                                    {
                                        Proj_ProjectCode = p.Proj_ProjectCode,
                                        UProj_UserProjectID = ufp.UProj_UserProjectID,
                                        UProj_ProjectID = ufp.UProj_ProjectID,
                                        ProjectName = p.Proj_ProjectName,
                                        UProj_UserID = ufp.UProj_UserID,
                                        Username = u.Usr_Username,
                                        UProj_StartDate = ufp.UProj_StartDate,
                                        UProj_EndDate = ufp.UProj_EndDate,
                                        UProj_ParticipationPercentage = ufp.UProj_ParticipationPercentage,
                                        UProj_ActiveStatus = ufp.UProj_ActiveStatus,
                                        UProj_Version = ufp.UProj_Version,
                                        UProj_CreatedBy = ufp.UProj_CreatedBy,
                                        UProj_CreatedDate = ufp.UProj_CreatedDate,
                                        UProj_ModifiedBy = ufp.UProj_ModifiedBy,
                                        UProj_ModifiedDate = ufp.UProj_ModifiedDate,
                                        UProj_isDeleted = ufp.UProj_isDeleted,

                                    }).OrderBy(x => x.ProjectName).ThenBy(x => x.Username)
                              .ThenBy(x => x.UProj_ParticipationPercentage).ToList();

                        //  return lstResponse;
                        return temp;

                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                }
            }
            else
            {
                using (var db = new EvolutyzCornerDataEntities())
                {
                    try
                    {
                        var query = (from ufp in db.UserProjects
                                     join p in db.Projects on ufp.UProj_ProjectID equals p.Proj_ProjectID
                                     join u in db.Users on ufp.UProj_UserID equals u.Usr_UserID
                                     where ufp.UProj_ProjectID == Projectid && ufp.UProj_isDeleted == false
                                     select new ProjectAllocationEntity
                                     {

                                         UProj_UserProjectID = ufp.UProj_UserProjectID,
                                         UProj_ProjectID = ufp.UProj_ProjectID,
                                         ProjectName = p.Proj_ProjectName,
                                         UProj_UserID = ufp.UProj_UserID,
                                         Username = u.Usr_Username,
                                         UProj_StartDate = ufp.UProj_StartDate,
                                         UProj_EndDate = ufp.UProj_EndDate,
                                         UProj_ParticipationPercentage = ufp.UProj_ParticipationPercentage,
                                         UProj_ActiveStatus = ufp.UProj_ActiveStatus,
                                         UProj_Version = ufp.UProj_Version,
                                         UProj_CreatedBy = ufp.UProj_CreatedBy,
                                         UProj_CreatedDate = ufp.UProj_CreatedDate,
                                         UProj_ModifiedBy = ufp.UProj_ModifiedBy,
                                         UProj_ModifiedDate = ufp.UProj_ModifiedDate,
                                         UProj_isDeleted = ufp.UProj_isDeleted

                                     }).OrderBy(x => x.ProjectName).ThenBy(x => x.Username)
                              .ThenBy(x => x.UProj_ParticipationPercentage).ToList();

                        return query;
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                }

            }

        }
        #endregion

        #region To get particular ProjectAllocation details from Database
        public ProjectAllocationEntity GetProjectAllocationDetailByID(int ID)
        {
            ProjectAllocationEntity response = new ProjectAllocationEntity();

            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    response = (from ufp in db.UserProjects
                                join p in db.Projects on ufp.UProj_ProjectID equals p.Proj_ProjectID
                                join u in db.Users on ufp.UProj_UserID equals u.Usr_UserID
                                where ufp.UProj_isDeleted == false && ufp.UProj_ActiveStatus == true
                                && ufp.UProj_UserProjectID == ID
                                select new ProjectAllocationEntity
                                {
                                    Proj_ProjectCode = p.Proj_ProjectCode,
                                    UProj_UserProjectID = ufp.UProj_UserProjectID,
                                    UProj_ProjectID = ufp.UProj_ProjectID,
                                    ProjectName = p.Proj_ProjectName,
                                    UProj_UserID = ufp.UProj_UserID,
                                    Username = u.Usr_Username,
                                    UProj_StartDate = ufp.UProj_StartDate,
                                    UProj_EndDate = ufp.UProj_EndDate,
                                    UProj_ParticipationPercentage = ufp.UProj_ParticipationPercentage,
                                    UProj_ActiveStatus = ufp.UProj_ActiveStatus,
                                    UProj_Version = ufp.UProj_Version,
                                    UProj_CreatedBy = ufp.UProj_CreatedBy,
                                    UProj_CreatedDate = ufp.UProj_CreatedDate,
                                    UProj_ModifiedBy = ufp.UProj_ModifiedBy,
                                    UProj_ModifiedDate = ufp.UProj_ModifiedDate,
                                    UProj_isDeleted = ufp.UProj_isDeleted,
                                    Is_L1_Manager = ufp.Is_L1_Manager,
                                    Is_L2_Manager = ufp.Is_L2_Manager,
                                    UProj_L1_ManagerId = ufp.UProj_L1_ManagerId,
                                    UProj_L2_ManagerId = ufp.UProj_L2_ManagerId

                                }).FirstOrDefault();

                    response.IsSuccessful = true;
                    return response;
                }
                catch (Exception ex)
                {
                    response.IsSuccessful = false;
                    response.Message = "Error Occured in GetProjectAllocationDetailByID(ID)";
                    response.Detail = ex.Message.ToString();
                    return response;
                }
            }
        }
        #endregion

        //#region To get ProjectAllocation details for select list
        //public List<ProjectAllocationEntity> SelectProjectAllocation()
        //{
        //    using (var db = new EvolutyzCornerDataEntities())
        //    {
        //        try
        //        {
        //            var query = (from ufp in db.UsersForProjects
        //                         join p in db.Projects on ufp.Ufp_ProjectID equals p.Proj_ProjectID
        //                         join u in db.Users on ufp.Ufp_UserID equals u.Usr_UserID
        //                         where ufp.Ufp_isDeleted == false && ufp.Ufp_ActiveStatus == true
        //                         select new ProjectAllocationEntity
        //                         {
        //                             Ufp_UsersForProjectsID = ufp.Ufp_UsersForProjectsID,
        //                             Ufp_ProjectID = ufp.Ufp_ProjectID,
        //                             ProjectName = p.Proj_ProjectName,
        //                             Ufp_UserID = ufp.Ufp_UserID,
        //                             Username = u.Usr_Username,
        //                             Ufp_StartDate = ufp.Ufp_StartDate,
        //                             Ufp_EndDate = ufp.Ufp_EndDate,
        //                             Ufp_ParticipationPercentage = ufp.Ufp_ParticipationPercentage,
        //                             Ufp_ActiveStatus = ufp.Ufp_ActiveStatus,
        //                             Ufp_Version = ufp.Ufp_Version,
        //                             Ufp_CreatedBy = ufp.Ufp_CreatedBy,
        //                             Ufp_CreatedDate = ufp.Ufp_CreatedDate,
        //                             Ufp_ModifiedBy = ufp.Ufp_ModifiedBy,
        //                             Ufp_ModifiedDate = ufp.Ufp_ModifiedDate,
        //                             Ufp_isDeleted = ufp.Ufp_isDeleted,
        //                         }).OrderBy(x => x.ProjectName).ThenBy(x => x.Username)
        //                                .ThenBy(x => x.Ufp_ParticipationPercentage).ToList();

        //            return query;
        //        }
        //        catch (Exception ex)
        //        {
        //            return null;
        //        }
        //    }
        //}
        //#endregion
    }
}


