using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scout.Entities
{
    [Table("Managers")]
    public class Manager : ScoutUserBase
    {
        public int CountOfViewProfile { get; set; }

        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likes { get; set; }
        
       
       
    }
}
