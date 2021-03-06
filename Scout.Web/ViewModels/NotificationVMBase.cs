﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scout.Web.ViewModels
{
    public class NotificationVMBase<T>
    {
        public List<T> Items { get; set; }
        public string Header { get; set; }
        public string Title { get; set; }
        public bool IsRedirecting { get; set; }
        public string RedirectingUrl { get; set; }
        public int RedirectingTimeout { get; set; }

        public NotificationVMBase()
        {
            Header = "Yönlendiriliyorsunuz";
            Title = "Geçersiz İşlem";
            IsRedirecting = true;
            RedirectingUrl = "/Home/Index";
            RedirectingTimeout = 3000;
            Items = new List<T>();
        }
    }
}