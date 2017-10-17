using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace TestConsoleApplication
{
    class Program
    {
        static HttpClient client = new HttpClient();

        public class TextResult : IHttpActionResult
        {
            string _value;
            HttpRequestMessage _request;

            public TextResult(string value, HttpRequestMessage request)
            {
                _value = value;
                _request = request;
            }
            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                var response = new HttpResponseMessage()
                {
                    Content = new StringContent(_value),
                    RequestMessage = _request
                };
                return Task.FromResult(response);
            }
        }

        public class ValuesController : ApiController
        {
            public IHttpActionResult Get()
            {
                return new TextResult("hello", Request);
            }
        }

        static void Main(string[] args)
        {
            var val = new ValuesController();
            var resp = val.Get();
            resp.ExecuteAsync
        }
    }
}
