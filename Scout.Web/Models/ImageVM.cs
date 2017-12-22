using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scout.Web.Models
{
    public class ImageVM
    {
        public HttpPostedFileWrapper ShareImageFileName { get; set; }

        public HttpPostedFileWrapper ShareVideoFileName { get; set; }
       
        public DateTime CreatedDate { get; set; }
       
        public DateTime ModifiedDate { get; set; }
        public int LikeCount { get; set; }


       

    }
}