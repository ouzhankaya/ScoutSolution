using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scout.Entities
{
    [Table("Comments")]
    public class Comment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentId { get; set; }
        public string CommentText { get; set; }
        [Column(TypeName = "DateTime2"), DisplayFormat(DataFormatString = "{0:MM/dd/yyyy, hh.mm tt}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }
        [Column(TypeName = "DateTime2"), DisplayFormat(DataFormatString = "{0:MM/dd/yyyy, hh.mm tt}", ApplyFormatInEditMode = true)]
        public DateTime ModifiedDate { get; set; }

       
        public virtual Share Share { get; set; }
        public virtual Manager Manager { get; set; }
    }
}
