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
    [Table("Countries")]
    public class Country
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CountryId { get; set; }
        [DisplayName("Ülke")]
        public string CountryName { get; set; }

        public virtual List<Footballer> Footballers { get; set; }
        public virtual List<Province> Provinces { get; set; }


        public Country()
        {
            Footballers = new List<Footballer>();
            Provinces = new List<Province>();
        }
    }
}
