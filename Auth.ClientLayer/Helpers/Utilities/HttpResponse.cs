using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Auth.ClientLayer.Helpers.Utilities
{
    public static class ApiResponse
    {


        public static OkObjectResult OK(Object content)
        {
            return new OkObjectResult(content);
        }

        public static BadRequestObjectResult BadRequest(Object content)
        {
            return new BadRequestObjectResult(content);
        }

        public static NotFoundObjectResult NotFound(Object content)
        {
            return new NotFoundObjectResult(content);
        }


        public static Object CreateErrorObject(string message)
        {
            return new { Error = message };
        }



    }
}
