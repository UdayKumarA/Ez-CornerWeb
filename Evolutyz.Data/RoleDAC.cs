using Evolutyz.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Data.SqlClient;
using System.Configuration;

namespace Evolutyz.Data
{
    public class RoleDAC : DataAccessComponent
    {
        #region To add Role Detail in Database
        //public int AddRole(RoleEntity user)
        //{
        //    int retVal = 0;
        //    Role Role = new Role();

        //    using (var db = new DbContext(CONNECTION_NAME))
        //    {
        //        try
        //        {
        //            Role = db.Set<Role>().Where(s => s.Rol_RoleID == user.Rol_RoleID).FirstOrDefault<Role>();
        //            if (Role != null)
        //            {
        //                return retVal;
        //            }
        //            db.Set<Role>().Add(new Role
        //            {
        //                Rol_AccountID = user.Rol_AccountID,
        //                Rol_RoleCode = user.Rol_RoleCode,
        //                Rol_RoleName = user.Rol_RoleName,
        //                Rol_RoleDescription = user.Rol_RoleDescription,
        //                Rol_ActiveStatus = user.Rol_ActiveStatus,
        //                Rol_Version = user.Rol_Version,
        //                Rol_createdBy = user.Rol_createdBy,
        //                Rol_CreatedDate = System.DateTime.Now,
        //                Rol_isDeleted = false
        //            });
        //            db.SaveChanges();
        //            retVal = 1;
        //        }
        //        catch (Exception ex)
        //        {
        //            retVal = -1;
        //        }
        //    }
        //    return retVal;
        //}

