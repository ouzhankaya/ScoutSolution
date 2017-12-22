using Scout.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scout.Web.Filter
{
    public class AuthLoginAdmin:FilterAttribute,IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if(CurrentSession.admin== null)
            {
                filterContext.Result = new RedirectResult("/Home/Adminstator");
            }
        }

       
    }
}