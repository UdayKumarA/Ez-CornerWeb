using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;


namespace Evolutyz.Entities
{
    public class LoginEntity
    {
        private static HttpSessionState session { get { return HttpContext.Current.Session; } }
        public string UserName
        {
            get
            {
                return (string)session["UserName"];
            }
            set
            {
                session["UserName"] = value;
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
        //public string UserName { get; set; }
        //public string Password { get; set; }
        public string Vcode { get; set; }
    }
}
