using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api_CodeReview.Models
{
    [Table("tbBranch")]
    public class Branch
    {
        [Key]
        public int Id_branch { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 1)]
        public int Id_repositorio { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 6)]
        public string Nm_branch { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 10)]
        public string Nm_email_dev { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 10)]
        public string Nm_email_review { get; set; }

        //[ForeignKey("Id_repositorio")]
        //public Repositorio repositorio { get; set; }
    }
}
