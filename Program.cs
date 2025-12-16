using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Security;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace pr10
{
    internal class Program
    {
        static string ClientId = "019b267d-0b72-72ce-8d6a-efd54474216c";

        static string AuthorizationKey = "MDE5YjI2N2QtMGI3Mi03MmNlLThkNmEtZWZkNTQ0NzQyMTZjOmU0MTE4ZWRlLTg2ZTAtNGVmYy04ODQyLWFhZmRjMGU5OGJiMA==";

        public static async Task<string> GetToken(string rqUID, string bearer)
        {
            string ReturnToken = null;
            string Url = "https://ngw.devices.sberbank.ru:9443/api/v2/oauth";

            using (HttpClientHandler Handler = new HttpClientHandler())
            {
                Handler.ServerCertificateCustomValidationCallback = (message, cert, chain, SslPolicyErrors) => true;

                using (HttpClient Clien = new HttpClient(Handler)) 
                {
                HttpRequestMessage Request = new HttpRequestMessage(HttpMethod.Post, Url);

                    Request.Headers.Add("Accept", "application/json");
                    Request.Headers.Add("RqUID", rqUID);
                    Request.Headers.Add("Authorization", $"Bearer {bearer}");

                    var Data = new List<KeyValuePair<string, string>> { 
                    new KeyValuePair<string, string>("scope", "GIGACHAT_API_PERS")
                    };

                    Request.Content = new FormUrlEncodedContent(Data);

                    HttpResponseMessage Response = await Clien.SendAsync(Request);

                    if (Response.IsSuccessStatusCode)
                    {
                        string ResponseContent = await Response.Content.ReadAsStringAsync();
                        ResponseToken Token = JsonConvert.DeserializeObject<ResponseToken>(ResponseContent);
                        ReturnToken = Token.access_token;
                    }
                }
            }

            return ReturnToken;
        }
        static void Main(string[] args)
        {

        }
    }
}
