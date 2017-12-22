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
    [Table("Provinces")]
    public class Province
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ProvinceId { get; set; }
        [DisplayName("Şehir")]
        public string ProvinceName { get; set; }
        public virtual List<Footballer> Footballers { get; set; }
        public Country Country { get; set; }

        public Province()
        {
            Footballers = new List<Footballer>();
        }
    }
}
