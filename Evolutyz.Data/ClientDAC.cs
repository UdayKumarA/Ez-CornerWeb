using Evolutyz.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Evolutyz.Data
{
    public class ClientDAC : DataAccessComponent
    {
        
        #region GetcountryNamesForDropdown

        public List<ProjectEntity> GetCountryNames()
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var countryNames = (from c in db.Countries
                                        where c.StatusId == true
                                        select new ProjectEntity
                                        {
                                            CountryId = c.CountryId,
                                            CountryName = c.CountryName

                                        }).ToList();

                    //countryNames.Add(new ProjectEntity
                    //{
                    //    CountryId = 0,
                    //    CountryName = "Select Countries"
                    //});
                    return countryNames;
                }
                catch (Exception ex)
                {
                    return null;

                }

            }

        }
        #endregion


        public List<ProjectEntity> GetTimeSheetModes(int AccountId)
        {

            using (var db = new EvolutyzCornerDataEntities())
            {
                bool OrgCheck = db.Set<Account>().Where(s => s.Acc_AccountID == AccountId).FirstOrDefault<Account>().is_UsAccount;
                if (OrgCheck== true)
                {
                    try
                    {
                        var timesheetmodes = (from c in db.Master_TimesheetMode
                                              where c.TimesheetMode_id != 3
                                              select new ProjectEntity
                                              {
                                                  TimesheetMode_id = c.TimesheetMode_id,
                                                  TimeModeName = c.TimeModeName

                                              }).ToList();


                        return timesheetmodes;
                    }
                    catch (Exception ex)
                    {
                        return null;

                    }
                }
                else
                {
                    try
                    {
                        var timesheetmodes = (from c in db.Master_TimesheetMode

                                              select new ProjectEntity
                                              {
                                                  TimesheetMode_id = c.TimesheetMode_id,
                                                  TimeModeName = c.TimeModeName

                                              }).ToList();


                        return timesheetmodes;
                    }
                    catch (Exception ex)
                    {
                        return null;

                    }
                }
                

            }

        }


        #region GetStateNamesForDropdown

        public List<ProjectEntity> GetStateNames(int CountryId)
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var StateNames = (from s in db.States
                                      where s.Countryid == CountryId && s.StatusId == true
                                      select new ProjectEntity
                                      {
                                          StateId = s.StateId,
                                          StateName = s.StateName

                                      }).ToList();

                    //StateNames.Add(new ProjectEntity
                    //{
                    //     StateId = 0,
                    //    StateName= "Select TaskName"
                    //});
                    return StateNames;
                }
                catch (Exception ex)
                {
                    return null;

                }

            }

        }
        #endregion

        #region To add Project Detail in Database

        public ProjectEntity AddProject(ProjectEntity _project)
        {
            int Proj_ProjectID;
            int retVal = 0;
            ClientDAC dac = new ClientDAC();
            Project Project = new Project();
            ProjectEntity response = new ProjectEntity();
            response.Proj_ProjectID = 0;

            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    Project = db.Set<Project>().Where(s => (s.Proj_ProjectName == _project.Proj_ProjectName&& s.Proj_AccountID== _project.Proj_AccountID)).FirstOrDefault<Project>();
                    if (Project != null)
                    {
                        return response;
                    }

                    db.Set<Project>().Add(new Project
                    {
                        Proj_AccountID = _project.Proj_AccountID,
                        Proj_ProjectCode = _project.Proj_ProjectCode,
                        Proj_ProjectName = _project.Proj_ProjectName,
                        CountryID = _project.CountryId,
                        StateID = _project.StateId,
                        WebUrl = _project.WebUrl,
                        Proj_ProjectDescription = _project.Proj_ProjectDescription,
                         Plan_StartDate = System.DateTime.Now,
                         Plan_EndDate = System.DateTime.Now,
                        Proj_ActiveStatus = _project.Proj_ActiveStatus,
                        Proj_Version = _project.Proj_Version,
                        Is_Timesheet_ProjectSpecific= _project.Is_Timesheet_ProjectSpecific,
                        Proj_CreatedBy = _project.Proj_CreatedBy,
                        Proj_CreatedDate = System.DateTime.Now,
                        Proj_isDeleted = false
                    });

                    retVal = db.SaveChanges();

                    Proj_ProjectID = db.Set<Project>().OrderByDescending(p => p.Proj_ProjectID).Select(p => p.Proj_ProjectID).FirstOrDefault();
                    _project.Proj_ProjectID = Proj_ProjectID;
                    response.Message = "Success";
                }
                catch (Exception ex)
                {
                    retVal = -1;
                    response.Proj_ProjectID = -1;
                }
            }
            return _project;
        }
        #endregion

        public ProjectAllocationEntities GetClientProjbyid(int ID)
        {
            // int projectid = Convert.ToInt32(proid);
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    //var response = (from u in db.Users
                    //                join a in db.Accounts on u.Usr_AccountID equals a.Acc_AccountID
                    //                join up in db.UsersProfiles on u.Usr_UserID equals up.UsrP_UserID
                    //                join uf in db.UserProjects on u.Usr_UserID equals uf.UProj_UserID
                    //                join p in db.Projects on uf.UProj_ProjectID equals p.Proj_ProjectID
                    //                join r in db.Roles on u.Usr_RoleID equals r.Rol_RoleID

                    var response = (from u in db.ClientProjects
                                    where u.CL_ProjectID == ID 
                                    select new ProjectAllocationEntities
                                    {
                                        // AccountID = u.Accountid;
                                        Accid = u.Accountid,
                                        ProjectID = u.Proj_ProjectID,

                                        Cl_projid = u.CL_ProjectID,
                                        clientprojecttitle = u.ClientProjTitle,
                                        clientprojectdescription = u.ClientProjDesc

                                    }).FirstOrDefault();
                    return response;
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }
        public string updateuserdetails(ClientEntity orgDtl)
        {
            User userdtl = null;
            string response = string.Empty;
            UserSessionInfo objinfo = new UserSessionInfo();
            var accountid = objinfo.AccountId;
            int rolename = Convert.ToInt32(orgDtl.RoleName);
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    userdtl = db.Set<User>().Where(s => s.Usr_UserID == orgDtl.Usr_UserID).FirstOrDefault<User>();
                   
                    string password = db.Set<User>().Where(r => (r.Usr_UserID == orgDtl.Usr_UserID)).FirstOrDefault<User>().Usr_Password;
                    if (userdtl == null)
                    {
                        response = "record not updated";
                        return null;
                    }
                    if (orgDtl.Usr_Password == "")
                    {
                        password = password;
                    }
                    else
                    {
                        password = orgDtl.Usr_Password;
                    }
                    if (orgDtl.Usrp_ProfilePicture == "undefined")
                    {
                        //int roleid = db.Set<Role>().Where(r => (r.Rol_AccountID == accountid && r.Rol_RoleName == rolename)).FirstOrDefault<Role>().Rol_RoleID;

                        User u = new User();

                        userdtl.Usr_AccountID = accountid;
                        userdtl.Usr_UserTypeID = orgDtl.Usr_UserTypeID;
                        userdtl.Usr_RoleID = rolename;
                        userdtl.Usr_Username = orgDtl.Usr_Username;
                        userdtl.Usr_LoginId = orgDtl.Email.Trim();
                        userdtl.Usr_Password = password;
                        userdtl.Usr_ActiveStatus = orgDtl.UProj_ActiveStatus;
                        userdtl.Usr_TaskID = orgDtl.Usr_TaskID;
                        userdtl.Usr_CreatedBy = orgDtl.UProj_CreatedBy;
                        userdtl.Usr_CreatedDate = DateTime.Now;
                        userdtl.Usr_Version = 1;
                        userdtl.Usr_isDeleted = false;
                        db.Entry(userdtl).State = System.Data.Entity.EntityState.Modified;
                        //db.Set<User>().Add(u);
                        db.SaveChanges();

                        int userid = u.Usr_UserID;

                        UsersProfile userprofiledtl = new UsersProfile();
                        userprofiledtl = db.Set<UsersProfile>().Where(s => s.UsrP_UserID == orgDtl.Usr_UserID).FirstOrDefault<UsersProfile>();
                        //userprofiledtl.UsrP_UserID = orgDtl.Usr_UserID;
                        userprofiledtl.UsrP_FirstName = orgDtl.UsrP_FirstName;
                        userprofiledtl.UsrP_LastName = orgDtl.UsrP_LastName;
                        userprofiledtl.UsrP_EmailID = orgDtl.Email.Trim();
                        //userprofiledtl.Usrp_ProfilePicture = orgDtl.Usrp_ProfilePicture;
                        userprofiledtl.Usrp_DOJ = orgDtl.Usrp_DOJ;
                        userprofiledtl.UsrP_EmployeeID = orgDtl.UsrP_EmployeeID;
                        userprofiledtl.Usrp_MobileNumber = orgDtl.Usrp_MobileNumber;
                        userprofiledtl.Usrp_CountryCode = orgDtl.Usrp_CountryCode;
                        userprofiledtl.Usr_Titleid = orgDtl.Usr_Titleid;
                        userprofiledtl.Usr_GenderId = orgDtl.Usr_GenderId;
                        userprofiledtl.UsrP_CreatedBy = orgDtl.UProj_CreatedBy;
                        userprofiledtl.UsrP_CreatedDate = DateTime.Now;
                        userprofiledtl.UsrP_ActiveStatus = orgDtl.UProj_ActiveStatus;
                        userprofiledtl.UsrP_Version = 1;
                        db.Entry(userprofiledtl).State = System.Data.Entity.EntityState.Modified;
                        // db.Set<UsersProfile>().Add(uf);
                        db.SaveChanges();

                        UserProject userprojectdtl = new UserProject();
                        userprojectdtl = db.Set<UserProject>().Where(s => (s.UProj_UserID == orgDtl.Usr_UserID &&s.UProj_ProjectID==orgDtl.Proj_ProjectID)).FirstOrDefault<UserProject>();
                        //userprojectdtl.UProj_UserID = userid;
                        userprojectdtl.UProj_ProjectID = orgDtl.Proj_ProjectID;
                        userprojectdtl.ClientprojID = orgDtl.CL_ProjectID;
                        userprojectdtl.UProj_L1_ManagerId = orgDtl.ManagerName;
                        userprojectdtl.UProj_L2_ManagerId = orgDtl.Managername2;
                        userprojectdtl.UProj_StartDate = System.DateTime.Now; ;
                        userprojectdtl.UProj_EndDate = System.DateTime.Now; ;
                        userprojectdtl.UProj_ParticipationPercentage = orgDtl.UProj_ParticipationPercentage;
                        userprojectdtl.UProj_ActiveStatus = orgDtl.UProj_ActiveStatus;
                        userprojectdtl.UProj_CreatedBy = orgDtl.UProj_CreatedBy;
                        userprojectdtl.UProj_CreatedDate = DateTime.Now;
                        userprojectdtl.UProj_Version = 1;
                        userprojectdtl.UProj_isDeleted = false;
                        userprojectdtl.TimesheetMode = orgDtl.TimesheetMode_id;
                        //userprojectdtl.Is_L1_Manager = true;
                        //db.Set<UserProject>().Add(up);
                        db.Entry(userprojectdtl).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        response = "Successfully updated";
                    }
                    else
                    {
                        //int roleid = db.Set<Role>().Where(r => (r.Rol_AccountID == accountid && r.Rol_RoleName == rolename)).FirstOrDefault<Role>().Rol_RoleID;

                        User u = new User();
                        userdtl.Usr_AccountID = accountid;
                        userdtl.Usr_UserTypeID = orgDtl.Usr_UserTypeID;
                        userdtl.Usr_RoleID = rolename;
                        userdtl.Usr_Username = orgDtl.Email.Trim();
                        userdtl.Usr_LoginId = orgDtl.Email.Trim();
                        userdtl.Usr_Password = password;
                        userdtl.Usr_ActiveStatus = orgDtl.UProj_ActiveStatus;
                        userdtl.Usr_TaskID = orgDtl.Usr_TaskID;
                        userdtl.Usr_CreatedBy = orgDtl.UProj_CreatedBy;
                        userdtl.Usr_CreatedDate = DateTime.Now;
                        userdtl.Usr_Version = 1;
                        userdtl.Usr_isDeleted = false;
                        db.Entry(userdtl).State = System.Data.Entity.EntityState.Modified;
                        //db.Set<User>().Add(u);
                        db.SaveChanges();

                        int userid = u.Usr_UserID;

                        UsersProfile userprofiledtl = new UsersProfile();
                        userprofiledtl = db.Set<UsersProfile>().Where(s => s.UsrP_UserID == orgDtl.Usr_UserID).FirstOrDefault<UsersProfile>();
                        //userprofiledtl.UsrP_UserID = userid;
                        userprofiledtl.UsrP_FirstName = orgDtl.UsrP_FirstName;
                        userprofiledtl.UsrP_LastName = orgDtl.UsrP_LastName;
                        userprofiledtl.UsrP_EmailID = orgDtl.Email.Trim();
                        userprofiledtl.Usrp_ProfilePicture = orgDtl.Usrp_ProfilePicture;
                        userprofiledtl.Usrp_DOJ = DateTime.Now;
                        userprofiledtl.UsrP_EmployeeID = orgDtl.UsrP_EmployeeID;
                        userprofiledtl.Usrp_MobileNumber = orgDtl.Usrp_MobileNumber;
                        userprofiledtl.Usrp_CountryCode = orgDtl.Usrp_CountryCode;
                        userprofiledtl.Usr_Titleid = orgDtl.Usr_Titleid;
                        userprofiledtl.Usr_GenderId = orgDtl.Usr_GenderId;
                        userprofiledtl.UsrP_CreatedBy = orgDtl.UProj_CreatedBy;
                        userprofiledtl.UsrP_CreatedDate = DateTime.Now;
                        userprofiledtl.UsrP_ActiveStatus = orgDtl.UProj_ActiveStatus;
                        userprofiledtl.UsrP_Version = 1;
                        db.Entry(userprofiledtl).State = System.Data.Entity.EntityState.Modified;
                        // db.Set<UsersProfile>().Add(uf);
                        db.SaveChanges();

                        UserProject userprojectdtl = new UserProject();
                        userprojectdtl = db.Set<UserProject>().Where(s => s.UProj_UserID == orgDtl.Usr_UserID).FirstOrDefault<UserProject>();
                        //userprojectdtl.UProj_UserID = userid;
                        userprojectdtl.UProj_ProjectID = orgDtl.Proj_ProjectID;
                        userprojectdtl.ClientprojID = orgDtl.CL_ProjectID;
                        userprojectdtl.UProj_L1_ManagerId = orgDtl.ManagerName;
                        userprojectdtl.UProj_L2_ManagerId = orgDtl.Managername2;
                        userprojectdtl.UProj_StartDate = System.DateTime.Now;
                        userprojectdtl.UProj_EndDate = System.DateTime.Now;
                        userprojectdtl.UProj_ParticipationPercentage = orgDtl.UProj_ParticipationPercentage;
                        userprojectdtl.UProj_ActiveStatus = orgDtl.UProj_ActiveStatus;
                        userprojectdtl.UProj_CreatedBy = orgDtl.UProj_CreatedBy;
                        userprojectdtl.UProj_CreatedDate = DateTime.Now;
                        userprojectdtl.UProj_Version = 1;
                        userprojectdtl.UProj_isDeleted = false;
                        userprojectdtl.TimesheetMode = orgDtl.TimesheetMode_id;
                        //userprojectdtl.Is_L1_Manager = true;
                        db.Entry(userprojectdtl).State = System.Data.Entity.EntityState.Modified;
                        //db.Set<UserProject>().Add(up);
                        db.SaveChanges();

                        response = "Successfully updated";
                    }

                    // Role roleid = new Role();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return response;
        }


        #region To update existing client Detail in Database
        public int UpdateclientDetails(ProjectEntity project)
        {
            Project projDTl = null;
            // History_UserType _userTypeHistory = new History_UserType();

            int retVal = 0;

            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    projDTl = db.Set<Project>().Where(s => s.Proj_ProjectID == project.Proj_ProjectID).FirstOrDefault<Project>();

                    if (project == null)
                    {
                        return retVal;
                    }
      

                    // projDTl.Proj_AccountID = project.Proj_AccountID;
                   // projDTl.Proj_ProjectCode = project.Proj_ProjectCode;
                    projDTl.Proj_ProjectName = project.Proj_ProjectName;
                    projDTl.CountryID = project.CountryId;
                    projDTl.StateID = project.StateId;
                    projDTl.WebUrl = project.WebUrl;
                    projDTl.Proj_ProjectDescription = project.Proj_ProjectDescription;
                    projDTl.Plan_StartDate =System.DateTime.Now;
                    projDTl.Plan_EndDate = System.DateTime.Now;
                    projDTl.Proj_ActiveStatus = project.Proj_ActiveStatus;
                    projDTl.Proj_Version = project.Proj_Version;
                    projDTl.Proj_CreatedBy = project.Proj_CreatedBy;
                    projDTl.Proj_CreatedDate = System.DateTime.Now;
                    projDTl.Is_Timesheet_ProjectSpecific = project.Is_Timesheet_ProjectSpecific;
                    projDTl.Proj_ModifiedDate = System.DateTime.Now;
                    projDTl.Proj_ModifiedBy = project.Proj_ModifiedBy;
                    projDTl.Proj_isDeleted = project.Proj_isDeleted;
                  
                    db.Entry(projDTl).State = System.Data.Entity.EntityState.Modified;
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
        #region To add ProjectAllocation Detail in Database
        public int AddManager(ProjectAllocationEntity _ProjectAllocation)
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
                        //Is_L2_Manager = _ProjectAllocation.Is_L2_Manager,
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
        public string passcodesequence()
        {

            // conn.ConnectionString = "Data Source=DESKTOP-7AA2POF;Initial Catalog=EvolutyzCorner_Developer;User ID=sa;Password=A12#abhi";

            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(constr);
            conn.Open();

            // SqlCommand cmd = new SqlCommand("select max(AqId) FROM [AcademicQuestionPaper]", conn);
            SqlCommand cmd = new SqlCommand("select NEXT VALUE FOR [dbo].[CreatePasscode]", conn);
            string k = cmd.ExecuteScalar().ToString();
            //int k=cmd.ExecuteNonQuery();
            // k = k + 1;
            return k;

        }
        #region To get particular Project details from Database
        public ProjectEntity GetClientDetailByID(int ID)
        {
            ProjectEntity response = new ProjectEntity();

            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {

                    response = (from q in db.Projects
                                join a in db.Accounts on q.Proj_AccountID equals a.Acc_AccountID
                                where q.Proj_ProjectID == ID && q.Proj_isDeleted== false
                                select new ProjectEntity
                                {
                                    Proj_ProjectID = q.Proj_ProjectID,
                                    Proj_AccountID = q.Proj_AccountID,
                                    AccountName = a.Acc_AccountName,
                                    CountryId = q.CountryID,
                                    StateId = q.StateID,
                                    WebUrl = q.WebUrl,
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
                                    Is_Timesheet_ProjectSpecific= q.Is_Timesheet_ProjectSpecific
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

        public List<OrganizationAccountEntity> GetRoleNames(int AccountId)
        {
            try
            {
                using (var db = new EvolutyzCornerDataEntities())
                {

                    var query = (from UT in db.GenericRoles
                                 join r in db.Roles on UT.GenericRoleID equals r.Rol_RoleName
                                 where UT.GenericRoleID == 1007 && r.Rol_AccountID == AccountId
                                 select new OrganizationAccountEntity
                                 {
                                     GenericRoleID = r.Rol_RoleID,
                                     Title = r.Rol_RoleDescription
                                 }).ToList();
                    return query;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<OrganizationAccountEntity> GetAllRoles()
        {
            UserSessionInfo objinfo = new UserSessionInfo();
            int accid = objinfo.AccountId;
            try
            {
                using (var db = new EvolutyzCornerDataEntities())
                {

                    var query = (from UT in db.Roles
                                 join gr in db.GenericRoles on UT.Rol_RoleName equals gr.GenericRoleID
                                 where UT.Rol_AccountID == accid && UT.Rol_isDeleted==false

                                 select new OrganizationAccountEntity
                                 {
                                     Title = UT.Rol_RoleDescription,
                                     GenericRoleID = UT.Rol_RoleID
                                 }).ToList();
                    return query;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<ClientEntity> getclientprojects(int projid)
        {
            UserSessionInfo objinfo = new UserSessionInfo();
            int accid = objinfo.AccountId;
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {

                  var  OBJ = (from q in db.ClientProjects
                           join a in db.Accounts on q.Accountid equals a.Acc_AccountID
                            where q.Accountid == accid && q.Proj_ProjectID== projid 
                              select new ClientEntity
                           {
                                  CL_ProjectID=q.CL_ProjectID,
                                  ProjectID = q.Proj_ProjectID,
                               Accountid = q.Accountid,
                               ClientProjTitle = q.ClientProjTitle,
                               ClientProjDesc = q.ClientProjDesc
                           }).ToList();

                    return OBJ;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public List<ClientEntity> getclientprojectsdropdown(int projid)
        {
            UserSessionInfo objinfo = new UserSessionInfo();
            int accid = objinfo.AccountId;
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {

                    var OBJ = (from q in db.ClientProjects
                               join a in db.Accounts on q.Accountid equals a.Acc_AccountID
                               where q.Accountid == accid && q.Proj_ProjectID == projid
                               select new ClientEntity
                               {
                                   CL_ProjectID = q.CL_ProjectID,
                                   ProjectID = q.Proj_ProjectID,
                                   Accountid = q.Accountid,
                                   ClientProjTitle = q.ClientProjTitle,
                                   ClientProjDesc = q.ClientProjDesc
                               }).ToList();

                    return OBJ;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        
        public List<ProjectAllocationEntities> GetAlltasknames(int projectid,int Roleid)
        {
            UserSessionInfo info = new UserSessionInfo();

            int accid = info.AccountId;
          //  int roleid = info.RoleId;
            try
            {
                using (var db = new EvolutyzCornerDataEntities())
                {
                   


                    var roleid = db.Roles.Where(d => d.Rol_RoleID == Roleid).FirstOrDefault().Rol_RoleName;

                    var query = (
                        from UT in db.ClientProjectsTasks
                        join AT in db.AccountSpecificTasks on UT.acc_specifictaskid equals AT.Acc_SpecificTaskId
                        where UT.Accountid== accid && UT.rol_roleid== roleid 
                        

                        select new ProjectAllocationEntities
                                 {
                                     Proj_SpecificTaskId = AT.Acc_SpecificTaskId,
                                     Proj_SpecificTaskName = AT.Acc_SpecificTaskName
                                 }).ToList();
                    return query;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<ProjectAllocationEntities> gettasknames(int projectid)
        {
            UserSessionInfo info = new UserSessionInfo();

            int accid = info.AccountId;
            //  int roleid = info.RoleId;
            try
            {
                using (var db = new EvolutyzCornerDataEntities())
                {



                    //var roleid = db.Roles.Where(d => d.Rol_RoleID == Roleid).FirstOrDefault().Rol_RoleName;

                    var query = (
                        from UT in db.ClientProjectsTasks
                        join AT in db.AccountSpecificTasks on UT.acc_specifictaskid equals AT.Acc_SpecificTaskId
                        where UT.Accountid == accid // && UT.rol_roleid == roleid 


                        select new ProjectAllocationEntities
                        {
                            Proj_SpecificTaskId = AT.Acc_SpecificTaskId,
                            Proj_SpecificTaskName = AT.Acc_SpecificTaskName
                        }).ToList();
                    return query;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }


        public List<OrganizationAccountEntity> GetRoleNamesbyemp(int AccountId)
        {
            try
            {
                using (var db = new EvolutyzCornerDataEntities())
                {

                    var query = (from UT in db.GenericRoles
                                 join r in db.Roles on UT.GenericRoleID equals r.Rol_RoleName
                                 where UT.GenericRoleID != 1007 && r.Rol_AccountID == AccountId && UT.GenericRoleID != 1002 
                                 select new OrganizationAccountEntity
                                 {
                                     GenericRoleID = r.Rol_RoleID,
                                     Title = r.Rol_RoleDescription
                                 }).ToList();

                    //var query = (from UT in db.GenericRoles
                    //             join r in db.Roles on UT.GenericRoleID equals r.Rol_RoleName
                    //             where UT.GenericRoleID != 1007 && r.Rol_AccountID == AccountId
                    //             select new OrganizationAccountEntity
                    //             {
                    //                 GenericRoleID = UT.GenericRoleID,
                    //                 Title = UT.Title
                    //             }).ToList();
                    //return query;
                    return query;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        #region GetManagerNames

        public List<UserEntity> GetManagerNames(int projid)
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var ManagerNames = (from UPF in db.UsersProfiles
                                        join UP in db.UserProjects on UPF.UsrP_UserID equals UP.UProj_UserID
                                        where UP.Is_L1_Manager == true && UP.UProj_ProjectID == projid && UPF.UsrP_isDeleted== false
                                        select new UserEntity
                                        {
                                            Usr_UserID = UPF.UsrP_UserID,
                                            UsrP_FirstName = UPF.UsrP_FirstName + "" + UPF.UsrP_LastName,


                                        }).ToList();
                    ManagerNames.Add(new UserEntity
                    {
                       
                        UsrP_FirstName = "Select Manager"
                    });
                    ManagerNames.OrderBy(p => p.Usr_UserID).ToList();
                    //taskNames.Add(new UserEntity
                    //{
                    //    Usr_TaskID=0,
                    //    Taskname = "Select TaskName"
                    //});
                    return ManagerNames;
                }
                catch (Exception ex)
                {
                    return null;

                }

            }

        }
        #endregion

        #region GetManagerBYRolesNames

        public List<UserEntity> ManagerByRole(int projid, int userid, int AccountId)
        {



            using (var db = new EvolutyzCornerDataEntities())
            {
                List<UserEntity> ManagerNames = new List<UserEntity>();
                try
                {
                    dynamic Managers;
                    int getRolename = (from u in db.Users
                                       join r in db.Roles on u.Usr_RoleID equals r.Rol_RoleID
                                       join gr in db.GenericRoles on r.Rol_RoleName equals gr.GenericRoleID
                                       where u.Usr_UserID == userid && u.Usr_AccountID == AccountId && u.Usr_isDeleted== false
                                       select gr.GenericRoleID).FirstOrDefault();

                    if (getRolename == 1007)
                    {
                        Managers = (from UPF in db.UsersProfiles
                                    join UP in db.UserProjects on UPF.UsrP_UserID equals UP.UProj_UserID
                                    where UP.Is_L1_Manager == true && UP.UProj_ProjectID == projid && UPF.UsrP_UserID != userid && UPF.UsrP_isDeleted == false
                                    select new UserEntity
                                    {
                                        Usr_UserID = UPF.UsrP_UserID,
                                        UsrP_FirstName = UPF.UsrP_FirstName + "" + UPF.UsrP_LastName,

                                    }).ToList();
                    }
                    else
                    {
                        Managers = (from UPF in db.UsersProfiles
                                    join UP in db.UserProjects on UPF.UsrP_UserID equals UP.UProj_UserID
                                    where UP.Is_L1_Manager == true && UP.UProj_ProjectID == projid && UPF.UsrP_isDeleted == false
                                    select new UserEntity
                                    {
                                        Usr_UserID = UPF.UsrP_UserID,
                                        UsrP_FirstName = UPF.UsrP_FirstName + "" + UPF.UsrP_LastName,

                                    }).ToList();
                    }
                    //Managers.Add(new UserEntity
                    //{
                       
                    //    UsrP_FirstName = "Select Manager"
                    //});
                    ManagerNames = Managers;

                    ManagerNames = ManagerNames.OrderBy(p => p.Usr_UserID).ToList();



                    //ManagerNames.Add(new UserEntity
                    //{
                    //    Usr_UserID = 0,
                    //    UsrP_FirstName = "Select Manager"
                    //});

                    //taskNames.Add(new UserEntity
                    //{
                    //    Usr_TaskID=0,
                    //    Taskname = "Select TaskName"
                    //});
                    return ManagerNames;
                    //taskNames.Add(new UserEntity
                    //{
                    //    Usr_TaskID=0,
                    //    Taskname = "Select TaskName"
                    //});

                }
                catch (Exception ex)
                {
                    return null;

                }

            }

        }
        #endregion

        #region GetManageroncChange

        public List<UserEntity> GetManagerOnChange(int projid, int ManagerID)
        {

            using (var db = new EvolutyzCornerDataEntities())
            {
                List<UserEntity> ManagerNames = new List<UserEntity>();
                try
                {
                    ManagerNames = (from UPF in db.UsersProfiles
                                    join UP in db.UserProjects on UPF.UsrP_UserID equals UP.UProj_UserID
                                    where UP.Is_L1_Manager == true && UP.UProj_ProjectID == projid && UPF.UsrP_UserID != ManagerID && UPF.UsrP_isDeleted == false
                                    select new UserEntity
                                    {
                                        Usr_UserID = UPF.UsrP_UserID,
                                        UsrP_FirstName = UPF.UsrP_FirstName + "" + UPF.UsrP_LastName,

                                    }).ToList();
                    ManagerNames.Add(new UserEntity
                    {
                       
                        UsrP_FirstName = "Select Manager"
                    });

                    ManagerNames = ManagerNames.OrderBy(p => p.Usr_UserID).ToList();


                    return ManagerNames;


                }
                catch (Exception ex)
                {
                    return null;

                }

            }

        }
        #endregion

        #region GetManagerNames

        public List<UserEntity> GetManagerNames2(int projid)
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {

                    var ManagerNames = (from UPF in db.UsersProfiles
                                        join UP in db.UserProjects on UPF.UsrP_UserID equals UP.UProj_UserID
                                        where UP.Is_L1_Manager == true && UP.UProj_ProjectID == projid && UPF.UsrP_isDeleted == false
                                        select new UserEntity
                                        {
                                            Usr_UserID = UPF.UsrP_UserID,
                                            UsrP_FirstName = UPF.UsrP_FirstName + "" + UPF.UsrP_LastName,

                                        }).ToList();
                    //ManagerNames.Add(new UserEntity
                    //{
                    //    //Usr_UserID = ,
                    //    UsrP_FirstName = "Select Manager"
                    //});
                    ManagerNames = ManagerNames.OrderBy(p => p.Usr_UserID).ToList();
                    //ManagerNames.Add(new UserEntity
                    //{
                    //    Usr_UserID = 0,
                    //    UsrP_FirstName = "Select Manager"
                    //});

                    
                    return ManagerNames;
                  

                }
                catch (Exception ex)
                {
                    return null;

                }

            }

        }
        #endregion

        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = System.Text.Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                //lower  
                byte2String += targetData[i].ToString("x2");
                //upper  
                //byte2String += targetData[i].ToString("X2");  
            }
            return byte2String;
        }

        public string Savemanager(ClientEntity orgDtl)
        {
            string response = string.Empty;
            UserSessionInfo objinfo = new UserSessionInfo();
            var accountid = objinfo.AccountId;
            int rolename;
            bool Manager2;
            int? manager2 = orgDtl.Managername2;
            if (manager2==0)
            {
                Manager2 = false;
            }
            else
            {
                Manager2 = true;
            }
            rolename = Convert.ToInt32(orgDtl.RoleName);


            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                   var UserName = db.Set<User>().Where(s => s.Usr_Username == orgDtl.Usr_Username.Trim()).FirstOrDefault<User>();
                    var LoginId = db.Set<User>().Where(s => s.Usr_LoginId == orgDtl.Email.Trim()).FirstOrDefault<User>();
                    // Role roleid = new Role();
                    //int roleid = db.Set<Role>().Where(r => (r.Rol_AccountID == accountid && r.Rol_RoleName == rolename)).FirstOrDefault<Role>().Rol_RoleID;
                    if (UserName!= null)
                    {
                      return  response = "UserName already Exists";
                    }
                    else if (LoginId!= null)
                    {
                        return response = "Loginid already Exists";
                    }
                    User u = new User();
                    u.Usr_AccountID = accountid;
                    u.Usr_UserTypeID = orgDtl.Usr_UserTypeID;
                    u.Usr_RoleID = rolename;
                    u.Usr_Username = orgDtl.Usr_Username.Trim();
                    u.Usr_LoginId = orgDtl.Email.Trim();
                    u.Usr_Password = (orgDtl.Usr_Password);
                    u.Usr_ActiveStatus = orgDtl.UProj_ActiveStatus;
                    u.Usr_TaskID = orgDtl.Usr_TaskID;
                    u.Usr_CreatedBy = orgDtl.UProj_CreatedBy;
                    u.Usr_CreatedDate = DateTime.Now;
                    u.Usr_Version = 1;
                    u.Usr_isDeleted = false;

                    db.Set<User>().Add(u);
                    db.SaveChanges();

                    int userid = u.Usr_UserID;
                    string passcode = passcodesequence();
                    UsersProfile uf = new UsersProfile();
                    uf.UsrP_UserID = userid;
                    uf.UsrP_FirstName = orgDtl.UsrP_FirstName;
                    uf.UsrP_LastName = orgDtl.UsrP_LastName;
                    uf.UsrP_EmailID = orgDtl.Email.Trim();
                    uf.Usrp_ProfilePicture = orgDtl.Usrp_ProfilePicture;
                    uf.Usrp_DOJ = orgDtl.Usrp_DOJ;
                    uf.UsrP_EmployeeID = orgDtl.UsrP_EmployeeID;
                    uf.Usrp_MobileNumber = orgDtl.Usrp_MobileNumber;
                    uf.Usrp_CountryCode = orgDtl.Usrp_CountryCode;
                    uf.Usr_Titleid = orgDtl.Usr_Titleid;
                    uf.Usr_GenderId = orgDtl.Usr_GenderId;
                    uf.UsrP_CreatedBy = orgDtl.UProj_CreatedBy;
                    uf.UsrP_CreatedDate = DateTime.Now;
                    uf.UsrP_ActiveStatus = orgDtl.UProj_ActiveStatus;
                    uf.UsrP_Version = 1;
                    uf.passcode = passcode;
                    db.Set<UsersProfile>().Add(uf);
                    db.SaveChanges();
                    UserProject up = new UserProject();
                    up.UProj_UserID = userid;
                    up.UProj_ProjectID = orgDtl.Proj_ProjectID;
                    up.ClientprojID = orgDtl.CL_ProjectID;
                    up.UProj_L1_ManagerId = orgDtl.ManagerName;
                    up.UProj_L2_ManagerId = orgDtl.Managername2;
                    up.UProj_StartDate = System.DateTime.Now;
                    up.UProj_EndDate = System.DateTime.Now;
                    up.UProj_ParticipationPercentage = orgDtl.UProj_ParticipationPercentage;
                    up.UProj_ActiveStatus = orgDtl.UProj_ActiveStatus;
                    up.UProj_CreatedBy = orgDtl.UProj_CreatedBy;
                    up.UProj_CreatedDate = DateTime.Now;
                    up.UProj_Version = 1;
                    up.UProj_isDeleted = false;
                    up.Is_L1_Manager = true;
                    up.Is_L2_Manager = Manager2;
                    up.TimesheetMode = orgDtl.TimesheetMode_id;
                    db.Set<UserProject>().Add(up);
                    db.SaveChanges();

                    response = "Successfully Added";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return response;
        }

        public string SaveEmployee(ClientEntity orgDtl)
        {
            string response = string.Empty;
            UserSessionInfo objinfo = new UserSessionInfo();
            var accountid = objinfo.AccountId;
            int rolename = Convert.ToInt32(orgDtl.RoleName);
         
            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    // Role roleid = new Role();
                    //int roleid = db.Set<Role>().Where(r => (r.Rol_AccountID == accountid && r.Rol_RoleName == rolename)).FirstOrDefault<Role>().Rol_RoleID;
                    var UserName = db.Set<User>().Where(s => s.Usr_Username == orgDtl.Usr_Username.Trim()).FirstOrDefault<User>();
                    var LoginId = db.Set<User>().Where(s => s.Usr_LoginId == orgDtl.Email.Trim()).FirstOrDefault<User>();
                    // Role roleid = new Role();
                    //int roleid = db.Set<Role>().Where(r => (r.Rol_AccountID == accountid && r.Rol_RoleName == rolename)).FirstOrDefault<Role>().Rol_RoleID;
                    if (UserName != null)
                    {
                        return response = "UserName already Exists";
                    }
                    else if (LoginId != null)
                    {
                        return response = "Loginid already Exists";
                    }
                    User u = new User();
                    u.Usr_AccountID = accountid;
                    u.Usr_UserTypeID = orgDtl.Usr_UserTypeID;
                    u.Usr_RoleID = rolename;
                    u.Usr_Username = orgDtl.Usr_Username;
                    u.Usr_LoginId = orgDtl.Email.Trim();
                    u.Usr_Password = (orgDtl.Usr_Password);
                    u.Usr_ActiveStatus = orgDtl.UProj_ActiveStatus;
                    u.Usr_TaskID = orgDtl.Usr_TaskID;
                    u.Usr_CreatedBy = orgDtl.UProj_CreatedBy;
                    u.Usr_CreatedDate = DateTime.Now;
                    u.Usr_Version = 1;
                    u.Usr_isDeleted = false;

                    db.Set<User>().Add(u);
                    db.SaveChanges();

                    int userid = u.Usr_UserID;
                    string passcode = passcodesequence();
                    UsersProfile uf = new UsersProfile();
                    uf.UsrP_UserID = userid;
                    uf.UsrP_FirstName = orgDtl.UsrP_FirstName;
                    uf.UsrP_LastName = orgDtl.UsrP_LastName;
                    uf.UsrP_EmailID = orgDtl.Email.Trim();
                    uf.Usrp_ProfilePicture = orgDtl.Usrp_ProfilePicture;
                    uf.Usrp_DOJ = orgDtl.Usrp_DOJ;
                    uf.UsrP_EmployeeID = orgDtl.UsrP_EmployeeID;
                    uf.Usrp_MobileNumber = orgDtl.Usrp_MobileNumber;
                    uf.Usrp_CountryCode = orgDtl.Usrp_CountryCode;
                    uf.Usr_Titleid = orgDtl.Usr_Titleid;
                    uf.Usr_GenderId = orgDtl.Usr_GenderId;
                    uf.UsrP_CreatedBy = orgDtl.UProj_CreatedBy;
                    uf.UsrP_CreatedDate = DateTime.Now;
                    uf.UsrP_ActiveStatus = orgDtl.UProj_ActiveStatus;
                    uf.UsrP_Version = 1;
                    uf.passcode = passcode;
                    db.Set<UsersProfile>().Add(uf);
                    db.SaveChanges();
                    UserProject up = new UserProject();
                    up.UProj_UserID = userid;
                    up.UProj_ProjectID = orgDtl.Proj_ProjectID;
                    up.ClientprojID = orgDtl.CL_ProjectID;
                    up.UProj_L1_ManagerId = orgDtl.ManagerName;
                    up.UProj_L2_ManagerId = orgDtl.Managername2;
                    up.UProj_StartDate = System.DateTime.Now;
                    up.UProj_EndDate = System.DateTime.Now;
                    up.UProj_ParticipationPercentage = orgDtl.UProj_ParticipationPercentage;
                    up.UProj_ActiveStatus = orgDtl.UProj_ActiveStatus;
                    up.UProj_CreatedBy = orgDtl.UProj_CreatedBy;
                    up.UProj_CreatedDate = DateTime.Now;
                    up.UProj_Version = 1;
                    up.UProj_isDeleted = false;
                    up.Is_L1_Manager = false;
                    up.Is_L2_Manager = false;
                    up.TimesheetMode = orgDtl.TimesheetMode_id;
                    db.Set<UserProject>().Add(up);
                    db.SaveChanges();

                    response = "Successfully Added";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return response;
        }
        
        public string AssociateEmployee(ClientEntity orgDtl)
        {
            string response = string.Empty;
            UserSessionInfo objinfo = new UserSessionInfo();
            var accountid = objinfo.AccountId;
            int rolename = Convert.ToInt32(orgDtl.RoleName);
          
            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                   
                   
                    UserProject up = new UserProject();
                    up.UProj_UserID = orgDtl.Usr_UserID;
                    up.UProj_ProjectID = orgDtl.Proj_ProjectID;
                    up.UProj_L1_ManagerId = orgDtl.ManagerName;
                    up.UProj_L2_ManagerId = orgDtl.Managername2;
                    up.UProj_StartDate = orgDtl.UProj_StartDate;
                    up.UProj_EndDate = orgDtl.UProj_EndDate;
                    up.UProj_ParticipationPercentage = orgDtl.UProj_ParticipationPercentage;
                    up.UProj_ActiveStatus = orgDtl.UProj_ActiveStatus;
                    up.UProj_CreatedBy = orgDtl.UProj_CreatedBy;
                    up.UProj_CreatedDate = DateTime.Now;
                    up.UProj_Version = 1;
                    up.UProj_isDeleted = false;
                    up.Is_L1_Manager = false;
                    up.Is_L2_Manager = false;
                    db.Set<UserProject>().Add(up);
                    db.SaveChanges();

                    response = "Successfully Added";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return response;
        }

        public string AssociateManager(ClientEntity orgDtl)
        {
            string response = string.Empty;
            UserSessionInfo objinfo = new UserSessionInfo();
            var accountid = objinfo.AccountId;
            int rolename = Convert.ToInt32(orgDtl.RoleName);

            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    if (orgDtl.managertype == 1)
                    {
                        UserProject up = new UserProject();
                        up.UProj_UserID = orgDtl.Usr_UserID;
                        up.UProj_ProjectID = orgDtl.CL_ProjectID;
                        up.UProj_L1_ManagerId = orgDtl.ManagerName;
                        up.UProj_L2_ManagerId = orgDtl.Managername2;
                        up.UProj_StartDate = orgDtl.UProj_StartDate;
                        up.UProj_EndDate = orgDtl.UProj_EndDate;
                        up.UProj_ParticipationPercentage = orgDtl.UProj_ParticipationPercentage;
                        up.UProj_ActiveStatus = orgDtl.UProj_ActiveStatus;
                        up.UProj_CreatedBy = orgDtl.UProj_CreatedBy;
                        up.UProj_CreatedDate = DateTime.Now;
                        up.UProj_Version = 1;
                        up.UProj_isDeleted = false;
                        up.Is_L1_Manager = true;
                        up.Is_L2_Manager = false;
                        db.Set<UserProject>().Add(up);
                        db.SaveChanges();
                    }
                    else if (orgDtl.managertype == 2)
                    {
                        UserProject up = new UserProject();
                        up.UProj_UserID = orgDtl.Usr_UserID;
                        up.UProj_ProjectID = orgDtl.CL_ProjectID;
                        up.UProj_L1_ManagerId = orgDtl.ManagerName;
                        up.UProj_L2_ManagerId = orgDtl.Managername2;
                        up.UProj_StartDate = orgDtl.UProj_StartDate;
                        up.UProj_EndDate = orgDtl.UProj_EndDate;
                        up.UProj_ParticipationPercentage = orgDtl.UProj_ParticipationPercentage;
                        up.UProj_ActiveStatus = orgDtl.UProj_ActiveStatus;
                        up.UProj_CreatedBy = orgDtl.UProj_CreatedBy;
                        up.UProj_CreatedDate = DateTime.Now;
                        up.UProj_Version = 1;
                        up.UProj_isDeleted = false;
                        up.Is_L1_Manager = false;
                        up.Is_L2_Manager = true;
                        db.Set<UserProject>().Add(up);
                        db.SaveChanges();
                    }

                   

                    response = "Successfully Added";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return response;
        }


        public List<ProjectAllocationEntities> GetGenders()
        {
            try
            {
                using (var db = new EvolutyzCornerDataEntities())
                {
                    var query = (from UT in db.UserGenders
                                     //where !db.LeaveSchemes.Any(l => l.LSchm_UserTypeID == UT.UsT_UserTypeID)
                                 select new ProjectAllocationEntities
                                 {
                                     Usr_GenderId = UT.Usr_GenderId,
                                     Gender = UT.Gender
                                 }).ToList();
                    return query;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<ProjectAllocationEntities> Getaccountspecifictasks()
        {
            UserSessionInfo info = new UserSessionInfo();
            int accountId = info.AccountId;
            string RoleId = info.RoleName;

            try
            {
                if (RoleId == "Super Admin")
                {
                    using (var db = new EvolutyzCornerDataEntities())
                    {
                        var query = (from UT in db.AccountSpecificTasks
                                     where UT.isDeleted== false
                                         //where !db.LeaveSchemes.Any(l => l.LSchm_UserTypeID == UT.UsT_UserTypeID)
                                     select new ProjectAllocationEntities
                                     {
                                         Acc_SpecificTaskId = UT.Acc_SpecificTaskId,
                                         Acc_SpecificTaskName = UT.Acc_SpecificTaskName
                                     }).ToList();
                        return query;
                    }
                }
                else
                {
                    using (var db = new EvolutyzCornerDataEntities())
                    {
                        var query = (from UT in db.AccountSpecificTasks
                                     where UT.AccountID== accountId && UT.isDeleted == false
                                     //where !db.LeaveSchemes.Any(l => l.LSchm_UserTypeID == UT.UsT_UserTypeID)
                                     select new ProjectAllocationEntities
                                     {
                                         Acc_SpecificTaskId = UT.Acc_SpecificTaskId,
                                         Acc_SpecificTaskName = UT.Acc_SpecificTaskName
                                     }).ToList();
                        return query;
                    }
                }
              
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        public List<ProjectAllocationEntities> GetTitle()
        {
            try
            {
                using (var db = new EvolutyzCornerDataEntities())
                {
                    var query = (from UT in db.UserTitles
                                     //where !db.LeaveSchemes.Any(l => l.LSchm_UserTypeID == UT.UsT_UserTypeID)
                                 select new ProjectAllocationEntities
                                 {
                                     Usr_Titleid = UT.Usr_Titleid,
                                     TitlePrefix = UT.TitlePrefix
                                 }).ToList();
                    return query;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<ProjectAllocationEntities> GetCountryCodes()
        {
            try
            {
                using (var db = new EvolutyzCornerDataEntities())
                {
                    var query = (from UT in db.Countries
                                   where UT.StatusId==true
                                 select new ProjectAllocationEntities
                                 {
                                     CountryId = UT.CountryId,
                                     PhoneCode = UT.PhoneCode
                                 }).ToList();
                    return query;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<ProjectAllocationEntities> GetProjectUserDetails(int id)
        {
            //  ProjectAllocationEntities response = new ProjectAllocationEntities();
            UserSessionInfo info = new UserSessionInfo();
            int accountid = info.AccountId;
            var roleid = info.RoleName;
            if (roleid == "1001")
            {
                using (var db = new EvolutyzCornerDataEntities())
                {
                    try
                    {

                        var response = (from u in db.Users
                                        join a in db.Accounts on u.Usr_AccountID equals a.Acc_AccountID
                                        join up in db.UsersProfiles on u.Usr_UserID equals up.UsrP_UserID
                                        join uf in db.UserProjects on u.Usr_UserID equals uf.UProj_UserID
                                        join p in db.Projects on uf.UProj_ProjectID equals p.Proj_ProjectID
                                        join cp in db.ClientProjects on p.Proj_ProjectID equals cp.Proj_ProjectID
                                        where u.Usr_isDeleted == false
                                        select new ProjectAllocationEntities
                                        {
                                            Usr_UserID = u.Usr_UserID,
                                            Usr_RoleID = u.Usr_RoleID,
                                            Email = u.Usr_LoginId,
                                          //  UsrP_EmployeeID = up.UsrP_EmployeeID,
                                            Proj_ProjectID = uf.UProj_ProjectID,
                                            Proj_ProjectName = p.Proj_ProjectName,
                                            UProj_ParticipationPercentage = uf.UProj_ParticipationPercentage,
                                            UProj_StartDate = uf.UProj_StartDate,
                                            UProj_EndDate = uf.UProj_EndDate,
                                            Usrp_DOJ = up.Usrp_DOJ,
                                            UsrP_FirstName = up.UsrP_FirstName + " " + up.UsrP_LastName,
                                            UsrP_LastName = up.UsrP_LastName,
                                            project=cp.ClientProjTitle


                                        }).ToList();

                        // response.IsSuccessful = true;
                        return response;
                    }
                    catch (Exception ex)
                    {
                        //  response.IsSuccessful = false;
                        //response.Message = "Error Occured in GetProjectDetailByID(ID)";
                        //  response.Detail = ex.Message.ToString();
                        throw ex;
                    }
                }
            }
            else
            {
                using (var db = new EvolutyzCornerDataEntities())
                {
                    try
                    {

                         var response = (from u in db.Users
                                        join a in db.Accounts on u.Usr_AccountID equals a.Acc_AccountID
                                        join up in db.UsersProfiles on u.Usr_UserID equals up.UsrP_UserID
                                        join uf in db.UserProjects on u.Usr_UserID equals uf.UProj_UserID
                                        join p in db.Projects on uf.UProj_ProjectID equals p.Proj_ProjectID
                                        join cp in db.ClientProjects on uf.ClientprojID equals cp.CL_ProjectID
                                        join r in db.Roles on u.Usr_RoleID equals r.Rol_RoleID
                                        join gr in db.GenericRoles on r.Rol_RoleName equals gr.GenericRoleID
                                        where u.Usr_AccountID == accountid && p.Proj_ProjectID == id && u.Usr_isDeleted == false
                                        select new ProjectAllocationEntities
                                        {
                                            Usr_UserID = u.Usr_UserID,
                                            Usr_RoleID = u.Usr_RoleID,
                                            Email = u.Usr_LoginId,
                                           // UsrP_EmployeeID = up.UsrP_EmployeeID,
                                            Proj_ProjectID = uf.UProj_ProjectID,
                                            Proj_ProjectName = p.Proj_ProjectName,
                                           project = cp.ClientProjTitle,
                                            RoleName = gr.Title,
                                            UProj_ParticipationPercentage = uf.UProj_ParticipationPercentage,
                                            UProj_StartDate = uf.UProj_StartDate,
                                            UProj_EndDate = uf.UProj_EndDate,
                                            Usrp_DOJ = up.Usrp_DOJ,
                                            UsrP_FirstName = up.UsrP_FirstName + " " + up.UsrP_LastName,
                                            UsrP_LastName = up.UsrP_LastName,


                                        }).ToList();

                        // response.IsSuccessful = true;
                        return response;
                    }
                    catch (Exception ex)
                    {
                        //  response.IsSuccessful = false;
                        //response.Message = "Error Occured in GetProjectDetailByID(ID)";
                        //  response.Detail = ex.Message.ToString();
                        throw ex;
                    }
                }
            }

        }
        
        public ProjectAllocationEntities GetUserDetailById(int ID,string proid)
        {
            int projectid = Convert.ToInt32(proid);
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var response = (from u in db.Users
                                    join a in db.Accounts on u.Usr_AccountID equals a.Acc_AccountID
                                    join up in db.UsersProfiles on u.Usr_UserID equals up.UsrP_UserID
                                    join uf in db.UserProjects on u.Usr_UserID equals uf.UProj_UserID
                                    join p in db.Projects on uf.UProj_ProjectID equals p.Proj_ProjectID
                                    join r in db.Roles on u.Usr_RoleID equals r.Rol_RoleID

                                    where u.Usr_UserID == ID && uf.UProj_ProjectID== projectid && u.Usr_isDeleted==false
                                    select new ProjectAllocationEntities
                                    {
                                        Usr_UserID = u.Usr_UserID,
                                        Usr_RoleID = r.Rol_RoleID,
                                        Email = u.Usr_LoginId,
                                        UsrP_EmployeeID = up.UsrP_EmployeeID,
                                        Proj_ProjectID = uf.UProj_ProjectID,
                                        Proj_ProjectName = p.Proj_ProjectName,
                                        UProj_ParticipationPercentage = uf.UProj_ParticipationPercentage,
                                        UProj_StartDate = uf.UProj_StartDate,
                                        UProj_EndDate = uf.UProj_EndDate,
                                        Usrp_DOJ = up.Usrp_DOJ,
                                        UsrP_FirstName = up.UsrP_FirstName,
                                        Usr_Username= u.Usr_Username,
                                        UsrP_LastName = up.UsrP_LastName,
                                        Usr_Password = (u.Usr_Password),
                                        Usr_Titleid = up.Usr_Titleid,
                                        Usrp_MobileNumber = up.Usrp_MobileNumber,
                                        Usrp_CountryCode = up.Usrp_CountryCode,
                                        Usr_GenderId = up.Usr_GenderId,
                                        Usrp_ProfilePicture = up.Usrp_ProfilePicture,
                                        Usr_UserTypeID = u.Usr_UserTypeID,
                                        Usr_TaskID = u.Usr_TaskID,
                                        ManagerName = uf.UProj_L1_ManagerId,
                                        Managername2 = uf.UProj_L2_ManagerId,
                                        TimesheetMode_id = uf.TimesheetMode,
                                        UProj_ActiveStatus = uf.UProj_ActiveStatus,
                                      Cl_projid=uf.ClientprojID
                                    }).FirstOrDefault();
                    return response;
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }

        public ProjectAllocationEntities GetassUserDetailById(int ID)
        {
            //int projectid = Convert.ToInt32(proid);
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var response = (from u in db.Users
                                    join a in db.Accounts on u.Usr_AccountID equals a.Acc_AccountID
                                    join up in db.UsersProfiles on u.Usr_UserID equals up.UsrP_UserID
                                    join uf in db.UserProjects on u.Usr_UserID equals uf.UProj_UserID
                                    join p in db.Projects on uf.UProj_ProjectID equals p.Proj_ProjectID
                                    join r in db.Roles on u.Usr_RoleID equals r.Rol_RoleID

                                    where u.Usr_UserID == ID   && u.Usr_isDeleted==false
                                    select new ProjectAllocationEntities
                                    {
                                        Usr_UserID = u.Usr_UserID,
                                        Usr_RoleID = r.Rol_RoleID,
                                        Email = u.Usr_LoginId,
                                        UsrP_EmployeeID = up.UsrP_EmployeeID,
                                        Proj_ProjectID = uf.UProj_ProjectID,
                                        Proj_ProjectName = p.Proj_ProjectName,
                                        UProj_ParticipationPercentage = uf.UProj_ParticipationPercentage,
                                        UProj_StartDate = uf.UProj_StartDate,
                                        UProj_EndDate = uf.UProj_EndDate,
                                        Usrp_DOJ = up.Usrp_DOJ,
                                        UsrP_FirstName = up.UsrP_FirstName,
                                        Usr_Username = u.Usr_Username,
                                        UsrP_LastName = up.UsrP_LastName,
                                        Usr_Password = (u.Usr_Password),
                                        Usr_Titleid = up.Usr_Titleid,
                                        Usrp_MobileNumber = up.Usrp_MobileNumber,
                                        Usr_GenderId = up.Usr_GenderId,
                                        Usrp_ProfilePicture = up.Usrp_ProfilePicture,
                                        Usr_UserTypeID = u.Usr_UserTypeID,
                                        Usr_TaskID = u.Usr_TaskID,
                                        ManagerName = uf.UProj_L1_ManagerId,
                                        Managername2 = uf.UProj_L2_ManagerId,
                                        UProj_ActiveStatus = uf.UProj_ActiveStatus

                                    }).FirstOrDefault();
                    return response;
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }

        public int DeleteUser(int userid)
        {
            int retVal = 0;
            User userdtl = null;
            UsersProfile ufdtl = null;
            UserProject updtl = null;

            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    userdtl = db.Set<User>().Where(s => s.Usr_UserID == userid).FirstOrDefault<User>();
                    if (userdtl == null)
                    {
                        return retVal;
                    }
                    userdtl.Usr_isDeleted = true;
                    db.Entry(userdtl).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    retVal = 1;
                    ufdtl = db.Set<UsersProfile>().Where(s => s.UsrP_UserID == userid).FirstOrDefault<UsersProfile>();
                    if (ufdtl == null)
                    {
                        return retVal;
                    }
                    ufdtl.UsrP_isDeleted = true;
                    db.Entry(ufdtl).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    retVal = 1;
                    updtl = db.Set<UserProject>().Where(s => s.UProj_UserID == userid).FirstOrDefault<UserProject>();
                    if (ufdtl == null)
                    {
                        return retVal;
                    }
                    updtl.UProj_isDeleted = true;
                    db.Entry(updtl).State = System.Data.Entity.EntityState.Modified;
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
        
        public List<ProjectAllocationEntities> GetHolidays(int projectid)
        {
          
            UserSessionInfo info = new UserSessionInfo();
            int accountid = info.AccountId;
            var roleid = info.RoleName;

            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    int holidaydtlid = (from temp in db.HolidayCalendars
                                        where temp.ProjectID == projectid && temp.AccountID == accountid && temp.isDeleted == false
                                        select temp.HolidayCalendarID).FirstOrDefault();
                


                    if (holidaydtlid == 0)
                    {
                        int? OptionalHolidays = (from H in db.HolidayCalendars
                                                where H.AccountID == accountid && H.isDeleted == false && H.isOptionalHoliday==true
                                                select H.isOptionalHoliday).Count();
                        int? useroptholidays = (from temp in db.Acc_Spec_OptionalHolidays
                                               where temp.AccoutID == accountid && temp.IsDeleted == false
                                               select temp.NoofOptionalHolidays).FirstOrDefault(); 

                        var response = (from H in db.HolidayCalendars
                                        join F in db.FinancialYears on H.Year equals F.FinancialYearId
                                        where H.AccountID == accountid && H.isDeleted == false
                                        select new ProjectAllocationEntities
                                        {
                                            HolidayCalendarID = H.HolidayCalendarID,
                                            AccountID = H.AccountID,
                                            HolidayName = H.HolidayName,
                                            FinancialYearId = F.FinancialYearId,
                                            Year = H.Year,
                                            HolidayDate = H.HolidayDate,
                                            financialyear = F.StartDate + "-" + F.EndDate,
                                            isOptionalHoliday = H.isOptionalHoliday,
                                            isActive = H.isActive,
                                            optionalholidays= OptionalHolidays,
                                            useroptionalholidays = useroptholidays

                                        }).ToList();

                      
                        return response;
                    }
                    else
                    {
                        int? OptionalHolidays = (from H in db.HolidayCalendars
                                                where H.ProjectID == projectid && H.AccountID == accountid && H.isDeleted == false && H.isOptionalHoliday == true
                                                select H.isOptionalHoliday).Count();
                        int? useroptholidays = (from temp in db.Acc_Spec_OptionalHolidays
                                               where temp.ProjectId == projectid && temp.AccoutID == accountid && temp.IsDeleted == false
                                               select temp.NoofOptionalHolidays).FirstOrDefault();
                        var response = (from H in db.HolidayCalendars
                                        join F in db.FinancialYears on H.Year equals F.FinancialYearId
                                        where H.ProjectID == projectid && H.AccountID == accountid && H.isDeleted == false    
                                        select new ProjectAllocationEntities
                                        {
                                            HolidayCalendarID = H.HolidayCalendarID,
                                            AccountID = H.AccountID,
                                            HolidayName = H.HolidayName,
                                            Year = H.Year,
                                            HolidayDate = H.HolidayDate,
                                            FinancialYearId = F.FinancialYearId,
                                            financialyear = F.StartDate + "-" + F.EndDate,
                                            isOptionalHoliday = H.isOptionalHoliday,
                                            isActive = H.isActive,
                                            ProjectID = H.ProjectID,
                                            optionalholidays = OptionalHolidays,
                                            useroptionalholidays= useroptholidays

                                        }).ToList();
                        return response;
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }



        }
        
        public List<ProjectAllocationEntities> Getprotasks(int projectid)
        {
            
            UserSessionInfo info = new UserSessionInfo();
            int accountid = info.AccountId;
            var roleid = info.RoleName;

            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    
                        var response = (from H in db.ProjectSpecificTasks
                                       join ap in db.AccountSpecificTasks on H.tsk_TaskID equals ap.Acc_SpecificTaskId
                                        where H.ProjectId == projectid && H.isDeleted == false
                                        select new ProjectAllocationEntities
                                        {
                                            Proj_SpecificTaskId = H.Proj_SpecificTaskId,
                                            ProjectId = H.ProjectId,
                                            Proj_SpecificTaskName = H.Proj_SpecificTaskName,
                                            RTMId = H.RTMId,
                                            Actual_StartDate = H.Actual_StartDate,
                                            Actual_EndDate= H.Actual_EndDate,
                                            Plan_StartDate = H.Plan_StartDate,
                                            Plan_EndDate = H.Plan_EndDate,
                                            StatusId = H.StatusId,
                                            Acc_SpecificTaskId= ap.Acc_SpecificTaskId,
                                            Acc_SpecificTaskName= ap.Acc_SpecificTaskName,
                                            

                                        }).ToList();

                        // response.IsSuccessful = true;
                        return response;
                    

                }
                catch (Exception ex)
                {
                    //  response.IsSuccessful = false;
                    //response.Message = "Error Occured in GetProjectDetailByID(ID)";
                    //  response.Detail = ex.Message.ToString();
                    throw ex;
                }
            }



        }

        public List<ProjectAllocationEntities> getprojecttaskbyid(int projectid)
        {
           
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {

                  


                    var response = (from H in db.ProjectSpecificTasks
                                    join ap in db.AccountSpecificTasks on H.tsk_TaskID equals ap.Acc_SpecificTaskId
                                    join a in db.Projects on H.ProjectId equals a.Proj_ProjectID
                                    where H.Proj_SpecificTaskId == projectid && H.isDeleted == false
                                    select new ProjectAllocationEntities
                                    {
                                        Proj_SpecificTaskId = H.Proj_SpecificTaskId,
                                        ProjectId = H.ProjectId,
                                        Proj_SpecificTaskName = H.Proj_SpecificTaskName,
                                        RTMId = H.RTMId,
                                        Actual_StartDate = H.Actual_StartDate,
                                        Actual_EndDate = H.Actual_EndDate,
                                        Plan_StartDate = H.Plan_StartDate,
                                        Plan_EndDate = H.Plan_EndDate,
                                        StatusId = H.StatusId,
                                        Acc_SpecificTaskId = ap.Acc_SpecificTaskId,
                                        Acc_SpecificTaskName = ap.Acc_SpecificTaskName,


                                    }).ToList();

                   
                    return response;


                }
                catch (Exception ex)
                {
                   
                    throw ex;
                }
            }



        }
        
        public string Addprotasks(string Acc_SpecificTaskName, string ProjectID, string Proj_SpecificTaskName, string RTMId, string Actual_StartDate, string Actual_EndDate, string Plan_StartDate, string Plan_EndDate, string StatusId)
        {
            int acc_taskid = Convert.ToInt32(Acc_SpecificTaskName);
            int projectid = Convert.ToInt32(ProjectID);
          
            DateTime actualstartdate = Convert.ToDateTime(Actual_StartDate);
            DateTime actualenddate = Convert.ToDateTime(Actual_EndDate);
            DateTime planstartdate = Convert.ToDateTime(Plan_StartDate);
            DateTime planenddate = Convert.ToDateTime(Plan_EndDate);
            UserSessionInfo info = new UserSessionInfo();
            int userid = info.UserId;
          //  int sttus = Convert.ToInt32(StatusId);
            bool b = Convert.ToBoolean(StatusId);
            string strresponse = string.Empty;
            try
            {
                using (var db = new EvolutyzCornerDataEntities())
                {

                    db.Set<ProjectSpecificTask>().Add(new ProjectSpecificTask
                    {
                        ProjectId = projectid,
                        Proj_SpecificTaskName = Proj_SpecificTaskName,
                        RTMId = RTMId,
                        Actual_StartDate = actualstartdate,
                        Actual_EndDate = actualenddate,
                        Plan_StartDate = planstartdate,
                        Plan_EndDate = planenddate,
                        tsk_TaskID = acc_taskid,
                        StatusId = b,
                        isDeleted = false,
                        CreatedBy = userid,
                        CreatedDate = DateTime.Now

                    });
                    db.SaveChanges();
                    strresponse = "Successfully Added";
                }
            
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strresponse;
        }
        
        public string updatetasks(int id,string Acc_SpecificTaskName, string ProjectID, string Proj_SpecificTaskName, string RTMId, string Actual_StartDate, string Actual_EndDate, string Plan_StartDate, string Plan_EndDate, string StatusId)
        {
            int acc_taskid = Convert.ToInt32(Acc_SpecificTaskName);
            int projectid = Convert.ToInt32(ProjectID);

            DateTime actualstartdate = Convert.ToDateTime(Actual_StartDate);
            DateTime actualenddate = Convert.ToDateTime(Actual_EndDate);
            DateTime planstartdate = Convert.ToDateTime(Plan_StartDate);
            DateTime planenddate = Convert.ToDateTime(Plan_EndDate);
            UserSessionInfo info = new UserSessionInfo();
            int userid = info.UserId;
            int sttus = Convert.ToInt32(StatusId);
            bool b = Convert.ToBoolean(sttus);
            string strresponse = string.Empty;
          

            try
            {
                

                using (var db = new EvolutyzCornerDataEntities())
                {
                    ProjectSpecificTask taskdetails = db.Set<ProjectSpecificTask>().Where(s => s.Proj_SpecificTaskId == id).FirstOrDefault<ProjectSpecificTask>();

                    if (taskdetails == null)
                    {
                        return null;
                    }


                    taskdetails.Proj_SpecificTaskName = Proj_SpecificTaskName;
                    taskdetails.tsk_TaskID = acc_taskid;
                    taskdetails.StatusId = b;
                    taskdetails.RTMId = RTMId;
                    taskdetails.Actual_StartDate = actualstartdate;
                    taskdetails.Actual_EndDate = actualenddate;
                    taskdetails.Plan_StartDate = planstartdate;
                    taskdetails.Plan_EndDate = planenddate;

                    taskdetails.ModifiedBy = userid;
                    taskdetails.ModifiedDate = System.DateTime.Now;

                    db.Entry(taskdetails).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    strresponse = "Record successfully updated";





                }


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return strresponse;
        }
        
        public List<ProjectAllocationEntities> Associatemanagers(int projectid, int accountid)
        {
           // int ManagerType = Convert.ToInt32(managertype);
            List<ProjectAllocationEntities> response= new List<ProjectAllocationEntities>();
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    //if (ManagerType == 1)
                    //{
                         response = (from u in db.Users
                                        join a in db.Accounts on u.Usr_AccountID equals a.Acc_AccountID
                                        join up in db.UsersProfiles on u.Usr_UserID equals up.UsrP_UserID
                                        join uf in db.UserProjects on u.Usr_UserID equals uf.UProj_UserID
                                        join p in db.Projects on uf.UProj_ProjectID equals p.Proj_ProjectID
                                        join r in db.Roles on u.Usr_RoleID equals r.Rol_RoleID

                                        where r.Rol_RoleName == 1007 && uf.UProj_ProjectID != projectid && u.Usr_isDeleted==false && a.Acc_AccountID == accountid &&uf.Is_L1_Manager== true ||uf.Is_L2_Manager == true
                                     select new ProjectAllocationEntities
                                        {
                                            Usr_UserID = u.Usr_UserID,

                                            Usr_Username = u.Usr_Username,

                                        }).Distinct().ToList();

                        // response.IsSuccessful = true;
                        
                    //}
                    //else if(ManagerType == 2)
                    //{
                    //     response = (from u in db.Users
                    //                    join a in db.Accounts on u.Usr_AccountID equals a.Acc_AccountID
                    //                    join up in db.UsersProfiles on u.Usr_UserID equals up.UsrP_UserID
                    //                    join uf in db.UserProjects on u.Usr_UserID equals uf.UProj_UserID
                    //                    join p in db.Projects on uf.UProj_ProjectID equals p.Proj_ProjectID
                    //                    join r in db.Roles on u.Usr_RoleID equals r.Rol_RoleID

                    //                    where r.Rol_RoleName == 1007 && uf.UProj_ProjectID != projectid && a.Acc_AccountID == accountid && uf.Is_L2_Manager == true
                    //                    select new ProjectAllocationEntities
                    //                    {
                    //                        Usr_UserID = u.Usr_UserID,

                    //                        Usr_Username = u.Usr_Username,

                    //                    }).Distinct().ToList();

                    //    // response.IsSuccessful = true;
                       
                    //}


                    return response;

                }
                catch (Exception ex)
                {
                    //  response.IsSuccessful = false;
                    //response.Message = "Error Occured in GetProjectDetailByID(ID)";
                    //  response.Detail = ex.Message.ToString();
                    throw ex;
                }
            }


        }

        public List<ProjectAllocationEntities> AssociateEmployees(int projectid, int accountid)
        {


            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var response = (from u in db.Users
                                    join a in db.Accounts on u.Usr_AccountID equals a.Acc_AccountID
                                    join up in db.UsersProfiles on u.Usr_UserID equals up.UsrP_UserID
                                    join uf in db.UserProjects on u.Usr_UserID equals uf.UProj_UserID
                                    join p in db.Projects on uf.UProj_ProjectID equals p.Proj_ProjectID
                                    join r in db.Roles on u.Usr_RoleID equals r.Rol_RoleID

                                    where r.Rol_RoleName != 1001 && r.Rol_RoleName != 1002 && u.Usr_isDeleted==false  && r.Rol_RoleName != 1007 && uf.UProj_ProjectID != projectid && a.Acc_AccountID == accountid
                                    select new ProjectAllocationEntities
                                    {
                                        Usr_UserID = u.Usr_UserID,
                                        
                                        Usr_Username = u.Usr_Username,
                                       
                                    }).Distinct().ToList();

                   
                    return response;


                }
                catch (Exception ex)
                {
                   
                    throw ex;
                }
            }


        }

        public string DeleteProjecttask(string id)
        {
            int protaskid = Convert.ToInt32(id);
            string response = string.Empty; 
            ProjectSpecificTask taskdtl = null;

            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    taskdtl = db.Set<ProjectSpecificTask>().Where(s => s.Proj_SpecificTaskId == protaskid).FirstOrDefault<ProjectSpecificTask>();
                    if (taskdtl == null)
                    {
                        return null;
                    }
                    taskdtl.isDeleted = true;
                    db.Entry(taskdtl).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    response = "Successfully Deleted";


                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return response;
        }

        public string ChangeStatus(string id, string status)
        {
            string strResponse = string.Empty;
            ProjectSpecificTask holidayData = null;
            bool Status = Convert.ToBoolean(status);
            int did = Convert.ToInt32(id);
            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    holidayData = db.Set<ProjectSpecificTask>().Where(s => s.Proj_SpecificTaskId == did).FirstOrDefault<ProjectSpecificTask>();
                    if (holidayData == null)
                    {
                        return null;
                    }
                    holidayData.StatusId = Status;
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
