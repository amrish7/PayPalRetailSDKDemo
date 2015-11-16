using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PayPalRetailSDK;

namespace Net4WPFDemo
{
    class CustomNetworkHandler : IRetailNetworkRequestHandler
    {
        public bool DidHandleHttpRequest(RetailHttpRequest request, Action<RetailHttpResponse> callback)
        {
            if (request.Body != null)
            {
                string s = Encoding.UTF8.GetString(request.Body);
            }

            if (request.Url.AbsolutePath.EndsWith("/payment"))
            {
                var c = new RestSharp.RestClient("https://" + request.Url.DnsSafeHost);
                RestSharp.RestRequest req = new RestSharp.RestRequest(request.Url.AbsolutePath, (RestSharp.Method)Enum.Parse(typeof(RestSharp.Method), request.Method));
                foreach (var h in request.Headers)
                    req.AddHeader(h.Key, h.Value);

                if (request.Body != null)
                {
                    req.RequestFormat = RestSharp.DataFormat.Json;
                    req.AddParameter("application/json; charset=utf-8", Encoding.UTF8.GetString(request.Body), RestSharp.ParameterType.RequestBody);
                }

                var result = c.Execute(req);
                var headers = new Dictionary<string, string>();
                callback(new RetailHttpResponse
                {
                    Body = Encoding.UTF8.GetBytes(result.Content),
                    Headers = result.Headers.ToDictionary(o => o.Name, o => o.Value.ToString()),
                    StatusCode = (short)result.StatusCode
                });

                return true;
            }

            return false;
        }
    }
}
