using Evolutyz.Business;
using Evolutyz.Data;
using Evolutyz.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using EvolutyzCorner.UI.Web.Models;
using System.Configuration;
using evolCorner.Models;
using RestSharp;
using RestSharp.Authenticators;
using System.Net;

namespace EvolutyzCorner.UI.Web.Controllers
{
    public class TimesheetController : Controller
    {
        string str = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public string htmlStr = "";
        public int timeSheetID = 0;
        SqlConnection Conn = new SqlConnection();
        SqlDataAdapter da = new SqlDataAdapter(); DataSet ds = new DataSet(); DataTable dt = new DataTable();
        DataSet ds1 = new DataSet(); DataTable dtheadings = new DataTable(); DataTable dtData = new DataTable();
        EmailFormats objemail = new EmailFormats(); string StatusMsg = string.Empty;
        EvolutyzCornerDataEntities db = new EvolutyzCornerDataEntities();
        timesheet lstusers = new timesheet();
        ClientComponent obj = new ClientComponent();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult WeeklyTimesheets()
        {
            UserSessionInfo _objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
            int AccountId = _objSessioninfo.AccountId;
            int UserId = _objSessioninfo.UserId;
            ViewBag.AccountID = AccountId;
            ViewBag.UserId = UserId;
            return View();

        }