        public string AddRole(List<RoleEntity> moduleaccess, RoleEntity user)
        {
            int id = 0;
            int rolenamegenericid = Convert.ToInt32(user.Rol_RoleName);
            string strresponse = string.Empty;
            try
            {
                using (var db = new DbContext(CONNECTION_NAME))
                {

                    Role orgCheck = db.Set<Role>().Where(s => (s.Rol_RoleCode == user.Rol_RoleCode && s.Rol_AccountID== user.Rol_AccountID)).FirstOrDefault<Role>();
                    Role RoleCheck = db.Set<Role>().Where(s => (s.Rol_RoleName == rolenamegenericid && s.Rol_AccountID == user.Rol_AccountID)).FirstOrDefault<Role>();
                    if (orgCheck != null)
                    {
                        return strresponse = "RoleCode Already Exist In this Account ";
                    }
                    if (RoleCheck != null)
                    {
                        return strresponse = "RoleName Already Exist In this Account ";
                    }
                    Role u = new Role();
                    u.Rol_AccountID = user.Rol_AccountID;
                    u.Rol_RoleCode = user.Rol_RoleCode;
                    u.Rol_RoleName = rolenamegenericid;
                    u.Rol_RoleDescription = user.Rol_RoleDescription;
                    u.Rol_ActiveStatus = user.Rol_ActiveStatus;
                   // u.Rol_Version = user.Rol_Version;
                    u.Rol_createdBy = user.Rol_createdBy;
                    u.Rol_CreatedDate = System.DateTime.Now;
                    u.Rol_isDeleted = false;
                   // u.IsManagerRole = user.IsManagerRole;
                    db.Set<Role>().Add(u);

                    db.SaveChanges();


                    id = u.Rol_RoleID;

                    for (int i = 0; i <= moduleaccess.Count - 1; i++)
                    {


                        int rmid = rolemoduleidsequence();
                        db.Set<RoleModule>().Add(new RoleModule
                        {
                            RMod_RoleModuleID = rmid,
                            RMod_AccountID = moduleaccess[i].Rol_AccountID,
                            RMod_RoleID = id,
                            Sub_ModuleID = moduleaccess[i].Mod_ModuleID,
                            ModuleAccessTypeID = moduleaccess[i].ModuleAccessTypeID,
                            RMod_ActiveStatus = user.Rol_ActiveStatus,
                            RMod_CreatedBy = user.Rol_createdBy,
                            RMod_CreatedDate = System.DateTime.Now,
                            RMod_isDeleted = false,



                        });
                        db.SaveChanges();
                        strresponse = "Successfully Role added";



                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return strresponse;
        }
        public string UpdateRole(int id, List<RoleEntity> moduleaccess, RoleEntity user)
        {

            int rolenamegenericid = Convert.ToInt32(user.Rol_RoleName);
            string strresponse = string.Empty;
            Role u = null;
            try
            {
                using (var db = new DbContext(CONNECTION_NAME))
                {
                    u = db.Set<Role>().Where(s => s.Rol_RoleID == id).FirstOrDefault<Role>();
                    if (u != null)
                    {
                        u.Rol_AccountID = user.Rol_AccountID;
                        u.Rol_RoleCode = user.Rol_RoleCode;
                        u.Rol_RoleName = rolenamegenericid;
                        u.Rol_RoleDescription = user.Rol_RoleDescription;
                        u.Rol_ActiveStatus = user.Rol_ActiveStatus;
                       // u.Rol_Version = user.Rol_Version;
                        u.Rol_createdBy = user.Rol_createdBy;
                        u.Rol_CreatedDate = System.DateTime.Now;
                        u.Rol_isDeleted = false;
                       // u.IsManagerRole = user.IsManagerRole;
                    }



                    db.SaveChanges();


                    id = u.Rol_RoleID;

                    for (int i = 0; i <= moduleaccess.Count - 1; i++)
                    {


                        int rmid = rolemoduleidsequence();
                        db.Set<RoleModule>().Add(new RoleModule
                        {
                            RMod_RoleModuleID = rmid,
                            RMod_AccountID = moduleaccess[i].Rol_AccountID,
                            RMod_RoleID = id,
                            Sub_ModuleID = moduleaccess[i].Mod_ModuleID,
                            ModuleAccessTypeID = moduleaccess[i].ModuleAccessTypeID,
                            RMod_ActiveStatus = user.Rol_ActiveStatus,
                            RMod_CreatedBy = user.Rol_createdBy,
                            RMod_CreatedDate = System.DateTime.Now,
                            RMod_isDeleted = false,
                                  


                        });
                        db.SaveChanges();
                        strresponse = "Successfully Role Updated";



                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return strresponse;
        }
        #endregion

        public int rolemoduleidsequence()
        {

            // conn.ConnectionString = "Data Source=DESKTOP-7AA2POF;Initial Catalog=EvolutyzCorner_Developer;User ID=sa;Password=A12#abhi";

            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(constr);
            conn.Open();

            // SqlCommand cmd = new SqlCommand("select max(AqId) FROM [AcademicQuestionPaper]", conn);
            SqlCommand cmd = new SqlCommand("select NEXT VALUE FOR [dbo].[RoleModuleID] ", conn);
            int k = Convert.ToInt16(cmd.ExecuteScalar());
            //int k=cmd.ExecuteNonQuery();
            // k = k + 1;
            return k;

        }

        #region To update existing Role Detail in Database
        public int UpdateRoleDetail(RoleEntity Role)
        {
            Role _RoleDtl = null;
            History_Role _RoleHistory = new History_Role();

            int retVal = 0;

            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    _RoleDtl = db.Set<Role>().Where(s => s.Rol_RoleID == Role.Rol_RoleID).FirstOrDefault<Role>();

                    if (_RoleDtl == null)
                    {
                        return retVal;
                    }

                    #region Saving History into History_Role Table

                    //check for  _RoleDtl.Rol_ModifiedDate as created date to history table
                    DateTime dtAccCreatedDate = _RoleDtl.Rol_ModifiedDate ?? DateTime.Now;

                    db.Set<History_Role>().Add(new History_Role
                    {
                        History_Rol_RoleID = _RoleDtl.Rol_RoleID,
                        History_Rol_AccountID = _RoleDtl.Rol_AccountID,
                        History_Rol_RoleCode = _RoleDtl.Rol_RoleCode,
                        // History_Rol_RoleName = _RoleDtl.Rol_RoleName,
                        History_Rol_RoleDescription = _RoleDtl.Rol_RoleDescription,
                        History_Rol_ActiveStatus = _RoleDtl.Rol_ActiveStatus,
                        History_Rol_Version = _RoleDtl.Rol_Version,
                        History_Rol_CreatedDate = dtAccCreatedDate,
                        History_Rol_createdBy = _RoleDtl.Rol_createdBy,
                        History_Rol_ModifiedDate = _RoleDtl.Rol_ModifiedDate,
                        History_Rol_ModifiedBy = _RoleDtl.Rol_ModifiedBy,
                        History_Rol_isDeleted = _RoleDtl.Rol_isDeleted,

                    });
                    #endregion

                    #region Saving Role info Table

                    _RoleDtl.Rol_AccountID = Role.Rol_AccountID;
                    _RoleDtl.Rol_RoleCode = Role.Rol_RoleCode;
                    // _RoleDtl.Rol_RoleName = Role.Rol_RoleName;
                    _RoleDtl.Rol_RoleDescription = Role.Rol_RoleDescription;
                    _RoleDtl.Rol_ActiveStatus = Role.Rol_ActiveStatus;
                    _RoleDtl.Rol_Version = Role.Rol_Version;
                    _RoleDtl.Rol_ModifiedDate = System.DateTime.Now;
                    _RoleDtl.Rol_ModifiedBy = Role.Rol_ModifiedBy;
                    _RoleDtl.Rol_isDeleted = Role.Rol_isDeleted;
                    _RoleDtl.IsManagerRole = Role.IsManagerRole;
                    #endregion
                    db.Entry(_RoleDtl).State = System.Data.Entity.EntityState.Modified;
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

        #region To delete existing Role details from Database
        public int DeleteRoleDetail(int ctID)
        {
            int retVal = 0;
            Role _RoleDtl = null;

            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    _RoleDtl = db.Set<Role>().Where(s => s.Rol_RoleID == ctID).FirstOrDefault<Role>();
                    if (_RoleDtl == null)
                    {
                        return retVal;
                    }
                    _RoleDtl.Rol_isDeleted = true;
                    db.Entry(_RoleDtl).State = System.Data.Entity.EntityState.Modified;
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

        #region To delete existing rolemodule details from Database
        public bool DeleteRolemodules(int ctID)
        {
            bool strResponse = false;
            List<RoleModule> _LeaveSchemeDtl = null;

            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {

                    _LeaveSchemeDtl = db.Set<RoleModule>().Where(s => s.RMod_RoleID == ctID).ToList();
                    if (_LeaveSchemeDtl != null)
                    {
                        _LeaveSchemeDtl.ToList().ForEach(u =>
                        {
                            u.RMod_isDeleted = true;

                        });
                        // db.Set<RoleModule>().RemoveRange(_LeaveSchemeDtl);
                        db.SaveChanges();
                        strResponse = true;
                    }

                }
                catch (Exception ex)
                {

                }
            }
            return strResponse;

        }
        #endregion

        #region To get all details of Role from Database
        public List<RoleEntity> GetRoleDetail(int accountid,string rolename)
        {
            UserSessionInfo objinfo = new UserSessionInfo();
            //int roleid = objinfo.RoleId;
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    if (rolename== "Super Admin")
                    {
                        var query = (from q in db.Roles
                                  
                                     join a in db.Accounts on q.Rol_AccountID equals a.Acc_AccountID
                                     join gr in db.GenericRoles on q.Rol_RoleName equals gr.GenericRoleID
                                     where q.Rol_isDeleted == false && q.Rol_AccountID != 502 && q.Rol_RoleName ==1002
                                     select new RoleEntity
                                     {
                                         Rol_RoleID = q.Rol_RoleID,
                                         Rol_AccountID = q.Rol_AccountID,
                                         AccountName = a.Acc_AccountName,
                                         Rol_RoleCode = q.Rol_RoleCode,
                                         Rol_RoleName = gr.Title,
                                         Rol_RoleDescription = q.Rol_RoleDescription,
                                         Rol_ActiveStatus = q.Rol_ActiveStatus,
                                         Rol_Version = q.Rol_Version,
                                         Rol_createdBy = q.Rol_createdBy,
                                         Rol_CreatedDate = q.Rol_CreatedDate,
                                         Rol_ModifiedBy = q.Rol_ModifiedBy,
                                         Rol_ModifiedDate = q.Rol_ModifiedDate,
                                         Rol_isDeleted = q.Rol_isDeleted,
                                     }).OrderBy(x => x.Rol_RoleCode).ThenBy(x => x.Rol_RoleName).ToList();

                        return query;
                    }
                    else if(rolename == "Admin")
                    {
                        var query = (from q in db.Roles
                                     where q.Rol_isDeleted == false && q.Rol_AccountID == accountid && q.Rol_RoleName!= 1002
                                     join a in db.Accounts on q.Rol_AccountID equals a.Acc_AccountID
                                     join gr in db.GenericRoles on q.Rol_RoleName equals gr.GenericRoleID

                                     select new RoleEntity
                                     {
                                         Rol_RoleID = q.Rol_RoleID,
                                         Rol_AccountID = q.Rol_AccountID,
                                         AccountName = a.Acc_AccountName,
                                         Rol_RoleCode = q.Rol_RoleCode,
                                         Rol_RoleName = gr.Title,
                                         Rol_RoleDescription = q.Rol_RoleDescription,
                                         Rol_ActiveStatus = q.Rol_ActiveStatus,
                                         Rol_Version = q.Rol_Version,
                                         Rol_createdBy = q.Rol_createdBy,
                                         Rol_CreatedDate = q.Rol_CreatedDate,
                                         Rol_ModifiedBy = q.Rol_ModifiedBy,
                                         Rol_ModifiedDate = q.Rol_ModifiedDate,
                                         Rol_isDeleted = q.Rol_isDeleted,
                                     }).OrderBy(x => x.Rol_RoleCode).ThenBy(x => x.Rol_RoleName).ToList();

                        return query;
                    }
                    else
                    {
                        var query = (from q in db.Roles
                                     where q.Rol_isDeleted == false && q.Rol_AccountID == accountid
                                     join a in db.Accounts on q.Rol_AccountID equals a.Acc_AccountID
                                     join gr in db.GenericRoles on q.Rol_RoleName equals gr.GenericRoleID

                                     select new RoleEntity
                                     {
                                         Rol_RoleID = q.Rol_RoleID,
                                         Rol_AccountID = q.Rol_AccountID,
                                         AccountName = a.Acc_AccountName,
                                         Rol_RoleCode = q.Rol_RoleCode,
                                         Rol_RoleName = gr.Title,
                                         Rol_RoleDescription = q.Rol_RoleDescription,
                                         Rol_ActiveStatus = q.Rol_ActiveStatus,
                                         Rol_Version = q.Rol_Version,
                                         Rol_createdBy = q.Rol_createdBy,
                                         Rol_CreatedDate = q.Rol_CreatedDate,
                                         Rol_ModifiedBy = q.Rol_ModifiedBy,
                                         Rol_ModifiedDate = q.Rol_ModifiedDate,
                                         Rol_isDeleted = q.Rol_isDeleted,
                                     }).OrderBy(x => x.Rol_RoleCode).ThenBy(x => x.Rol_RoleName).ToList();

                        return query;
                    }
                    
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        #endregion

        #region To get particular Role details from Database
        public RoleEntity GetRoleDetailByID(int ID)
        {
            RoleEntity response = new RoleEntity();

            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    response = (from q in db.Roles
                                join a in db.Accounts on q.Rol_AccountID equals a.Acc_AccountID
                                join gr in db.GenericRoles on q.Rol_RoleName equals gr.GenericRoleID
                                where q.Rol_RoleID == ID
                                select new RoleEntity
                                {
                                    Rol_RoleID = q.Rol_RoleID,
                                    Rol_AccountID = q.Rol_AccountID,
                                    AccountName = a.Acc_AccountName,
                                    Rol_RoleCode = q.Rol_RoleCode,
                                    Rol_RoleName = gr.Title,
                                    GenericRoleID = gr.GenericRoleID,
                                    Rol_RoleDescription = q.Rol_RoleDescription,
                                    Rol_ActiveStatus = q.Rol_ActiveStatus,
                                    Rol_Version = q.Rol_Version,
                                    Rol_createdBy = q.Rol_createdBy,
                                    Rol_CreatedDate = q.Rol_CreatedDate,
                                    Rol_ModifiedBy = q.Rol_ModifiedBy,
                                    Rol_ModifiedDate = q.Rol_ModifiedDate,
                                    Rol_isDeleted = q.Rol_isDeleted,
                                    IsManagerRole = q.IsManagerRole
                                }).FirstOrDefault();

                    response.IsSuccessful = true;
                    return response;
                }
                catch (Exception ex)
                {
                    response.IsSuccessful = false;
                    response.Message = "Error Occured in GetRoleDetailByID(ID)";
                    response.Detail = ex.Message.ToString();
                    return response;
                }
            }
        }
        #endregion
        #region To get particular Role details from Database
        public List<RoleEntity> Getmodulesselected(int ID)
        {
            //RoleEntity response = new RoleEntity();

            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var response = (from q in db.RoleModules
                                    join sm in db.Master_Sub_Module on q.Sub_ModuleID equals sm.Sub_ModuleID
                                    join mm in db.Master_Module on sm.Mod_ModuleID equals mm.Mod_ModuleID
                                    //  join a in db.Roles on q.RMod_RoleID equals a.Rol_RoleID
                                    where q.RMod_RoleID == ID && q.RMod_isDeleted == false
                                    select new RoleEntity
                                    {
                                        ModuleAccessTypeID = q.ModuleAccessTypeID,
                                        RMod_RoleID = q.RMod_RoleID,
                                        Mod_ModuleID = mm.Mod_ModuleID,
                                        Sub_ModuleID = q.Sub_ModuleID

                                    }).ToList();


                    return response;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        #endregion

        #region To get Role details for select list
        public List<RoleEntity> SelectRole()
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var query = (from q in db.Roles
                                 join a in db.Accounts on q.Rol_AccountID equals a.Acc_AccountID
                                 join gr in db.GenericRoles on q.Rol_RoleName equals gr.GenericRoleID
                                 where q.Rol_isDeleted == false && q.Rol_ActiveStatus == true
                                 select new RoleEntity
                                 {
                                     Rol_RoleID = q.Rol_RoleID,
                                     Rol_AccountID = q.Rol_AccountID,
                                     AccountName = a.Acc_AccountName,
                                     Rol_RoleCode = q.Rol_RoleCode,
                                     Rol_RoleName = gr.Title,
                                     Rol_RoleDescription = q.Rol_RoleDescription,
                                     Rol_ActiveStatus = q.Rol_ActiveStatus,
                                     Rol_Version = q.Rol_Version,
                                     Rol_createdBy = q.Rol_createdBy,
                                     Rol_CreatedDate = q.Rol_CreatedDate,
                                     Rol_ModifiedBy = q.Rol_ModifiedBy,
                                     Rol_ModifiedDate = q.Rol_ModifiedDate,
                                     Rol_isDeleted = q.Rol_isDeleted,
                                 }).OrderBy(x => x.Rol_RoleName).ToList();

                    return query;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        #endregion

        #region Get Max RoleID Details
        public Role GetMaxRoleIDDetials()
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var RoleMaxEntry = db.Roles.OrderByDescending(x => x.Rol_RoleID).FirstOrDefault();
                    return RoleMaxEntry;
                }

                catch (Exception Exception)
                {
                    return null;
                }
            }
        }

        #endregion

        //History

        #region To get particular Role details from Database
        public List<History_RoleEntity> GetHistoryRoleDetailsByID(int ID)
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var query = (from q in db.History_Role
                                 join a in db.Accounts on q.History_Rol_AccountID equals a.Acc_AccountID
                                 where q.History_Rol_RoleID == ID
                                 select new History_RoleEntity
                                 {
                                     History_Role_ID = q.History_Role_ID,
                                     History_Rol_RoleID = q.History_Rol_RoleID,
                                     History_Rol_AccountID = q.History_Rol_AccountID,
                                     AccountName = a.Acc_AccountName,
                                     History_Rol_RoleCode = q.History_Rol_RoleCode,
                                     History_Rol_RoleName = q.History_Rol_RoleName,
                                     History_Rol_RoleDescription = q.History_Rol_RoleDescription,
                                     History_Rol_ActiveStatus = q.History_Rol_ActiveStatus,
                                     History_Rol_Version = q.History_Rol_Version,
                                     History_Rol_createdBy = q.History_Rol_createdBy,
                                     History_Rol_CreatedDate = q.History_Rol_CreatedDate,
                                     History_Rol_ModifiedBy = q.History_Rol_ModifiedBy,
                                     History_Rol_ModifiedDate = q.History_Rol_ModifiedDate,
                                     History_Rol_isDeleted = q.History_Rol_isDeleted,
                                 }).OrderBy(x => x.History_Rol_RoleName).ToList();

                    return query;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        #endregion
        
        public List<RoleEntity> Getallmodules()
        {
            try
            {
                using (var db = new EvolutyzCornerDataEntities())
                {
                    var query = (from UT in db.Master_Module

                                 select new RoleEntity
                                 {
                                     Mod_ModuleID = UT.Mod_ModuleID,
                                     Mod_ModuleName = UT.Mod_ModuleName
                                 }).ToList();
                    return query;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<RoleEntity> Getallaccess()
        {
            try
            {
                using (var db = new EvolutyzCornerDataEntities())
                {
                    var query = (from UT in db.ModuleAccessTypes

                                 select new RoleEntity
                                 {
                                     ModuleAccessTypeID = UT.ModuleAccessTypeID,
                                     ModuleAccessType1 = UT.ModuleAccessType1
                                 }).ToList();
                    return query;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<RoleEntity> Getsubmodules(int modid)
        {
            UserSessionInfo info = new UserSessionInfo();
            int roleid = info.RoleId;
            try
            {
                using (var db = new EvolutyzCornerDataEntities())
                {
                    //if (roleid == 1001)
                    //{
                        var query = (from UT in db.Master_Sub_Module
                                     join mm in db.Master_Module on UT.Mod_ModuleID equals mm.Mod_ModuleID
                                     where UT.Mod_ModuleID == modid && UT.Sub_ModuleID != 100

                                     select new RoleEntity
                                     {
                                         Mod_ModuleID = mm.Mod_ModuleID,
                                         Sub_ModuleID = UT.Sub_ModuleID,
                                         Sub_ModuleName = UT.Sub_ModuleName

                                     }).ToList();
                        return query;
                    //}
                    //else
                    //{
                    //    var query = (from UT in db.Master_Sub_Module
                    //                 join mm in db.Master_Module on UT.Mod_ModuleID equals mm.Mod_ModuleID
                    //                 where UT.Mod_ModuleID == modid && UT.Sub_ModuleID != 100

                    //                 select new RoleEntity
                    //                 {
                    //                     Mod_ModuleID = mm.Mod_ModuleID,
                    //                     Sub_ModuleID = UT.Sub_ModuleID,
                    //                     Sub_ModuleName = UT.Sub_ModuleName
                    //                 }).ToList();
                    //    return query;
                    //}



                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<RoleEntity> GetallsubModule()
        {
            UserSessionInfo info = new UserSessionInfo();
            int roleid = info.RoleId;
            try
            {
                using (var db = new EvolutyzCornerDataEntities())
                {
                    //if (roleid == 1001)
                    //{
                    //    var query = (from UT in db.Master_Sub_Module
                    //                 join mm in db.Master_Module on UT.Mod_ModuleID equals mm.Mod_ModuleID

                    //                 select new RoleEntity
                    //                 {
                    //                     Sub_ModuleID = UT.Sub_ModuleID,
                    //                     Sub_ModuleName = UT.Sub_ModuleName,
                    //                     Mod_ModuleID = mm.Mod_ModuleID
                    //                 }).ToList();
                    //    return query;
                    //}
                    //else
                    //{
                        var query = (from UT in db.Master_Sub_Module
                                     join mm in db.Master_Module on UT.Mod_ModuleID equals mm.Mod_ModuleID
                                     where UT.Sub_ModuleID != 100
                                     select new RoleEntity
                                     {
                                         Sub_ModuleID = UT.Sub_ModuleID,
                                         Sub_ModuleName = UT.Sub_ModuleName,
                                         Mod_ModuleID = mm.Mod_ModuleID
                                     }).ToList();
                        return query;
                    //}
                }
            }
            catch (Exception)
            {
                return null;
            }
        }


        public string ChangeStatus(string id, string status)
        {
            string strResponse = string.Empty;
            Role holidayData = null;
            bool Status = Convert.ToBoolean(status);
            int did = Convert.ToInt32(id);
            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    holidayData = db.Set<Role>().Where(s => s.Rol_RoleID == did).FirstOrDefault<Role>();
                    if (holidayData == null)
                    {
                        return null;
                    }
                    holidayData.Rol_ActiveStatus = Status;
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

