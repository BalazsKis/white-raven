using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WhiteRaven.Web.Api.Controllers
{
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public async Task<ContentResult> Get()
        {
            var indexHtmlContent = await System.IO.File.ReadAllTextAsync("wwwroot/index.html");

            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = indexHtmlContent
            };
        }
    }
}