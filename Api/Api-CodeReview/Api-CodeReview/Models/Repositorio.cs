using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api_CodeReview.Models
{
    [Table("tbRepositorio")]
    public class Repositorio
    {
        [Key]
        public int Id_repositorio { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string  Nm_repositorio { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 10)]
        public string Nm_url_clone { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string Nm_usuario { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 8)]
        public string Nm_senha { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 10)]
        public string Nm_email_admin { get; set; }
    }
}
