using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scout.Web.ViewModels
{
    public class OkVM : NotificationVMBase<string>
    {
        public OkVM()
        {
            Title = "İşlem Başarılı";
        }
    }
}