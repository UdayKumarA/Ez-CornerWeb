using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolutyz.Entities
{
    public class ResponseHeader
    {
        public bool IsSuccessful { get; set; }

        public string Message { get; set; }

        public string Detail { get; set; }
    }
}
