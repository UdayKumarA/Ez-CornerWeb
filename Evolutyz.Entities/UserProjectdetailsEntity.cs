using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.SessionState;

namespace evolCorner.Models
{
    public class UserProjectdetailsEntity
    {

        private static HttpSessionState session { get { return HttpContext.Current.Session; } }


        public string AccountName { get; set; }

        public int User_ID
        {
            get
            {
                return (int)session["userID"];
            }
            set
            {
                session["userID"] = value;
            }
        }
        public int Account_ID
        {
            get
            {
                return (int)session["AccountID"];
            }
            set
            {
                session["AccountID"] = value;
            }
        }


        public string Usr_Username
        {
            get
            {
                return (string)session["Usr_Username"];
            }
            set
            {
                session["Usr_Username"] = value;
            }
        }


        public string UserType { get; set; }

        public string Usr_LoginId
        {
            get
            {
                return (string)session["Usr_LoginId"];
            }
            set
            {
                session["Usr_LoginId"] = value;
            }
        }

        public string Usr_Password
        {
            get
            {
                return (string)session["Usr_Password"];
            }
            set
            {
                session["Usr_Password"] = value;
            }
        }


        // public string Usr_Username { get; set; }
        //  public string Usr_LoginId { get; set; }
        // public string Usr_Password { get; set; }
        public int Proj_ProjectID { get; set; }
        public string projectName { get; set; }
        public string ProjectClientName { get; set; }
        public int tsktaskID { get; set; }

        public string RoleCode
        {
            get
            {
                return (string)session["RoleCode"];
            }
            set
            {
                session["RoleCode"] = value;
            }
        }

    }


}