        #region Insertion (addSubmitTimeSheet)
        [HttpPost]
        public ActionResult AddSubmitTimesheet(TotalTimeSheetTimeDetails sheetObj)
        {

            Conn = new SqlConnection(str);
            //  SendEmail objMail = new SendEmail();Admin@evolutyz.in

            int Trans_Output = 0;
            try
            {
                if (Conn.State != System.Data.ConnectionState.Open)
                    Conn.Open();
                SqlCommand objCommand = new SqlCommand("[AddSubmitTimesheetWeb]", Conn);
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.Parameters.AddWithValue("@UserID", sheetObj.timesheets.UserID);
                objCommand.Parameters.AddWithValue("@TimesheetMonth", Convert.ToDateTime(sheetObj.timesheets.TimeSheetMonth).ToShortDateString());
                objCommand.Parameters.AddWithValue("@Comments", sheetObj.timesheets.Comments);
                //  objCommand.Parameters.AddWithValue("@ProjectID", sheetObj.timesheets.ProjectID);
                objCommand.Parameters.AddWithValue("@SubmittedType", sheetObj.timesheets.SubmittedType);
                objCommand.Parameters.AddWithValue("@L1ApproverStatus", false);
                objCommand.Parameters.AddWithValue("@L2ApproverStatus", false);
                objCommand.Parameters.Add("@TimesheetID", SqlDbType.Int);
                objCommand.Parameters["@TimesheetID"].Direction = ParameterDirection.Output;
                objCommand.Parameters.Add("@Trans_Output", SqlDbType.Int);
                objCommand.Parameters["@Trans_Output"].Direction = ParameterDirection.Output;
                objCommand.ExecuteNonQuery();
                timeSheetID = int.Parse(objCommand.Parameters["@TimesheetID"].Value.ToString());
                Trans_Output = int.Parse(objCommand.Parameters["@Trans_Output"].Value.ToString());


                if (timeSheetID > 0)
                {
                    foreach (var item in sheetObj.listtimesheetdetails)
                    {

                        if ((Trans_Output == 105) || (Trans_Output == 106))
                        {
                            objCommand = new SqlCommand("[EditSubmitTaskDetails]", Conn);
                            objCommand.CommandType = System.Data.CommandType.StoredProcedure;
                            objCommand.Parameters.AddWithValue("@TimesheetID", timeSheetID);
                            objCommand.Parameters.AddWithValue("@ProjectID", item.projectid);
                            objCommand.Parameters.AddWithValue("@TaskId", item.taskid);
                            objCommand.Parameters.AddWithValue("@HoursWorked", item.hoursWorked);
                            if (item.taskDate != null)
                            {
                                string dt = DateTime.Parse(item.taskDate.Trim())
     .ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

                                objCommand.Parameters.AddWithValue("@TaskDate", dt);
                            }
                            else
                            {


                            }

                            objCommand.Parameters.Add("@Trans_Output", SqlDbType.Int);
                            objCommand.Parameters["@Trans_Output"].Direction = ParameterDirection.Output;
                            objCommand.ExecuteNonQuery();

                        }
                        else
                        {
                            objCommand = new SqlCommand("[AddSubmitTaskDetails]", Conn);
                            objCommand.CommandType = System.Data.CommandType.StoredProcedure;
                            objCommand.Parameters.AddWithValue("@TimesheetID", timeSheetID);
                            objCommand.Parameters.AddWithValue("@ProjectID", item.projectid);
                            objCommand.Parameters.AddWithValue("@TaskId", item.taskid);

                            objCommand.Parameters.AddWithValue("@HoursWorked", item.hoursWorked);
                            if (item.taskDate != null)
                            {
                                string dt = DateTime.Parse(item.taskDate.Trim())
     .ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

                                objCommand.Parameters.AddWithValue("@TaskDate", dt);
                            }
                            else
                            {


                            }
                            objCommand.ExecuteNonQuery();
                        }

                    }

                }

                Conn.Close();
                if (Trans_Output == 1)
                {

                    if (sheetObj.timesheets.SubmittedType == "Submit")
                    {
                        SendMailsForApprovals(sheetObj);
                        StatusMsg = "TimeSheet Submitted Successfully..";

                    }

                    else
                    {
                        StatusMsg = "TimeSheet Saved Successfully..";

                    }
                }

                else
                {

                    if (Trans_Output == 104)
                    {

                        StatusMsg = "TimeSheet for this particular Month '" + sheetObj.timesheets.TimeSheetMonth + "' is already exists for this User";

                        //return StatusMsg;
                    }

                    else if (Trans_Output == 105)
                    {
                        StatusMsg = "TimeSheet for this particular Month '" + sheetObj.timesheets.TimeSheetMonth + "' is Saved Sucessfully";
                    }

                    else if (Trans_Output == 106)
                    {
                        SendMailsForApprovals(sheetObj);
                        StatusMsg = "TimeSheet for this particular Month '" + sheetObj.timesheets.TimeSheetMonth + "' is Submitted Sucessfully";
                    }

                    else if (Trans_Output == 111)
                    {
                        StatusMsg = "TimeSheet for this particular Month '" + sheetObj.timesheets.TimeSheetMonth + "' is Already  Submitted";
                    }

                    else
                    {
                        StatusMsg = "Precondition Failed";
                    }

                }

            }

            catch (Exception ex)
            {

                StatusMsg = ex.Message.ToString();


            }
            finally
            {
                if (Conn != null)
                {
                    if (Conn.State == ConnectionState.Open)
                    {
                        Conn.Close();
                        Conn.Dispose();
                    }
                }

            }
            //  return objmessageDetails;

            return Json(StatusMsg, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region SendMails
        public string SendMailsForApprovals(TotalTimeSheetTimeDetails sheetObj)
        {
            Conn = new SqlConnection(str); int Userid = 0; int sessionuserid = 0;
            DataTable dtaccMgr = new DataTable();

            List<manageremails> objManagerlist = new List<manageremails>();
            if ((sheetObj.timesheets.UserID != 0))
            {
                Userid = sheetObj.timesheets.UserID;
            }
            else
            {
                UserSessionInfo sessId = Session["UserSessionInfo"] as UserSessionInfo;
                sessionuserid = sessId.UserId;
            }

            try
            {
                if (Conn.State != System.Data.ConnectionState.Open)
                    Conn.Open();
                SqlCommand objCommand = new SqlCommand("[GetManagerEmailIds]", Conn);
                objCommand.CommandType = CommandType.StoredProcedure;
                if ((sheetObj.timesheets.UserID != 0))
                {
                    objCommand.Parameters.AddWithValue("@userid", Userid);
                }
                else
                {
                    objCommand.Parameters.AddWithValue("@userid", sessionuserid);
                }
                da = new SqlDataAdapter(objCommand);

                da.Fill(ds);
                dt = ds.Tables[0];
                dtaccMgr = ds.Tables[1];
                SqlDataReader dr = objCommand.ExecuteReader();

                if (dt != null)
                {
                    lstusers = new timesheet()
                    {
                        ManagerEmail1 = dt.Rows[0]["ManagerLevel1"].ToString(),
                        ManagerEmail2 = dt.Rows[0]["ManagerLevel2"].ToString(),
                        AccManagerEmail = dt.Rows[0]["Acc_EmailID"].ToString(),
                        UserName = dt.Rows[0]["UserName"].ToString(),
                        UserEmailId = dt.Rows[0]["UserEmailid"].ToString(),
                        ManagerID1 = dt.Rows[0]["L1_ManagerID"].ToString(),
                        ManagerID2 = dt.Rows[0]["L2_ManagerID"].ToString(),
                        ManagerId = sheetObj.timesheets.ManagerId,
                        EmailAppOrRejStatus = sheetObj.timesheets.EmailAppOrRejStatus,
                        SubmittedFlag = sheetObj.timesheets.SubmittedFlag,
                        ManagerName1 = dt.Rows[0]["L1_ManagerName"].ToString(),
                        ManagerName2 = dt.Rows[0]["L2_ManagerName"].ToString(),
                    };

                }

                foreach (DataRow accmanager in ds.Tables[1].Rows)
                {

                    objManagerlist.Add(new manageremails
                    {
                        manageremail = accmanager["Usr_LoginId"].ToString(),

                    });

                }

                Conn.Close();
                Conn.Open();
                objCommand = new SqlCommand("[getTimeSheetEmailDetails]", Conn);
                objCommand.CommandType = System.Data.CommandType.StoredProcedure;
                if ((sheetObj.timesheets.UserID != 0))
                {
                    objCommand.Parameters.AddWithValue("@userid", Userid);
                }
                else
                {
                    objCommand.Parameters.AddWithValue("@userid", sessionuserid);
                }
                objCommand.Parameters.AddWithValue("@Timesheetmonth", Convert.ToDateTime(sheetObj.timesheets.TimeSheetMonth).ToShortDateString());
                ds1 = new DataSet();
                da = new SqlDataAdapter(objCommand);
                da.Fill(ds1);
                dtheadings = new DataTable();
                dtData = new DataTable();
                dtheadings = ds1.Tables[0];
                dtData = ds1.Tables[1];
                string subject = string.Empty;
                if ((!string.IsNullOrEmpty(sheetObj.timesheets.SubmittedType)) && (sheetObj.timesheets.SubmittedType == "Submit"))
                {
                    //if (lstusers.ManagerEmail1 != null && lstusers.ManagerEmail2 != null && flag == 1)
                    //{
                    string body = objemail.SendEmail(lstusers, ConvertDataTableToHTML(dtheadings, dtData, "1"));

                    SendMailsByMailGun(lstusers, objManagerlist, dtheadings, body, 1, "");

                    // }
                }

                if ((!string.IsNullOrEmpty(sheetObj.timesheets.SubmittedFlag)) && sheetObj.timesheets.SubmittedFlag.ToString() == "2")
                {

                    if ((sheetObj.timesheets.Transoutput == 1) || (sheetObj.timesheets.Transoutput == 2) || (sheetObj.timesheets.Transoutput == 3) || (sheetObj.timesheets.Transoutput == 4))
                    {
                        string body = objemail.SendEmail(lstusers, ConvertDataTableToHTML(dtheadings, dtData, "2"));
                        StatusMsg = SendMailsByMailGun(lstusers, objManagerlist, dtheadings, body, 2, sheetObj.timesheets.ActionType);
                        return StatusMsg;
                    }

                    else
                    {
                        string body = objemail.SendEmail(lstusers, ConvertDataTableToHTML(dtheadings, dtData, "2"));
                        return body;
                    }



                }

                if (sheetObj.timesheets.SubmittedType.ToString() == "3")
                {
                    UserSessionInfo sessId = Session["UserSessionInfo"] as UserSessionInfo;
                    sessionuserid = sessId.UserId;
                    lstusers = new timesheet()
                    {
                        ManagerEmail1 = dt.Rows[0]["ManagerLevel1"].ToString(),
                        ManagerEmail2 = dt.Rows[0]["ManagerLevel2"].ToString(),
                        AccManagerEmail = dt.Rows[0]["Acc_EmailID"].ToString(),
                        UserName = dt.Rows[0]["UserName"].ToString(),
                        UserEmailId = dt.Rows[0]["UserEmailid"].ToString(),
                        ManagerID1 = dt.Rows[0]["L1_ManagerID"].ToString(),
                        ManagerID2 = dt.Rows[0]["L2_ManagerID"].ToString(),
                        EmailAppOrRejStatus = "1",
                        ManagerId = Convert.ToString(sessionuserid),
                        ManagerName1 = dt.Rows[0]["L1_ManagerName"].ToString(),
                        ManagerName2 = dt.Rows[0]["L2_ManagerName"].ToString(),

                    };
                    string body = objemail.SendEmail(lstusers, ConvertDataTableToHTML(dtheadings, dtData, "2"));
                    SendMailsByMailGun(lstusers, objManagerlist, dtheadings, body, 2, "2");
                    StatusMsg = lstusers.ManagerName1 + "-" + lstusers.ManagerName2;

                }

                if (sheetObj.timesheets.SubmittedType.ToString() == "4")
                {
                    UserSessionInfo sessId = Session["UserSessionInfo"] as UserSessionInfo;
                    sessionuserid = sessId.UserId;
                    lstusers = new timesheet()
                    {
                        ManagerEmail1 = dt.Rows[0]["ManagerLevel1"].ToString(),
                        ManagerEmail2 = dt.Rows[0]["ManagerLevel2"].ToString(),
                        AccManagerEmail = dt.Rows[0]["Acc_EmailID"].ToString(),
                        UserName = dt.Rows[0]["UserName"].ToString(),
                        UserEmailId = dt.Rows[0]["UserEmailid"].ToString(),
                        ManagerID1 = dt.Rows[0]["L1_ManagerID"].ToString(),
                        ManagerID2 = dt.Rows[0]["L2_ManagerID"].ToString(),
                        EmailAppOrRejStatus = "0",
                        ManagerId = Convert.ToString(sessionuserid),
                        ManagerName1 = dt.Rows[0]["L1_ManagerName"].ToString(),
                        ManagerName2 = dt.Rows[0]["L2_ManagerName"].ToString(),

                    };
                    string body = objemail.SendEmail(lstusers, ConvertDataTableToHTML(dtheadings, dtData, "2"));
                    SendMailsByMailGun(lstusers, objManagerlist, dtheadings, body, 2, "2");
                    StatusMsg = lstusers.ManagerName1 + "-" + lstusers.ManagerName2;
                    // StatusMsg = lstusers.ManagerName1 + "-" + lstusers.ManagerName2;
                    //string body = objemail.SendEmail(lstusers, ConvertDataTableToHTML(dtheadings, dtData, "2"));
                    //StatusMsg = SendMailsByMailGun(lstusers, objManagerlist, dtheadings, body, 2, "2");
                    //return StatusMsg;
                }


            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            return StatusMsg;

        }
        #endregion

        #region SendMailsByMailGun
        public static string SendMailsByMailGun(timesheet lstusers, List<manageremails> objManagerlist, DataTable dtHeading, string body, int flag, string ActionType)
        {


            string[] ToMuliId = new string[0];
            string subject = string.Empty;
            string ManagersIDs = string.Empty; bool flagforemail = false;
            RestClient client = new RestClient();
            string href = string.Empty; string ManagerID = string.Empty;
            string LevelManagerID = string.Empty, TimesheetId = string.Empty, Timesheetmonth = string.Empty,
            UserID = string.Empty; string disbledstr = string.Empty; string Projectid = string.Empty;
            string ManagerNameL1 = string.Empty; string ManagerNameL2 = string.Empty;

            HttpStatusCode statusCode; int numericStatusCode; RestRequest request = new RestRequest();
            string host = System.Web.HttpContext.Current.Request.Url.Host;
            string port = System.Web.HttpContext.Current.Request.Url.Port.ToString();
            string port1 = System.Configuration.ConfigurationManager.AppSettings["RedirectURL"];

            string UrlEmailAddress = string.Empty;

            if (host == "localhost")
            {
                UrlEmailAddress = host + ":" + port;
            }
            else
            {
                UrlEmailAddress = port1;
            }


            int j = 0;
            try
            {
                client.BaseUrl = new Uri("https://api.mailgun.net/v3");
                client.Authenticator = new HttpBasicAuthenticator("api", "key-25b8e8c23e4a09ef67b7957d77eb7413");
                //request.AddHeader

                if (flag == 1)
                {

                    Encrypt objEncrypt;
               //  ToMuliId = new string[2] { "sreelakshmi.evolutyz@gmail.com", "" };

                     ToMuliId = new string[2] { lstusers.ManagerEmail1.ToString().Trim(), lstusers.ManagerEmail2.ToString().Trim()};


                    for (int i = 0; i < ToMuliId.Length; i++)
                    {
                        request = new RestRequest();
                        request.AddParameter("domain", "evolutyzstaging.com", ParameterType.UrlSegment);
                        request.Resource = "{domain}/messages";
                        request.AddParameter("from", "Evolutyz IT Services<mailgun@evolutyzstaging.com>");
                        if (i == 0 && flagforemail == false)
                        {
                            request.AddParameter("to", ToMuliId[0]);
                            ManagerID = lstusers.ManagerID1.ToString();
                            string LevelManagerID1 = string.Empty;
                            subject = "Timesheet of " + lstusers.UserName + " for month " + dtHeading.Rows[0]["TimesheetMonth"].ToString() + " is  submitted  and waiting for your approval";
                            objEncrypt = new Encrypt();
                            LevelManagerID1 = objEncrypt.Encryption(ManagerID).ToString();
                            TimesheetId = objEncrypt.Encryption(dtHeading.Rows[0]["TimesheetId"].ToString());
                            Timesheetmonth = objEncrypt.Encryption(Convert.ToString(dtHeading.Rows[0]["TimesheetMonthforMail"]));
                            UserID = objEncrypt.Encryption(dtHeading.Rows[0]["Usr_UserID"].ToString());
                            Projectid = objEncrypt.Encryption(Convert.ToString(dtHeading.Rows[0]["ProjectID"].ToString()));
                            ManagerNameL1 = objEncrypt.Encryption(lstusers.ManagerName1.ToString());
                            ManagerNameL2 = objEncrypt.Encryption(lstusers.ManagerName2.ToString());
                            if (body.Contains("ApproveID"))
                            {
                                j = 1;
                                if (ManagerID != "0")
                                {
                                    href = "href='" + "http://" + UrlEmailAddress + "/TimeSheetActions.aspx?MID=" + LevelManagerID1 + "&TID=" + TimesheetId + "&AT=" + objEncrypt.Encryption(Convert.ToString(j)) + "&TD=" + Timesheetmonth + "&Uid=" + UserID + "&F=" + objEncrypt.Encryption("2") + "&Pr=" + Projectid + "&MNOL=" + ManagerNameL1 + "'";
                                    body = body.Replace("href", href);
                                }
                            }
                            if (body.Contains("RejectId"))
                            {
                                j = 0;
                                href = string.Empty;
                                if (ManagerID != "0")
                                {
                                    href = "href='" + "http://" + UrlEmailAddress + "/TimeSheetActions.aspx?MID=" + LevelManagerID1 + "&TID=" + TimesheetId + "&AT=" + objEncrypt.Encryption(Convert.ToString(j)) + "&TD=" + Timesheetmonth + "&Uid=" + UserID + "&F=" + objEncrypt.Encryption("2") + "&Pr=" + Projectid + "&MNOL=" + ManagerNameL1 + "'";
                                    body = body.Replace("title", href);
                                }
                                //  ping to title
                            }
                        }
                        if (i == 1 && flagforemail == true)
                        {
                            request.AddParameter("to", ToMuliId[1]);
                            ManagerID = string.Empty;
                            ManagerID = lstusers.ManagerID2.ToString();
                            subject = "Timesheet of " + lstusers.UserName + " for month " + dtHeading.Rows[0]["TimesheetMonth"].ToString() + " is  submitted  and waiting for your approval";
                            string LevelManagerID2 = string.Empty;
                            objEncrypt = new Encrypt();
                            LevelManagerID2 = objEncrypt.Encryption(ManagerID).ToString();
                            TimesheetId = objEncrypt.Encryption(dtHeading.Rows[0]["TimesheetId"].ToString());
                            Timesheetmonth = objEncrypt.Encryption(Convert.ToString(dtHeading.Rows[0]["TimesheetMonthforMail"]));
                            UserID = objEncrypt.Encryption(dtHeading.Rows[0]["Usr_UserID"].ToString());
                            Projectid = objEncrypt.Encryption(Convert.ToString(dtHeading.Rows[0]["ProjectID"].ToString()));
                            ManagerNameL1 = objEncrypt.Encryption(lstusers.ManagerName1.ToString());
                            ManagerNameL2 = objEncrypt.Encryption(lstusers.ManagerName2.ToString());
                            if (body.Contains("ApproveID"))
                            {
                                j = 1;
                                href = string.Empty;
                                body = body.Replace("name", href);
                                if (ManagerID != "0")
                                {
                                    href = "href='" + "http://" + UrlEmailAddress + "/TimeSheetActions.aspx?MID=" + LevelManagerID2 + "&TID=" + TimesheetId + "&AT=" + objEncrypt.Encryption(Convert.ToString(j)) + "&TD=" + Timesheetmonth + "&Uid=" + UserID + "&F=" + objEncrypt.Encryption("2") + "&Pr=" + Projectid + "&MNOL=" + ManagerNameL2 + "'";
                                    body = body.Replace("name", href);
                                }
                            }

                            if (body.Contains("RejectId"))
                            {

                                j = 0;

                                href = string.Empty;
                                body = body.Replace("rev", href);
                                if (ManagerID != "0")
                                {
                                    href = "href='" + "http://" + UrlEmailAddress + "/TimeSheetActions.aspx?MID=" + LevelManagerID2 + "&TID=" + TimesheetId + "&AT=" + objEncrypt.Encryption(Convert.ToString(j)) + "&TD=" + Timesheetmonth + "&Uid=" + UserID + "&F=" + objEncrypt.Encryption("2") + "&Pr=" + Projectid + "&MNOL=" + ManagerNameL2 + "'";
                                    body = body.Replace("rev", href);

                                }
                            }

                        }


                        var emailcontent = "<html>" +
                                                          "<body />" +
                                                          "<table style='width:100%;border:1px solid #d6d6d6;border-collapse: collapse;'>" +
                                                          " <tr style='background: linear-gradient(#0a639a,#0689bd);background-color: #6cb0c9;'>" +
                                                          "<td style=' padding: 8px 4px; '>" +
                                                          "<img alt='Company Logo' src='http://evolutyz.in/img/evolutyz-logo.png'/>" +
                                                          " </td>" +
                                                          "</tr>" +
                                                          " <tr>" +
                                                          "<td style=' padding: 10px 5px;    background-color: #ffffff; '>" +
                                                          body +
                                                          // "HI" +
                                                          " </td>" +
                                                          "</tr>" +
                                                          " <tr>" +
                                                          "<td style=' padding: 10px 5px;    background-color: #ffffff; '>" +
                                                          //"<b>" + msg + "</b> " +
                                                          " </td>" +
                                                          "</tr>" +
                                                          " <tr style='background-color: #6cb0c9;'>" +
                                                          "<td style='font-size: 9px;text-align:center; '>" +
                                                          "<a style= 'color: #ffffff;' href='www.evolutyz.com' target='_blank'>www.evolutyz.com</a>" +
                                                          " </td>" +
                                                          "</tr>" +
                                                          "</table>" +
                                                          "</body>" +
                                                          "</html>";


                        request.AddParameter("subject", subject);
                        request.AddParameter("html", emailcontent);
                        request.Method = Method.POST;
                        var a = client.Execute(request);
                        statusCode = a.StatusCode;
                        numericStatusCode = (int)statusCode;

                        if (numericStatusCode == 200)
                        {
                            flagforemail = true;


                        }
                        else
                        {
                            flagforemail = true;
                        }


                    }


                    for (int AcctMgrid = 0; AcctMgrid < objManagerlist.Count; AcctMgrid++)//Account Managerids
                    {

                        request = new RestRequest();
                        request.AddParameter("domain", "evolutyzstaging.com", ParameterType.UrlSegment);
                        request.Resource = "{domain}/messages";
                        request.AddParameter("from", "Evolutyz ITServices <mailgun@evolutyzstaging.com>");
                        request.AddParameter("to", objManagerlist[AcctMgrid].manageremail);
                        subject = "Timesheet of " + lstusers.UserName + " for month " + dtHeading.Rows[0]["TimesheetMonth"].ToString() + " is submitted to managers levels";
                        //disbledstr = "disabled='true'";

                        disbledstr = "display:none;";

                        if (body.Contains("ApproveID"))
                        {
                            body = body.Replace("display:block", disbledstr);
                        }

                        if (body.Contains("RejectId"))
                        {
                            body = body.Replace("display:block", disbledstr);

                        }

                        var emailcontent = "<html>" +
                                                          "<body />" +
                                                          "<table style='width:100%;border:1px solid #d6d6d6;border-collapse: collapse;'>" +
                                                          " <tr style='background: linear-gradient(#0a639a,#0689bd);background-color: #6cb0c9;'>" +
                                                          "<td style=' padding: 8px 4px; '>" +
                                                          //"<img src= " + UrlEmailAddress + "'/Content/images/CompanyLogos/502.png'  style='display: block; max-width: 100%; height: 40px;'>" +
                                                          "<img alt='Company Logo' src='http://evolutyz.in/img/evolutyz-logo.png'/>" +
                                                          " </td>" +
                                                          "</tr>" +
                                                          " <tr>" +
                                                          "<td style=' padding: 10px 5px;    background-color: #ffffff; '>" +
                                                          body +
                                                          // "HI" +
                                                          " </td>" +
                                                          "</tr>" +
                                                          " <tr>" +
                                                          "<td style=' padding: 10px 5px;    background-color: #ffffff; '>" +
                                                          //"<b>" + msg + "</b> " +
                                                          " </td>" +
                                                          "</tr>" +
                                                          " <tr style='background-color: #6cb0c9;'>" +
                                                          "<td style='font-size: 9px;text-align:center; '>" +
                                                          "<a style= 'color: #ffffff;' href='www.evolutyz.com' target='_blank'>www.evolutyz.com</a>" +
                                                          " </td>" +
                                                          "</tr>" +
                                                          "</table>" +
                                                          "</body>" +
                                                          "</html>";


                        request.AddParameter("subject", subject);
                        request.AddParameter("html", emailcontent);
                        request.Method = Method.POST;
                        var a1 = client.Execute(request);

                        statusCode = a1.StatusCode;
                        numericStatusCode = (int)statusCode;

                        if (numericStatusCode == 200)
                        {
                            flagforemail = true;


                        }
                        else
                        {
                            flagforemail = true;
                        }
                    }


                }


                if (flag == 2)
                {

                    if (lstusers.ManagerId.ToString() == lstusers.ManagerID1.ToString())
                    {

                   // ToMuliId = new string[2] { "sreelakshmi.evolutyz@gmail.com", "" };
                        ToMuliId = new string[2] { lstusers.ManagerEmail2.ToString().Trim(), lstusers.UserEmailId.ToString().Trim() };

                        if (lstusers.EmailAppOrRejStatus.ToString() == "1")
                        {
                            subject = "Timesheet of " + lstusers.UserName + " for month " + dtHeading.Rows[0]["TimesheetMonth"].ToString() + " is  approved by level_1 manager (" + lstusers.ManagerName1 + ")";
                        }
                        else if (lstusers.EmailAppOrRejStatus.ToString() == "0")
                        {
                            subject = "Timesheet of " + lstusers.UserName + " for Month " + dtHeading.Rows[0]["TimesheetMonth"].ToString() + " is  rejected by level_1 manager " + lstusers.ManagerName1 + "";
                        }

                        for (int i = 0; i < ToMuliId.Length; i++)
                        {
                            request = new RestRequest();
                            request.AddParameter("domain", "evolutyzstaging.com", ParameterType.UrlSegment);
                            request.Resource = "{domain}/messages";
                            request.AddParameter("from", "Evolutyz ITServices <mailgun@evolutyzstaging.com>");
                            ManagerID = lstusers.ManagerID1.ToString();


                            if (i == 0 && flagforemail == false)
                            {
                                request.AddParameter("to", ToMuliId[0]);

                            }
                            if (i == 1 && flagforemail == true)
                            {
                                request.AddParameter("to", ToMuliId[1]);

                                //if (lstusers.EmailAppOrRejStatus.ToString() == "0")
                                //{
                                //    subject = "Timesheet of " + lstusers.UserName + " for month " + dtHeading.Rows[0]["TimesheetMonth"].ToString() + " is  rejected by level_1 manager and resubmit the timesheet for month " + dtHeading.Rows[0]["TimesheetMonth"].ToString() + "";
                                //}
                            }
                            var emailcontent2 = "<html>" +
                                                      "<body />" +
                                                      "<table style='width:100%;border:1px solid #d6d6d6;border-collapse: collapse;'>" +
                                                      " <tr style='background: linear-gradient(#0a639a,#0689bd);background-color: #6cb0c9;'>" +
                                                      "<td style=' padding: 8px 4px; '>" +
                                                      "<img alt='Company Logo' src='http://evolutyz.in/img/evolutyz-logo.png'/>" +
                                                      " </td>" +
                                                      "</tr>" +
                                                      " <tr>" +
                                                      "<td style=' padding: 10px 5px;    background-color: #ffffff; '>" +
                                                      body +
                                                      // "HI" +
                                                      " </td>" +
                                                      "</tr>" +
                                                      " <tr>" +
                                                      "<td style=' padding: 10px 5px;    background-color: #ffffff; '>" +
                                                      //"<b>" + msg + "</b> " +
                                                      " </td>" +
                                                      "</tr>" +
                                                      " <tr style='background-color: #6cb0c9;'>" +
                                                      "<td style='font-size: 9px;text-align:center; '>" +
                                                      "<a style= 'color: #ffffff;' href='www.evolutyz.com' target='_blank'>www.evolutyz.com</a>" +
                                                      " </td>" +
                                                      "</tr>" +
                                                      "</table>" +
                                                      "</body>" +
                                                      "</html>";


                            request.AddParameter("subject", subject);
                            request.AddParameter("html", emailcontent2);
                            request.Method = Method.POST;
                            var a6 = client.Execute(request);
                            statusCode = a6.StatusCode;
                            numericStatusCode = (int)statusCode;

                            if (numericStatusCode == 200)
                            {
                                flagforemail = true;
                            }
                            else
                            {
                                flagforemail = true;
                            }
                        }


                        for (int AcctMgrid = 0; AcctMgrid < objManagerlist.Count; AcctMgrid++)//Account Managerids
                        {

                            request = new RestRequest();
                            request.AddParameter("domain", "evolutyzstaging.com", ParameterType.UrlSegment);
                            request.Resource = "{domain}/messages";
                            request.AddParameter("from", "Evolutyz ITServices <mailgun@evolutyzstaging.com>");
                            request.AddParameter("to", objManagerlist[AcctMgrid].manageremail);
                            //disbledstr = "disabled='true'";

                            disbledstr = "display:none;";

                            if (body.Contains("ApproveID"))
                            {
                                body = body.Replace("display:block", disbledstr);
                            }

                            if (body.Contains("RejectId"))
                            {
                                body = body.Replace("display:block", disbledstr);

                            }

                            var emailcontent = "<html>" +
                                                              "<body />" +
                                                              "<table style='width:100%;border:1px solid #d6d6d6;border-collapse: collapse;'>" +
                                                              " <tr style='background: linear-gradient(#0a639a,#0689bd);background-color: #6cb0c9;'>" +
                                                              "<td style=' padding: 8px 4px; '>" +
                                                              //"<img src= " + UrlEmailAddress + "'/Content/images/CompanyLogos/502.png'  style='display: block; max-width: 100%; height: 40px;'>" +
                                                              "<img alt='Company Logo' src='http://evolutyz.in/img/evolutyz-logo.png'/>" +
                                                              " </td>" +
                                                              "</tr>" +
                                                              " <tr>" +
                                                              "<td style=' padding: 10px 5px;    background-color: #ffffff; '>" +
                                                              body +
                                                              // "HI" +
                                                              " </td>" +
                                                              "</tr>" +
                                                              " <tr>" +
                                                              "<td style=' padding: 10px 5px;    background-color: #ffffff; '>" +
                                                              //"<b>" + msg + "</b> " +
                                                              " </td>" +
                                                              "</tr>" +
                                                              " <tr style='background-color: #6cb0c9;'>" +
                                                              "<td style='font-size: 9px;text-align:center; '>" +
                                                              "<a style= 'color: #ffffff;' href='www.evolutyz.com' target='_blank'>www.evolutyz.com</a>" +
                                                              " </td>" +
                                                              "</tr>" +
                                                              "</table>" +
                                                              "</body>" +
                                                              "</html>";


                            request.AddParameter("subject", subject);
                            request.AddParameter("html", emailcontent);
                            request.Method = Method.POST;
                            var a1 = client.Execute(request);

                            statusCode = a1.StatusCode;
                            numericStatusCode = (int)statusCode;

                            if (numericStatusCode == 200)
                            {
                                flagforemail = true;


                            }
                            else
                            {
                                flagforemail = true;
                            }
                        }

                    }

                    if (lstusers.ManagerId.ToString() == lstusers.ManagerID2.ToString())
                    {

                      // ToMuliId = new string[2] { "sreelakshmi.evolutyz@gmail.com", "" };
                          ToMuliId = new string[2] { lstusers.ManagerEmail1.ToString().Trim(), lstusers.UserEmailId.ToString().Trim() };
                        if (lstusers.EmailAppOrRejStatus.ToString() == "1")
                        {

                            subject = "Timesheet of " + lstusers.UserName + " for month " + dtHeading.Rows[0]["TimesheetMonth"].ToString() + " is  approved by level_2 manager " + lstusers.ManagerName2 + "";

                        }
                        else if (lstusers.EmailAppOrRejStatus.ToString() == "0")
                        {
                            subject = "Timesheet of " + lstusers.UserName + " for month " + dtHeading.Rows[0]["TimesheetMonth"].ToString() + " is  rejected by level_2 manager " + lstusers.ManagerName2 + "";
                        }
                        for (int i = 0; i < ToMuliId.Length; i++)
                        {
                            request = new RestRequest();
                            request.AddParameter("domain", "evolutyzstaging.com", ParameterType.UrlSegment);
                            request.Resource = "{domain}/messages";
                            request.AddParameter("from", "Evolutyz ITServices <mailgun@evolutyzstaging.com>");
                            ManagerID = lstusers.ManagerID1.ToString();


                            if (i == 0 && flagforemail == false)
                            {
                                request.AddParameter("to", ToMuliId[0]);

                            }
                            if (i == 1 && flagforemail == true)
                            {
                                request.AddParameter("to", ToMuliId[1]);
                                //if (lstusers.EmailAppOrRejStatus.ToString() == "0")
                                //{
                                //    subject = "Timesheet of " + lstusers.UserName + " for month " + dtHeading.Rows[0]["TimesheetMonth"].ToString() + " is  rejected by level_2 manager and resubmit the timesheet for month " + dtHeading.Rows[0]["TimesheetMonth"].ToString() + "";
                                //}
                            }

                            var emailcontent = "<html>" +
                                                              "<body />" +
                                                              "<table style='width:100%;border:1px solid #d6d6d6;border-collapse: collapse;'>" +
                                                              " <tr style='background: linear-gradient(#0a639a,#0689bd);background-color: #6cb0c9;'>" +
                                                              "<td style=' padding: 8px 4px; '>" +
                                                              //"<img src=" + UrlEmailAddress + "'/Content/images/CompanyLogos/502.png'  style='display: block; max-width: 100%; height: 40px;'>" +
                                                              "<img alt='Company Logo' src='http://evolutyz.in/img/evolutyz-logo.png'/>" +
                                                              " </td>" +
                                                              "</tr>" +
                                                              " <tr>" +
                                                              "<td style=' padding: 10px 5px;    background-color: #ffffff; '>" +
                                                              body +
                                                              " </td>" +
                                                              "</tr>" +
                                                              " <tr>" +
                                                              "<td style=' padding: 10px 5px;    background-color: #ffffff; '>" +
                                                              " </td>" +
                                                              "</tr>" +
                                                              " <tr style='background-color: #6cb0c9;'>" +
                                                              "<td style='font-size: 9px;text-align:center; '>" +
                                                              "<a style= 'color: #ffffff;' href='www.evolutyz.com' target='_blank'>www.evolutyz.com</a>" +
                                                              " </td>" +
                                                              "</tr>" +
                                                              "</table>" +
                                                              "</body>" +
                                                              "</html>";


                            request.AddParameter("subject", subject);
                            request.AddParameter("html", emailcontent);
                            request.Method = Method.POST;
                            var a5 = client.Execute(request);
                            statusCode = a5.StatusCode;
                            numericStatusCode = (int)statusCode;
                            if (numericStatusCode == 200)
                            {
                                flagforemail = true;
                            }
                            else
                            {
                                flagforemail = true;

                            }


                        }


                        for (int AcctMgrid = 0; AcctMgrid < objManagerlist.Count; AcctMgrid++)//Account Managerids
                        {

                            request = new RestRequest();
                            request.AddParameter("domain", "evolutyzstaging.com", ParameterType.UrlSegment);
                            request.Resource = "{domain}/messages";
                            request.AddParameter("from", "Evolutyz ITServices <mailgun@evolutyzstaging.com>");
                            request.AddParameter("to", objManagerlist[AcctMgrid].manageremail);

                            var emailcontent = "<html>" +
                                                              "<body />" +
                                                              "<table style='width:100%;border:1px solid #d6d6d6;border-collapse: collapse;'>" +
                                                              " <tr style='background: linear-gradient(#0a639a,#0689bd);background-color: #6cb0c9;'>" +
                                                              "<td style=' padding: 8px 4px; '>" +
                                                              //"<img src= " + UrlEmailAddress + "'/Content/images/CompanyLogos/502.png'  style='display: block; max-width: 100%; height: 40px;'>" +
                                                              "<img alt='Company Logo' src='http://evolutyz.in/img/evolutyz-logo.png'/>" +
                                                              " </td>" +
                                                              "</tr>" +
                                                              " <tr>" +
                                                              "<td style=' padding: 10px 5px;    background-color: #ffffff; '>" +
                                                              body +
                                                              // "HI" +
                                                              " </td>" +
                                                              "</tr>" +
                                                              " <tr>" +
                                                              "<td style=' padding: 10px 5px;    background-color: #ffffff; '>" +
                                                              //"<b>" + msg + "</b> " +
                                                              " </td>" +
                                                              "</tr>" +
                                                              " <tr style='background-color: #6cb0c9;'>" +
                                                              "<td style='font-size: 9px;text-align:center; '>" +
                                                              "<a style= 'color: #ffffff;' href='www.evolutyz.com' target='_blank'>www.evolutyz.com</a>" +
                                                              " </td>" +
                                                              "</tr>" +
                                                              "</table>" +
                                                              "</body>" +
                                                              "</html>";


                            request.AddParameter("subject", subject);
                            request.AddParameter("html", emailcontent);
                            request.Method = Method.POST;
                            var a1 = client.Execute(request);

                            statusCode = a1.StatusCode;
                            numericStatusCode = (int)statusCode;

                            if (numericStatusCode == 200)
                            {
                                flagforemail = true;


                            }
                            else
                            {
                                flagforemail = true;
                            }
                        }

                    }
                    //objTA.MailsByMailGun(lstusers, body);

                    return body;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return "Success";
        }
        #endregion

        #region ConvertDataTableToHTML 
        public static string ConvertDataTableToHTML(DataTable dtHeading, DataTable TimeSheetdt, string flag)
        {
            string html = ""; string html1 = string.Empty; string ApproveBtn = string.Empty, RejectBtn = string.Empty;
            if (flag == "1")
            {
                ApproveBtn = "block";
                RejectBtn = "block";
            }
            else
            {
                ApproveBtn = "none";
                RejectBtn = "none";
            }
            var totalRows = TimeSheetdt.Rows.Count;
            int halfway = totalRows / 2;
            var firstHalfdt = TimeSheetdt.AsEnumerable().Take(halfway).CopyToDataTable();
            var secondHalfdt = TimeSheetdt.AsEnumerable().Skip(halfway).CopyToDataTable();


            html += "<form style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;background-color:#ffffff;'>" +
             "<div style='-webkit-box-sizing: border-box;-moz-box-sizing:border-box;box-sizing:border-box;padding:0 10px;'> " +
           " <table style='width:100%;border-spacing:0px 20px;border-collapse:separate;border:0;-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;'>" +
          "<tbody style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box'>" +
          "<tr style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box'>" +
          "<th style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;text-align:left;'> Employee Name: </ th > " +
          "<td style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;text-align:left;'>" + dtHeading.Rows[0]["Employee Name"].ToString() + "</td>" +
          "<th style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;text-align:left;'> Client Name: </th> " +
          "<td style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;text-align:left;'>" + dtHeading.Rows[0]["Proj_ClientName"].ToString() + " </ td > " +
          "<th style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;text-align:left;'> Project Name: </th> " +
          "<td style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;text-align:left;'>" + dtHeading.Rows[0]["ProjectName"].ToString() + " </td> " +
          "</tr><tr style='-webkit-box-sizing: border-box;-moz-box-sizing:border-box; box-sizing: border-box'> " +
          "<th style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;text-align:left;'>Timesheet Cycle: </ th >" +
          "<td style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;text-align:left;'> Monthly </ td >" +
          "<th style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;text-align:left;'> Duration : </ th >" +
          "<td style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;text-align:left;'>" + dtHeading.Rows[0]["TimesheetMonth"].ToString() + "</td>" +
           "<th style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;text-align:left;'> Submitted Date: </th>" +
          "<td style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;text-align:left;' > " + dtHeading.Rows[0]["SubmittedDate"].ToString() + "</td>" +
           "</tr></tbody></table></div>";
            //fisrt table

            html += "<table id='tblvertical' style='width:100%;table-layout: fixed;'><tr><td style='vertical-align: top;'>";
            html += "<div style='overflow-x:auto;'><section style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;position:relative;min-height:1px;' > " +
           "<table style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;border-collapse:collapse;border-spacing:0;width:100%;border:1px solid #ddd;background-color: #fff;' >" +
           "<thead style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box'>" +
           "<tr style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box'> " +
           "<th style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;text-align:left;padding:8px;border:1px solid #ccc;font-size:13px;background:#666;color:white;'>Date</th> " +
           "<th style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;text-align:left;padding:8px;border:1px solid #ccc;font-size:13px;background:#666;color:white;'> Task </ th > " +
           "<th style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;text-align:left;padding:8px;border:1px solid #ccc;font-size:13px;background:#666;color:white;'> Hours </ th >" +
           "</tr></thead><tbody style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box'>";

            for (int i = 0; i < firstHalfdt.Rows.Count; i++)
            {
                html += "<tr style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;background-color:#ffffff;'>";

                for (int j = 0; j < firstHalfdt.Columns.Count; j++)
                {
                    html += "<td style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;text-align:left;padding:8px;border:1px solid #ccc;font-size:13px;'>" + firstHalfdt.Rows[i][j].ToString() + "</td>";

                }
                html += "</tr>";
            }

            html += "</tbody></table></section></div></td>";
            //second datatable
            html += "<td style='vertical-align: top;'><div style='overflow-x:auto;'><section style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;position:relative;min-height:1px;' > " +
              "<table style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;border-collapse:collapse;border-spacing:0;width:100%;border:1px solid #ddd;background-color: #fff;' >" +
              "<thead style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box'>" +
              "<tr style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box'> " +
              "<th style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;text-align:left;padding:8px;border:1px solid #ccc;font-size:13px;background:#666;color:white;'>Date</th> " +
              "<th style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;text-align:left;padding:8px;border:1px solid #ccc;font-size:13px;background:#666;color:white;'> Task </ th > " +
              "<th style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;text-align:left;padding:8px;border:1px solid #ccc;font-size:13px;background:#666;color:white;'> Hours </ th >" +
              "</tr></thead><tbody style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box'>";

            for (int i = 0; i < secondHalfdt.Rows.Count; i++)
            {
                html += "<tr style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;background-color:#ffffff;'>";


                for (int j = 0; j < secondHalfdt.Columns.Count; j++)
                {
                    html += "<td style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;text-align:left;padding:8px;border:1px solid #ccc;font-size:13px;'>" + secondHalfdt.Rows[i][j].ToString() + "</td>";

                }
                html += "</tr>";
            }

            html += "</tbody></table></section></div></td></tr></table>";
            // comments tab
            html += "<div style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;padding:10px;' > " +
   "<div style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;'>" +
   "<label style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;margin:0010px;font-weight:700;display:none;' for='comment'> Comments :</label>" +
   "<textarea style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;border-radius:5px;box-shadow:none;border-color:#d2d6de;font-family:sans-serif;-webkit-appearance:none;-moz-appearance:none;appearance:none;width:100%;display:none;' rows='3' id='comment'></textarea></div></div>";


            //start add Approve and Reject Buttons

            html += "<div style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:" +
                "border-box;overflow-x:auto;text-align:center;padding:10px;'>" +
                 "<section id='secIdApp'  style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;" +
                 "text-align:right;width:20%;float:left;position:relative;padding:0 15px;min-height:1px;'>" +
                 "<a id='ApproveID'   runat='server'  style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;width:100%;display:" + ApproveBtn + ";padding:6px 12px;margin-bottom:0;font-size:14px;font-weight:400;line-height:1.42857143;" +
                 "text-align:center;white-space:nowrap;vertical-align:middle;-ms-touch-action:manipulation;touch-action:manipulation;cursor:pointer;-webkit-user-select:none; -moz-user-select:none;-ms-user-select:none;" +
                 "user-select:none;border:1px solid #090;border-radius:4px;color:#fff;background-color:#090;" +
                 "text-decoration:underline;'  href name  target='_blank'>Approve</a></section> " +

                 "<section  style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;text-align:left;" +
                 "width:30%;float:left;position:relative;padding:0 15px;min-height:1px;'>" +
                 "<a id='RejectId' title style='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;" +
                 "display:" + RejectBtn + ";padding:6px 12px;margin-bottom:0; font-size:14px; font-weight:400; " +
                 "line-height:1.42857143;text-align:center;white-space:nowrap;vertical-align:middle;-ms-touch-action:manipulation; " +
                 "touch-action: manipulation;cursor:pointer;-webkit-user-select:none;-moz-user-select: none;" +
                 "-ms-user-select:none;user-select:none;border:1px solid #C30;" +
                 "border-radius:4px;color:#fff;background-color:#C30;text-decoration:underline;" +
                 "' rev  target='_blank'>Reject</a></section>";

            //end Approve and Reject Buttons


            html += "<section style='-webkit-box-sizing: border-box; -moz-box-sizing: border-box; box-sizing: border-box;width: 50%; float: left; position: relative; padding: 0 15px; min-height: 1px;overflow: auto;'><table style ='-webkit-box-sizing: border-box;-moz-box-sizing:border-box;box-sizing:border-box;table-layout: fixed;" +
              "border-collapse:collapse;border-spacing:0;width:100%;border:1px solid #ddd;background-color:#fff;'>" +
              "<tbody style ='-webkit-box-sizing: border-box;-moz-box-sizing:border-box;box-sizing:border-box;'>" +
              "<tr style ='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing: border-box'>" +
              "<th style ='-webkit-box-sizing: border-box; -moz-box-sizing: border-box; box-sizing: border-box;" +
              " font-size:18px; text-align:center; padding: 8px;border: 1px solid #ccc;background: #666; color: white;'> " +
              "Total Working Hours :</th>" +
              "<td style ='-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;font-weight:600;width:75px;" +
              "font-size:18px;text-align:center;padding:8px;border:1px solid #ccc;'> " + dtHeading.Rows[0]["TotalWorkingHours"].ToString() + " </ td > " +
              "</tr></tbody></table></section></div></form> </div></section></body></html>";

            return html;

        }
        #endregion



        //Based on login Credentials data is retrived
        public ActionResult Usertimesheet()
        {
            UserSessionInfo objUserSession = Session["UserSessionInfo"] as UserSessionInfo;
            string _userID = objUserSession.LoginId;
            string _paswd = objUserSession.Password;
            UserProjectdetailsEntity objuser = new UserProjectdetailsEntity();
            int UserName = objuser.User_ID;
            string Password = objuser.Usr_Password;
            LoginComponent loginComponent = new LoginComponent();
            //TempData["Error"] = "Event not raised";
            //TempData["Error"] = "Event not raised";
            //TempData["Error"] = "Event not raised";
            var usersprojects = loginComponent.GetUserProjectsDetailsInfo(objUserSession);
            if (usersprojects != null)
            {
                objuser.User_ID = usersprojects.User_ID;
                objuser.AccountName = usersprojects.AccountName;
                objuser.Usr_Username = usersprojects.Usr_Username;
                objuser.projectName = usersprojects.projectName;
                objuser.ProjectClientName = usersprojects.ProjectClientName;
                objuser.tsktaskID = usersprojects.tsktaskID;
                objuser.Proj_ProjectID = usersprojects.Proj_ProjectID;
                objuser.RoleCode = usersprojects.RoleCode;

                this.Session["TaskId"] = objuser;
            }
            return View(objuser);
        }

        public ActionResult PreviewTimesheets()
        {

            HomeController hm = new HomeController();
            var obj = hm.GetAdminMenu();
            foreach (var item in obj)
            {
                if (item.ModuleName == "Add/Submit Timesheet")
                {
                    var mk = item.ModuleAccessType;


                    ViewBag.a = mk;

                }
            }
            return View();
        }

        public ActionResult EditTimesheet()
        {
            return View();
        }

        public ActionResult GetPreviewTimesheets()
        {
            UserSessionInfo sessId = Session["UserSessionInfo"] as UserSessionInfo;
            int UserID = sessId.UserId;
            string RoleName = sessId.RoleName;
            string roleid = sessId.RoleId.ToString();
            ManagerDetails objmanagerdetails = new ManagerDetails();
            Conn = new SqlConnection(str);
            try
            {
                objmanagerdetails.mytimesheets = new List<UserTimesheets>();
                objmanagerdetails.timesheetsforapproval = new List<ManagerTimesheetsforApprovals>();

                Conn.Open();
                SqlCommand cmd = new SqlCommand("[WebGetTimeSheetforApproval]", Conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userid", UserID);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sda.Fill(ds);

                if ((roleid == "1001") || (roleid == "1002") || (roleid == "1007"))
                {

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {

                            objmanagerdetails.mytimesheets.Add(new UserTimesheets
                            {
                                ProjectId = Convert.ToInt32(dr["Proj_ProjectID"]),
                                ProjectName = dr["Proj_ProjectName"].ToString(),
                                TimesheetID = Convert.ToInt16(dr["Timesheetid"]),
                                Month_Year = dr["MonthYearName"].ToString(),
                                CompanyBillingHours = dr["Companyworkinghours"].ToString(),
                                ResourceWorkingHours = dr["WorkedHours"].ToString(),
                                TimesheetApprovalStatus = dr["ResultSubmitStatus"].ToString(),
                                Usr_UserID = Convert.ToInt32(dr["UserID"].ToString()),
                                userName = dr["Usr_Username"].ToString(),
                                ManagerApprovalStatus = dr["FinalStatus"].ToString(),
                                SubmittedDate = dr["SubmittedDate"].ToString(),
                                ApprovedDate = dr["L1_ApproverDate"].ToString(),
                                ManagerName1 = dr["L1_ManagerName"].ToString(),
                                ManagerName2 = dr["L2_ManagerName"].ToString(),
                            });

                        }
                    }

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow druser in ds.Tables[1].Rows)
                        {

                            objmanagerdetails.timesheetsforapproval.Add(new ManagerTimesheetsforApprovals
                            {
                                ProjectId = Convert.ToInt32(druser["Proj_ProjectID"]),
                                ProjectName = druser["Proj_ProjectName"].ToString(),
                                TimesheetID = Convert.ToInt16(druser["Timesheetid"]),
                                Month_Year = druser["monthyearname"].ToString(),
                                ResourceWorkingHours = Convert.ToInt16(druser["workedhours"]),
                                CompanyBillingHours = Convert.ToInt16(druser["Companyworkinghours"]),
                                TimesheetApprovalStatus = druser["ResultSubmitStatus"].ToString(),
                                Usr_UserID = Convert.ToInt32(druser["UserID"].ToString()),
                                userName = druser["Usr_Username"].ToString(),
                                ManagerApprovalStatus = druser["FinalStatus"].ToString(),
                                SubmittedDate = druser["SubmittedDate"].ToString(),
                                ApprovedDate = druser["L1_ApproverDate"].ToString(),
                                ManagerName1 = druser["L1_ManagerName"].ToString(),
                                ManagerName2 = druser["L2_ManagerName"].ToString(),
                            });

                        }
                    }




                }


                else
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            objmanagerdetails.mytimesheets.Add(new UserTimesheets
                            {
                                ProjectId = Convert.ToInt32(dr["Proj_ProjectID"]),
                                ProjectName = dr["Proj_ProjectName"].ToString(),
                                TimesheetID = Convert.ToInt16(dr["Timesheetid"]),
                                Month_Year = dr["MonthYearName"].ToString(),
                                CompanyBillingHours = dr["Companyworkinghours"].ToString(),
                                ResourceWorkingHours = dr["WorkedHours"].ToString(),
                                TimesheetApprovalStatus = dr["ResultSubmitStatus"].ToString(),
                                Usr_UserID = Convert.ToInt32(dr["UserID"].ToString()),
                                userName = dr["Usr_Username"].ToString(),
                                ManagerApprovalStatus = dr["FinalStatus"].ToString(),
                                SubmittedDate = dr["SubmittedDate"].ToString(),
                                ApprovedDate = dr["L1_ApproverDate"].ToString(),
                                ManagerName1= dr["L1_ManagerName"].ToString(),
                                ManagerName2 = dr["L2_ManagerName"].ToString(),
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Conn != null)
                {
                    if (Conn.State == ConnectionState.Open)
                    {
                        Conn.Close();
                        Conn.Dispose();
                    }
                }
            }


            var result = new { objmanagerdetails.mytimesheets, objmanagerdetails.timesheetsforapproval, roleid };


            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region (getUserTimesheet)Preview the Specific MonthData of  Specific userdata 
        public ActionResult ViewSubmitedTimesheet(string TimesheetMonth, int TimesheetUserid)
        {

            UserSessionInfo sessId = Session["UserSessionInfo"] as UserSessionInfo;
            int UserID = sessId.UserId;
            Timesheetlist objGetTimeSheet = new Timesheetlist();
            objGetTimeSheet.timeSheetDetails = new List<TimeSheetDetails>();
            // objGetTimeSheet.Status = new List<statusMessage>();

            Conn = new SqlConnection(str);
            try
            {
                Conn.Open();
                SqlCommand cmd = new SqlCommand("WebgetUserTimesheet", Conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userid", TimesheetUserid);
                cmd.Parameters.AddWithValue("@Timesheetmonth", TimesheetMonth);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                dataAdapter.Fill(ds);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    objGetTimeSheet.timeSheetDetails.Add(new TimeSheetDetails
                    {
                        TimesheetID = Convert.ToInt32(dr["TimesheetID"]),
                        Taskid = Convert.ToInt32(dr["taskid"]),
                        Month_Year = dr["monthyearname"].ToString(),
                        Taskname = dr["taskname"].ToString(),
                        NoofHoursWorked = Convert.ToDouble(dr["HoursWorked"].ToString()),
                        MonthYearName = dr["MonthYearName"].ToString(),
                        Comments = dr["Comments"].ToString(),
                        UserName = dr["Usr_Username"].ToString(),
                        TotalMonthName = dr["TotalMonthName"].ToString(),
                        ProjectName = dr["Proj_ProjectName"].ToString(),
                        ProjectClientName = dr["ClientProjTitle"].ToString(),
                        ManagerName1 = dr["L1_ManagerName"].ToString(),
                        ManagerName2 = dr["L2_ManagerName"].ToString(),
                        SubmittedDate=dr["SubmittedDate"].ToString(),
                        ApprovedDate = dr["L1_ApproverDate"].ToString(),

                    });


                }


            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (Conn != null)
                {
                    if (Conn.State == ConnectionState.Open)
                    {
                        Conn.Close();
                        Conn.Dispose();
                    }
                }
            }

            return Json(objGetTimeSheet.timeSheetDetails, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region UpdateTaskDetails

        public ActionResult updateTimesheetTaskDetails(TotalTimeSheetTimeDetails sheetObj)
        {
            Conn = new SqlConnection(str); int Trans_Output = 0; string result = string.Empty;
            UserSessionInfo sessId = Session["UserSessionInfo"] as UserSessionInfo;
            int UserID = sessId.UserId;
            try
            {
                if (Conn.State != System.Data.ConnectionState.Open)
                    Conn.Open();
                SqlCommand objCommand = new SqlCommand("[WebEditSubmitTimesheet]", Conn);
                objCommand.CommandType = System.Data.CommandType.StoredProcedure;
                objCommand.Parameters.AddWithValue("@TimesheetID", sheetObj.timesheets.TimesheetID);
                objCommand.Parameters.AddWithValue("@UserID", UserID);
                if (!string.IsNullOrEmpty(sheetObj.timesheets.Comments))
                {
                    objCommand.Parameters.AddWithValue("@Comments", sheetObj.timesheets.Comments);
                }
                else
                {
                    objCommand.Parameters.AddWithValue("@Comments", null);
                }

                objCommand.Parameters.AddWithValue("@SubmittedType", sheetObj.timesheets.SubmittedType);
                objCommand.Parameters.Add("@Trans_Output", SqlDbType.Int);
                objCommand.Parameters["@Trans_Output"].Direction = ParameterDirection.Output;
                objCommand.ExecuteNonQuery();
                Trans_Output = int.Parse(objCommand.Parameters["@Trans_Output"].Value.ToString());

                if (Trans_Output == 104)
                {
                    foreach (var item in sheetObj.listtimesheetdetails)
                    {

                        objCommand = new SqlCommand("[WebEditSubmitTaskDetails]", Conn);
                        objCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        objCommand.Parameters.AddWithValue("@TimesheetID", sheetObj.timesheets.TimesheetID);
                        //objCommand.Parameters.AddWithValue("@ProjectID", item.projectid);
                        objCommand.Parameters.AddWithValue("@TaskId", item.taskid);
                        objCommand.Parameters.AddWithValue("@HoursWorked", item.hoursWorked);
                        objCommand.Parameters.AddWithValue("@TaskDate", Convert.ToDateTime(item.taskDate).ToShortDateString());
                        objCommand.Parameters.Add("@Trans_Output", SqlDbType.Int);
                        objCommand.Parameters["@Trans_Output"].Direction = ParameterDirection.Output;
                        objCommand.ExecuteNonQuery();
                        Trans_Output = int.Parse(objCommand.Parameters["@Trans_Output"].Value.ToString());

                    }

                    if (Trans_Output == 1)
                    {

                        if (sheetObj.timesheets.SubmittedType == "Submit")
                        {
                            SendMailsForApprovals(sheetObj);
                            result = "Data Submited and Updated Successfully";
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }

                        else
                        {
                            result = "Data Saved and Updated Successfully";
                            return Json(result, JsonRequestBehavior.AllowGet);

                        }
                    }
                }
                else if (Trans_Output == 105)
                {
                    result = "Invalid TimesheetId";
                }

                else
                {
                    result = "Wrong Inputs";
                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Conn != null)
                {
                    if (Conn.State == ConnectionState.Open)
                    {
                        Conn.Close();
                        Conn.Dispose();
                    }
                }
            }
        }
        #endregion


        #region CheckTimesheetExists

        public ActionResult CheckTimesheetExits(string timesheetsdate)
        {
            Conn = new SqlConnection(str); int Trans_Output = 0; string result = string.Empty;
            UserSessionInfo sessId = Session["UserSessionInfo"] as UserSessionInfo;
            int UserID = sessId.UserId;
            try
            {
                if (Conn.State != System.Data.ConnectionState.Open)
                    Conn.Open();
                SqlCommand objCommand = new SqlCommand("[WebValidTimesheetMonthExits]", Conn);
                objCommand.CommandType = System.Data.CommandType.StoredProcedure;
                objCommand.Parameters.AddWithValue("@TimesheetMonth", timesheetsdate);
                objCommand.Parameters.AddWithValue("@UserID", UserID);
                objCommand.Parameters.Add("@Trans_Output", SqlDbType.Int);
                objCommand.Parameters["@Trans_Output"].Direction = ParameterDirection.Output;
                objCommand.ExecuteNonQuery();
                Trans_Output = int.Parse(objCommand.Parameters["@Trans_Output"].Value.ToString());


                return Json(Trans_Output, JsonRequestBehavior.AllowGet);

            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Conn != null)
                {
                    if (Conn.State == ConnectionState.Open)
                    {
                        Conn.Close();
                        Conn.Dispose();
                    }
                }
            }
        }
        #endregion

        #region TimeSheetManagerActions
        public ActionResult TimeSheetManagerActionWeb(TotalTimeSheetTimeDetails sheetObj)
        {
            timesheet lstobjtime = new timesheet();

            int Trans_Output = 0; string Userid = string.Empty;
            UserSessionInfo sessId = Session["UserSessionInfo"] as UserSessionInfo;

            if ((sheetObj.timesheets.UserID != 0))
            {
                Userid = sheetObj.timesheets.UserID.ToString();
            }
            else
            {

                Userid = sessId.UserId.ToString();
            }
            try
            {
                Conn = new SqlConnection(str);
                if (Conn.State != System.Data.ConnectionState.Open)
                    Conn.Open();
                SqlCommand objCommand = new SqlCommand("[ManagerActionsfromEmail]", Conn);
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.Parameters.AddWithValue("@UserID", sheetObj.timesheets.UserID);
                objCommand.Parameters.AddWithValue("@TimesheetID", sheetObj.timesheets.TimesheetID);
                objCommand.Parameters.AddWithValue("@Projectid", sheetObj.timesheets.ProjectID);
                objCommand.Parameters.AddWithValue("@ManagerId", sessId.UserId);
                objCommand.Parameters.AddWithValue("@Comments", sheetObj.timesheets.Comments);
                if (sheetObj.timesheets.SubmittedType == "3")
                {
                    objCommand.Parameters.AddWithValue("@SubmittedType", "1");
                }
                else if (sheetObj.timesheets.SubmittedType == "4")
                {
                    objCommand.Parameters.AddWithValue("@SubmittedType", "0");
                }


                objCommand.Parameters.AddWithValue("@Trans_Output", SqlDbType.Int);
                objCommand.Parameters["@Trans_Output"].Direction = ParameterDirection.Output;
                objCommand.ExecuteNonQuery();
                Trans_Output = int.Parse(objCommand.Parameters["@Trans_Output"].Value.ToString());
                if (Trans_Output == 0)
                {
                    lstobjtime = new timesheet()
                    {
                        Transoutput = Trans_Output,
                        Position = "L1",
                        Message = "Timesheet is rejected,Emp has to resubmit the timesheet",
                        SubmittedState = "Once",
                        EmailAppOrRejStatus = sheetObj.timesheets.EmailAppOrRejStatus,
                        UserID = sheetObj.timesheets.UserID,
                        ManagerId = sheetObj.timesheets.ManagerId,

                    };
                }
                if (Trans_Output == 900)
                {
                    lstobjtime = new timesheet()
                    {
                        Transoutput = Trans_Output,
                        Position = "",
                        Message = "ManagerId is Incorrect",
                        SubmittedState = "Once",
                        EmailAppOrRejStatus = sheetObj.timesheets.EmailAppOrRejStatus,
                        UserID = sheetObj.timesheets.UserID,
                        ManagerId = sheetObj.timesheets.ManagerId,

                    };
                }

                if (Trans_Output == 1)
                {

                    string Message = SendMailsForApprovals(sheetObj);
                    string[] objm1 = Message.Split('-');
                    lstobjtime = new timesheet()
                    {
                        Transoutput = Trans_Output,
                        Position = "L1",
                        Message = "Aprroved by" + " " + objm1[0],
                        EmailAppOrRejStatus = sheetObj.timesheets.EmailAppOrRejStatus,
                        UserID = sheetObj.timesheets.UserID,
                        ManagerId = sheetObj.timesheets.ManagerId,

                    };
                   
                }

                if (Trans_Output == 2)
                {
                    string Message = SendMailsForApprovals(sheetObj);
                    string[] objm1 = Message.Split('-');
                    lstobjtime = new timesheet()
                    {

                        Transoutput = Trans_Output,
                        Position = "L1",
                        Message = "Rejected by" + " " + objm1[0],
                        EmailAppOrRejStatus = sheetObj.timesheets.EmailAppOrRejStatus,
                        UserID = sheetObj.timesheets.UserID,
                        ManagerId = sheetObj.timesheets.ManagerId,

                    };



                }
                if (Trans_Output == 3)
                {
                    string Message = SendMailsForApprovals(sheetObj);
                    string[] objm1 = Message.Split('-');

                    lstobjtime = new timesheet()
                    {
                        Transoutput = Trans_Output,
                        Position = "L2",
                        // Message = "Approved by Level-2 Manager",
                        Message = "Approved by" + " " +  objm1[1],
                        EmailAppOrRejStatus = sheetObj.timesheets.EmailAppOrRejStatus,
                        UserID = sheetObj.timesheets.UserID,
                        ManagerId = sheetObj.timesheets.ManagerId,

                    };
                }
                if (Trans_Output == 4)
                {
                    string Message = SendMailsForApprovals(sheetObj);
                    string[] objm1 = Message.Split('-');
                    lstobjtime = new timesheet()
                    {
                        Transoutput = Trans_Output,
                        Position = "L2",
                        Message = "Rejected by" + " " + objm1[1],
                        EmailAppOrRejStatus = sheetObj.timesheets.EmailAppOrRejStatus,
                        UserID = sheetObj.timesheets.UserID,
                        ManagerId = sheetObj.timesheets.ManagerId,

                    };
                }

                if (Trans_Output == 104)
                {
                    string Message = SendMailsForApprovals(sheetObj);
                    string[] objm1 = Message.Split('-');
                    lstobjtime = new timesheet()
                    {
                        Transoutput = Trans_Output,
                        Position = "L1",
                        Message = "Timesheet is already approved by" + " " + objm1[0],
                        EmailAppOrRejStatus = sheetObj.timesheets.EmailAppOrRejStatus,
                        UserID = sheetObj.timesheets.UserID,
                        ManagerId = sheetObj.timesheets.ManagerId,

                    };
                }

                if (Trans_Output == 106)
                {
                    string Message = SendMailsForApprovals(sheetObj);
                    string[] objm1 = Message.Split('-');
                    lstobjtime = new timesheet()
                    {
                        Transoutput = Trans_Output,
                        Position = "L1",
                        // Message = "Timesheet is already rejected by Level1 manager",
                        Message = "Timesheet is already rejected by" + " " + objm1[0],
                        EmailAppOrRejStatus = sheetObj.timesheets.EmailAppOrRejStatus,
                        UserID = sheetObj.timesheets.UserID,
                        ManagerId = sheetObj.timesheets.ManagerId,

                    };
                }
                if (Trans_Output == 105)
                {
                    string Message = SendMailsForApprovals(sheetObj);
                    string[] objm1 = Message.Split('-');

                    lstobjtime = new timesheet()
                    {
                        Transoutput = Trans_Output,
                        Position = "L2",
                        // Message = "Timesheet is already approved by Level2 manager",
                        Message = "Timesheet is already approved by" + " " + objm1[1],
                        EmailAppOrRejStatus = sheetObj.timesheets.EmailAppOrRejStatus,
                        UserID = sheetObj.timesheets.UserID,
                        ManagerId = sheetObj.timesheets.ManagerId,

                    };

                }
                if (Trans_Output == 107)
                {
                    string Message = SendMailsForApprovals(sheetObj);
                    string[] objm1 = Message.Split('-');
                    lstobjtime = new timesheet()
                    {
                        Transoutput = Trans_Output,
                        Position = "L2",
                        Message = "Timesheet is already rejected by" + " " + objm1[1],
                        EmailAppOrRejStatus = sheetObj.timesheets.EmailAppOrRejStatus,
                        UserID = sheetObj.timesheets.UserID,
                        ManagerId = sheetObj.timesheets.ManagerId,

                    };
                }





            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Conn != null)
                {
                    if (Conn.State == ConnectionState.Open)
                    {
                        Conn.Close();
                        Conn.Dispose();
                    }
                }
            }




            return Json(lstobjtime.Message, JsonRequestBehavior.AllowGet);

        }
        #endregion


        #region CheckTimesheetExists

        public ActionResult GetHoursWorkedByCalender(string tdate, string Userid, string cmpnyAccountid, string UsrProjectId)
        {
            Timesheetlist objGetTimeSheet = new Timesheetlist();
            timesheet objEmpInfo = new timesheet();
            objGetTimeSheet.timeSheetDetails = new List<TimeSheetDetails>();
            Conn = new SqlConnection(str);
            try
            {
                if (Conn.State != System.Data.ConnectionState.Open)
                    Conn.Open();
                //SqlCommand objCommand = new SqlCommand("[TempSubmitTimesheet]", Conn);
               SqlCommand objCommand = new SqlCommand("[TempTimesheetonLeavesandHolidays]", Conn);
                objCommand.CommandType = System.Data.CommandType.StoredProcedure;
                objCommand.Parameters.AddWithValue("@TimesheetMonth", tdate);
                objCommand.Parameters.AddWithValue("@Userid", Userid);
                objCommand.Parameters.AddWithValue("@Accountid", cmpnyAccountid);
                objCommand.Parameters.AddWithValue("@Projectid", UsrProjectId);
                SqlDataAdapter sda = new SqlDataAdapter(objCommand);
                DataSet ds = new DataSet();
                sda.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        objGetTimeSheet.timeSheetDetails.Add(new TimeSheetDetails
                        {

                            TaskDate = dr["MonthYearName"].ToString(),
                            NoofHoursWorked = Convert.ToDouble(dr["HoursWorked"].ToString()),                          
                            colour = dr["colour"].ToString(),
                        });
                    }
                }

                if (ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr1 in ds.Tables[1].Rows)
                    {
                        objEmpInfo = new timesheet()
                        {

                            UserName=dr1["Username"].ToString(),
                            ManagerName1 = dr1["L1_ManagerName"].ToString(),
                            ManagerName2 = dr1["L2_ManagerName"].ToString(),
                            TotalMonthName = dr1["TotalMonthName"].ToString(),

                        };

                    }
                }

            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Conn != null)
                {
                    if (Conn.State == ConnectionState.Open)
                    {
                        Conn.Close();
                        Conn.Dispose();
                    }
                }
            }

        var result=    new { objGetTimeSheet.timeSheetDetails, objEmpInfo };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion



        #region ApplyColoursaccording to Leaves  and Holidays

        public ActionResult HoursWorkedTimesheet(string TimesheetMonth, int TimesheetUserid)
        {

            UserSessionInfo sessId = Session["UserSessionInfo"] as UserSessionInfo;
            int UserID = sessId.UserId;
            Timesheetlist objGetTimeSheet = new Timesheetlist();
            objGetTimeSheet.timeSheetDetails = new List<TimeSheetDetails>();

            // objGetTimeSheet.Status = new List<statusMessage>();

            Conn = new SqlConnection(str);
            try
            {
                Conn.Open();
                SqlCommand cmd = new SqlCommand("WebgetUserTimesheet", Conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userid", TimesheetUserid);
                cmd.Parameters.AddWithValue("@Timesheetmonth", TimesheetMonth);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                dataAdapter.Fill(ds);



                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    objGetTimeSheet.timeSheetDetails.Add(new TimeSheetDetails
                    {
                        colour = dr["colour"].ToString(),

                    });

                }



            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (Conn != null)
                {
                    if (Conn.State == ConnectionState.Open)
                    {
                        Conn.Close();
                        Conn.Dispose();
                    }
                }
            }

            return Json(objGetTimeSheet.timeSheetDetails, JsonRequestBehavior.AllowGet);

        }
        #endregion




    }
}