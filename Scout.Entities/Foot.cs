using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scout.Entities
{
    [Table("Feet")]
    public class Foot
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FootId { get; set; }
        [DisplayName("Kullanılan Ayak")]
        public string FootName { get; set; }
        public virtual List<Footballer> Footballers { get; set; }

        public Foot()
        {
            Footballers = new List<Footballer>();
        }
    }
}
