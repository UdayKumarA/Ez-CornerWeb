using Evolutyz.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Evolutyz.Data;

namespace Evolutyz.Data
{
    public class LeaveSchemeDAC : DataAccessComponent
    {
      
        #region To get all details of LeaveScheme from Database

        public List<LeaveSchemeEntity> GetLeaveSchemeDetail()
        {
            UserSessionInfo info = new UserSessionInfo();
            int accid = info.AccountId;
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var query = (from ls in db.LeaveSchemes
                                 join u in db.UserTypes on ls.LSchm_UserTypeID equals u.UsT_UserTypeID
                                 join a in db.Accounts on ls.LSchm_AccountID equals a.Acc_AccountID
                                 join f in db.FinancialYears on ls.FinancialYearId equals f.FinancialYearId
                                 join lt in db.LeaveTypes on ls.LSchm_LeaveTypeID equals lt.LTyp_LeaveTypeID
                               where ls.LSchm_AccountID ==accid
                               

                                 group ls by
                                 new
                                 {
                                     ls.LSchm_UserTypeID,
                                     ls.LSchm_AccountID,
                                     a.Acc_AccountName,
                                     u.UsT_UserType,
                                     f.StartDate,
                                     f.EndDate,
                                     ls.FinancialYearId



                                 } into gs

                                 select new LeaveSchemeEntity
                                 {
                                     StartDate= gs.Key.StartDate,
                                     EndDate = gs.Key.EndDate,
                                    financialyear=(gs.Key.StartDate+"-"+gs.Key.EndDate).ToString(),
                                    FinancialYearId= gs.Key.FinancialYearId,
                                     Noofdays = gs.Sum(p => p.LSchm_LeaveCount),
                                     UserType = gs.Key.UsT_UserType,
                                     AccountName = gs.Key.Acc_AccountName,
                                     LSchm_AccountID = gs.Key.LSchm_AccountID,
                                     LSchm_UserTypeID = gs.Key.LSchm_UserTypeID
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
        
        #region to save leave scheme days   

        public string SaveLeaveScheme(List<LeaveSchemeModel> jsonobj)
        {
            string strresponse = "";

            try
            {

                for (int i = 0; i <= jsonobj.Count - 1; i++)
                {
                    using (var db = new DbContext(CONNECTION_NAME))
                    {

                        db.Set<LeaveScheme>().Add(new LeaveScheme
                        {
                            LSchm_AccountID = jsonobj[i].LSchm_AccountID,
                            LSchm_LeaveTypeID = jsonobj[i].LSchm_LeaveTypeID,
                            LSchm_UserTypeID = jsonobj[i].LSchm_UserTypeID,
                            LSchm_LeaveCount = jsonobj[i].LSchm_LeaveCount,
                            LSchm_ActiveStatus = Convert.ToBoolean(jsonobj[i].LSchm_ActiveStatus),
                            LSchm_CreatedDate = DateTime.Now,
                            LSchm_CreatedBy = '1',
                            LSchm_LeaveFrequency = "1",
                            LSchm_LeaveSchemeCode = "xxx",
                            LSchm_LeaveScheme = "",
                            LSchm_LeaveSchemeDescription = "",
                            LSchm_Version = 1,
                            FinancialYearId=Convert.ToInt32(jsonobj[i].FinancialYearId)





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
        #endregion

        #region To updateleavecount 
        public string updateLeavecount(List<LeaveSchemeModel> jsonobj)
        {  
             
            string strresponse = "";
            LeaveScheme ques = null;
            try
            {

                for (int i = 0; i <= jsonobj.Count - 1; i++)
                {
                    using (var db = new DbContext(CONNECTION_NAME))
                    {
                        var userid = Convert.ToInt32(jsonobj[i].LSchm_UserTypeID);
                        var leavetypeid = Convert.ToInt32(jsonobj[i].LSchm_LeaveTypeID);
                        ques = db.Set<LeaveScheme>().Where(s => (s.LSchm_UserTypeID == userid) && (s.LSchm_LeaveTypeID == leavetypeid)).FirstOrDefault<LeaveScheme>();
                        if (ques != null)
                        {
                            ques.LSchm_LeaveCount = jsonobj[i].LSchm_LeaveCount;
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

        #endregion

        #region To UpdateLeavescheme 
        public string UpdateLeavescheme(string id, string userid, string accountid)
        {
            string strresponse = "";

            try
            {


                using (var db = new DbContext(CONNECTION_NAME))
                {
                    var ID = Convert.ToInt32(id);
                    var Userid = Convert.ToInt32(userid);
                    var Accountid = Convert.ToInt32(accountid);

                    db.Set<LeaveScheme>().Where(x => x.LSchm_UserTypeID == ID).ToList().ForEach(x =>
                    {
                        x.LSchm_UserTypeID = Userid;
                        x.LSchm_AccountID = Accountid;
                    });


                    db.SaveChanges();
                    strresponse = "Successfully updated";

                }


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return strresponse;
        }

        #endregion
        
        #region To Get Leave Types
        public List<LeaveSchemeEntity> GetLeaveTypes(string ID, int yearid)
        {


            using (var db = new EvolutyzCornerDataEntities())
            {
                var id = Convert.ToInt32(ID);
                var yid = Convert.ToInt32(yearid);
                try
                {
                    var response = (from ls in db.LeaveSchemes
                                    join l in db.LeaveTypes on ls.LSchm_LeaveTypeID equals l.LTyp_LeaveTypeID
                                    where ls.LSchm_UserTypeID == id && ls.FinancialYearId == yid
                                    select new LeaveSchemeEntity
                                    {
                                        LSchm_LeaveTypeID = ls.LSchm_LeaveTypeID,
                                        LSchm_LeaveCount = ls.LSchm_LeaveCount,
                                        LeaveType = l.LTyp_LeaveType

                                    }).ToList();

                    return response;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        #endregion
        
        #region To getleaveschemebyid
        public List<LeaveSchemeEntity> Getleaveschemebyid(string ID)
        {


            using (var db = new EvolutyzCornerDataEntities())
            {
                var id = Convert.ToInt32(ID);
                try
                {
                    var response = (from ls in db.LeaveSchemes
                                    join u in db.UserTypes on ls.LSchm_UserTypeID equals u.UsT_UserTypeID
                                    join a in db.Accounts on ls.LSchm_AccountID equals a.Acc_AccountID
                                    where ls.LSchm_UserTypeID == id

                                    group ls by
                                    new
                                    {
                                        ls.LSchm_UserTypeID,
                                        ls.LSchm_AccountID,
                                        a.Acc_AccountName,
                                        u.UsT_UserType,



                                    } into gs

                                    select new LeaveSchemeEntity
                                    {


                                        UserType = gs.Key.UsT_UserType,
                                        AccountName = gs.Key.Acc_AccountName,
                                        LSchm_AccountID = gs.Key.LSchm_AccountID,
                                        LSchm_UserTypeID = gs.Key.LSchm_UserTypeID
                                    }).ToList();





                    return response;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        #endregion
        
        public List<FinancialYearEntity> GetFinanacialYears()
        {

            UserSessionInfo info = new UserSessionInfo();
            int accid = info.AccountId;
            using (var db = new EvolutyzCornerDataEntities())
            {

                try
                {
                    var response = (from ls in db.FinancialYears
                                    //where ls.IsDeleted== true 

                                    select new FinancialYearEntity
                                    {
                                        FinancialYearId = ls.FinancialYearId,
                                        StartDate = ls.StartDate,
                                        EndDate = ls.EndDate,
                                        financialyear = ls.StartDate + "-" + ls.EndDate,
                                        IsDeleted = ls.IsDeleted


                                    }).Distinct().ToList();

                    return response;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public string SaveFinancialyears(string startyear, string endyear, string status)
        {
            string strresponse = "";
            int strtyear =Convert.ToInt32(startyear);
            int edyear = Convert.ToInt32(endyear);
            int sttus = Convert.ToInt32(status);
            FinancialYear fid = new FinancialYear();
            bool b = Convert.ToBoolean(sttus);
            try
            {
                using (var db = new DbContext(CONNECTION_NAME))
                {
                     fid= db.Set<FinancialYear>().Where(s =>( s.StartDate == strtyear && s.EndDate==edyear)).FirstOrDefault<FinancialYear>();
                    if (fid != null)
                    {
                       return strresponse = "Record Already Exist";
                    }
                    db.Set<FinancialYear>().Add(new FinancialYear
                    {
                        StartDate= strtyear,
                        EndDate= edyear,
                        IsDeleted= b

                    });
                     db.SaveChanges();
                    strresponse = "Successfully Added";
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return strresponse;
            }

        public bool checkyear(int usertypeid, int yearvalue)
        {
            EvolutyzCornerDataEntities entities = new EvolutyzCornerDataEntities();
            bool bresult = false;
            var year = entities.LeaveSchemes.Where(d => d.LSchm_UserTypeID == usertypeid && d.FinancialYearId == yearvalue).FirstOrDefault();
            if (year != null)
            {

                bresult = true;

            }
            else
            {
                bresult = false;
            }
            return bresult;
        }
        
        public string ChangeStatus(string id, string status)
        {
            string strResponse = string.Empty;
            FinancialYear holidayData = null;
            bool Status = Convert.ToBoolean(status);
            int did = Convert.ToInt32(id);
            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    holidayData = db.Set<FinancialYear>().Where(s => s.FinancialYearId == did).FirstOrDefault<FinancialYear>();
                    if (holidayData == null)
                    {
                        return null;
                    }
                    holidayData.IsDeleted = Status;
                 
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

