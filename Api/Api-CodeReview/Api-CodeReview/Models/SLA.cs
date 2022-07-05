using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api_CodeReview.Models
{
    [Table("tbSLA")]
    public class SLA
    {
        [Key]
        public int Id_SLA { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 1)]
        public int Id_repositorio { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public int Nr_dias_sla_commit { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public int Nr_dias_sla_review { get; set; }

        //[ForeignKey("Id_repositorio")]
        //public Repositorio repositorio { get; set; }
    }
}
