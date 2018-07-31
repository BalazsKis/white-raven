using Newtonsoft.Json;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace WhiteRaven.Web.Api
{
    /// <summary>
    /// Contains methods for creating common objects of the JSON API (http://jsonapi.org)
    /// </summary>
    public class JsonApi
    {
        /// <summary>
        /// Returns a JSON API error object
        /// </summary>
        /// <param name="statusCode">The response status code</param>
        /// <param name="title">The error title</param>
        /// <param name="detail">The error details</param>
        /// <returns>A JSON API error object</returns>
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

        /// <summary>
        /// Returns a JSON API data object
        /// </summary>
        /// <param name="data">The data</param>
        /// <returns>A JSON API data object</returns>
        public static object DataObject(object data) =>
            new { data };

        /// <summary>
        /// Returns a JSON API OK data object
        /// </summary>
        /// <param name="data">The data</param>
        /// <returns>A JSON API OK data object</returns>
        public static ObjectResult OkDataObject(object data) =>
            new OkObjectResult(DataObject(data));
    }
}