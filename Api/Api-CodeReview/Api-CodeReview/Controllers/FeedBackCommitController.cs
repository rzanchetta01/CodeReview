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

        [HttpGet("{idCommit}/{feedback}")]
        public ContentResult PostFeedback(string idCommit, string feedback)
        {
            try
            {
                service.PostFeedback(idCommit, feedback);
                return base.Content($"<h2>COMMIT APROVADO {feedback}</h2>", "text/html");
            }
            catch (Exception)
            {
                return base.Content("<h2>ERRO AO APROVAR UM NOVO COMMIT</h2>", "text/html");
            }
        }

        [HttpGet()]
        public ContentResult GetFormFeedback()
        {
            string content = ""
            + "<body>"
            + " <form id = \"form\">"
            + "      <div>"
            + "         <li>id do commit</li>"
            + "       <input id = \"idCommit\" type = \"text\"></input>"
            + "     </div>"
            + "     <div style = \"margin-top:10px\">"
            + "         <li>feedback</li>"
            + "  <textarea id=\"textarea\"></textarea>"
            + "      </div>"
            + "      <button id =\"myButton\" type =\"button\">Enviar</button>"
            + " </form>"
            + " <script type = \"text/javascript\">"
            + "     document.getElementById(\"myButton\").onclick = function() { "
            + "         var textarea = document.getElementById(\"textarea\"); "
            + "         var idCommit = document.getElementById(\"idCommit\"); "
            + "         if (textarea.value !== \"\" && idCommit.value !== \"\")"
            + "         {"
            + "             location.href = \"http://localhost:9798/api/FeedBackCommit/\" + idCommit.value + \" / \" + textarea.value;"
            + "         }"
            + "     };"
            + " </script>"
            + "</body>";
            return base.Content(content, "text/html");
        }
    }
}
