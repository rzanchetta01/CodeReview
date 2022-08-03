using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Api_CodeReview.Models
{
    [Table("tbFeedbackCommit")]
    public class FeedbackCommit
    {

        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Id_Commit { get; set; }

        [Required]
        public string Status_resposta { get; set; }

        public string Feedback { get; set; }
    }
}
