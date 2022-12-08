using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JIRAtoDF
{
    class JiraConnector
    {
        private string userName = @"username";
        private const string PASSWORD = "somepassword";
        private const string BASEURL = "https://genericURL.net/rest/api/3/";

        private string getConnectionString(string searchQuery)
        {
            return $"{BASEURL}{searchQuery}";            
        }

        private string getEncodedCredentials()
        {
            string mergedCredentials = string.Format("{0}:{1}", userName, PASSWORD);
            byte[] byteCredentials = Encoding.UTF8.GetBytes(mergedCredentials);
            return Convert.ToBase64String(byteCredentials);
        }

        public dynamic GetDataFromRequest(string requestQuery)
        {
            var request = WebRequest.Create(getConnectionString(requestQuery)) as HttpWebRequest;
            request.Method = "GET";
            request.Accept = "application/json";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Basic " + getEncodedCredentials());

            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var reader = new StreamReader(response.GetResponseStream());
                    string str = reader.ReadToEnd();
                    return JsonConvert.DeserializeObject<dynamic>(str);
                }
                else { return null; }
            }
        }
    }
}
