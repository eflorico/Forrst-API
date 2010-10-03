using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Forrst
{
    public class ForrstClient
    {
        public ForrstClient() {
            this.RequestLog = new List<Uri>();
        }

        public static Uri BaseUri = new Uri("http://forrst.com/");
        protected static Uri ApiBaseUri = new Uri("http://api.forrst.com/api/v1/");

        public List<Uri> RequestLog { get; private set; }

        public JToken Request(string action, Dictionary<string, string> parameters, string resultField) {
            var query = parameters.Aggregate("", (sum, parameter) => 
                sum + Uri.EscapeUriString(parameter.Key) + "=" + Uri.EscapeUriString(parameter.Value) + "&");
            var uri = new Uri(new Uri(ForrstClient.ApiBaseUri, action).ToString() + "?" + query);

            this.RequestLog.Add(uri);

            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            var response = request.GetResponse();
            var responseReader = new StreamReader(response.GetResponseStream());
            var responseBody = responseReader.ReadToEnd();
            responseReader.Close();
            response.Close();

            return JObject.Parse(responseBody)["resp"][resultField];
        }
    }
}
