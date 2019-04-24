using Evolutyz.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace Evolutyz.Data
{
    public class ProjectDAC : DataAccessComponent
    {
        #region To add Project Detail in Database

        public int AddProject(ProjectEntity _project)
        {
            int retVal = 0;
            Project Project = new Project();

            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    //Project = db.Set<Project>().Where(s => s.Proj_ProjectID == _project.Proj_ProjectID).FirstOrDefault<Project>();
                    if (Project == null)
                    {
                        return retVal;
                    }
                    db.Set<Project>().Add(new Project
                    {
                        Proj_AccountID = _project.Proj_AccountID,
                        Proj_ProjectCode = _project.Proj_ProjectCode,
                        Proj_ProjectName = _project.Proj_ProjectName,
                        Proj_ProjectDescription = _project.Proj_ProjectDescription,
                        Plan_StartDate = _project.Proj_StartDate,
                        Plan_EndDate = _project.Proj_EndDate,
                        Proj_ActiveStatus = _project.Proj_ActiveStatus,
                        Proj_Version = _project.Proj_Version,
                        Proj_CreatedBy = _project.Proj_CreatedBy,
                        Proj_CreatedDate = System.DateTime.Now,
                        Is_Timesheet_ProjectSpecific= _project.Is_Timesheet_ProjectSpecific,
                        Proj_isDeleted = false
                    });
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

        #region To update existing Project Detail in Database
        public int UpdateProjectDetail(ProjectEntity Project)
        {
            Project _ProjectDtl = null;
            History_Projects _ProjectHistory = new History_Projects();

            int retVal = 0;

            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    _ProjectDtl = db.Set<Project>().Where(s => s.Proj_ProjectID == Project.Proj_ProjectID).FirstOrDefault<Project>();

                    if (_ProjectDtl == null)
                    {
                        return retVal;
                    }

                    #region Saving History into History_Project Table

                    //check for  _ProjectDtl.Proj_ModifiedDate as created date to history table
                    DateTime dtAccCreatedDate = _ProjectDtl.Proj_ModifiedDate ?? DateTime.Now;

                    db.Set<History_Projects>().Add(new History_Projects
                    {
                        History_Proj_ProjectID = _ProjectDtl.Proj_ProjectID,
                        History_Proj_AccountID = _ProjectDtl.Proj_AccountID,
                        History_Proj_ProjectCode = _ProjectDtl.Proj_ProjectCode,
                        History_Proj_ProjectName = _ProjectDtl.Proj_ProjectName,
                        History_Proj_ProjectDescription = _ProjectDtl.Proj_ProjectDescription,
                        History_Proj_StartDate = _ProjectDtl.Plan_StartDate,
                        History_Proj_EndDate = _ProjectDtl.Plan_EndDate,
                        History_Proj_ActiveStatus = _ProjectDtl.Proj_ActiveStatus,
                        History_Proj_Version = _ProjectDtl.Proj_Version,
                        History_Proj_CreatedDate = dtAccCreatedDate,
                        History_Proj_CreatedBy = _ProjectDtl.Proj_CreatedBy,
                        History_Proj_ModifiedDate = _ProjectDtl.Proj_ModifiedDate,
                        History_Proj_ModifiedBy = _ProjectDtl.Proj_ModifiedBy,
                        History_Proj_isDeleted = _ProjectDtl.Proj_isDeleted
                    });
                    #endregion

                    #region Saving Project info Table

                    _ProjectDtl.Proj_AccountID = Project.Proj_AccountID;
                    _ProjectDtl.Proj_ProjectCode = Project.Proj_ProjectCode;
                    _ProjectDtl.Proj_ProjectName = Project.Proj_ProjectName;
                    _ProjectDtl.Proj_ProjectDescription = Project.Proj_ProjectDescription;
                    _ProjectDtl.Plan_StartDate = Project.Proj_StartDate;
                    _ProjectDtl.Plan_EndDate = Project.Proj_EndDate;
                    _ProjectDtl.Proj_ActiveStatus = Project.Proj_ActiveStatus;
                    _ProjectDtl.Proj_Version = Project.Proj_Version;
                    _ProjectDtl.Proj_ModifiedDate = System.DateTime.Now;
                    _ProjectDtl.Proj_ModifiedBy = Project.Proj_ModifiedBy;
                    _ProjectDtl.Is_Timesheet_ProjectSpecific = Project.Is_Timesheet_ProjectSpecific;
                    _ProjectDtl.Proj_isDeleted = Project.Proj_isDeleted;
                    #endregion
                    db.Entry(_ProjectDtl).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    retVal = 1;
                }
                catch (Exception ex)
                {
                    retVal = -1;
                }
                return retVal;
            }
        }
        #endregion

        #region To delete existing Project details from Database
        public int DeleteProjectDetail(int ctID)
        {
            int retVal = 0;
            Project _ProjectDtl = null;

            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    _ProjectDtl = db.Set<Project>().Where(s => s.Proj_ProjectID == ctID).FirstOrDefault<Project>();
                    if (_ProjectDtl == null)
                    {
                        return retVal;
                    }
                    _ProjectDtl.Proj_isDeleted = true;
                    db.Entry(_ProjectDtl).State = System.Data.Entity.EntityState.Modified;
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

        #region To get all details of Project from Database
        public List<ProjectEntity> GetProjectDetail(int AccountId)
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                  
                    var query = (from q in db.Projects
                                 join a in db.Accounts on q.Proj_AccountID equals a.Acc_AccountID
                                 where q.Proj_isDeleted == false && q.Proj_AccountID == AccountId
                                 select new ProjectEntity
                                 {
                                     Proj_ProjectID = q.Proj_ProjectID,
                                     Proj_AccountID = q.Proj_AccountID,
                                     AccountName = a.Acc_AccountName,
                                     Proj_ProjectCode = q.Proj_ProjectCode,
                                     Proj_ProjectName = q.Proj_ProjectName,
                                     Proj_ProjectDescription = q.Proj_ProjectDescription,
                                     Proj_StartDate = q.Plan_StartDate,
                                     Proj_EndDate = q.Plan_EndDate,
                                     Proj_ActiveStatus = q.Proj_ActiveStatus,
                                     Proj_Version = q.Proj_Version,
                                     Proj_CreatedBy = q.Proj_CreatedBy,
                                     Proj_CreatedDate = q.Proj_CreatedDate,
                                     Proj_ModifiedBy = q.Proj_ModifiedBy,
                                     Proj_ModifiedDate = q.Proj_ModifiedDate,
                                     Proj_isDeleted = q.Proj_isDeleted,
                                     Is_Timesheet_ProjectSpecific= q.Is_Timesheet_ProjectSpecific,
                                 }).OrderBy(x => x.Proj_ProjectCode).ThenBy(x => x.Proj_ProjectName).ToList();

                    return query;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        #endregion

        #region To get particular Project details from Database
        public ProjectEntity GetProjectDetailByID(int ID)
        {
            ProjectEntity response = new ProjectEntity();

            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {

                    response = (from q in db.Projects
                                join a in db.Accounts on q.Proj_AccountID equals a.Acc_AccountID
                                where q.Proj_ProjectID == ID
                                select new ProjectEntity
                                {
                                    Proj_ProjectID = q.Proj_ProjectID,
                                    Proj_AccountID = q.Proj_AccountID,
                                    AccountName = a.Acc_AccountName,
                                    Proj_ProjectCode = q.Proj_ProjectCode,
                                    Proj_ProjectName = q.Proj_ProjectName,
                                    Proj_ProjectDescription = q.Proj_ProjectDescription,
                                    Proj_StartDate = q.Plan_StartDate,
                                    Proj_EndDate = q.Plan_EndDate,
                                    Proj_ActiveStatus = q.Proj_ActiveStatus,
                                    Proj_Version = q.Proj_Version,
                                    Proj_CreatedBy = q.Proj_CreatedBy,
                                    Proj_CreatedDate = q.Proj_CreatedDate,
                                    Proj_ModifiedBy = q.Proj_ModifiedBy,
                                    Proj_ModifiedDate = q.Proj_ModifiedDate,
                                    Proj_isDeleted = q.Proj_isDeleted,
                                    Is_Timesheet_ProjectSpecific= q.Is_Timesheet_ProjectSpecific,
                                }).FirstOrDefault();

                    response.IsSuccessful = true;
                    return response;
                }
                catch (Exception ex)
                {
                    response.IsSuccessful = false;
                    response.Message = "Error Occured in GetProjectDetailByID(ID)";
                    response.Detail = ex.Message.ToString();
                    return response;
                }
            }
        }
        #endregion

        #region To get Project details for select list
        public List<ProjectEntity> SelectProject()
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var query = (from q in db.Projects
                                 join a in db.Accounts on q.Proj_AccountID equals a.Acc_AccountID
                                 where q.Proj_isDeleted == false && q.Proj_ActiveStatus == true
                                 select new ProjectEntity
                                 {
                                     Proj_ProjectID = q.Proj_ProjectID,
                                     Proj_AccountID = q.Proj_AccountID,
                                     AccountName = a.Acc_AccountName,
                                     Proj_ProjectCode = q.Proj_ProjectCode,
                                     Proj_ProjectName = q.Proj_ProjectName,
                                     Proj_ProjectDescription = q.Proj_ProjectDescription,
                                     Proj_StartDate = q.Plan_StartDate,
                                     Proj_EndDate = q.Plan_EndDate,
                                     Proj_ActiveStatus = q.Proj_ActiveStatus,
                                     Proj_Version = q.Proj_Version,
                                     Proj_CreatedBy = q.Proj_CreatedBy,
                                     Proj_CreatedDate = q.Proj_CreatedDate,
                                     Proj_ModifiedBy = q.Proj_ModifiedBy,
                                     Proj_ModifiedDate = q.Proj_ModifiedDate,
                                     Proj_isDeleted = q.Proj_isDeleted,
                                     Is_Timesheet_ProjectSpecific= q.Is_Timesheet_ProjectSpecific,
                                 }).OrderBy(x => x.Proj_ProjectName).ToList();

                    return query;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        #endregion

        #region Get Max ProjectID Details
        public Project GetMaxProjectIDDetials()
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var ProjectMaxEntry = db.Projects.OrderByDescending(x => x.Proj_ProjectID).FirstOrDefault();
                    return ProjectMaxEntry;
                }

                catch (Exception Exception)
                {
                    return null;
                }
            }
        }

        #endregion

        //History

        #region To get particular Project details from Database
        public List<History_ProjectsEntity> GetHistoryProjectDetailsByID(int ID)
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var query = (from q in db.History_Projects
                                 join a in db.Accounts on q.History_Proj_AccountID equals a.Acc_AccountID
                                 where q.History_Proj_ProjectID == ID
                                 select new History_ProjectsEntity
                                 {
                                     History_Project_ID = q.History_Project_ID,
                                     History_Proj_ProjectID = q.History_Proj_ProjectID,
                                     History_Proj_AccountID = q.History_Proj_AccountID,
                                     AccountName = a.Acc_AccountName,
                                     History_Proj_ProjectCode = q.History_Proj_ProjectCode,
                                     History_Proj_ProjectName = q.History_Proj_ProjectName,
                                     History_Proj_ProjectDescription = q.History_Proj_ProjectDescription,
                                     History_Proj_StartDate = q.History_Proj_StartDate,
                                     History_Proj_EndDate = q.History_Proj_EndDate,
                                     History_Proj_ActiveStatus = q.History_Proj_ActiveStatus,
                                     History_Proj_Version = q.History_Proj_Version,
                                     History_Proj_CreatedBy = q.History_Proj_CreatedBy,
                                     History_Proj_CreatedDate = q.History_Proj_CreatedDate,
                                     History_Proj_ModifiedBy = q.History_Proj_ModifiedBy,
                                     History_Proj_ModifiedDate = q.History_Proj_ModifiedDate,
                                     History_Proj_isDeleted = q.History_Proj_isDeleted,
                                 }).OrderBy(x => x.History_Proj_ProjectName).ToList();

                    return query;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        #endregion


        #region GetUserNames

        public List<ProjectAllocationEntity> GetUserNames()
        {
            List<ProjectAllocationEntity> lstUsers = new List<ProjectAllocationEntity>();

            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {

                    lstUsers = (from u in db.Users
                                select new ProjectAllocationEntity
                                {
                                    UProj_UserID = u.Usr_UserID,
                                    Username = u.Usr_Username

                                }).ToList();

                    lstUsers.Add(new ProjectAllocationEntity
                    {
                        UProj_UserID = 0,
                        Username = "Select User Name"
                    });

                    lstUsers = lstUsers.OrderBy(p => p.UProj_UserID).ToList();

                    return lstUsers;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        #endregion

        public List<ProjectEntity> GetProjectNames()
        {

            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var query = (from u in db.Projects
                                 select new ProjectEntity
                                 {
                                     Proj_ProjectID = u.Proj_ProjectID,
                                     Proj_ProjectName = u.Proj_ProjectName

                                 }).ToList();

                    return query;


                }
                catch (Exception ex)
                {
                    return null;
                }

            }
        }

        #region GetUserRoleNames

        public List<RoleEntity> GetUserRoles()
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var query = (from u in db.Roles
                                 join gr in db.GenericRoles on u.Rol_RoleName equals gr.GenericRoleID
                                 select new RoleEntity
                                 {
                                     Rol_RoleID = u.Rol_RoleID,
                                     Rol_RoleName = gr.Title

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

        public string ChangeStatus(string id, string status)
        {
            string strResponse = string.Empty;
            Project holidayData = null;
            bool Status = Convert.ToBoolean(status);
            int did = Convert.ToInt32(id);
            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    holidayData = db.Set<Project>().Where(s => s.Proj_ProjectID == did).FirstOrDefault<Project>();
                    if (holidayData == null)
                    {
                        return null;
                    }
                    holidayData.Proj_ActiveStatus = Status;
                    // holidayData.isActive = false;
                    db.Entry(holidayData).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    strResponse = "Status Changed Successfully";
                }
                catch (Exception ex)
                {
                    strResponse = ex.Message.ToString();
                }
            }
            return strResponse;
        }
    }
}

