using Scout.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scout.Web.Filter
{
    public class AuthManagerLogin : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if(CurrentSession.manager == null)
            {
                filterContext.Result = new RedirectResult("/Home/ManagerLogin");
            }
        }
    }
}