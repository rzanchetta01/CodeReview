using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api_CodeReview.Models
{
    [Table("tbRepositorio")]
    public class Repositorio
    {
        [Key]
        public int Id_repositorio { get; set; }
        public string  Nm_repositorio { get; set; }
        public string Nm_url_clone { get; set; }
        public string Nm_usuario { get; set; }
        public string Nm_senha { get; set; }
        public string Nm_email_admin { get; set; }
    }
}
