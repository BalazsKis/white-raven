using Newtonsoft.Json;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace WhiteRaven.Web.Api
{
    public class JsonApi
    {
        public static string ErrorObject(HttpStatusCode statusCode, string title, string detail)
        {
            return JsonConvert.SerializeObject(new
            {
                errors = new[]
                {
                    new
                    {
                        status = ((int) statusCode).ToString(),
                        title,
                        detail
                    }
                }
            });
        }

        public static object DataObject(object data) =>
            new { data };

        public static ObjectResult OkDataObject(object data) =>
            new OkObjectResult(DataObject(data));
    }
}