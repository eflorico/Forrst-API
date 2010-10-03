using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Forrst
{
    /// <summary>
    /// The client that allows you to access the Forrst API.
    /// </summary>
    public class ForrstClient
    {
        public ForrstClient() {
            this.RequestLog = new List<Uri>();
        }

        public static Uri BaseUri = new Uri("http://forrst.com/");
        protected static Uri ApiBaseUri = new Uri("http://api.forrst.com/api/v1/");

        /// <summary>
        /// A log of requests that have been made to obtain the data you accessed.
        /// </summary>
        public List<Uri> RequestLog { get; private set; }

        /// <summary>
        /// Requests data from the Forrst API. You should not use this method directly, but rather use the properties provided by objects like User and Post.
        /// </summary>
        /// <param name="action">The path to query, relative to the api base uri.</param>
        /// <param name="parameters">The parameters to add to the query.</param>
        /// <param name="resultField">The name of the field containing the result JSON data to be returned.</param>
        /// <returns></returns>
        public JToken Request(string action, Dictionary<string, string> parameters, string resultField) {
            //Build request URI
            var query = parameters.Aggregate("", (sum, parameter) => 
                sum + Uri.EscapeUriString(parameter.Key) + "=" + Uri.EscapeUriString(parameter.Value) + "&");
            var uri = new Uri(new Uri(ForrstClient.ApiBaseUri, action).ToString() + "?" + query);

            this.RequestLog.Add(uri);

            //Create request
            var request = (HttpWebRequest)WebRequest.Create(uri);
            //Make sure the content is automatically decompressed if necessary
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            //Receive response
            var response = request.GetResponse();
            var responseReader = new StreamReader(response.GetResponseStream());
            var responseBody = responseReader.ReadToEnd();
            responseReader.Close();
            response.Close();

            //Parse JSON
            return JObject.Parse(responseBody)["resp"][resultField];
        }
    }
}
