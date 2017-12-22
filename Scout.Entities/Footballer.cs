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
    [Table("Footballers")]
    public class Footballer : ScoutUserBase
    {
        public virtual Country Country { get; set; }
        public virtual Province Province { get; set; }
        public virtual Foot Foot { get; set; }
        public virtual Position Position { get; set; }
        public virtual OtherPosition OtherPosition { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }

        public virtual List<Share> Shares { get; set; }
        public virtual List<Notification> Notification { get; set; }

        [DisplayName("Ülke")]
        public Nullable<int> CountryId { get; set; }
        public Nullable<int> ProvinceId { get; set; }
        public Nullable<int> FootId { get; set; }
        public Nullable<int> PositionId { get; set; }
        public Nullable<int> OtherPositionId { get; set; }
       



    }
}
