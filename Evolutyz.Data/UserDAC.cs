using Evolutyz.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Evolutyz.Data
{
    public class UserDAC : DataAccessComponent
    {
        UserEntity getlist = new UserEntity();
        // new script//

        #region To get all details of User from Database
        public List<UserEntity> GetUserDetail(int acntID, string RoleId)
        {
            if (RoleId == "Super Admin")
            {
                using (var db = new EvolutyzCornerDataEntities())
                {
                    try
                    {
                        var query = (


                        from a in db.Accounts
                        join u in db.Users on a.Acc_AccountID equals u.Usr_AccountID
                        join ut in db.UserTypes on u.Usr_UserTypeID equals ut.UsT_UserTypeID
                        join R in db.Roles on u.Usr_RoleID equals R.Rol_RoleID
                        join gr in db.GenericRoles on R.Rol_RoleName equals gr.GenericRoleID
                        // where a.Acc_AccountID == acntID && 
                        where u.Usr_isDeleted == false && R.Rol_RoleName == 1002

                        select new UserEntity
                        {
                            Usr_UserID = u.Usr_UserID,
                            Usr_AccountID = u.Usr_AccountID,
                            AccountName = a.Acc_AccountName,
                            Usr_UserTypeID = u.Usr_UserTypeID,
                            UserType = ut.UsT_UserType,
                            Usr_RoleID = u.Usr_RoleID,
                            RoleName = gr.Title,
                            Usr_Username = u.Usr_Username,
                            Usr_LoginId = u.Usr_LoginId,
                            Usr_Password = u.Usr_Password,
                            Usr_ActiveStatus = u.Usr_ActiveStatus,
                            Usr_Version = u.Usr_Version,
                            //////////Usr_CreatedBy = u.Usr_CreatedBy,
                            Usr_CreatedDate = u.Usr_CreatedDate,
                            //////////Usr_ModifiedBy = u.Usr_ModifiedBy,
                            Usr_ModifiedDate = u.Usr_ModifiedDate,
                            Usr_isDeleted = u.Usr_isDeleted,
                            ////Usr_Manager =Convert.ToInt32(db.Roles.Where(r => r.Rol_RoleID == u.Usr_Manager).FirstOrDefault().Rol_RoleName),
                            Usr_Manager = u.Usr_Manager,
                            Usr_Manager2 = u.Usr_Manager2,
                            ManagerName = db.Users.Where(x => x.Usr_UserID == u.Usr_Manager).Select(c => c.Usr_Username).FirstOrDefault(),
                            Managername2 = db.Users.Where(x => x.Usr_UserID == u.Usr_Manager2).Select(c => c.Usr_Username).FirstOrDefault(),
                            // Usr_TaskID = u.Usr_TaskID,
                            Taskname = db.GenericTasks.Where(x => x.tsk_TaskID == u.Usr_TaskID).Select(c => c.tsk_TaskName).FirstOrDefault(),
                        }).OrderBy(x => x.AccountName).ThenBy(x => x.RoleName)
                                        .ThenBy(x => x.UserType).ThenBy(x => x.Usr_LoginId).ToList();



                        return query;
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
                        var query = (

                        from a in db.Accounts
                        join u in db.Users on a.Acc_AccountID equals u.Usr_AccountID
                        join ut in db.UserTypes on u.Usr_UserTypeID equals ut.UsT_UserTypeID
                        join R in db.Roles on u.Usr_RoleID equals R.Rol_RoleID
                        join gr in db.GenericRoles on R.Rol_RoleName equals gr.GenericRoleID
                        where a.Acc_AccountID == acntID && u.Usr_isDeleted == false

                        select new UserEntity
                        {
                            Usr_UserID = u.Usr_UserID,
                            Usr_AccountID = u.Usr_AccountID,
                            AccountName = a.Acc_AccountName,
                            Usr_UserTypeID = u.Usr_UserTypeID,
                            UserType = ut.UsT_UserType,
                            Usr_RoleID = u.Usr_RoleID,
                            RoleName = gr.Title,
                            Usr_Username = u.Usr_Username,
                            Usr_LoginId = u.Usr_LoginId,
                            Usr_Password = u.Usr_Password,
                            Usr_ActiveStatus = u.Usr_ActiveStatus,
                            Usr_Version = u.Usr_Version,
                            //////////Usr_CreatedBy = u.Usr_CreatedBy,
                            Usr_CreatedDate = u.Usr_CreatedDate,
                            //////////Usr_ModifiedBy = u.Usr_ModifiedBy,
                            Usr_ModifiedDate = u.Usr_ModifiedDate,
                            Usr_isDeleted = u.Usr_isDeleted,
                            ////Usr_Manager =Convert.ToInt32(db.Roles.Where(r => r.Rol_RoleID == u.Usr_Manager).FirstOrDefault().Rol_RoleName),
                            Usr_Manager = u.Usr_Manager,
                            Usr_Manager2 = u.Usr_Manager2,
                            ManagerName = db.Users.Where(x => x.Usr_UserID == u.Usr_Manager).Select(c => c.Usr_Username).FirstOrDefault(),
                            Managername2 = db.Users.Where(x => x.Usr_UserID == u.Usr_Manager2).Select(c => c.Usr_Username).FirstOrDefault(),
                            // Usr_TaskID = u.Usr_TaskID,
                            Taskname = db.GenericTasks.Where(x => x.tsk_TaskID == u.Usr_TaskID).Select(c => c.tsk_TaskName).FirstOrDefault(),
                        }).OrderBy(x => x.AccountName).ThenBy(x => x.RoleName)
                                        .ThenBy(x => x.UserType).ThenBy(x => x.Usr_LoginId).ToList();



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

        #region To add User Detail in Database
        public string Userpasscodesequence()
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

        public int AddUser(UserEntity user)
        {
            int retVal = 0;
            User User = new User();
            User UserName = new User();
            //User UserName = new User();

            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    UserName = db.Set<User>().Where(s => s.Usr_Username == user.Usr_Username.Trim() ).FirstOrDefault<User>();
                   var LoginId = db.Set<User>().Where(s => s.Usr_LoginId == user.Usr_LoginId.Trim()).FirstOrDefault<User>();
                    User = db.Set<User>().Where(s => s.Usr_UserID == user.Usr_UserID).FirstOrDefault<User>();
                    if (UserName == null)
                    {
                        if (LoginId != null)
                        {
                           return retVal = 3;
                        }
                        if (User != null)
                        {
                            return retVal;
                        }
                        db.Set<User>().Add(new User

                        {

                            Usr_AccountID = user.Usr_AccountID,
                            Usr_UserTypeID = user.Usr_UserTypeID,
                            Usr_RoleID = user.Usr_RoleID,
                            Usr_Username = user.Usr_Username.Trim(),
                            Usr_LoginId = user.Usr_LoginId.Trim(),
                            Usr_TaskID = user.Usr_TaskID,
                            Usr_Manager = user.Usr_Manager,
                            Usr_Manager2 = user.Usr_Manager2,
                            Usr_Password = user.Usr_Password.Trim(),
                            Usr_ActiveStatus = user.Usr_ActiveStatus,
                            Usr_Version = user.Usr_Version,
                            Usr_CreatedBy = user.Usr_CreatedBy,
                            Usr_CreatedDate = System.DateTime.Now,
                            Usr_ModifiedDate = System.DateTime.Now,
                            Usr_ModifiedBy = user.Usr_CreatedBy,
                           
                            Usr_isDeleted = false,
                            // Usr_salt= user.Usr_salt,

                        });
                        db.SaveChanges();
                        int Userid = db.Set<User>().OrderByDescending(p => p.Usr_UserID).Select(p => p.Usr_UserID).FirstOrDefault();
                        if (user.Usr_Titleid == 1)
                        {

                            user.Genderid = 1;
                        }

                        else
                        {

                            user.Genderid = 2;

                        }
                        string passcode = Userpasscodesequence();
                        db.Set<UsersProfile>().Add(new UsersProfile
                        {
                            UsrP_FirstName = user.UsrP_FirstName,
                            UsrP_LastName = user.UsrP_LastName,
                            Usrp_DOJ = user.Usrp_DOJ,
                            Usrp_ProfilePicture = user.Usrp_ProfilePicture,
                            UsrP_EmailID = user.Usr_LoginId,
                            UsrP_UserID = Userid,//user.Usr_UserID,
                           // UsrP_EmployeeID = user.UsrP_EmployeeID,
                            UsrP_ActiveStatus = user.Usr_ActiveStatus,
                            UsrP_Version = user.Usr_Version,
                            UsrP_CreatedBy = user.Usr_CreatedBy,
                            UsrP_CreatedDate = System.DateTime.Now,
                            UsrP_ModifiedDate = System.DateTime.Now,
                            UsrP_ModifiedBy = user.Usr_CreatedBy,
                            Usr_Titleid = user.Usr_Titleid,
                            Usr_GenderId = user.Genderid,
                            passcode = passcode,

                        UsrP_isDeleted = false,
                            // Usr_salt = user.Usr_salt,

                        });
                        db.SaveChanges();
                        retVal = 1;
                    }
                    else
                    {
                        retVal = 2;
                    }
                   
                }
                catch (Exception ex)
                {
                    retVal = -1;
                }
            }
            return retVal;
        }
        #endregion

        #region To get particular User details from Database


        public UserEntity GetUserDetailByID(int ID)
        {


            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    User temp = null;
                    temp = db.Users.Single(u => u.Usr_UserID == ID);

                    getlist.Usr_UserID = temp.Usr_UserID;
                    getlist.Usr_AccountID = temp.Usr_AccountID;
                    getlist.AccountName = db.Accounts.Where(a => a.Acc_AccountID == temp.Usr_AccountID).FirstOrDefault().Acc_AccountName;
                    getlist.Usr_UserTypeID = temp.Usr_UserTypeID;
                    getlist.UserType = db.UserTypes.Where(ut => ut.UsT_UserTypeID == temp.Usr_UserTypeID).FirstOrDefault().UsT_UserType;
                    getlist.Usr_RoleID = temp.Usr_RoleID;
                    getlist.id = db.Roles.Where(r => r.Rol_RoleID == temp.Usr_RoleID).FirstOrDefault().Rol_RoleName;
                    getlist.RoleName = db.GenericRoles.Where(r => r.GenericRoleID == getlist.id).FirstOrDefault().Title;

                    //getlist.RoleName = db.Roles.Where(r => r.Rol_RoleID == temp.Usr_RoleID).FirstOrDefault().Rol_RoleName;
                    getlist.Usr_LoginId = temp.Usr_LoginId;
                    getlist.Usr_Password = temp.Usr_Password;
                    getlist.Usr_ActiveStatus = temp.Usr_ActiveStatus;
                    getlist.Usr_Username = temp.Usr_Username;
                    getlist.Usr_CreatedDate = temp.Usr_CreatedDate;
                    getlist.Usr_ModifiedDate = temp.Usr_ModifiedDate;
                    getlist.Usr_isDeleted = temp.Usr_isDeleted;
                    getlist.Usr_TaskID = temp.Usr_TaskID;

                    getlist.Usr_Manager = temp.Usr_Manager;
                    getlist.Usr_Manager2 = temp.Usr_Manager2;
                    getlist.Usrp_ProfilePicture = db.UsersProfiles.Where(up => up.UsrP_UserID == temp.Usr_UserID).FirstOrDefault().Usrp_ProfilePicture;
                    getlist.UsrP_FirstName = db.UsersProfiles.Where(up => up.UsrP_UserID == temp.Usr_UserID).FirstOrDefault().UsrP_FirstName;
                    getlist.UsrP_LastName = db.UsersProfiles.Where(up => up.UsrP_UserID == temp.Usr_UserID).FirstOrDefault().UsrP_LastName;
                    getlist.Usrp_DOJ = db.UsersProfiles.Where(up => up.UsrP_UserID == temp.Usr_UserID).FirstOrDefault().Usrp_DOJ;
                    getlist.Usr_Titleid = db.UsersProfiles.Where(up => up.UsrP_UserID == temp.Usr_UserID).FirstOrDefault().Usr_Titleid;
                    
                    //getlist.UsrP_EmployeeID = db.UsersProfiles.Where(up => up.UsrP_UserID == temp.Usr_UserID).FirstOrDefault().UsrP_EmployeeID;

                    getlist.IsSuccessful = true;
                    return getlist;
                }
                catch (Exception ex)
                {
                    getlist.IsSuccessful = false;
                    getlist.Message = "Error Occured in GetUserDetailByID(ID)";
                    getlist.Detail = ex.Message.ToString();
                    return getlist;
                }
            }
        }
        #endregion

        #region To update existing User Detail in Database
        public int UpdateUserDetail(UserEntity User)
        {
            User _UserDtl = null;
            UsersProfile _UserProfileDtl = null;


            int retVal = 0;

            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    _UserDtl = db.Set<User>().Where(s => s.Usr_UserID == User.Usr_UserID).FirstOrDefault<User>();
                    if (_UserDtl == null)
                    {
                        return retVal;
                    }
                    #region Saving User info Table

                    _UserDtl.Usr_AccountID = User.Usr_AccountID;
                    _UserDtl.Usr_UserTypeID = User.Usr_UserTypeID;
                    _UserDtl.Usr_RoleID = User.Usr_RoleID;
                    _UserDtl.Usr_Username = User.Usr_Username;
                    _UserDtl.Usr_TaskID = User.Usr_TaskID;
                    _UserDtl.Usr_Manager = User.Usr_Manager;
                    _UserDtl.Usr_Manager2 = User.Usr_Manager2;
                    _UserDtl.Usr_LoginId = User.Usr_LoginId.Trim();
                    _UserDtl.Usr_Password = User.Usr_Password.Trim();
                    _UserDtl.Usr_ActiveStatus = User.Usr_ActiveStatus;
                    _UserDtl.Usr_Version = User.Usr_Version;
                    _UserDtl.Usr_ModifiedDate = System.DateTime.Now;
                    _UserDtl.Usr_ModifiedBy = User.Usr_ModifiedBy;
                    _UserDtl.Usr_isDeleted = User.Usr_isDeleted;
                    #endregion
                    db.Entry(_UserDtl).State = System.Data.Entity.EntityState.Modified;

                    _UserProfileDtl = db.Set<UsersProfile>().Where(s => s.UsrP_UserID == User.Usr_UserID).FirstOrDefault<UsersProfile>();


                    _UserProfileDtl.UsrP_FirstName = User.UsrP_FirstName;
                    _UserProfileDtl.UsrP_LastName = User.UsrP_LastName;
                    _UserProfileDtl.Usrp_DOJ = User.Usrp_DOJ;
                    _UserProfileDtl.Usrp_ProfilePicture = User.Usrp_ProfilePicture;
                    _UserProfileDtl.UsrP_EmailID = User.UsrP_EmailID;
                    //_UserProfileDtl.UsrP_EmployeeID = User.UsrP_EmployeeID;
                    _UserProfileDtl.UsrP_UserID = User.Usr_UserID;
                    _UserProfileDtl.UsrP_ActiveStatus = User.Usr_ActiveStatus;
                    _UserProfileDtl.UsrP_Version = User.Usr_Version;
                    _UserProfileDtl.UsrP_CreatedBy = User.Usr_CreatedBy;
                    _UserProfileDtl.UsrP_CreatedDate = System.DateTime.Now;
                    _UserProfileDtl.UsrP_ModifiedDate = System.DateTime.Now;
                    _UserProfileDtl.UsrP_ModifiedBy = User.Usr_CreatedBy;
                    _UserProfileDtl.Usr_Titleid = User.Usr_Titleid;

                    db.Entry(_UserProfileDtl).State = System.Data.Entity.EntityState.Modified;



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
        
        #region To delete existing User details from Database
        public int DeleteUserDetail(int ctID)
        {
            int retVal = 0;
            User _UserDtl = null;

            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    _UserDtl = db.Set<User>().Where(s => s.Usr_UserID == ctID).FirstOrDefault<User>();
                    if (_UserDtl == null)
                    {
                        return retVal;
                    }
                    _UserDtl.Usr_isDeleted = true;
                    db.Entry(_UserDtl).State = System.Data.Entity.EntityState.Modified;
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
        public int DeleteSkill(int skillId)
        {
            int retVal = 0;
            Skill _UserDtl = null;

            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    _UserDtl = db.Set<Skill>().Where(s => s.SkillId == skillId).FirstOrDefault<Skill>();
                    if (_UserDtl == null)
                    {
                        return retVal;
                    }
                    _UserDtl.Sk_isDeleted = true;
                    db.Entry(_UserDtl).State = System.Data.Entity.EntityState.Modified;
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

        #region To get User details for select list
        public List<UserEntity> SelectUser()
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var query = (from u in db.Users
                                 join ut in db.UserTypes on u.Usr_UserTypeID equals ut.UsT_UserTypeID
                                 join r in db.Roles on u.Usr_RoleID equals r.Rol_RoleID
                                 join a in db.Accounts on u.Usr_AccountID equals a.Acc_AccountID
                                 join gr in db.GenericRoles on r.Rol_RoleName equals gr.GenericRoleID
                                 select new UserEntity
                                 {
                                     Usr_UserID = u.Usr_UserID,
                                     Usr_AccountID = u.Usr_AccountID,
                                     AccountName = a.Acc_AccountName,
                                     Usr_UserTypeID = u.Usr_UserTypeID,
                                     UserType = ut.UsT_UserType,
                                     Usr_RoleID = u.Usr_RoleID,
                                     RoleName = gr.Title,
                                     Usr_LoginId = u.Usr_LoginId,
                                     Usr_Password = u.Usr_Password,
                                     Usr_ActiveStatus = u.Usr_ActiveStatus,
                                     Usr_Version = u.Usr_Version,
                                     //////////Usr_CreatedBy = u.Usr_CreatedBy,
                                     Usr_CreatedDate = u.Usr_CreatedDate,
                                     //////////Usr_ModifiedBy = u.Usr_ModifiedBy,
                                     Usr_ModifiedDate = u.Usr_ModifiedDate,
                                     Usr_isDeleted = u.Usr_isDeleted,
                                 }).OrderBy(x => x.AccountName).ThenBy(x => x.RoleName)
                                    .ThenBy(x => x.UserType).ThenBy(x => x.Usr_LoginId).ToList();

                    return query;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        #endregion

        #region Get Max UserID Details
        public User GetMaxUserIDDetials()
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var UserMaxEntry = db.Users.OrderByDescending(x => x.Usr_UserID).FirstOrDefault();
                    return UserMaxEntry;
                }

                catch (Exception Exception)
                {
                    return null;
                }
            }
        }

        #endregion

        //History

        #region To get particular User Account details from Database
        public List<History_UsersEntity> GetHistoryUserDetailsByID(int ID)
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var query = (from hu in db.History_Users
                                 join a in db.Accounts on hu.History_Usr_AccountID equals a.Acc_AccountID
                                 join ut in db.UserTypes on hu.History_Usr_UserTypeID equals ut.UsT_UserTypeID
                                 join r in db.Roles on hu.History_Usr_RoleID equals r.Rol_RoleID
                                 join gr in db.GenericRoles on r.Rol_RoleName equals gr.GenericRoleID
                                 where hu.History_Usr_UserID == ID
                                 select new History_UsersEntity
                                 {
                                     History_Usr_UserID = hu.History_Usr_UserID,
                                     History_Usr_AccountID = hu.History_Usr_AccountID,
                                     AccountName = a.Acc_AccountName,
                                     History_Usr_UserTypeID = hu.History_Usr_UserTypeID,
                                     UserType = ut.UsT_UserType,
                                     History_Usr_RoleID = hu.History_Usr_RoleID,
                                     RoleName = gr.Title,
                                     History_Usr_LoginId = hu.History_Usr_LoginId,
                                     History_Usr_Password = hu.History_Usr_Password,
                                     History_Usr_ActiveStatus = hu.History_Usr_ActiveStatus,
                                     History_Usr_Version = hu.History_Usr_Version,
                                     //////////History_Usr_CreatedBy = hu.History_Usr_CreatedBy,
                                     History_Usr_CreatedDate = hu.History_Usr_CreatedDate,
                                     //////////History_Usr_ModifiedBy = hu.History_Usr_ModifiedBy,
                                     History_Usr_ModifiedDate = hu.History_Usr_ModifiedDate,
                                     History_Usr_isDeleted = hu.History_Usr_isDeleted,
                                 }).OrderBy(x => x.AccountName).ThenBy(x => x.RoleName)
                                    .ThenBy(x => x.UserType).ThenBy(x => x.History_Usr_LoginId).ToList();

                    return query;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        #endregion

        #region GetUserRoleNames

        public List<UserEntity> GetRoleNames(int AccountId, string RoleId)
        {
            if (RoleId == "Super Admin")
            {
                using (var db = new EvolutyzCornerDataEntities())
                {
                    try
                    {
                        var roleNames = (from r in db.Roles
                                         join gr in db.GenericRoles on r.Rol_RoleName equals gr.GenericRoleID
                                         where gr.GenericRoleID == 1002 && r.Rol_AccountID == AccountId
                                         select new UserEntity
                                         {
                                             Usr_RoleID = r.Rol_RoleID,
                                             RoleName = r.Rol_RoleCode

                                         }).ToList();
                       
                        return roleNames;
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
                        var roleNames = (from r in db.Roles
                                         join gr in db.GenericRoles on r.Rol_RoleName equals gr.GenericRoleID
                                         where gr.Title != "Super Admin" && r.Rol_AccountID == AccountId
                                         select new UserEntity
                                         {
                                             Usr_RoleID = r.Rol_RoleID,
                                             RoleName = r.Rol_RoleCode

                                         }).ToList();
                      

                        return roleNames;
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                }
            }



        }  

        #endregion
       
        #region Get UserTypes
        public List<UserEntity> getUserTypes(int AccountId)
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {

                    var UserTypesData = (from ut in db.UserTypes
                                         where ut.UsT_AccountID == AccountId
                                         select new UserEntity
                                         {
                                             Usr_UserTypeID = ut.UsT_UserTypeID,
                                             UserType = ut.UsT_UserType,

                                         }).ToList();
                    return UserTypesData;
                    //UserTypesData.Add(new UserEntity
                    //{
                    //    Usr_UserTypeID = 0,
                    //    UserType = "Select UserType"
                    //});
                    //return null;

                }
                catch (Exception ex)
                {
                    return null;

                }

            }


        }
        #endregion
        #region GetManagerNames

        public List<UserEntity> GetManagerNames(int AccountId)
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {

                    var taskNames = (from u in db.Users
                                     join up in db.UserProjects on u.Usr_UserID equals up.UProj_UserID
                                     where u.Usr_AccountID == AccountId && up.Is_L1_Manager == true
                                     select new UserEntity
                                     {
                                         Usr_UserID = u.Usr_UserID,
                                         Usr_Username = u.Usr_Username

                                     }).Distinct().ToList();

                    //taskNames.Add(new UserEntity
                    //{
                    //    Usr_TaskID=0,
                    //    Taskname = "Select TaskName"
                    //});
                    return taskNames;
                }
                catch (Exception ex)
                {
                    return null;

                }

            }

        }
        #endregion

        #region GetManagerNames

        public List<UserEntity> GetManagerNames2(int AccountId)
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var taskNames = (from u in db.Users
                                     join up in db.UserProjects on u.Usr_UserID equals up.UProj_UserID
                                     where u.Usr_AccountID == AccountId && up.Is_L1_Manager == true
                                     select new UserEntity
                                     {
                                         Usr_UserID = u.Usr_UserID,
                                         Usr_Username = u.Usr_Username

                                     }).Distinct().ToList();


                    //taskNames.Add(new UserEntity
                    //{
                    //    Usr_TaskID=0,
                    //    Taskname = "Select TaskName"
                    //});
                    return taskNames;
                }
                catch (Exception ex)
                {
                    return null;

                }

            }

        }
        #endregion

        #region GetTaskNamesForDropdown

        public List<UserEntity> GetTaskNames(int AccountId)
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var taskNames = (from t in db.GenericTasks
                                     where t.Acc_AccountID == AccountId
                                     select new UserEntity
                                     {
                                         Usr_TaskID = t.tsk_TaskID,
                                         Task_Name = t.tsk_TaskName

                                     }).ToList();

                    //taskNames.Add(new UserEntity
                    //{
                    //    Usr_TaskID=0,
                    //    Taskname = "Select TaskName"
                    //});
                    return taskNames;
                }
                catch (Exception ex)
                {
                    return null;

                }

            }

        }
        #endregion

        #region To update existing Profile Detail in Database

        //public int UpdateUserDetailByImage(UserEntity User)
        //{
        //    User _UserDtl = null;
        //    UsersProfile _UserProfileDtl = null;
        //    History_Users _UserHistory = new History_Users();

        //    int retVal = 0;

        //    using (var db = new DbContext(CONNECTION_NAME))
        //    {
        //        try
        //        {
        //            _UserDtl = db.Set<User>().Where(s => s.Usr_UserID == User.Usr_UserID).FirstOrDefault<User>();

        //            _UserProfileDtl = db.Set<UsersProfile>().Where(s => s.UsrP_UserID == User.Usr_UserID).FirstOrDefault<UsersProfile>();


        //            if (_UserDtl == null)
        //            {
        //                return retVal;
        //            }

        //            #region Saving History into History_Users Table

        //            //check for  _UserDtl.Usr_ModifiedDate as created date to history table
        //            DateTime dtAccCreatedDate = _UserDtl.Usr_ModifiedDate ?? DateTime.Now;

        //            db.Set<History_Users>().Add(new History_Users
        //            {
        //                History_Usr_UserID = _UserDtl.Usr_UserID,
        //                History_Usr_AccountID = _UserDtl.Usr_AccountID,
        //                History_Usr_UserTypeID = _UserDtl.Usr_UserTypeID,
        //                History_Usr_RoleID = _UserDtl.Usr_RoleID,
        //                History_Usr_Username = _UserDtl.Usr_Username,
        //                History_Usr_LoginId = _UserDtl.Usr_LoginId,
        //                History_Usr_Password = _UserDtl.Usr_Password,
        //                History_Usr_ActiveStatus = _UserDtl.Usr_ActiveStatus,
        //                History_Usr_Version = _UserDtl.Usr_Version,
        //                History_Usr_CreatedDate = dtAccCreatedDate,
        //                History_Usr_CreatedBy = _UserDtl.Usr_CreatedBy,
        //                History_Usr_ModifiedDate = _UserDtl.Usr_ModifiedDate,
        //                History_Usr_ModifiedBy = _UserDtl.Usr_ModifiedBy,
        //                History_Usr_isDeleted = _UserDtl.Usr_isDeleted
        //            });
        //            #endregion

        //            #region Saving User info Table

        //            _UserDtl.Usr_AccountID = User.Usr_AccountID;
        //            _UserDtl.Usr_UserTypeID = User.Usr_UserTypeID;
        //            _UserDtl.Usr_RoleID = User.Usr_RoleID;
        //            _UserDtl.Usr_Username = User.Usr_Username;
        //            _UserDtl.Usr_TaskID = User.Usr_TaskID;
        //            _UserDtl.Usr_Manager = User.Usr_Manager;
        //            _UserDtl.Usr_Manager2 = User.Usr_Manager2;
        //            _UserDtl.Usr_LoginId = User.Usr_LoginId;
        //            _UserDtl.Usr_Password = User.Usr_Password;
        //            _UserDtl.Usr_ActiveStatus = User.Usr_ActiveStatus;
        //            _UserDtl.Usr_Version = User.Usr_Version;
        //            _UserDtl.Usr_ModifiedDate = System.DateTime.Now;
        //            _UserDtl.Usr_ModifiedBy = User.Usr_ModifiedBy;
        //            _UserDtl.Usr_isDeleted = User.Usr_isDeleted;
        //            #endregion
        //            db.Entry(_UserDtl).State = System.Data.Entity.EntityState.Modified;



        //            _UserProfileDtl.UsrP_FirstName = User.UsrP_FirstName;
        //            _UserProfileDtl.UsrP_LastName = User.UsrP_LastName;
        //            _UserProfileDtl.Usrp_DOJ = User.Usrp_DOJ;
        //            //  _UserProfileDtl.Usrp_ProfilePicture = User.Usrp_ProfilePicture;
        //            _UserProfileDtl.UsrP_EmailID = User.UsrP_EmailID;
        //            _UserProfileDtl.UsrP_UserID = User.Usr_UserID;
        //            _UserProfileDtl.UsrP_ActiveStatus = User.Usr_ActiveStatus;
        //            _UserProfileDtl.UsrP_Version = User.Usr_Version;
        //            _UserProfileDtl.UsrP_CreatedBy = User.Usr_CreatedBy;
        //            _UserProfileDtl.UsrP_CreatedDate = System.DateTime.Now;
        //            _UserProfileDtl.UsrP_ModifiedDate = System.DateTime.Now;
        //            _UserProfileDtl.UsrP_ModifiedBy = User.Usr_CreatedBy;

        //            // _UserProfileDtl.UsrP_isDeleted = false;
        //            // Usr_salt= user.Usr_salt,
        //            db.Entry(_UserProfileDtl).State = System.Data.Entity.EntityState.Modified;



        //            db.SaveChanges();
        //            retVal = 1;
        //        }
        //        catch (Exception ex)
        //        {
        //            retVal = -1;
        //        }
        //        return retVal;
        //    }
        //}

        #endregion
        ////#region GetUserAccountNames

        ////public List<UserEntity> GetAccountNames(int userId)
        ////{

        ////    List<UserEntity> lstUsers = new List<UserEntity>();
        ////    try
        ////    {
        ////        using (var db = new EvolutyzCornerDataEntities())
        ////        {
        ////            //Role role=role.
        ////            int roleid = 56;
        ////            if (roleid == 56)
        ////            {
        ////                var accountNames = (from a in db.Accounts

        ////                                    select new UserEntity
        ////                                    {
        ////                                        Usr_AccountID = a.Acc_AccountID,
        ////                                        AccountName = a.Acc_AccountName

        ////                                    }).ToList();
        ////                accountNames.Add(new UserEntity
        ////                {
        ////                    Usr_AccountID = 0,
        ////                    AccountName = "Select AccountName"
        ////                });
        ////                lstUsers = accountNames;
        ////            }
        ////            else if (roleid == 56)
        ////            {
        ////                var accountNames = (from a in db.Accounts

        ////                                    select new UserEntity
        ////                                    {
        ////                                        Usr_AccountID = a.Acc_AccountID,
        ////                                        AccountName = a.Acc_AccountName

        ////                                    }).ToList();
        ////                accountNames.Add(new UserEntity
        ////                {
        ////                    Usr_AccountID = 0,
        ////                    AccountName = "Select AccountName"
        ////                });

        ////                lstUsers = accountNames;
        ////            }

        ////        }


        ////    }
        ////    catch(Exception ex)
        ////    {

        ////    }
        ////    return lstUsers;
        ////}


        //////public enum EvolutyzRoles
        //////{

        //////    SuperAdmin=500,
        //////    Admin=501,
        //////    CEO	=502,
        //////    CTO	=504,
        //////    VicePrecedent=	505,
        //////    Director=	506,
        //////    ProjectManager=	507,
        //////    HRManager= 	508,
        //////    TeamLead=	509,
        //////    Sr.SoftwareEngineer=510,
        //////    SoftwareEngineer=511,
        //////    Sr.TestEngineer=512,
        //////    TestEngineer=513,
        //////    UIDeveloper=514,
        //////    INTERN=	515,
        //////    DevOps= 516,
        //////    SystemAdmin=	517,
        //////    TechnicalTrainee=	518
        //////};



        public List<UserEntity> GetAccountNames(int acntID, int userid)
        {
            List<UserEntity> lstUsers = new List<UserEntity>();


            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var accountNames = (
                                         from a in db.Accounts
                                         join u in db.Users on a.Acc_AccountID equals u.Usr_AccountID
                                         join ut in db.UserTypes on u.Usr_UserTypeID equals ut.UsT_UserTypeID
                                         join R in db.Roles on u.Usr_RoleID equals R.Rol_RoleID
                                         join gr in db.GenericRoles on R.Rol_RoleName equals gr.GenericRoleID
                                         where a.Acc_AccountID == acntID && u.Usr_UserID == userid
                                         select new UserEntity
                                         {
                                             Usr_AccountID = a.Acc_AccountID,
                                             AccountName = a.Acc_AccountName

                                         }).ToList();
                    accountNames.Add(new UserEntity
                    {
                        Usr_AccountID = 0,
                        AccountName = "Select AccountName"
                    });

                    lstUsers = accountNames;
                }
                catch (Exception)
                {

                    return null;
                }
            }



            return lstUsers;
        }

        public List<UserEntity> GetAccountNamesbyid(int userId)
        {
            List<UserEntity> lstUsers = new List<UserEntity>();

            if (userId == 501)
            {
                using (var db = new EvolutyzCornerDataEntities())
                {

                    try
                    {

                        var accountNames = (from a in db.Accounts

                                            select new UserEntity
                                            {
                                                Usr_AccountID = a.Acc_AccountID,
                                                AccountName = a.Acc_AccountName

                                            }).ToList();
                        
                        lstUsers = accountNames;
                    }
                    catch (Exception)
                    {

                        return null;
                    }
                }

            }

            else if (userId == 502 || userId == 503)

            {
                using (var db = new EvolutyzCornerDataEntities())
                {
                    try
                    {
                        var accountNames = (from a in db.Accounts
                                            join u in db.Users on a.Acc_AccountID equals u.Usr_AccountID
                                            where u.Usr_UserID == userId
                                            select new UserEntity
                                            {
                                                Usr_AccountID = a.Acc_AccountID,
                                                AccountName = a.Acc_AccountName

                                            }).ToList();
                       

                        lstUsers = accountNames;
                    }
                    catch (Exception)
                    {

                        return null;
                    }
                }

            };

            return lstUsers;
        }
        
        public List<UserEntity> GetAccountNames1(int acntID, string RoleId)
        {
            List<UserEntity> lstUsers = new List<UserEntity>();

            if (RoleId == "Super Admin")
            {
                using (var db = new EvolutyzCornerDataEntities())
                {
                    try
                    {
                        var accountNames = (
                                             from a in db.Accounts
                                             where a.Acc_AccountID!=502
                                             select new UserEntity
                                             {
                                                 Usr_AccountID = a.Acc_AccountID,
                                                 AccountName = a.Acc_AccountName

                                             }).ToList();
                      

                        lstUsers = accountNames;
                    }
                    catch (Exception)
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
                        var accountNames = (
                                             from a in db.Accounts
                                             join u in db.Users on a.Acc_AccountID equals u.Usr_AccountID
                                             join ut in db.UserTypes on u.Usr_UserTypeID equals ut.UsT_UserTypeID
                                             join R in db.Roles on u.Usr_RoleID equals R.Rol_RoleID
                                             join gr in db.GenericRoles on R.Rol_RoleName equals gr.GenericRoleID
                                             where a.Acc_AccountID == acntID && gr.Title == RoleId
                                             select new UserEntity
                                             {
                                                 Usr_AccountID = a.Acc_AccountID,
                                                 AccountName = a.Acc_AccountName

                                             }).ToList();
                       

                        lstUsers = accountNames;
                    }
                    catch (Exception)
                    {

                        return null;
                    }
                }
            }
            lstUsers = lstUsers.OrderBy(p => p.Usr_AccountID).ToList();
            return lstUsers;
        }
        
        #region To update existing Profile Detail in Database

        public int UpdateUserDetailByImage(UserEntity User)
        {
            User _UserDtl = null;
            UsersProfile _UserProfileDtl = null;
           

            int retVal = 0;

            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    _UserDtl = db.Set<User>().Where(s => s.Usr_UserID == User.Usr_UserID).FirstOrDefault<User>();

                    _UserProfileDtl = db.Set<UsersProfile>().Where(s => s.UsrP_UserID == User.Usr_UserID).FirstOrDefault<UsersProfile>();


                    if (_UserDtl == null)
                    {
                        return retVal;
                    }

                    #region Saving User info Table

                    _UserDtl.Usr_AccountID = User.Usr_AccountID;
                    _UserDtl.Usr_UserTypeID = User.Usr_UserTypeID;
                    _UserDtl.Usr_RoleID = User.Usr_RoleID;
                    _UserDtl.Usr_Username = User.Usr_Username;
                    _UserDtl.Usr_TaskID = User.Usr_TaskID;
                    _UserDtl.Usr_Manager = User.Usr_Manager;
                    _UserDtl.Usr_Manager2 = User.Usr_Manager2;
                    _UserDtl.Usr_LoginId = User.Usr_LoginId;
                    _UserDtl.Usr_Password = User.Usr_Password;
                    _UserDtl.Usr_ActiveStatus = User.Usr_ActiveStatus;
                    _UserDtl.Usr_Version = User.Usr_Version;
                    _UserDtl.Usr_ModifiedDate = System.DateTime.Now;
                    _UserDtl.Usr_ModifiedBy = User.Usr_ModifiedBy;
                    _UserDtl.Usr_isDeleted = User.Usr_isDeleted;
                    #endregion
                    db.Entry(_UserDtl).State = System.Data.Entity.EntityState.Modified;



                    _UserProfileDtl.UsrP_FirstName = User.UsrP_FirstName;
                    _UserProfileDtl.UsrP_LastName = User.UsrP_LastName;
                    _UserProfileDtl.Usrp_DOJ = User.Usrp_DOJ;
                   // _UserProfileDtl.UsrP_EmployeeID = User.UsrP_EmployeeID;
                    //  _UserProfileDtl.Usrp_ProfilePicture = User.Usrp_ProfilePicture;
                    _UserProfileDtl.UsrP_EmailID = User.UsrP_EmailID;
                    _UserProfileDtl.UsrP_UserID = User.Usr_UserID;
                    _UserProfileDtl.UsrP_ActiveStatus = User.Usr_ActiveStatus;
                    _UserProfileDtl.UsrP_Version = User.Usr_Version;
                    _UserProfileDtl.UsrP_CreatedBy = User.Usr_CreatedBy;
                    _UserProfileDtl.UsrP_CreatedDate = System.DateTime.Now;
                    _UserProfileDtl.UsrP_ModifiedDate = System.DateTime.Now;
                    _UserProfileDtl.UsrP_ModifiedBy = User.Usr_CreatedBy;
                    _UserProfileDtl.Usr_Titleid = User.Usr_Titleid;

                    // _UserProfileDtl.UsrP_isDeleted = false;
                    // Usr_salt= user.Usr_salt,
                    db.Entry(_UserProfileDtl).State = System.Data.Entity.EntityState.Modified;



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

        #region GetUserAccountNames

        public List<UserEntity> GetAccountNames()
        {
            using (var db = new EvolutyzCornerDataEntities())
            {

                try
                {
                    var accountNames = (from a in db.Accounts
                                        where a.Acc_isDeleted == false
                                        select new UserEntity
                                        {
                                            Usr_AccountID = a.Acc_AccountID,
                                            AccountName = a.Acc_AccountName

                                        }).ToList();


                    return accountNames;
                }
                catch (Exception)
                {

                    return null;
                }
            }

        }
        #endregion
        
        public string ChangeStatus(string id, string status)
        {
            string strResponse = string.Empty;
            User holidayData = null;
            bool Status = Convert.ToBoolean(status);
            int did = Convert.ToInt32(id);
            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    holidayData = db.Set<User>().Where(s => s.Usr_UserID == did).FirstOrDefault<User>();
                    if (holidayData == null)
                    {
                        return null;
                    }
                    holidayData.Usr_ActiveStatus = Status;
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
        public string SkillChangeStatus(string id, string status)
        {
            string strResponse = string.Empty;
            Skill holidayData = null;
            int? Status=0;
            if (status == "true")
            {
                  Status = 1;
            }
            else
            {
                Status = 0;
            }
            //int? Status = Convert.ToInt32(status);
            int did = Convert.ToInt32(id);
            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    holidayData = db.Set<Skill>().Where(s => s.SkillId == did).FirstOrDefault<Skill>();
                    if (holidayData == null)
                    {
                        return null;
                    }
                    holidayData.StatusID = Status;
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
