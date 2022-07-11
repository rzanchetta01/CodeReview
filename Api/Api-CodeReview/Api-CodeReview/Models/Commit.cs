using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api_CodeReview.Models
{
    [Table("tbCommit")]
    public class Commit
    {
        [Key]
        public string Id_commit { get; set; }
        public int Id_branch { get; set; }
        public string Nm_mensagem { get; set; }
        public string Nm_autor { get; set; }
        public DateTime Dt_commit { get; set; }

        //[ForeignKey("Id_branch")]
        //public Branch branch { get; set; }
    }
}
