using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evolutyz.Entities;
using evolCorner.Models;

namespace Evolutyz.Data
{
    public class LoginDAC
    {
        #region to check the user before login 
        public User LoginMethod(LoginEntity ent)
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    //var saltcode = (from q in db.Users where q.Usr_LoginId == ent.UserName select q).ToList().FirstOrDefault();
                    //if(saltcode!=null)
                    //{
                    //    ent.Vcode = saltcode.Usr_salt;
                    //    var hashCode = ent.Vcode;
                    //    //Password Hasing Process Call Helper Class Method    
                    //    var encodingPasswordString = Helper.EncodePassword(ent.Password, hashCode);
                    //    //Check Login Detail User Name Or Password 

                    var query = (from q in db.Users
                                 where q.Usr_LoginId == ent.UserName && q.Usr_Password == ent.Password
                                 select q).ToList().FirstOrDefault();
                    if (query != null)
                    {
                        //UserSessionInfo objSessioninfo = new UserSessionInfo();
                        //objSessioninfo.UserId = query.Usr_UserID;
                        //objSessioninfo.UserName = query.Usr_LoginId;
                        //objSessioninfo.AccountId =query.Usr_AccountID;
                        //Session["UserSessionInfo"] = objSessioninfo;
                        return query;
                    }
                    //}
                    //else
                    //{
                    //    return null;
                    //}



