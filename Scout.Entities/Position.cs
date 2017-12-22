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
    [Table("Positions")]
    public class Position
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PositionId { get; set; }
        [DisplayName("Mevki")]
        public string PositionName { get; set; }

        public virtual List<Footballer> Footballers { get; set; }

        public Position()
        {
            Footballers = new List<Footballer>();
        }
    }
}
