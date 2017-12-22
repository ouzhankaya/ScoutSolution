using Scout.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scout.Web.Models
{
    public class CurrentSession
    {
        public static Footballer footballer
        {
            get
            {
                return Get<Footballer>("login");
            }
        }
        public static Manager manager
        {
            get
            {
                return Get<Manager>("loginm");
            }
        }
        public static Admin admin
        {
            get
            {
                return Get<Admin>("logina");
            }
        }

        public static void Set<T>(string key, T obj)
        {
            HttpContext.Current.Session[key] = obj;
        }

        public static T Get<T>(string key)
        {
            if (HttpContext.Current.Session[key] != null)
            {
                return (T)HttpContext.Current.Session[key];
            }

            return default(T);
        }
    
        public static void Remove(string key)
        {
            if (HttpContext.Current.Session[key] != null)
            {
                HttpContext.Current.Session.Remove(key);
            }
        }

        public static void Clear()
        {
            HttpContext.Current.Session.Clear();
        }
    }
}
