using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scout.Web.ViewModels
{
    public class InfoVM : NotificationVMBase<string>
    {
        public InfoVM()
        {
            Title = "Bilgilendirme";
        }
    }
}