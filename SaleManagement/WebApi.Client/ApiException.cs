using Newtonsoft.Json;
using System;
using System.Net;

namespace WebApi.Client
{
    public class ApiException : Exception
    {
        public class ExpectedExceptionContents
        {
            public string Headline { get; set; }
            public string Message { get; set; }
            public string Detail { get; set; }
        }

        public int StatusCode { get; private set; }

        public string Response { get; private set; }

        public bool UserInformed { get; set; }

        public string HeadLine { get; set; }
        public string Detail { get; set; } 


        public IReadOnlyDictionary<string, IEnumerable<string>> Headers { get; private set; }

        public ApiException(string message, int statusCode, string response, IReadOnlyDictionary<string, IEnumerable<string>> headers, Exception innerException)
            : base(message + "Status: " + statusCode + "\nResponse: \n" + ((response == null) ? "(null)" : response.Substring(0, response.Length >= 512 ? 512 : response.Length)), innerException)
        {
            if (statusCode == (int)HttpStatusCode.BadRequest)
            {
                var expectedException = JsonConvert.DeserializeObject<ExpectedExceptionContents>(response);
                if (expectedException is not null)
                {
                    HeadLine = expectedException.Headline;
                    Response = expectedException.Message;
                    Detail = expectedException.Detail;
                }
            }
            else
            {
                Response = response;
            }
            StatusCode = statusCode;
            Headers = headers;
            UserInformed = false;
            ExceptionOutlet.PreThrow(statusCode == (int)HttpStatusCode.BadRequest, this);
        }

        public override string ToString()
        {
            return string.Format("HTTP Response: \n\n{0}\n\n{1}", Response, base.ToString());
        }
    }
}
