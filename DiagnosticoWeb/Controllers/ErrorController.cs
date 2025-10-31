using System.Net;
using DiagnosticoWeb.Code;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace DiagnosticoWeb.Controllers
{
    public class ErrorController : ControllerBase
    {
        [Route("/Error")]
        public ActionResult Error([FromServices] IHostingEnvironment webHostEnvironment)
        {
            var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var ex = feature?.Error;
            var isDev = webHostEnvironment.IsDevelopment();
            var problemDetails = new ProblemDetails
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Instance = feature?.Path,
                Title = isDev ? $"{ex.GetType().Name}: {ex.Message}" : "An error occurred.",
            };
            Excepcion.Registrar(ex);
            return StatusCode(problemDetails.Status.Value, problemDetails);
        }
    }
}