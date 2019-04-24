using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolutyz.Data
{
    public class CommonRepositoryDAC
    {
        #region To fetch Member password by using username
        public string fetchUserPassword(string username)
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var password = (from u in db.Users
                                        where u.Usr_LoginId == username
                                            select u.Usr_Password).FirstOrDefault();

                    return password;
                }
                catch (Exception ex)
                {
                    return ex.Message.ToString();
                }
            }
        }
        #endregion
    }
}
