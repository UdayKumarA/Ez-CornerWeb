using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evolutyz.Data;

namespace Evolutyz.Business
{
    public class CommonRepostoryComponent
    {
        public string FetchfetchUserPassword(string username)
        {
            var obj = new CommonRepositoryDAC();
            return obj.fetchUserPassword(username);
        }
    }
}
