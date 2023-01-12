using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ConsoleClient
{
    internal class Program
    {
        static HttpClient client = null;
        static void Main(string[] args)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7028/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            while (true)
            {
                _ = Console.ReadLine();

                try
                {
                    var token = GetToken().Result;

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var t = client.GetAsync("Subjects").Result;
                    t.EnsureSuccessStatusCode();
                    Console.WriteLine(t.Content.ReadAsStringAsync().Result);
                }
                catch (Exception)
                {

                }
            }

            Console.WriteLine("Hello, World!");
        }

        static async Task<string> GetToken()
        {
            var token = string.Empty;

            if (File.Exists("D:/token.txt"))
                token = File.ReadAllText("D:/token.txt").Trim();

            if (string.IsNullOrEmpty(token))
                token = await Login();
            else
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);
                if (jwtSecurityToken.ValidTo < DateTime.UtcNow)
                    token = await Login();
            }

            return token;
        }

        static async Task<string> Login()
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("auth", new { Username = "mahdmu", Password = "Test.1234" });
            response.EnsureSuccessStatusCode();
            var token = await response.Content.ReadAsStringAsync();
            token = token.Trim('\"');
            File.WriteAllText("D:/token.txt", token);
            return token.Trim('\"');
        }
    }
}