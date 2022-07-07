using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api_CodeReview.Models
{
    [Table("tbSLA")]
    public class SLA
    {
        [Key]
        public int Id_SLA { get; set; }
        public int Id_repositorio { get; set; }
        public int Nr_dias_sla_commit { get; set; }
        public int Nr_dias_sla_review { get; set; }

        //[ForeignKey("Id_repositorio")]
        //public Repositorio repositorio { get; set; }
    }
}
