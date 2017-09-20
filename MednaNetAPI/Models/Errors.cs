using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using System.Net;

namespace MednaNetAPI.Models
{
    public static class Errors
    {
        public static HttpResponseMessage InstallCheckinFailed(HttpRequestMessage Request)
        {
            HttpError err = new HttpError("Install check in failed. Check you are passing an existing installKey.");
            return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
        }

        public static HttpResponseMessage InstallKeyBlank(HttpRequestMessage Request)
        {
            HttpError err = new HttpError("Install key cannot be blank");
            return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
        }
    }
}