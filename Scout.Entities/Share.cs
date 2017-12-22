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
    [Table("Shares")]
    public class Share
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ShareId { get; set; }

        public string ShareText { get; set; }

        public string ShareImageFileName { get; set; }

        public string ShareVideoFileName { get; set; }
        [Required]
        [Column(TypeName = "DateTime2"), DisplayFormat(DataFormatString = "{0:MM/dd/yyyy, hh.mm tt}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }
        [Column(TypeName = "DateTime2"), DisplayFormat(DataFormatString = "{0:MM/dd/yyyy, hh.mm tt}", ApplyFormatInEditMode = true)]
        public DateTime ModifiedDate { get; set; }
        [DisplayName("Beğenilme")]
        public int LikeCount { get; set; }

        
        public virtual Footballer Owner { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likes { get; set; }


        public Share()
        {
            Comments = new List<Comment>();
            Likes = new List<Liked>();
        }
    }
}

