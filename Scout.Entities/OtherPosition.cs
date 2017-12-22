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
    [Table("OtherPositions")]
    public class OtherPosition
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OtherPositionId { get; set; }
        [DisplayName("Yan Mevki")]
        public string OtherPositionName { get; set; }

        public virtual List<Footballer> Footballers { get; set; }

        public OtherPosition()
        {
            Footballers = new List<Footballer>();
        }
    }
}