                    //return query;
                }
                catch (Exception ex)
                {
                    return null;
                }
                return null;
            }
        }

        public UserEntity ValidateLogin(LoginEntity ent)
        {
            try
            {
                using (var db = new EvolutyzCornerDataEntities())
                {
                   
                    try
                    {
                      
                        var response = (from u in db.Users
                                        join up  in db.UsersProfiles on u.Usr_UserID equals up.UsrP_UserID
                                        into upro from rt in upro.DefaultIfEmpty()
                                        join ut in db.UserTypes on u.Usr_UserTypeID equals ut.UsT_UserTypeID
                                        join r in db.Roles on u.Usr_RoleID equals r.Rol_RoleID
                                        join a in db.Accounts on u.Usr_AccountID equals a.Acc_AccountID
                                        join gr in db.GenericRoles on r.Rol_RoleName equals gr.GenericRoleID
                                       // join upr in db.UserProjects on u.Usr_UserID equals upr.UProj_UserID
                                        where u.Usr_LoginId == ent.UserName && u.Usr_Password == ent.Password && u.Usr_isDeleted == false ||
                                               u.Usr_Username==ent.UserName&&u.Usr_Password==ent.Password  && u.Usr_isDeleted==false      // ent.Password
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
                                           UsrP_FirstName = rt.UsrP_FirstName,
                                            UsrP_LastName = rt.UsrP_LastName,
                                           UserName=gr.Title,
                                           RoleId = gr.GenericRoleID,
                                          
                                            isusacc =a.is_UsAccount,
                                           Usrp_ProfilePicture =rt.Usrp_ProfilePicture,
                                           
                                        }).FirstOrDefault();

                        response.IsSuccessful = true;
                        return response;
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region
        public UserProjectdetailsEntity GetUserProjectsDetails(UserSessionInfo ent)
        {
            try
            {

                using (var db = new EvolutyzCornerDataEntities())
                {

                    try
                    {
                        var response = (from u in db.Users

                                        join ut in db.UserTypes on u.Usr_UserTypeID equals ut.UsT_UserTypeID
                                        join r in db.Roles on u.Usr_RoleID equals r.Rol_RoleID
                                        join a in db.Accounts on u.Usr_AccountID equals a.Acc_AccountID
                                        join up in db.UserProjects on u.Usr_UserID equals up.UProj_UserID
                                        join p in db.Projects on up.UProj_ProjectID equals p.Proj_ProjectID
                                        join ups in db.UsersProfiles on u.Usr_UserID equals ups.UsrP_UserID
                                        join t in db.AccountSpecificTasks on u.Usr_TaskID equals t.Acc_SpecificTaskId
                                        join cp in db.ClientProjects on up.ClientprojID equals cp.CL_ProjectID
                                        where u.Usr_LoginId == ent.LoginId && u.Usr_Password == ent.Password // ent.Password
                                        select new UserProjectdetailsEntity
                                        {
                                            User_ID = u.Usr_UserID,
                                            Usr_Username = ups.UsrP_FirstName + ups.UsrP_LastName,
                                            AccountName = a.Acc_AccountName,
                                            Proj_ProjectID = p.Proj_ProjectID,
                                            projectName = p.Proj_ProjectName,
                                            ProjectClientName = cp.ClientProjTitle,
                                            tsktaskID = t.Acc_SpecificTaskId,


                                        }).FirstOrDefault();


                        return response;
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion


        public UserEntity GerUserDetails(string UserName)
        {
                using (var db = new EvolutyzCornerDataEntities())
                {
                    
                    try
                    {

                        var response = (from u in db.Users
                                        join up in db.UsersProfiles on u.Usr_UserID equals up.UsrP_UserID
                                        into upro
                                        from rt in upro.DefaultIfEmpty()
                                        join ut in db.UserTypes on u.Usr_UserTypeID equals ut.UsT_UserTypeID
                                        join r in db.Roles on u.Usr_RoleID equals r.Rol_RoleID
                                        join a in db.Accounts on u.Usr_AccountID equals a.Acc_AccountID
                                        join gr in db.GenericRoles on r.Rol_RoleName equals gr.GenericRoleID
                                        where u.Usr_LoginId == UserName ||
                                               u.Usr_Username == UserName        // ent.Password
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
                                            UsrP_EmployeeID= rt.UsrP_EmployeeID,
                                            Usr_Password = u.Usr_Password,
                                            UsrP_FirstName = rt.UsrP_FirstName,
                                            UsrP_LastName = rt.UsrP_LastName,
                                            UserName = gr.Title,
                                            Usrp_ProfilePicture = rt.Usrp_ProfilePicture,

                                        }).FirstOrDefault();

                        response.IsSuccessful = true;
                        return response;
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                    return null;
                }
         

        }
        public string SaveToken(string token, int uid)
        {
            User userdtl = null;
            string response = string.Empty;

            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    userdtl = db.Set<User>().Where(s => s.Usr_UserID == uid).FirstOrDefault<User>();

                    if (userdtl == null)
                    {
                        response = "Please Reset Password again";
                        return null;
                    }

                    //int roleid = db.Set<Role>().Where(r => (r.Rol_AccountID == accountid && r.Rol_RoleName == rolename)).FirstOrDefault<Role>().Rol_RoleID;

                    User u = new User();


                    userdtl.Url_token = token;
                    db.Entry(userdtl).State = System.Data.Entity.EntityState.Modified;
                    //db.Set<User>().Add(u);
                    db.SaveChanges();


                    response = "Reset Link Send To Your Email ";


                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return response;
        }

        public string UpdatePasswordandtoken(string token, string Newpassword)
        {
            User userdtl = null;
            string response = string.Empty;
            string Token = token.Trim();
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    userdtl = db.Set<User>().Where(s => s.Url_token == Token).FirstOrDefault<User>();

                    if (userdtl == null)
                    {
                        response = "Please Reset Password again";
                        return null;
                    }

                    //int roleid = db.Set<Role>().Where(r => (r.Rol_AccountID == accountid && r.Rol_RoleName == rolename)).FirstOrDefault<Role>().Rol_RoleID;

                    User u = new User();


                    userdtl.Url_token =null;
                    userdtl.Usr_Password = Newpassword;
                    db.Entry(userdtl).State = System.Data.Entity.EntityState.Modified;
                    //db.Set<User>().Add(u);
                    db.SaveChanges();


                    response = "Password Reset  Successfull";


                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return response;
        }

    }
}
