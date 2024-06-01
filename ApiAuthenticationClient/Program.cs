using System.Text.Json;
using Newtonsoft.Json.Linq;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Step 1: Authenticate and obtain JWT token
            var token = await AuthenticateAndGetToken();

            // Step 2: Call the protected endpoint with the obtained token
            await CallProtectedEndpoint(token);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }

    static async Task<string> AuthenticateAndGetToken()
    {
        using (var httpClient = new HttpClient())
        {
            var model = new { Username = "admin", Password = "admin123" };

            var modelJson = JsonSerializer.Serialize(model);
            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7051/api/Auth/login");

            var content = new StringContent(modelJson, null, "application/json");

            request.Content = content;

            var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var token = await response.Content.ReadAsStringAsync();

            return token;
        }
    }

    static async Task CallProtectedEndpoint(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return;
        }

        var jsonObject = JObject.Parse(token);
        if (jsonObject == null)
        {
            return;
        }

        string tokenValue = jsonObject["token"].ToString();

        using (var httpClient = new HttpClient())
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7051/api/protected");

                request.Headers.Add("Authorization", $"Bearer {tokenValue}");

                var response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Protected Endpoint Response: " + content);
                }
                else
                {
                    Console.WriteLine("Failed to access protected endpoint. Status code: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }
}
