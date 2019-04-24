using Evolutyz.Data;
using Evolutyz.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Evolutyz.Business
{
    public class AdminComponent
    {
        string str = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        #region LookUp
        public List<TaskLookupEntity> GetLookUp()
        {
            LookUpDAC lookupDAC = new LookUpDAC();
            return lookupDAC.GetLookUp();
        }
        #endregion


        #region GetLookUpByEmpId
        public List<TaskLookupEntity> GetLookUpByEmpId(int Userid)
        {

            LookUpDAC lookupDAC = new LookUpDAC();
            return lookupDAC.GetLookUpByEmpId(Userid);
        }
        #endregion



        public List<LeaveTypeEntity> GetLeaveTypes(string id)
        {
            LeaveTypeDAC lookupDAC = new LeaveTypeDAC();
            return lookupDAC.GetLeaveTypes(id);
        }

        public List<timesheetEntity> GetAllManagersEmails(string empid)
        {
            int Empid = Convert.ToInt32(empid);
            List<timesheetEntity> lst = new List<timesheetEntity>();
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Get_ManagerEmailDetails", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", Empid);
            SqlDataReader dt = cmd.ExecuteReader();
            var JSONString = new StringBuilder();
            if (dt.HasRows)
            {
                while (dt.Read())
                {

                    lst.Add(new timesheetEntity
                    {

                        ManagerLevel1 = dt["ManagerLevel1"].ToString() + "/" + dt["L1_ManagerID"].ToString() + "/" + dt["L1_ManagerName"].ToString(),
                        ManagerLevel2 = dt["ManagerLevel2"].ToString() + "/" + dt["L2_ManagerID"].ToString() + "/" + dt["L2_ManagerName"].ToString(),
                    });
                }
            }
            conn.Close();

            return lst;
        }

        public int saveleavecount(List<AppliedLeaves> leaveupdate, string LeaveStartDate, string LeaveEndDate, string Usrl_UserId)
        {
            LeaveTypeDAC leavedac = new LeaveTypeDAC();
            return leavedac.saveleavecount(leaveupdate, LeaveStartDate, LeaveEndDate, Usrl_UserId);
        }

        public string saveUserLeaveStatus(string leaveid, string status, string managerid, string managerlevel)
        {
            LeaveTypeDAC leavedac = new LeaveTypeDAC();
            return leavedac.saveUserLeaveStatus(leaveid, status, managerid, managerlevel);
        }
        public string saveWebWFHApprovalStatus(string UWFHID, string Leavestatus, string ManagerId)
        {
            LeaveTypeDAC leavedac = new LeaveTypeDAC();
            return leavedac.saveWebWFHApprovalStatus(UWFHID, Leavestatus, ManagerId);
        }



        public List<LeaveTypeEntity> GetApprovedLeaveCount()
        {
            //int Empid = Convert.ToInt32(empid);
            List<LeaveTypeEntity> leavelst = new List<LeaveTypeEntity>();
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Get_UserLeavesCount", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@UserId", Empid);
            SqlDataReader dt = cmd.ExecuteReader();
            var JSONString = new StringBuilder();
            if (dt.HasRows)
            {
                while (dt.Read())
                {

                    leavelst.Add(new LeaveTypeEntity
                    {
                        Usrl_UserId = Convert.ToInt32(dt["Usrl_UserId"]),
                        UsrP_EmployeeID = dt["UsrP_EmployeeID"].ToString(),
                        UsrP_FirstName = dt["UsrP_FirstName"].ToString(),
                        UsrP_LastName = dt["UsrP_LastName"].ToString(),
                        Usrl_LeaveId = Convert.ToInt32(dt["Usrl_LeaveId"]),
                        LeaveStartDate = Convert.ToDateTime(dt["LeaveStartDate"]),
                        LeaveEndDate = Convert.ToDateTime(dt["LeaveEndDate"]),
                    });
                }
            }
            conn.Close();

            return leavelst;
        }


        public List<sp_GetLeaveTypes_Result> GetLeaves(string id)
        {
            LeaveTypeDAC lookupDAC = new LeaveTypeDAC();
            return lookupDAC.GetLeaves(id);
        }

        //public string GetApprovedLeaveCount()
        //{
        //    //EvolutyzCornerDataEntities db = new EvolutyzCornerDataEntities();
        //    //var getleavecount = db.Get_UserLeavesCount(date);

        //    //return getleavecount.ToString();
        //    LeaveTypeDAC leavedac = new LeaveTypeDAC();
        //    return leavedac.GetApprovedLeaveCount();


        //}

        public int savewrkfrmhome(string LeaveStartDate, string LeaveEndDate, string Usrl_UserId)
        {
            LeaveTypeDAC leavedac = new LeaveTypeDAC();
            return leavedac.savewrkfrmhome(LeaveStartDate, LeaveEndDate, Usrl_UserId);
        }



        public string SaveWorkFromHomeStatus(string userwfhId, string status, string managerid)
        {
            LeaveTypeDAC leavedac = new LeaveTypeDAC();
            return leavedac.SaveWorkFromHomeStatus(userwfhId, status, managerid);
        }
        public LeaveTypeEntity GetUSaccmail(int uid)
        {
            LeaveTypeDAC lookupDAC = new LeaveTypeDAC();
            return lookupDAC.GetUSaccmail(uid);
        }
        public List<LeaveTypeEntity> GetHolidayDates(int accountid)
        {
            HolidayCalendarDAC holidayDAC = new HolidayCalendarDAC();
            return holidayDAC.GetHolidayDates(accountid);
        }

        public int saveleavecountForUS(string LeaveStartDate, string LeaveEndDate, string Usrl_UserId, int businessdays)
        {
            LeaveTypeDAC leavedac = new LeaveTypeDAC();
            return leavedac.saveleavecountForUS(LeaveStartDate, LeaveEndDate, Usrl_UserId, businessdays);
        }

        public int savewrkfrmhomeForUS(string LeaveStartDate, string LeaveEndDate, string Usrl_UserId, int businessdays)
        {
            LeaveTypeDAC leavedac = new LeaveTypeDAC();
            return leavedac.savewrkfrmhomeForUS(LeaveStartDate, LeaveEndDate, Usrl_UserId, businessdays);
        }

        public string saveWebApprovalStatus(string leaveid, string Leavestatus, string ManagerId)
        {
            LeaveTypeDAC leavedac = new LeaveTypeDAC();
            return leavedac.saveWebApprovalStatus(leaveid, Leavestatus, ManagerId);
        }

    }

}
