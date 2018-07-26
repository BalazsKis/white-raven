using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace WhiteRaven.Web.Api
{
    public class JsonApi
    {
        public static ObjectResult ErrorObject(HttpStatusCode statusCode, string title, string detail)
        {
            var statusAsNumber = (int)statusCode;

            var result = new ObjectResult(new
            {
                errors = new[]
                {
                    new
                    {
                        status = statusAsNumber.ToString(),
                        title,
                        detail
                    }
                }
            });

            result.StatusCode = statusAsNumber;

            return result;
        }
    }
}