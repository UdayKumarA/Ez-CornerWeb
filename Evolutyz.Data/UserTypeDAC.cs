using Evolutyz.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Evolutyz.Data
{
    public class UserTypeDAC : DataAccessComponent
    {
        #region To add UserType Detail in Database
        public int AddUserType(UserTypeEntity user)
        {
            int retVal = 0;
            UserType userType = new UserType();
            UserType CodeCheck = new UserType();
            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    userType = db.Set<UserType>().Where(s => s.UsT_UserTypeID == user.UsT_UserTypeID).FirstOrDefault<UserType>();
                    CodeCheck = db.Set<UserType>().Where(s => s.UsT_UserTypeCode == user.UsT_UserTypeCode).FirstOrDefault<UserType>();
                    UserType TypeCheck = db.Set<UserType>().Where(s => s.UsT_UserTypeCode == user.UsT_UserTypeCode).FirstOrDefault<UserType>();
                    if (CodeCheck != null)
                    {
                        return retVal=2;
                    }
                    if (TypeCheck != null)
                    {
                        return retVal = 3;
                    }
                    if (userType != null)
                    {
                        return retVal;
                    }
                    db.Set<UserType>().Add(new UserType
                    {
                        UsT_AccountID = user.UsT_AccountID,
                        UsT_UserTypeCode = user.UsT_UserTypeCode,
                        UsT_UserType = user.UsT_UserType,
                        UsT_UserTypeDescription = user.UsT_UserTypeDescription,
                        UsT_ActiveStatus = user.UsT_ActiveStatus,
                        UsT_Version = user.UsT_Version,
                        UsT_CreatedBy = user.UsT_CreatedBy,
                        UsT_CreatedDate = System.DateTime.Now,
                        UsT_isDeleted = false
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

        #region To update existing UserType Detail in Database
        public int UpdateUserTypeDetail(UserTypeEntity userType)
        {
            UserType _userTypeDtl = null;
            History_UserType _userTypeHistory = new History_UserType();

            int retVal = 0;

            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    _userTypeDtl = db.Set<UserType>().Where(s => s.UsT_UserTypeID == userType.UsT_UserTypeID).FirstOrDefault<UserType>();

                    if (_userTypeDtl == null)
                    {
                        return retVal;
                    }

                    #region Saving History into History_UserType Table

                    //check for  _userTypeDtl.UsT_ModifiedDate as created date to history table
                    DateTime dtAccCreatedDate = _userTypeDtl.UsT_ModifiedDate ?? DateTime.Now;

                    db.Set<History_UserType>().Add(new History_UserType
                    {
                        History_UsT_UserTypeID = _userTypeDtl.UsT_UserTypeID,
                        History_AccountID = _userTypeDtl.UsT_AccountID,
                        History_UsT_UserTypeCode = _userTypeDtl.UsT_UserTypeCode,
                        History_UsT_UserType = _userTypeDtl.UsT_UserType,
                        History_UsT_UserTypeDescription = _userTypeDtl.UsT_UserTypeDescription,
                        History_UsT_ActiveStatus = _userTypeDtl.UsT_ActiveStatus,
                        History_UsT_Version = _userTypeDtl.UsT_Version,
                        History_UsT_CreatedDate = dtAccCreatedDate,
                        History_UsT_CreatedBy = _userTypeDtl.UsT_CreatedBy,
                        History_UsT_ModifiedDate = _userTypeDtl.UsT_ModifiedDate,
                        History_UsT_ModifiedBy = _userTypeDtl.UsT_ModifiedBy,
                        History_UsT_isDeleted = _userTypeDtl.UsT_isDeleted
                    });
                    #endregion

                    #region Saving UserType info Table

                    _userTypeDtl.UsT_AccountID = userType.UsT_AccountID;
                    _userTypeDtl.UsT_UserTypeCode = userType.UsT_UserTypeCode;
                    _userTypeDtl.UsT_UserType = userType.UsT_UserType;
                    _userTypeDtl.UsT_UserTypeDescription = userType.UsT_UserTypeDescription;
                    _userTypeDtl.UsT_ActiveStatus = userType.UsT_ActiveStatus;
                    _userTypeDtl.UsT_Version = userType.UsT_Version;
                    _userTypeDtl.UsT_ModifiedDate = System.DateTime.Now;
                    _userTypeDtl.UsT_ModifiedBy = userType.UsT_ModifiedBy;
                    _userTypeDtl.UsT_isDeleted = userType.UsT_isDeleted;
                    #endregion
                    db.Entry(_userTypeDtl).State = System.Data.Entity.EntityState.Modified;
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

        #region To delete existing UserType details from Database
        public int DeleteUserTypeDetail(int ctID)
        {
            int retVal = 0;
            UserType _UserTypeDtl = null;

            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    _UserTypeDtl = db.Set<UserType>().Where(s => s.UsT_UserTypeID == ctID).FirstOrDefault<UserType>();
                    if (_UserTypeDtl == null)
                    {
                        return retVal;
                    }
                    _UserTypeDtl.UsT_isDeleted = true;
                    db.Entry(_UserTypeDtl).State = System.Data.Entity.EntityState.Modified;
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

        #region To get all details of UserType from Database
        public List<UserTypeEntity> GetUserTypeDetail(int acntID, string RoleId)
        {
            if (RoleId == "Super Admin")

            {
                using (var db = new EvolutyzCornerDataEntities())
                {
                    try
                    {
                        var query = (from a in db.Accounts
                                     join q in db.UserTypes on a.Acc_AccountID equals q.UsT_AccountID
                                     //where a.Acc_AccountID == acntID && 
                                     where q.UsT_isDeleted == false


                                     //from a in db.Accounts
                                     //join u in db.Users on a.Acc_AccountID equals u.Usr_AccountID
                                     //join q in db.UserTypes on u.Usr_UserTypeID equals q.UsT_UserTypeID
                                     //join R in db.Roles on u.Usr_RoleID equals R.Rol_RoleID
                                     //join gr in db.GenericRoles on R.Rol_RoleName equals gr.GenericRoleID
                                     //where a.Acc_AccountID == acntID  && u.Usr_isDeleted == false
                                     select new UserTypeEntity
                                     {
                                         UsT_UserTypeID = q.UsT_UserTypeID,
                                         UsT_AccountID = q.UsT_AccountID,
                                         AccountName = a.Acc_AccountName,
                                         UsT_UserTypeCode = q.UsT_UserTypeCode,
                                         UsT_UserType = q.UsT_UserType,
                                         UsT_UserTypeDescription = q.UsT_UserTypeDescription,
                                         UsT_ActiveStatus = q.UsT_ActiveStatus,
                                         UsT_Version = q.UsT_Version,
                                         UsT_CreatedBy = q.UsT_CreatedBy,
                                         UsT_CreatedDate = q.UsT_CreatedDate,
                                         UsT_ModifiedBy = q.UsT_ModifiedBy,
                                         UsT_ModifiedDate = q.UsT_ModifiedDate,
                                         UsT_isDeleted = q.UsT_isDeleted,
                                     }).OrderBy(x => x.UsT_UserType).ThenBy(x => x.UsT_UserType).ToList();

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
                        var query = (from a in db.Accounts
                                     join q in db.UserTypes on a.Acc_AccountID equals q.UsT_AccountID
                                     where a.Acc_AccountID == acntID && q.UsT_isDeleted == false


                                     //from a in db.Accounts
                                     //join u in db.Users on a.Acc_AccountID equals u.Usr_AccountID
                                     //join q in db.UserTypes on u.Usr_UserTypeID equals q.UsT_UserTypeID
                                     //join R in db.Roles on u.Usr_RoleID equals R.Rol_RoleID
                                     //join gr in db.GenericRoles on R.Rol_RoleName equals gr.GenericRoleID
                                     //where a.Acc_AccountID == acntID  && u.Usr_isDeleted == false
                                     select new UserTypeEntity
                                     {
                                         UsT_UserTypeID = q.UsT_UserTypeID,
                                         UsT_AccountID = q.UsT_AccountID,
                                         AccountName = a.Acc_AccountName,
                                         UsT_UserTypeCode = q.UsT_UserTypeCode,
                                         UsT_UserType = q.UsT_UserType,
                                         UsT_UserTypeDescription = q.UsT_UserTypeDescription,
                                         UsT_ActiveStatus = q.UsT_ActiveStatus,
                                         UsT_Version = q.UsT_Version,
                                         UsT_CreatedBy = q.UsT_CreatedBy,
                                         UsT_CreatedDate = q.UsT_CreatedDate,
                                         UsT_ModifiedBy = q.UsT_ModifiedBy,
                                         UsT_ModifiedDate = q.UsT_ModifiedDate,
                                         UsT_isDeleted = q.UsT_isDeleted,
                                     }).OrderBy(x => x.UsT_UserType).ThenBy(x => x.UsT_UserType).ToList();

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

        #region To get particular UserType details from Database
        public UserTypeEntity GetUserTypeDetailByID(int ID)
        {
            UserTypeEntity response = new UserTypeEntity();

            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    response = (from q in db.UserTypes
                                join a in db.Accounts on q.UsT_AccountID equals a.Acc_AccountID
                                where q.UsT_UserTypeID == ID
                                select new UserTypeEntity
                                {
                                    UsT_UserTypeID = q.UsT_UserTypeID,
                                    UsT_AccountID = q.UsT_AccountID,
                                    AccountName = a.Acc_AccountName,
                                    UsT_UserTypeCode = q.UsT_UserTypeCode,
                                    UsT_UserType = q.UsT_UserType,
                                    UsT_UserTypeDescription = q.UsT_UserTypeDescription,
                                    UsT_ActiveStatus = q.UsT_ActiveStatus,
                                    UsT_Version = q.UsT_Version,
                                    UsT_CreatedBy = q.UsT_CreatedBy,
                                    UsT_CreatedDate = q.UsT_CreatedDate,
                                    UsT_ModifiedBy = q.UsT_ModifiedBy,
                                    UsT_ModifiedDate = q.UsT_ModifiedDate,
                                    UsT_isDeleted = q.UsT_isDeleted,
                                }).FirstOrDefault();

                    response.IsSuccessful = true;
                    return response;
                }
                catch (Exception ex)
                {
                    response.IsSuccessful = false;
                    response.Message = "Error Occured in GetUserTypeDetailByID(ID)";
                    response.Detail = ex.Message.ToString();
                    return response;
                }
            }
        }
        #endregion

        #region To get UserType details for select list
        public List<UserTypeEntity> SelectUserType()
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var query = (from q in db.UserTypes
                                 join a in db.Accounts on q.UsT_AccountID equals a.Acc_AccountID
                                 where q.UsT_isDeleted == false && q.UsT_ActiveStatus == true
                                 select new UserTypeEntity
                                 {
                                     UsT_UserTypeID = q.UsT_UserTypeID,
                                     UsT_AccountID = q.UsT_AccountID,
                                     AccountName = a.Acc_AccountName,
                                     UsT_UserTypeCode = q.UsT_UserTypeCode,
                                     UsT_UserType = q.UsT_UserType,
                                     UsT_UserTypeDescription = q.UsT_UserTypeDescription,
                                     UsT_ActiveStatus = q.UsT_ActiveStatus,
                                     UsT_Version = q.UsT_Version,
                                     UsT_CreatedBy = q.UsT_CreatedBy,
                                     UsT_CreatedDate = q.UsT_CreatedDate,
                                     UsT_ModifiedBy = q.UsT_ModifiedBy,
                                     UsT_ModifiedDate = q.UsT_ModifiedDate,
                                     UsT_isDeleted = q.UsT_isDeleted,
                                 }).OrderBy(x => x.UsT_UserType).ToList();

                    return query;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        #endregion

        #region Get Max UserTypeID Details
        public UserType GetMaxUserTypeIDDetials()
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var userTypeMaxEntry = db.UserTypes.OrderByDescending(x => x.UsT_UserTypeID).FirstOrDefault();
                    return userTypeMaxEntry;
                }

                catch (Exception Exception)
                {
                    return null;
                }
            }
        }

        #endregion

        //History

        #region To get particular UserType Account details from Database
        public List<History_UserTypeEntity> GetHistoryUserTypeDetailsByID(int ID)
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var query = (from q in db.History_UserType
                                 join a in db.Accounts on q.History_AccountID equals a.Acc_AccountID
                                 where q.History_UsT_UserTypeID == ID
                                 select new History_UserTypeEntity
                                 {
                                     History_UserType_ID = q.History_UserType_ID,
                                     History_UsT_UserTypeID = q.History_UsT_UserTypeID,
                                     History_UsT_AccountID = q.History_AccountID,
                                     AccountName = a.Acc_AccountName,
                                     History_UsT_UserTypeCode = q.History_UsT_UserTypeCode,
                                     History_UsT_UserType = q.History_UsT_UserType,
                                     History_UsT_UserTypeDescription = q.History_UsT_UserTypeDescription,
                                     History_UsT_ActiveStatus = q.History_UsT_ActiveStatus,
                                     History_UsT_Version = q.History_UsT_Version,
                                     History_UsT_CreatedBy = q.History_UsT_CreatedBy,
                                     History_UsT_CreatedDate = q.History_UsT_CreatedDate,
                                     History_UsT_ModifiedBy = q.History_UsT_ModifiedBy,
                                     History_UsT_ModifiedDate = q.History_UsT_ModifiedDate,
                                     History_UsT_isDeleted = q.History_UsT_isDeleted,
                                 }).OrderBy(x => x.History_UsT_UserType).ToList();

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
            UserType holidayData = null;
            bool Status = Convert.ToBoolean(status);
            int did = Convert.ToInt32(id);
            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    holidayData = db.Set<UserType>().Where(s => s.UsT_UserTypeID == did).FirstOrDefault<UserType>();
                    if (holidayData == null)
                    {
                        return null;
                    }
                    holidayData.UsT_ActiveStatus = Status;
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
