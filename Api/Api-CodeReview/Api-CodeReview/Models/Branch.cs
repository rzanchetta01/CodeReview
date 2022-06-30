using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api_CodeReview.Models
{
    [Table("tbBrach")]
    public class Branch
    {
        [Key]
        public int Id_branch { get; set; }
        public int Id_repositorio { get; set; }
        public string Nm_branch { get; set; }
        public string Nm_email_dev { get; set; }
        public string Nm_email_review { get; set; }

        [ForeignKey("Id_repositorio")]
        public Repositorio repositorio { get; set; }
    }
}
