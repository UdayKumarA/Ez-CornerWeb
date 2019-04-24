using Evolutyz.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Evolutyz.Data
{
    public class LeaveTypeDAC : DataAccessComponent
    {
        #region To add LeaveType Detail in Database
        public int AddLeaveType(LeaveTypeEntity _LeaveType)
        {
            int retVal = 0;
            LeaveType LeaveType = new LeaveType();

            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    LeaveType = db.Set<LeaveType>().Where(s => s.LTyp_LeaveTypeID == _LeaveType.LTyp_LeaveTypeID).FirstOrDefault<LeaveType>();
                    if (LeaveType != null)
                    {
                        return retVal;
                    }
                    db.Set<LeaveType>().Add(new LeaveType
                    {
                        LTyp_LeaveTypeID = _LeaveType.LTyp_LeaveTypeID,
                        LTyp_AccountID = _LeaveType.LTyp_AccountID,
                        LTyp_LeaveType = _LeaveType.LTyp_LeaveType,
                        LTyp_LeaveTypeDescription = _LeaveType.LTyp_LeaveTypeDescription,
                        LTyp_ActiveStatus = _LeaveType.LTyp_ActiveStatus,
                        LTyp_Version = _LeaveType.LTyp_Version,
                        LTyp_CreatedBy = _LeaveType.LTyp_CreatedBy,
                        LTyp_CreatedDate = System.DateTime.Now,
                        LTyp_isDeleted = false
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

        #region To update existing LeaveType Detail in Database
        public int UpdateLeaveTypeDetail(LeaveTypeEntity LeaveType)
        {
            LeaveType _LeaveTypeDtl = null;
            History_LeaveType _LeaveTypeHistory = new History_LeaveType();

            int retVal = 0;

            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    _LeaveTypeDtl = db.Set<LeaveType>().Where(s => s.LTyp_LeaveTypeID == LeaveType.LTyp_LeaveTypeID).FirstOrDefault<LeaveType>();

                    if (_LeaveTypeDtl == null)
                    {
                        return retVal;
                    }

                    #region Saving History into History_LeaveType Table

                    //check for  _LeaveTypeDtl.LTyp_ModifiedDate as created date to history table
                    DateTime dtAccCreatedDate = _LeaveTypeDtl.LTyp_ModifiedDate ?? DateTime.Now;

                    db.Set<History_LeaveType>().Add(new History_LeaveType
                    {
                        History_LTyp_LeaveTypeID = _LeaveTypeDtl.LTyp_LeaveTypeID,
                        History_LTyp_AccountID = _LeaveTypeDtl.LTyp_AccountID,
                        History_LTyp_LeaveType = _LeaveTypeDtl.LTyp_LeaveType,
                        History_LTyp_LeaveTypeDescription = _LeaveTypeDtl.LTyp_LeaveTypeDescription,
                        History_LTyp_ActiveStatus = _LeaveTypeDtl.LTyp_ActiveStatus,
                        History_LTyp_Version = _LeaveTypeDtl.LTyp_Version,
                        History_LTyp_CreatedDate = dtAccCreatedDate,
                        History_LTyp_CreatedBy = _LeaveTypeDtl.LTyp_CreatedBy,
                        History_LTyp_ModifiedDate = _LeaveTypeDtl.LTyp_ModifiedDate,
                        History_LTyp_ModifiedBy = _LeaveTypeDtl.LTyp_ModifiedBy,
                        History_LTyp_isDeleted = _LeaveTypeDtl.LTyp_isDeleted
                    });
                    #endregion

                    #region Saving LeaveType info Table

                    _LeaveTypeDtl.LTyp_AccountID = LeaveType.LTyp_AccountID;
                    _LeaveTypeDtl.LTyp_LeaveType = LeaveType.LTyp_LeaveType;
                    _LeaveTypeDtl.LTyp_LeaveTypeDescription = LeaveType.LTyp_LeaveTypeDescription;
                    _LeaveTypeDtl.LTyp_ActiveStatus = LeaveType.LTyp_ActiveStatus;
                    _LeaveTypeDtl.LTyp_Version = LeaveType.LTyp_Version;
                    _LeaveTypeDtl.LTyp_ModifiedDate = System.DateTime.Now;
                    _LeaveTypeDtl.LTyp_ModifiedBy = LeaveType.LTyp_ModifiedBy;
                    _LeaveTypeDtl.LTyp_isDeleted = LeaveType.LTyp_isDeleted;
                    #endregion
                    db.Entry(_LeaveTypeDtl).State = System.Data.Entity.EntityState.Modified;
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

        #region To delete existing LeaveType details from Database
        public int DeleteLeaveTypeDetail(int ctID)
        {
            int retVal = 0;
            LeaveType _LeaveTypeDtl = null;

            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    _LeaveTypeDtl = db.Set<LeaveType>().Where(s => s.LTyp_LeaveTypeID == ctID).FirstOrDefault<LeaveType>();
                    if (_LeaveTypeDtl == null)
                    {
                        return retVal;
                    }
                    _LeaveTypeDtl.LTyp_isDeleted = true;
                    db.Entry(_LeaveTypeDtl).State = System.Data.Entity.EntityState.Modified;
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

        #region To get all details of LeaveType from Database
        public List<LeaveTypeEntity> GetLeaveTypeDetail()
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var query = (from q in db.LeaveTypes
                                 join a in db.Accounts on q.LTyp_AccountID equals a.Acc_AccountID
                                 where q.LTyp_isDeleted == false
                                 select new LeaveTypeEntity
                                 {
                                     LTyp_LeaveTypeID = q.LTyp_LeaveTypeID,
                                     LTyp_AccountID = q.LTyp_AccountID,
                                     AccountName = a.Acc_AccountName,
                                     LTyp_LeaveType = q.LTyp_LeaveType,
                                     LTyp_LeaveTypeDescription = q.LTyp_LeaveTypeDescription,
                                     LTyp_ActiveStatus = q.LTyp_ActiveStatus,
                                     LTyp_Version = q.LTyp_Version,
                                     LTyp_CreatedBy = q.LTyp_CreatedBy,
                                     LTyp_CreatedDate = q.LTyp_CreatedDate,
                                     LTyp_ModifiedBy = q.LTyp_ModifiedBy,
                                     LTyp_ModifiedDate = q.LTyp_ModifiedDate,
                                     LTyp_isDeleted = q.LTyp_isDeleted,
                                 }).OrderBy(x => x.LTyp_LeaveType).ThenBy(x => x.LTyp_LeaveType).ToList();

                    return query;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        #endregion

        #region To get particular LeaveType details from Database
        public LeaveTypeEntity GetLeaveTypeDetailByID(int ID)
        {
            LeaveTypeEntity response = new LeaveTypeEntity();

            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    response = (from q in db.LeaveTypes
                                join a in db.Accounts on q.LTyp_AccountID equals a.Acc_AccountID
                                where q.LTyp_LeaveTypeID == ID
                                select new LeaveTypeEntity
                                {
                                    LTyp_LeaveTypeID = q.LTyp_LeaveTypeID,
                                    LTyp_AccountID = q.LTyp_AccountID,
                                    AccountName = a.Acc_AccountName,
                                    LTyp_LeaveType = q.LTyp_LeaveType,
                                    LTyp_LeaveTypeDescription = q.LTyp_LeaveTypeDescription,
                                    LTyp_ActiveStatus = q.LTyp_ActiveStatus,
                                    LTyp_Version = q.LTyp_Version,
                                    LTyp_CreatedBy = q.LTyp_CreatedBy,
                                    LTyp_CreatedDate = q.LTyp_CreatedDate,
                                    LTyp_ModifiedBy = q.LTyp_ModifiedBy,
                                    LTyp_ModifiedDate = q.LTyp_ModifiedDate,
                                    LTyp_isDeleted = q.LTyp_isDeleted,
                                }).FirstOrDefault();

                    response.IsSuccessful = true;
                    return response;
                }
                catch (Exception ex)
                {
                    response.IsSuccessful = false;
                    response.Message = "Error Occured in GetLeaveTypeDetailByID(ID)";
                    response.Detail = ex.Message.ToString();
                    return response;
                }
            }
        }
        #endregion

        #region To get LeaveType details for select list
        public List<LeaveTypeEntity> SelectLeaveType()
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var query = (from q in db.LeaveTypes
                                 join a in db.Accounts on q.LTyp_AccountID equals a.Acc_AccountID
                                 where q.LTyp_isDeleted == false && q.LTyp_ActiveStatus == true
                                 select new LeaveTypeEntity
                                 {
                                     LTyp_LeaveTypeID = q.LTyp_LeaveTypeID,
                                     LTyp_AccountID = q.LTyp_AccountID,
                                     AccountName = a.Acc_AccountName,
                                     LTyp_LeaveType = q.LTyp_LeaveType,
                                     LTyp_LeaveTypeDescription = q.LTyp_LeaveTypeDescription,
                                     LTyp_ActiveStatus = q.LTyp_ActiveStatus,
                                     LTyp_Version = q.LTyp_Version,
                                     LTyp_CreatedBy = q.LTyp_CreatedBy,
                                     LTyp_CreatedDate = q.LTyp_CreatedDate,
                                     LTyp_ModifiedBy = q.LTyp_ModifiedBy,
                                     LTyp_ModifiedDate = q.LTyp_ModifiedDate,
                                     LTyp_isDeleted = q.LTyp_isDeleted,
                                 }).OrderBy(x => x.LTyp_LeaveType).ToList();

                    return query;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        #endregion

        #region Get Max LeaveTypeID Details
        public LeaveType GetMaxLeaveTypeIDDetials()
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var LeaveTypeMaxEntry = db.LeaveTypes.OrderByDescending(x => x.LTyp_LeaveTypeID).FirstOrDefault();
                    return LeaveTypeMaxEntry;
                }

                catch (Exception Exception)
                {
                    return null;
                }
            }
        }

        #endregion

        //History

        #region To get particular LeaveType details from Database
        public List<History_LeaveTypeEntity> GetHistoryLeaveTypeDetailsByID(int ID)
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var query = (from q in db.History_LeaveType
                                 join a in db.Accounts on q.History_LTyp_AccountID equals a.Acc_AccountID
                                 where q.History_LTyp_LeaveTypeID == ID
                                 select new History_LeaveTypeEntity
                                 {
                                     History_LeaveType_ID = q.History_LeaveType_ID,
                                     History_LTyp_LeaveTypeID = q.History_LTyp_LeaveTypeID,
                                     History_LTyp_AccountID = q.History_LTyp_AccountID,
                                     AccountName = a.Acc_AccountName,
                                     History_LTyp_LeaveType = q.History_LTyp_LeaveType,
                                     History_LTyp_LeaveTypeDescription = q.History_LTyp_LeaveTypeDescription,
                                     History_LTyp_ActiveStatus = q.History_LTyp_ActiveStatus,
                                     History_LTyp_Version = q.History_LTyp_Version,
                                     History_LTyp_CreatedBy = q.History_LTyp_CreatedBy,
                                     History_LTyp_CreatedDate = q.History_LTyp_CreatedDate,
                                     History_LTyp_ModifiedBy = q.History_LTyp_ModifiedBy,
                                     History_LTyp_ModifiedDate = q.History_LTyp_ModifiedDate,
                                     History_LTyp_isDeleted = q.History_LTyp_isDeleted,
                                 }).OrderBy(x => x.History_LTyp_LeaveType).ToList();

                    return query;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        #endregion

        public List<LeaveTypeEntity> Getallleavetypes()
        {
            try
            {
                using (var db = new EvolutyzCornerDataEntities())
                {
                    var query = (from c in db.LeaveTypes
                                 where c.LTyp_isDeleted==false
                                     //where c.Statusid == Convert.ToBoolean(StatusEnum.Active)
                                 select new LeaveTypeEntity
                                 {
                                     LTyp_LeaveTypeID = c.LTyp_LeaveTypeID,
                                     LTyp_LeaveType = c.LTyp_LeaveType

                                 }).ToList();
                    return query;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<UserEntity> GetAllEmployeementtypes()
        {
            try
            {
                using (var db = new EvolutyzCornerDataEntities())
                {
                    var query = (from UT in db.UserTypes
                                     //where !db.LeaveSchemes.Any(l => l.LSchm_UserTypeID == UT.UsT_UserTypeID)
                                 select new UserEntity
                                 {
                                     Usr_UserTypeID = UT.UsT_UserTypeID,
                                     UserType = UT.UsT_UserType
                                 }).ToList();
                    return query;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<UserEntity> GetAllUserTypes()
        {
            try
            {
                using (var db = new EvolutyzCornerDataEntities())
                {
                    var query = (from UT in db.UserTypes

                                 select new UserEntity
                                 {
                                     Usr_UserTypeID = UT.UsT_UserTypeID,
                                     UserType = UT.UsT_UserType
                                 }).ToList();
                    return query;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<AccountEntity> GetallAccountnames(int accountid)
        {
            try
            {
                using (var db = new EvolutyzCornerDataEntities())
                {
                    var query = (from c in db.Accounts
                                 where c.Acc_AccountID == accountid
                                 //where c.Statusid == Convert.ToBoolean(StatusEnum.Active)
                                 select new AccountEntity
                                 {
                                     Acc_AccountID = c.Acc_AccountID,
                                     Acc_AccountName = c.Acc_AccountName

                                 }).ToList();
                    return query;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<AccountEntity> GetallAccountnames(int accountid,string roleid)
        {
            try
            {
                using (var db = new EvolutyzCornerDataEntities())
                {
                    if (roleid =="Super Admin")
                    {
                        var query = (from c in db.Accounts
                                     where c.Acc_AccountID!=502 && c.Acc_isDeleted==false
                                     //where c.Acc_AccountID == accountid
                                     //where c.Statusid == Convert.ToBoolean(StatusEnum.Active)
                                     select new AccountEntity
                                     {
                                         Acc_AccountID = c.Acc_AccountID,
                                         Acc_AccountName = c.Acc_AccountName

                                     }).ToList();
                        return query;
                    }
                    else
                    {
                        var query = (from c in db.Accounts
                                     where c.Acc_AccountID == accountid && c.Acc_isDeleted == false
                                     //where c.Statusid == Convert.ToBoolean(StatusEnum.Active)
                                     select new AccountEntity
                                     {
                                         Acc_AccountID = c.Acc_AccountID,
                                         Acc_AccountName = c.Acc_AccountName

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
        public List<FinancialYearEntity> Getallfinancialyears()
        {
            try
            {
                using (var db = new EvolutyzCornerDataEntities())
                {
                    var query = (from c in db.FinancialYears
                                 where c.IsDeleted==true
                                 select new FinancialYearEntity
                                 {
                                     FinancialYearId = c.FinancialYearId,
                                     StartDate = c.StartDate,
                                     EndDate = c.EndDate,
                                     financialyear = c.StartDate + "-" + c.EndDate
                                 }).OrderByDescending(x => x.FinancialYearId).ThenByDescending(x => x.financialyear).ToList();
                    return query;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        public int saveleavecount(List<AppliedLeaves> leaveupdate, string LeaveStartDate, string LeaveEndDate, string Usrl_UserId)
        {
            int id = 0;
            int strresponse = 0;
            try
            {
                using (var db = new DbContext(CONNECTION_NAME))
                {

                    UserLeaf u =new UserLeaf();
                    u.Usrl_UserId = Convert.ToInt32(Usrl_UserId);
                    u.LeaveStartDate = Convert.ToDateTime(LeaveStartDate);
                    u.LeaveEndDate = Convert.ToDateTime(LeaveEndDate);
                    
                    db.Set<UserLeaf>().Add(u);

                    db.SaveChanges();


                    id = u.Usrl_LeaveId;

                    for (int i = 0; i <= leaveupdate.Count - 1; i++)
                    {
                        if (leaveupdate[i].AppliedLeave != null)
                        {

                            db.Set<UserLeaveTypeConsumed>().Add(new UserLeaveTypeConsumed
                            {
                                Usrl_LeaveId = id,
                                LSchm_LeaveSchemeID = leaveupdate[i].lschm_leaveschemeid,
                                No_Of_Days = leaveupdate[i].AppliedLeave

                            });
                            db.SaveChanges();
                            strresponse = id;


                        }
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strresponse;
        }


        public List<LeaveTypeEntity> GetLeaveTypes(string id)
        {
            try
            {
                using (var db = new EvolutyzCornerDataEntities())
                {
                    var userid = Convert.ToInt32(id);
                    var query = (from u in db.Users
                                 join ul in db.UserLeaves on u.Usr_UserID equals ul.Usrl_UserId
                                 join Ult in db.UserLeaveTypeConsumeds on ul.Usrl_LeaveId equals Ult.Usrl_LeaveId
                                 join ls in db.LeaveSchemes on Ult.LSchm_LeaveSchemeID equals ls.LSchm_LeaveSchemeID
                                 join lt in db.LeaveTypes on ls.LSchm_LeaveTypeID equals lt.LTyp_LeaveTypeID
                                 join ut in db.UserTypes on u.Usr_UserTypeID equals ut.UsT_UserTypeID
                                 where u.Usr_UserID == userid
                                 group Ult by
                                 new
                                 {
                                     lt.LTyp_LeaveTypeID,
                                     lt.LTyp_LeaveType,
                                     // ls.LSchm_LeaveCount,
                                     //ls.LSchm_UserTypeID,
                                     //ut.UsT_UserType,
                                     //ul.Usrl_LeaveId,



                                 } into gs


                                 select new LeaveTypeEntity
                                 {
                                     LTyp_LeaveTypeID = gs.Key.LTyp_LeaveTypeID,
                                     LTyp_LeaveType = gs.Key.LTyp_LeaveType,
                                     // LSchm_LeaveCount = gs.Key.LSchm_LeaveCount,
                                     //UsT_UserTypeID = gs.Key.LSchm_UserTypeID,
                                     //UsT_UserType = gs.Key.UsT_UserType,
                                     //Usrl_LeaveId = gs.Key.Usrl_LeaveId,
                                     No_Of_Days = gs.Sum(p => p.No_Of_Days),

                                 }).ToList();
                    return query;

                }

            }
            catch (Exception)
            {
                return null;
            }
        }
   

        public List<LeaveTypeEntity> GetAllLeaveTypes()
        {
            try
            {
                using (var db = new EvolutyzCornerDataEntities())
                {
                    var query = (from c in db.LeaveTypes
                                 select new LeaveTypeEntity
                                 {
                                     LTyp_LeaveTypeID = c.LTyp_LeaveTypeID,
                                     LTyp_LeaveType = c.LTyp_LeaveType
                                 }).ToList();
                    return query;

                }

            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<LeaveTypeEntity> GetAllEmpIds()
        {
            try
            {
                using (var db = new EvolutyzCornerDataEntities())
                {
                    var query = (from ei in db.UsersProfiles
                                 select new LeaveTypeEntity
                                 {
                                     UsrP_UserProfileID = ei.UsrP_UserProfileID,
                                     UsrP_EmployeeID = ei.UsrP_EmployeeID
                                 }).ToList();
                    return query;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public string saveUserLeaveStatus(string leaveid, string status, string managerid, string managerlevel)
        {
            string strresponse = string.Empty;
            int statusid = Convert.ToInt32(status);
            string lid = leaveid;
            int mid = Convert.ToInt16(managerid);
            //   int mlvl = Convert.ToInt16(managerlevel);
            UserLeaf LeaveType = new UserLeaf();
            try
            {
                using (var db = new DbContext(CONNECTION_NAME))
                {

                    LeaveType = db.Set<UserLeaf>().Where(s => s.Usrl_LeaveId.ToString() == lid).FirstOrDefault<UserLeaf>();
                    //if (mlvl == 1)
                    //{
                    if (LeaveType != null)
                    {
                        if (statusid == 1)
                        {

                            LeaveType.ApprovedBy = mid;
                            LeaveType.ApprovedDate = DateTime.Now;
                            LeaveType.L_StatusId = statusid;

                        }
                        else if (statusid == 2)
                        {

                            LeaveType.RejectedBy = mid;
                            LeaveType.RejectedDate = DateTime.Now;
                            LeaveType.L_StatusId = statusid;

                        }
                        else
                        {


                            LeaveType.L_StatusId = statusid;

                        }

                    }
                    
                    db.SaveChanges();
                    strresponse = "success";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strresponse;
        }


        public List<DashboardMails> GetAllProjects()
        {
            try
            {
                using (var db = new EvolutyzCornerDataEntities())
                {
                    var query = (from p in db.Projects
                                 select new DashboardMails
                                 {
                                     Proj_ProjectID = p.Proj_ProjectID,
                                     Proj_ProjectName = p.Proj_ProjectName,

                                 }).ToList();
                    return query;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<sp_GetLeaveTypes_Result> GetLeaves(string id)
        {

            try
            {
                EvolutyzCornerDataEntities db = new EvolutyzCornerDataEntities();
                var userid = Convert.ToInt32(id);
                var query = (db.sp_GetLeaveTypes(userid)).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }


        public int savewrkfrmhome(string fromdate, string todate, string Usrl_UserId)
        {
            int id = 0;
            string strresponse = string.Empty;
            try
            {
                using (var db = new DbContext(CONNECTION_NAME))
                {

                    UserworkfromHome u = new UserworkfromHome();
                    u.Usrl_UserId = Convert.ToInt32(Usrl_UserId);
                    u.UserwfhStartDate = Convert.ToDateTime(fromdate);
                    u.UserwfhEndDate = Convert.ToDateTime(todate);
                    // u.Tot_No_Days = noofdays;
                    db.Set<UserworkfromHome>().Add(u);

                    db.SaveChanges();


                    id = u.UserwfhID;


                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return id;
        }


        public string ChangeStatus(string id, string status)
        {
            string strResponse = string.Empty;
            LeaveType holidayData = null;
            bool Status = Convert.ToBoolean(status);
            int did = Convert.ToInt32(id);
            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    holidayData = db.Set<LeaveType>().Where(s => s.LTyp_LeaveTypeID == did).FirstOrDefault<LeaveType>();
                    if (holidayData == null)
                    {
                        return null;
                    }
                    holidayData.LTyp_ActiveStatus = Status;
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



        public string SaveWorkFromHomeStatus(string userwfhId, string status, string managerid)
        {
            string strresponse = string.Empty;
            int statusid = Convert.ToInt32(status);
            int lid = Convert.ToInt32(userwfhId);
            int mid = Convert.ToInt16(managerid);
            UserworkfromHome LeaveType = new UserworkfromHome();
            try
            {
                using (var db = new DbContext(CONNECTION_NAME))
                {

                    LeaveType = db.Set<UserworkfromHome>().Where(s => s.UserwfhID == lid).FirstOrDefault<UserworkfromHome>();
                    if (LeaveType != null)
                    {
                        if (statusid == 1)
                        {

                            LeaveType.ApprovedBy = mid;
                            LeaveType.ApprovedDate = DateTime.Now;
                            LeaveType.L_StatusId = statusid;

                        }
                        else if (statusid == 2)
                        {

                            LeaveType.RejectedBy = mid;
                            LeaveType.RejectedDate = DateTime.Now;
                            LeaveType.L_StatusId = statusid;

                        }
                        else
                        {


                            LeaveType.L_StatusId = statusid;

                        }

                    }

                    db.SaveChanges();
                    strresponse = "success";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strresponse;
        }

        public string saveWebApprovalStatus(string leaveid, string Leavestatus, string ManagerId)
        {
            string strresponse = string.Empty;
            int statusid = Convert.ToInt32(Leavestatus);
            string lid = leaveid;
            int mid = Convert.ToInt16(ManagerId);

            UserLeaf LeaveType = new UserLeaf();
            try
            {
                using (var db = new DbContext(CONNECTION_NAME))
                {

                    LeaveType = db.Set<UserLeaf>().Where(s => s.Usrl_LeaveId.ToString() == lid).FirstOrDefault<UserLeaf>();

                    if (LeaveType != null)
                    {
                        if (statusid == 4)
                        {

                            LeaveType.ApprovedBy = mid;
                            LeaveType.ApprovedDate = DateTime.Now;
                            LeaveType.L_StatusId = statusid;

                        }
                        else if (statusid == 5)
                        {

                            LeaveType.RejectedBy = mid;
                            LeaveType.RejectedDate = DateTime.Now;
                            LeaveType.L_StatusId = statusid;

                        }
                        else
                        {


                            LeaveType.L_StatusId = statusid;

                        }

                    }


                    db.SaveChanges();
                    strresponse = "success";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strresponse;
        }



        public LeaveTypeEntity GetUSaccmail(int uid)
        {
            try
            {
                using (var db = new EvolutyzCornerDataEntities())
                {
                    //  var userid = Convert.ToInt32(uid);
                    var query = (from u in db.Users
                                 join a in db.Accounts on u.Usr_AccountID equals a.Acc_AccountID
                                 where u.Usr_UserID == uid
                                 select new LeaveTypeEntity
                                 {
                                     Acc_EmailID = a.Acc_EmailID,

                                 }).FirstOrDefault();
                    return query;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public int saveleavecountForUS(string LeaveStartDate, string LeaveEndDate, string Usrl_UserId, int businessdays)
        {
            int id = 0;
            int strresponse = 0;
            int usrid = Convert.ToInt32(Usrl_UserId);
            DateTime sdate = Convert.ToDateTime(LeaveStartDate);
            DateTime ldate = Convert.ToDateTime(LeaveEndDate);
            try
            {
                using (var db = new DbContext(CONNECTION_NAME))
                {
                    UserLeaf u = new UserLeaf();
                    u.Usrl_UserId = usrid;
                    u.LeaveStartDate = sdate;
                    u.LeaveEndDate = ldate;
                    db.Set<UserLeaf>().Add(u);
                    db.SaveChanges();
                    id = u.Usrl_LeaveId;
                    db.Set<UserLeaveTypeConsumed>().Add(new UserLeaveTypeConsumed
                    {
                        Usrl_LeaveId = id,
                        No_Of_Days = businessdays,
                    });
                    db.SaveChanges();
                    strresponse = id;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strresponse;
        }
        public int savewrkfrmhomeForUS(string LeaveStartDate, string LeaveEndDate, string Usrl_UserId, int businessdays)
        {
            int id = 0;
            string strresponse = string.Empty;
            try
            {
                using (var db = new DbContext(CONNECTION_NAME))
                {
                    UserworkfromHome u = new UserworkfromHome();
                    u.Usrl_UserId = Convert.ToInt32(Usrl_UserId);
                    u.UserwfhStartDate = Convert.ToDateTime(LeaveStartDate);
                    u.UserwfhEndDate = Convert.ToDateTime(LeaveEndDate);
                    u.Tot_No_Days = businessdays;
                    db.Set<UserworkfromHome>().Add(u);
                    db.SaveChanges();
                    id = u.UserwfhID;
                }
            }


            catch (Exception ex)
            {
                throw ex;
            }
            return id;
        }

        public string saveWebWFHApprovalStatus(string UWFHID, string Leavestatus, string ManagerId)
        {
            string strresponse = string.Empty;
            int statusid = Convert.ToInt32(Leavestatus);
            string lid = UWFHID;
            int mid = Convert.ToInt16(ManagerId);

            UserworkfromHome LeaveType = new UserworkfromHome();
            try
            {
                using (var db = new DbContext(CONNECTION_NAME))
                {

                    LeaveType = db.Set<UserworkfromHome>().Where(s => s.UserwfhID.ToString() == lid).FirstOrDefault<UserworkfromHome>();

                    if (LeaveType != null)
                    {
                        if (statusid == 4)
                        {

                            LeaveType.ApprovedBy = mid;
                            LeaveType.ApprovedDate = DateTime.Now;
                            LeaveType.L_StatusId = statusid;

                        }
                        else if (statusid == 5)
                        {

                            LeaveType.RejectedBy = mid;
                            LeaveType.RejectedDate = DateTime.Now;
                            LeaveType.L_StatusId = statusid;

                        }
                        else
                        {


                            LeaveType.L_StatusId = statusid;

                        }

                    }


                    db.SaveChanges();
                    strresponse = "success";
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

