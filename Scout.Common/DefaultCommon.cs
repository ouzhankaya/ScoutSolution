using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scout.Common
{
    public class DefaultCommon : ICommon
    {
        public string AdminGetUsername()
        {
            return "system";
        }

        public string FootballerGetUsername()
        {
            return "system";
        }

        public string ManagerGetUsername()
        {
            return "system";
        }
    }
}
