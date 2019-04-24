using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace Evolutyz.Entities
{
    public partial class UserSessionInfo
    {
        private static HttpSessionState session { get { return HttpContext.Current.Session; } }
        public string Usrp_ProfilePicture { get; set; }
        public int UserId
        {
            get
            {
                return (int)session["UserId"];
            }
            set
            {
                session["UserId"] = value;
            }
        }

        public bool? UsAccount
        {
            get
            {
                return (bool)session["UsAccount"];
            }
            set
            {
                session["UsAccount"] = value;
            }
        }
        public int? Projectid {
            get
            {
                return (int)session["Projectid"];
            }
            set
            {
                session["Projectid"] = value;
            }
        }
        public string UserName { get; set; }
        public string Usr_Username { get; set; }

        //public string RoleName { get; set; }
        public string RoleName
        {
            get
            {
                return (string)session["RoleName"];
            }
            set
            {
                session["RoleName"] = value;
            }
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string LoginId
        {
            get
            {
                return (string)session["LoginId"];
            }
            set
            {
                session["LoginId"] = value;
            }
        }

        public string Password
        {
            get
            {
                return (string)session["Password"];
            }
            set
            {
                session["Password"] = value;
            }
        }
        // public int RoleId { get; set; }
        public int RoleId
        {
            get
            {
                return (int)session["RoleId"];
            }
            set
            {
                session["RoleId"] = value;
            }
        }


        //public string LoginId { get; set; }
        //public string Password { get; set; }
        // public int AccountId { get; set; }
        public int AccountId
        {
            get
            {
                return (int)session["AccountId"];
            }
            set
            {
                session["AccountId"] = value;
            }
        }
        public int Usr_UserTypeID
        {
            get
            {
                return (int)session["Usr_UserTypeID"];
            }
            set
            {
                session["Usr_UserTypeID"] = value;
            }
        }

        public int? projectid
        {
            get
            {
                return (int?)session["projectid"];
            }
            set
            {
                session["projectid"] = value;
            }
        }
    }
}
