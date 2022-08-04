using Api_CodeReview.Context;
using Api_CodeReview.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_CodeReview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedBackCommitController : ControllerBase
    {
        private readonly FeedbackCommitService service;

        public FeedBackCommitController(AppDbContext context)
        {
            service = new(context);
        }

        [HttpGet("{status}/{idCommit}/{feedback}")]
        public ContentResult PostFeedback(string idCommit, string? feedback, string status)
        {
            try
            {
                if (feedback.Equals("commit-aprovado-7876"))
                    feedback = null;

                service.PostFeedback(idCommit, feedback, status);
                return base.Content($"<h2>COMMIT APROVADO</h2>", "text/html");
            }
            catch (Exception e)
            {
                return base.Content($"<h2>ERRO AO APROVAR UM NOVO COMMIT --> {e.Message}</h2>", "text/html");
            }
        }

        [HttpGet("{idCommit}")]
        public ContentResult GetFormFeedback(string idCommit)
        {
            string content = ""
            + "<body>"
            + " <form id = \"form\">"
            + "     <div style = \"margin-top:10px\">"
            + "         <li>feedback</li>"
            + "  <textarea id=\"textarea\"></textarea>"
            + "      </div>"
            + "      <button id =\"myButton\" type =\"button\">Enviar</button>"
            + " </form>"
            + " <script type = \"text/javascript\">"
            + "     document.getElementById(\"myButton\").onclick = function() { "
            + "         var textarea = document.getElementById(\"textarea\"); "
            + "         if (textarea.value !== \"\")"
            + "         {"
            + "             location.href = \"http://localhost:9798/api/FeedBackCommit/reprovado/\" + \""+ idCommit + "\" + \" / \" + textarea.value;"
            + "         }"
            + "     };"
            + " </script>"
            + "</body>";
            return base.Content(content, "text/html");
        }
    }
}
