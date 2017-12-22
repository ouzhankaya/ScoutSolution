using Scout.Common;
using Scout.Entities;
using Scout.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scout.Web.Init
{
    public class WebCommon : ICommon
    {
        public string FootballerGetUsername()
        {
            Footballer footballer = CurrentSession.footballer;
            if (footballer != null)
                return footballer.Username;
            else
                return "system";           
        }

        public string ManagerGetUsername()
        {
            Manager manager = CurrentSession.manager;
            if (manager != null)
                return manager.Username;
            else
                return "system";
        }
        public string AdminGetUsername()
        {
            Admin admin = CurrentSession.admin;
            if (admin != null)
                return admin.Username;
            else
                return "system";
        }

    }
}