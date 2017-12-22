using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scout.Entities
{
   public class Notification
    {
        public int NotificationId { get; set; }
        public string NotificationText { get; set; }
        public DateTime NotificationDate { get; set; }
        public bool NotificationSeen { get; set; }
        public Footballer Footballer { get; set; }
       
    }
}
