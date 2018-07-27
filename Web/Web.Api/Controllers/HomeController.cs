using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace WhiteRaven.Web.Api.Controllers
{
    public class HomeController : ControllerBase
    {
        public async Task<ContentResult> Index()
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