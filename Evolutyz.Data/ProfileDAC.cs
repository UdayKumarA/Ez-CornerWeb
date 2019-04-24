using Evolutyz.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Evolutyz.Data
{
    public class ProfileDAC : DataAccessComponent
    {
        #region To add Role Detail in Database
        public int AddProfile(UserProfileEntity user)
        {
            int retVal = 0;

            UsersProfile User = new UsersProfile();

            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    User = db.Set<UsersProfile>().Where(u => u.UsrP_UserID == user.UsrP_UserID).FirstOrDefault<UsersProfile>();
                    if (User != null)
                    {
                        return retVal;
                    }
                    db.Set<UsersProfile>().Add(new UsersProfile
                    {
                        UsrP_UserID = user.UsrP_UserID,
                        UsrP_FirstName = user.UsrP_FirstName,
                        UsrP_LastName = user.UsrP_LastName,
                        UsrP_EmployeeID = user.UsrP_EmployeeID,
                        UsrP_EmailID = user.UsrP_EmailID,
                        Usrp_MobileNumber = user.Usrp_MobileNumber,
                        //Usrp_PhoneNumber = user.Usrp_PhoneNumber,
                        UsrP_DOB = user.UsrP_DOB,
                        Usrp_DOJ = user.Usrp_DOJ,
                        UsrP_ActiveStatus = user.UsrP_ActiveStatus,
                        UsrP_Version = user.UsrP_Version,
                        UsrP_CreatedBy = user.UsrP_CreatedBy,
                        UsrP_CreatedDate = System.DateTime.Now,
                        UsrP_isDeleted = false

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

        #region To update existing Profile Detail in Database

        public int UpdateProfileDetail(UserProfileEntity Profile)
        {
            UsersProfile Userprofile = null;
            //UsersProfile Userprofile = new UsersProfile();

            int retVal = 0;

            try
            {

                using (var db = new DbContext(CONNECTION_NAME))
                {
                    Userprofile = db.Set<UsersProfile>().Where(s => s.UsrP_UserProfileID == Profile.UsrP_UserProfileID).FirstOrDefault<UsersProfile>();

                    if (Userprofile == null)
                    {
                        return retVal;
                    }

                    #region Saving ProjectAllocation info Table

                    Userprofile.UsrP_FirstName = Profile.UsrP_FirstName;
                    Userprofile.UsrP_LastName = Profile.UsrP_LastName;
                    Userprofile.Usrp_ProfilePicture = Profile.Usrp_ProfilePicture;
                    Userprofile.Usrp_DOJ = Profile.Usrp_DOJ;
                    Userprofile.UsrP_DOB = Profile.UsrP_DOB;
                    Userprofile.Usrp_MobileNumber = Profile.Usrp_MobileNumber;
                    Userprofile.UsrP_EmailID = Profile.UsrP_EmailID;
                    Userprofile.TemporaryAddress = Profile.TemporaryAddress;
                    Userprofile.PermanentAddress = Profile.PermanentAddress;
                    Userprofile.UsrP_ActiveStatus = Profile.UsrP_ActiveStatus;
                    Userprofile.UsrP_Version = Profile.UsrP_Version;
                    //Userprofile.UsrP_CreatedDate = System.DateTime.Now;
                    //Userprofile.UsrP_CreatedBy = Profile.UsrP_CreatedBy;
                    Userprofile.UsrP_ModifiedDate = System.DateTime.Now;
                    Userprofile.UsrP_ModifiedBy = Profile.UsrP_ModifiedBy;
                    Userprofile.UsrP_isDeleted = Profile.UsrP_isDeleted;
                    Userprofile.Marital_Status = Profile.Marital_Status;
                    Userprofile.Usr_GenderId = Profile.Usr_GenderId;
                   // Userprofile.Usr_Titleid = Profile.Usr_Titleid;
                   // Userprofile.UsrP_EmployeeID = Profile.UsrP_EmployeeID;
                   // Userprofile.Usrp_PhoneNumber = Profile.Usrp_PhoneNumber;
                    #endregion
                    db.Entry(Userprofile).State = System.Data.Entity.EntityState.Modified;

                    retVal = db.SaveChanges();
                    retVal = 1;

                };

            }

            catch (Exception ex)
            {
                var exc = ex;
                retVal = -1;
            }
            return retVal;
        }

        #endregion
        #region To update existing Profile Detail in Database

        public int UpdateProfileDetailByImage(UserProfileEntity Profile)
        {
            UsersProfile Userprofile = null;
            //UsersProfile Userprofile = new UsersProfile();

            int retVal = 0;

            try
            {

                using (var db = new DbContext(CONNECTION_NAME))
                {
                    Userprofile = db.Set<UsersProfile>().Where(s => s.UsrP_UserID == Profile.UsrP_UserID).FirstOrDefault<UsersProfile>();

                    if (Userprofile == null)
                    {
                        return retVal;
                    }

                    #region Saving ProjectAllocation info Table

                    Userprofile.UsrP_FirstName = Profile.UsrP_FirstName;
                    Userprofile.UsrP_LastName = Profile.UsrP_LastName;
                    //  Userprofile.Usrp_ProfilePicture = Profile.Usrp_ProfilePicture;
                    Userprofile.Usrp_DOJ = Profile.Usrp_DOJ;
                    Userprofile.UsrP_DOB = Profile.UsrP_DOB;
                    Userprofile.Usrp_MobileNumber = Profile.Usrp_MobileNumber;
                    Userprofile.UsrP_EmailID = Profile.UsrP_EmailID;
                    Userprofile.TemporaryAddress = Profile.TemporaryAddress;
                    Userprofile.PermanentAddress = Profile.PermanentAddress;
                    Userprofile.UsrP_ActiveStatus = Profile.UsrP_ActiveStatus;
                    Userprofile.UsrP_Version = Profile.UsrP_Version;
                   // Userprofile.UsrP_CreatedDate = System.DateTime.Now;
                 //   Userprofile.UsrP_CreatedBy = Profile.UsrP_CreatedBy;
                    Userprofile.UsrP_ModifiedDate = System.DateTime.Now;
                    Userprofile.UsrP_ModifiedBy = Profile.UsrP_ModifiedBy;
                    Userprofile.UsrP_isDeleted = Profile.UsrP_isDeleted;
                    Userprofile.Marital_Status = Profile.Marital_Status;
                    Userprofile.Usr_GenderId = Profile.Usr_GenderId;
                   // Userprofile.Usr_Titleid = Profile.Usr_Titleid;
                  //  Userprofile.UsrP_EmployeeID = Profile.UsrP_EmployeeID;
                   // Userprofile.Usrp_PhoneNumber = Profile.Usrp_PhoneNumber;
                    #endregion
                    db.Entry(Userprofile).State = System.Data.Entity.EntityState.Modified;

                    retVal = db.SaveChanges();
                    retVal = 1;

                };

            }

            catch (Exception ex)
            {
                var exc = ex;
                retVal = -1;
            }
            return retVal;
        }

        #endregion
        

        #region To get particular profile details from Database
        public UserProfileEntity GetProfileDetailByID(int ID)
        {
            UserProfileEntity response = new UserProfileEntity();

            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {

                    response = (from q in db.UsersProfiles
                                    //join a in db.Accounts on q.Proj_AccountID equals a.Acc_AccountID
                                where q.UsrP_UserID == ID
                                select new UserProfileEntity
                                {
                                    UsrP_UserProfileID = q.UsrP_UserProfileID,
                                    //Proj_AccountID = q.Proj_AccountID,
                                    // AccountName = a.Acc_AccountName,
                                    //Proj_ProjectCode = q.Proj_ProjectCode,
                                    UsrP_FirstName = q.UsrP_FirstName,
                                    UsrP_LastName = q.UsrP_LastName,
                                    UsrP_DOB = q.UsrP_DOB,
                                    Usrp_DOJ = q.Usrp_DOJ,
                                    UsrP_EmailID = q.UsrP_EmailID,
                                    Usrp_MobileNumber = q.Usrp_MobileNumber,
                                    PermanentAddress = q.PermanentAddress,
                                    TemporaryAddress = q.TemporaryAddress,
                                    Marital_Status = q.Marital_Status,
                                    Usr_GenderId = q.Usr_GenderId,
                                    UsrP_isDeleted = q.UsrP_isDeleted,
                                    UsrP_Version = q.UsrP_Version,
                                    UsrP_ActiveStatus = q.UsrP_ActiveStatus,
                                    Usrp_ProfilePicture = q.Usrp_ProfilePicture,
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
        #region To get particular profile details from Database
        public List<UserProfileEntity> GetSocialMediaByID(int ID)
        {
            List<UserProfileEntity> response = new List<UserProfileEntity>();

            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {

                    response = (from q in db.UserSocialMediaProfiles
                                  
                                where q.Usmp_UserID == ID
                                select new UserProfileEntity
                                {
                                   SocialMediaId = q.Usmp_SocialMediaId,
                                   Url = q.Url
                                }).ToList();

                   // response.IsSuccessful = true;
                    return response;
                }
                catch (Exception ex)
                {

                    return response;
                }
            }
        }
        #endregion

        #region To get particular profile details from Database
        public UserProfileEntity GetProfileDetailid(int ID)
        {
            UserProfileEntity response = new UserProfileEntity();

            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {

                    response = (from q in db.UsersProfiles
                                    //join a in db.Accounts on q.Proj_AccountID equals a.Acc_AccountID
                                where q.UsrP_UserID == ID
                                select new UserProfileEntity
                                {
                                    UsrP_UserProfileID = q.UsrP_UserProfileID,
                                    //Proj_AccountID = q.Proj_AccountID,
                                    // AccountName = a.Acc_AccountName,
                                    //Proj_ProjectCode = q.Proj_ProjectCode,
                                    //UsrP_FirstName = q.UsrP_FirstName,
                                    //UsrP_LastName = q.UsrP_LastName,
                                    //UsrP_DOB = q.UsrP_DOB,
                                    //Usrp_DOJ = q.Usrp_DOJ,
                                    //UsrP_EmailID = q.UsrP_EmailID,
                                    //Usrp_MobileNumber = q.Usrp_MobileNumber,
                                    //PermanentAddress = q.PermanentAddress,
                                    //TemporaryAddress = q.TemporaryAddress,
                                    //Marital_Status = q.Marital_Status,
                                    //Usr_GenderId = q.Usr_GenderId,
                                    //UsrP_isDeleted = q.UsrP_isDeleted,
                                    //UsrP_Version = q.UsrP_Version,
                                    //UsrP_ActiveStatus = q.UsrP_ActiveStatus,
                                    //Usrp_ProfilePicture = q.Usrp_ProfilePicture,
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

        public List<SkillEntity> GetSkillList(int id)
        {
            List<SkillEntity> response = new List<SkillEntity>();
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    response = (from h in db.Skills
                                where h.Acc_AccountID == id 
                                select new SkillEntity
                                {
                                    SkillId = h.SkillId,
                                    SkillTitle = h.SkillTitle,
                                   
                                }).ToList();


                    return response;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public string DeleteSkills(int id)
        {
            string strResponse = string.Empty;
            UserSkill holidayData = null;
           
            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    holidayData = db.Set<UserSkill>().Where(s => s.Usr_SkillId == id).FirstOrDefault<UserSkill>();
                    if (holidayData == null)
                    {
                        return null;
                    }
                    holidayData.Is_Deleted = true;
                    // holidayData.isActive = false;
                    db.Entry(holidayData).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    strResponse = "Record successfully deleted";
                }
                catch (Exception ex)
                {
                    strResponse = ex.Message.ToString();
                }
            }
            return strResponse;
        }

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

        public string CheckPassword(string oldpassword, string newpassword)
        {
            string strResponse = string.Empty;
            User holidayData = null;
            string OldPassword = GetMD5(oldpassword);
            string newPassword = GetMD5(newpassword);
            UserSessionInfo info = new UserSessionInfo();
            int userid = info.UserId;
            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    holidayData = db.Set<User>().Where(s => (s.Usr_UserID == userid && s.Usr_Password== OldPassword)).FirstOrDefault<User>();
                    if (holidayData == null)
                    {
                        return strResponse= "Please Enter Correct Password";
                    }
                    holidayData.Usr_Password = newPassword; 
                  
                    db.Entry(holidayData).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    strResponse = "Password Updated Successfully";
                }
                catch (Exception ex)
                {
                    strResponse = ex.Message.ToString();
                }
            }
            return strResponse;
        }
        
        public List<UserProfileEntity> GetAllSocialMediaIcons(int accountid)
        {
            List<UserProfileEntity> response = new List<UserProfileEntity>();
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    response = (from h in db.SocialMedias
                                //where h.Sm_AccountID == accountid
                                select new UserProfileEntity
                                {
                                    SocialMediaId= h.SocialMediaId,
                                    Icon = h.Icon,
                                    Title = h.Title

                                }).ToList();


                    return response;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public List<UserProfileEntity> GetUserSocialMediaurls(int userid)
        {
            List<UserProfileEntity> response = new List<UserProfileEntity>();
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    response = (from h in db.UserSocialMediaProfiles
                                where h.Usmp_UserID == userid
                                select new UserProfileEntity
                                {
                                    SocialMediaId = h.Usmp_SocialMediaId,
                                    Url = h.Url

                                }).ToList();


                    return response; 
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        
        public string AddUserSocialMedia(List<UserProfileEntity> jsonobj)
        {
            string strresponse = "";

            try
            {

                for (int i = 0; i <= jsonobj.Count - 1; i++)
                {
                    using (var db = new DbContext(CONNECTION_NAME))
                    {

                        db.Set<UserSocialMediaProfile>().Add(new UserSocialMediaProfile
                        {
                            Usmp_UserID = jsonobj[i].Usmp_UserID,
                            Usmp_SocialMediaId = jsonobj[i].Usmp_SocialMediaId,
                            Url = jsonobj[i].Url,
                            Usmp_CreatedDate = DateTime.Now,
                            
                        }); 
                        db.SaveChanges();
                        strresponse = "Successfully Added";

                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return strresponse;
        }

        public string UpdateUserSocialMedia(List<UserProfileEntity> jsonobj)
        {
            string strresponse = "";
            UserSocialMediaProfile ques = null;
            try
            {

                for (int i = 0; i <= jsonobj.Count - 1; i++)
                {
                    using (var db = new DbContext(CONNECTION_NAME))
                    {
                        var mediaid = Convert.ToInt32(jsonobj[i].Usmp_SocialMediaId);
                        var userid = Convert.ToInt32(jsonobj[i].Usmp_UserID);
                        ques = db.Set<UserSocialMediaProfile>().Where(s => (s.Usmp_UserID == userid) && (s.Usmp_SocialMediaId == mediaid)).FirstOrDefault<UserSocialMediaProfile>();
                        if (ques != null)
                        {
                            ques.Url = jsonobj[i].Url;
                        }
                        db.SaveChanges();

                        strresponse = "Successfully updated";

                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return strresponse;
        }
    }
}
