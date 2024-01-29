using Newtonsoft.Json;
using OpenAI.API;
using OpenAI.API.Completions;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;

namespace WallpaperAI
{
    internal class Wallpaper
    {
        /// <summary>
        /// Main function to generate the wallpaper
        /// Generates a prompt for the OpenAI API, then generates an image from the prompt
        /// and sets the image as the wallpaper
        /// </summary>
        /// <param name="openAiApi">Api key for openAI</param>
        /// <param name="weatherApi">Api key for openWeatherMap</param>
        /// <param name="imageFolder">location to store generated wallpaper</param>
        public async Task GenerateWallpaper(string openAiApi, string weatherApi, string imageFolder)
        {
            var (lat, lon) = await GetLocationFromIp();
            Debug.WriteLine("location: " + lat + " " + lon);

            string weather = await GetWeather(lat, lon, weatherApi);
            Debug.WriteLine("weather: " + weather);

            WeatherData.Root weatherJson = JsonConvert.DeserializeObject<WeatherData.Root>(weather);
            Debug.Assert(weatherJson != null, nameof(weatherJson) + " != null");
            string city = weatherJson.name;
            string country = weatherJson.sys.country;
            string location = $"{city} in {country}";

            OpenAIAPI client = new OpenAIAPI(openAiApi);
            string response = await GeneratePrompt(client, location, weather);
            Debug.WriteLine("prompt: " + response);

            string imageUrl = await GenerateImage(response, openAiApi);
            Debug.WriteLine("image: " + imageUrl);

            SetWallpaper(imageUrl, imageFolder);
        }
        
        private async Task<string> GenerateImage(string prompt, string openAiApi)
        {
            using HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type",
                "application/json; charset=utf-8");
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer "+openAiApi);

            var requestBody = new
            {
                prompt,
                n = 1,
                size = "1792x1024",
                model = "dall-e-3"
            };
            var jsonBody = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            const string url = "https://api.openai.com/v1/images/generations";
            var response = await httpClient.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                dynamic ? data = JsonConvert.DeserializeObject(responseContent);
                return data?.data[0].url ?? throw new InvalidOperationException();
            }

            return "Error";
        }

        private async Task<(string, string)> GetLocationFromIp()
        {
            string ipAddress = await new HttpClient().GetStringAsync("https://api.ipify.org");
            string url = $"https://ipapi.co/{ipAddress}/latlong/";
            using var client = new HttpClient();
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var latlong = responseBody.Split(',');
            return (latlong[0],latlong[1]);
        }

        private async Task<string> GetWeather(string lat, string lon, string openweatherApiKey)
        {
            try
            {
                string url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={openweatherApiKey}";
                using var client = new HttpClient();
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}\n\nDetails: {ex}");
                return ""; // or handle the error in an appropriate way
            }

        }

        private async Task<string> GeneratePrompt(OpenAIAPI client, string location, string weather) {
       
            var parameters = new CompletionRequest
            {
                Model = "gpt-3.5-turbo-instruct",
                Prompt = $"image generation prompt for a painting of {location} with this weather: {weather}",
                MaxTokens = 300
            };

            var response = await client.Completions.CreateCompletionAsync(parameters);
            return response.ToString();
        }

        private void SetWallpaper(string imageUrl, string imageFolder)
        {
            string filename = DateTime.Now.ToString("yyyy-MM-dd") + ".jpg";
            string path = $"{imageFolder}\\{filename}";
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(imageUrl, path);
            }

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            static extern int SystemParametersInfo(
                                    int uAction, int uParam,
                                    string lpvParam, int fuWinIni);

           SystemParametersInfo(20, 0, path, 1 | 2);

    }
    }
}
