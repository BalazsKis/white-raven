using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace WhiteRaven.Web.Api.Controllers
{
    /// <summary>
    /// Home page controller
    /// </summary>
    public class HomeController : ControllerBase
    {
        /// <summary>
        /// Returns the home page's HTML document
        /// </summary>
        /// <returns>The home page's HTML document</returns>
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