using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scout.Web.Models
{
    public class SearchModel
    {
        public Nullable<int> MaxHeight { get; set; }
        public Nullable<int> MinHeight { get; set; }
        public Nullable<int> MaxWeight { get; set; }
        public Nullable<int> MinWeight { get; set; }
        public Nullable<int> MaxAge { get; set; }
        public Nullable<int> MinAge { get; set; }


        public Nullable<int> Country { get; set; }
        public Nullable<int> Province { get; set; }
        public Nullable<int> Foot { get; set; }
        public Nullable<int> Position { get; set; }
        public Nullable<int> OtherPosition { get; set; }

        


    }
}