using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitListener
{
    public class Core
    {
        private readonly HttpClient httpClient;
        public Core()
        {
            httpClient = new HttpClient();
        }
        public async Task<string> SendHeadRequestToUrl(string url)
        {
            string statusCode;
            try
            {
                using HttpRequestMessage request = new(
                HttpMethod.Head,
                url);
                using HttpResponseMessage response = await httpClient.SendAsync(request);
                statusCode = response.StatusCode.ToString();
            }
            catch
            {
                statusCode = "Invalid Url";
            }
            return statusCode;
        }
        public void Logger(string url, string statusCode)
        {
            var logObject = new
            {
                ServiceName = "RabbitListener",
                Url = url,
                StatusCode = statusCode
            };

            var logJson = JsonConvert.SerializeObject(logObject);

            Console.WriteLine(logJson);
        }
    }
}
