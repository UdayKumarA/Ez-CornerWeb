using Evolutyz.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Evolutyz.Data
{
    public class HolidayCalendarDAC : DataAccessComponent
    {
        #region To add Holiday Calendar in Database
        public string AddHoliday(int accountid, string HolidayName, string HolidayDate, string FinancialYearId, string isOptionalHoliday, string isActive)
        {
            string strResponse = string.Empty;
            int financialyearid = Convert.ToInt16(FinancialYearId);
            bool isoptionalid = Convert.ToBoolean(isOptionalHoliday);
            bool isactive = Convert.ToBoolean(isActive);
            UserSessionInfo objinfo = new UserSessionInfo();
            int userid = objinfo.UserId;
            DateTime holidaydate = Convert.ToDateTime(HolidayDate);
            using (var db = new DbContext(CONNECTION_NAME))
            {
                HolidayCalendar holidayDetails = db.Set<HolidayCalendar>().Where(s =>( s.HolidayName == HolidayName && s.AccountID== accountid && s.ProjectID == null && s.Year== financialyearid) ).FirstOrDefault<HolidayCalendar>();
                HolidayCalendar holidayDetail = db.Set<HolidayCalendar>().Where(s => (s.HolidayDate == holidaydate && s.AccountID == accountid && s.ProjectID == null && s.Year == financialyearid)).FirstOrDefault<HolidayCalendar>();
                if (holidayDetails != null)
                {
                    return strResponse = "HolidayName Already Exist In this Account";
                }
                
                if (holidayDetail != null)
                {
                    return strResponse = "HolidayDate Already Exist In this Account";
                }
                try
                {
                    HolidayCalendar holidayData = new HolidayCalendar();
                    holidayData.AccountID = accountid;
                    holidayData.Year = financialyearid;
                    holidayData.HolidayName = HolidayName;
                    holidayData.HolidayDate = Convert.ToDateTime(HolidayDate);
                    // holidayData.Year = Convert.ToInt16(holiday.Year);
                    holidayData.isOptionalHoliday = isoptionalid;
                    holidayData.isActive = isactive;
                    holidayData.CreatedBy = userid;
                    holidayData.CreatedDate = System.DateTime.Now;

                    db.Set<HolidayCalendar>().Add(holidayData);
                    db.SaveChanges();
                    strResponse = "Record successfully created";
                }
                catch (Exception ex)
                {
                    strResponse = ex.Message.ToString();
                }
            }
            return strResponse;
        }
        #endregion

        #region To update existing Holiday in Database
        public string UpdateHoliday(string HolidayCalendarID, string HolidayName, string HolidayDate, string FinancialYearId, string isOptionalHoliday, string isActive)
        {
            HolidayCalendar holidayDetails = null;

            string strResponse = string.Empty;
            int hcid = Convert.ToInt32(HolidayCalendarID);
            int financialyearid = Convert.ToInt16(FinancialYearId);
            bool isoptionalid = Convert.ToBoolean(isOptionalHoliday);
            bool isactive = Convert.ToBoolean(isActive);
            UserSessionInfo objinfo = new UserSessionInfo();
            int userid = objinfo.UserId;
            DateTime holidaydate = Convert.ToDateTime(HolidayDate);
            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    holidayDetails = db.Set<HolidayCalendar>().Where(s => s.HolidayCalendarID == hcid).FirstOrDefault<HolidayCalendar>();
                    HolidayCalendar holidayDet = db.Set<HolidayCalendar>().Where(s => (s.HolidayName == HolidayName && s.HolidayCalendarID!= hcid && s.Year == financialyearid)).FirstOrDefault<HolidayCalendar>();
                    HolidayCalendar holidayDetail = db.Set<HolidayCalendar>().Where(s => (s.HolidayDate == holidaydate && s.HolidayCalendarID != hcid && s.Year == financialyearid)).FirstOrDefault<HolidayCalendar>();
                    if (holidayDet != null)
                    {
                        return strResponse = "HolidayName Already Exist";
                    }

                    if (holidayDetail != null)
                    {
                        return strResponse = "HolidayDate Already Exist";
                    }
                    if (holidayDetails == null)
                    {
                        return null;
                    }
                    //holidayDetails.AccountID = holiday.AccountID;
                    holidayDetails.HolidayName = HolidayName;
                    holidayDetails.HolidayDate = Convert.ToDateTime(HolidayDate);
                    holidayDetails.Year = financialyearid;
                    holidayDetails.isOptionalHoliday = isoptionalid;
                    holidayDetails.isActive = isactive;
                    holidayDetails.ModifiedBy = userid;
                    holidayDetails.ModifiedDate = System.DateTime.Now;

                    db.Entry(holidayDetails).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    strResponse = "Record successfully updated";
                }
                catch (Exception ex)
                {
                    strResponse = ex.Message.ToString();
                }
                return strResponse;
            }
        }
        #endregion

        #region To delete existing Holiday from Database
        public string DeleteHoliday(string ID)
        {
            string strResponse = string.Empty;
            HolidayCalendar holidayData = null;
            int did = Convert.ToInt32(ID);
            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    holidayData = db.Set<HolidayCalendar>().Where(s => s.HolidayCalendarID == did).FirstOrDefault<HolidayCalendar>();
                    if (holidayData == null)
                    {
                        return null;
                    }
                    holidayData.isDeleted = true;
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
        #endregion

        #region To get all details of HolidayCalendar from Database
        public List<HolidayCalendarEntity> GetHolidayCalendar(int accountID,int? projectid)
        {
            //UserSessionInfo info = new UserSessionInfo();
            //int? projectid = info.Projectid;
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    if (projectid == 0)
                    {
                        var query = (from h in db.HolidayCalendars
                                     join a in db.Accounts on h.AccountID equals a.Acc_AccountID
                                     join f in db.FinancialYears on h.Year equals f.FinancialYearId
                                     where h.AccountID == accountID && h.isDeleted == false && h.ProjectID == null
                                     select new HolidayCalendarEntity
                                     {
                                         HolidayCalendarID = h.HolidayCalendarID,
                                         HolidayName = h.HolidayName,
                                         HolidayDate = h.HolidayDate,
                                         Year = h.Year,
                                         AccountID = h.AccountID,
                                         AccountName = a.Acc_AccountName,
                                         isOptionalHoliday = h.isOptionalHoliday,
                                         isActive = h.isActive,
                                         StartDate = f.StartDate,
                                         EndDate = f.EndDate,
                                         financialyear = f.StartDate + "-" + f.EndDate,
                                         CreatedBy = h.CreatedBy,
                                         CreatedDate = h.CreatedDate,
                                         ModifiedBy = h.ModifiedBy,
                                         ModifiedDate = h.ModifiedDate,
                                         isDeleted = h.isDeleted,
                                     }).OrderBy(x => x.HolidayDate).ToList();
                        foreach (var item in query)
                        {
                            item.HolidayWeek = item.HolidayDate.DayOfWeek.ToString();
                        }
                        return query;

                    }
                    else
                    {
                        var query = (from h in db.HolidayCalendars
                                     join a in db.Accounts on h.AccountID equals a.Acc_AccountID
                                     join f in db.FinancialYears on h.Year equals f.FinancialYearId
                                     where h.AccountID == accountID && h.isDeleted == false && h.ProjectID == projectid
                                     select new HolidayCalendarEntity
                                     {
                                         HolidayCalendarID = h.HolidayCalendarID,
                                         HolidayName = h.HolidayName,
                                         HolidayDate = h.HolidayDate,
                                         Year = h.Year,
                                         AccountID = h.AccountID,
                                         AccountName = a.Acc_AccountName,
                                         isOptionalHoliday = h.isOptionalHoliday,
                                         isActive = h.isActive,
                                         StartDate = f.StartDate,
                                         EndDate = f.EndDate,
                                         financialyear = f.StartDate + "-" + f.EndDate,
                                         CreatedBy = h.CreatedBy,
                                         CreatedDate = h.CreatedDate,
                                         ModifiedBy = h.ModifiedBy,
                                         ModifiedDate = h.ModifiedDate,
                                         isDeleted = h.isDeleted,
                                     }).OrderBy(x => x.HolidayDate).ToList();
                        foreach (var item in query)
                        {
                            item.HolidayWeek = item.HolidayDate.DayOfWeek.ToString();
                        }
                        return query;
                    }
                }



                //}
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        #endregion

        #region To get particular Holiday details from Database
        public HolidayCalendarEntity GetHolidayByID(int ID)
        {
            HolidayCalendarEntity response = new HolidayCalendarEntity();

            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    response = (from h in db.HolidayCalendars
                                join a in db.Accounts on h.AccountID equals a.Acc_AccountID
                                where h.HolidayCalendarID == ID
                                select new HolidayCalendarEntity
                                {
                                    HolidayCalendarID = h.HolidayCalendarID,
                                    HolidayName = h.HolidayName,
                                    HolidayDate = h.HolidayDate,
                                    //Year = h.Year,
                                    FinancialYearId = h.Year,
                                    //HolidayWeek = h.HolidayDate.DayOfWeek.ToString(),
                                    //AccountID = h.AccountID,
                                    // AccountName = a.Acc_AccountName,
                                    isOptionalHoliday = h.isOptionalHoliday,
                                    isActive = h.isActive,
                                    CreatedBy = h.CreatedBy,
                                    CreatedDate = h.CreatedDate,
                                    ModifiedBy = h.ModifiedBy,
                                    ModifiedDate = h.ModifiedDate,
                                    isDeleted = h.isDeleted,
                                }).FirstOrDefault();
                    response.IsSuccessful = true;
                    return response;
                }
                catch (Exception ex)
                {
                    response.IsSuccessful = false;
                    response.Message = "Error Occured in GetTaskDetailDetailByID(ID)";
                    response.Detail = ex.Message.ToString();
                    return response;
                }
            }
        }
        #endregion

        #region To get TaskDetail details for select list
        public List<HolidayCalendarEntity> SelectHolidayDetail()
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var query = (from h in db.HolidayCalendars
                                 join a in db.Accounts on h.AccountID equals a.Acc_AccountID
                                 select new HolidayCalendarEntity
                                 {
                                     HolidayCalendarID = h.HolidayCalendarID,
                                     HolidayName = h.HolidayName,
                                     HolidayDate = h.HolidayDate,
                                     Year = h.Year,
                                     HolidayWeek = h.HolidayDate.DayOfWeek.ToString(),
                                     AccountID = h.AccountID,
                                     AccountName = a.Acc_AccountName,
                                     isActive = h.isActive,
                                     CreatedBy = h.CreatedBy,
                                     CreatedDate = h.CreatedDate,
                                     ModifiedBy = h.ModifiedBy,
                                     //ModifiedDate = h.ModifiedDate,
                                     isDeleted = h.isDeleted,
                                 }).OrderBy(x => x.AccountID).ThenBy(y => y.Year).ThenBy(z => z.HolidayDate).ToList();
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
            HolidayCalendar holidayData = null;
            bool Status = Convert.ToBoolean(status);
            int did = Convert.ToInt32(id);
            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    holidayData = db.Set<HolidayCalendar>().Where(s => s.HolidayCalendarID == did).FirstOrDefault<HolidayCalendar>();
                    if (holidayData == null)
                    {
                        return null;
                    }
                    holidayData.isActive = Status;
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

        public string AddHolidayforclient(int accountid, string HolidayName, string HolidayDate, string FinancialYearId, string isOptionalHoliday, string isActive, string ProjectID)
        {
            string strResponse = string.Empty;
            int financialyearid = Convert.ToInt16(FinancialYearId);
            bool isoptionalid = Convert.ToBoolean(isOptionalHoliday);
            bool isactive = Convert.ToBoolean(isActive);
            UserSessionInfo objinfo = new UserSessionInfo();
            int userid = objinfo.UserId;
            int peojectid = Convert.ToInt32(ProjectID);
            DateTime holidaydate = Convert.ToDateTime(HolidayDate);
            using (var db = new DbContext(CONNECTION_NAME))
            {
                HolidayCalendar holidayDetails = db.Set<HolidayCalendar>().Where(s => (s.HolidayName == HolidayName && s.AccountID == accountid && s.ProjectID == peojectid && s.Year == financialyearid)).FirstOrDefault<HolidayCalendar>();
                HolidayCalendar holidayDetail = db.Set<HolidayCalendar>().Where(s => (s.HolidayDate == holidaydate && s.AccountID == accountid && s.ProjectID == peojectid && s.Year == financialyearid)).FirstOrDefault<HolidayCalendar>();
                if (holidayDetails != null)
                {
                    return strResponse = "HolidayName Already Exist In this Client";
                }

                if (holidayDetail != null)
                {
                    return strResponse = "HolidayDate Already Exist In this Client";
                }
                try
                {
                    HolidayCalendar holidayData = new HolidayCalendar();
                    holidayData.AccountID = accountid;
                    holidayData.Year = financialyearid;
                    holidayData.HolidayName = HolidayName;
                    holidayData.HolidayDate = Convert.ToDateTime(HolidayDate);
                    // holidayData.Year = Convert.ToInt16(holiday.Year);
                    holidayData.isOptionalHoliday = isoptionalid;
                    holidayData.isActive = isactive;
                    holidayData.CreatedBy = userid;
                    holidayData.CreatedDate = System.DateTime.Now;
                    holidayData.ProjectID = peojectid;
                    db.Set<HolidayCalendar>().Add(holidayData);
                    db.SaveChanges();
                    strResponse = "Record successfully created";
                }
                catch (Exception ex)
                {
                    strResponse = ex.Message.ToString();
                }
            }
            return strResponse;
        }

       

        public List<LeaveTypeEntity> GetHolidayDates(int accountid)
        {
            try
            {
                using (var db = new EvolutyzCornerDataEntities())
                {
                    var query = (from h in db.HolidayCalendars
                                 join a in db.Accounts on h.AccountID equals a.Acc_AccountID
                                 where h.AccountID == accountid
                                 select new LeaveTypeEntity
                                 {
                                     HolidayCalendarID = h.HolidayCalendarID,
                                     HolidayName = h.HolidayName,
                                     HolidayDate = h.HolidayDate,
                                     //Year = h.Year,
                                     //AccountID = h.AccountID,
                                     //AccountName = a.Acc_AccountName,
                                     //isOptionalHoliday = h.isOptionalHoliday,
                                     //isActive = h.isActive,                                   
                                     //CreatedBy = h.CreatedBy,
                                     //CreatedDate = h.CreatedDate,
                                     //ModifiedBy = h.ModifiedBy,
                                     //ModifiedDate = h.ModifiedDate,
                                     //isDeleted = h.isDeleted,

                                 }).ToList();
                    return query;
                }
            }
            catch (Exception ex)
            {
                return null;
            }



        }
    }
}