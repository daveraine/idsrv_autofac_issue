using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using Thinktecture.IdentityModel.Client;
using Thinktecture.IdentityModel.Extensions;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var response = RequestToken();
            ShowResponse(response);
            CallService(response.AccessToken);

            Console.ReadLine();
        }

        static TokenResponse RequestToken()
        {
            var client = new OAuth2Client(
                new Uri("https://localhost:44333/connect/token"),
                "carbon",
                "secret");

            return client.RequestResourceOwnerPasswordAsync("bob", "secret", "api1").Result;
        }

        static void CallService(string token)
        {
            var baseAddress = "https://localhost:44333/api/";

            var client = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };

            client.SetBearerToken(token);
            var response = client.GetAsync("TestData").Result;

            if (response.IsSuccessStatusCode)
            {
                "\n\nIs Authenticated:".ConsoleGreen();
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                "\n\nError:".ConsoleRed();
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }
        }

        private static void ShowResponse(TokenResponse response)
        {
            if (!response.IsError)
            {
                "Token response:".ConsoleGreen();
                Console.WriteLine(response.Json);

                if (response.AccessToken.Contains("."))
                {
                    "\nAccess Token (decoded):".ConsoleGreen();

                    var parts = response.AccessToken.Split('.');
                    var header = parts[0];
                    var claims = parts[1];

                    Console.WriteLine(JObject.Parse(Encoding.UTF8.GetString(Base64Url.Decode(header))));
                    Console.WriteLine(JObject.Parse(Encoding.UTF8.GetString(Base64Url.Decode(claims))));
                }
            }
            else
            {
                if (response.IsHttpError)
                {
                    "HTTP error: ".ConsoleGreen();
                    Console.WriteLine(response.HttpErrorStatusCode);
                    "HTTP error reason: ".ConsoleGreen();
                    Console.WriteLine(response.HttpErrorReason);
                }
                else
                {
                    "Protocol error response:".ConsoleGreen();
                    Console.WriteLine(response.Json);
                }
            }
        }
    }
}
