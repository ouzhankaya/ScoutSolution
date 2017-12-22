using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scout.Web.ViewModels
{
    public class WarningVM : NotificationVMBase<string>
    {
        public WarningVM()
        {
            Title = "Uyarı";
        }
    }
}