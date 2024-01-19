using Microsoft.Win32;
using Newtonsoft.Json;
using OpenAI;
using OpenAI.API;
using OpenAI.API.Completions;
using OpenAI.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static WallpaperAI.WeatherData;

namespace WallpaperAI
{
    internal class Wallpaper
    {
        public async Task GenerateWallpaper(string openAiAPI, string weatherAPI, string imageFolder)
        {
            var (lat, lon) = await GetLocationFromIp();

            string weather = await GetWeather(lon, lat, weatherAPI);
            WeatherData.Root weatherJson = JsonConvert.DeserializeObject<WeatherData.Root>(weather);
            string city = weatherJson.name;
            string country = weatherJson.sys.country;
            string location = $"{city} in {country}";

            OpenAIAPI client = new OpenAIAPI(openAiAPI);
            var api = new OpenAI_API.OpenAIAPI(openAiAPI);
            string response = await generatePrompt(client, location, weather);

            string imageUrl = await GenerateImage(api, response, openAiAPI);

            setWallpaper(imageUrl, imageFolder);
        }

        private async Task<string> GenerateImage(OpenAI_API.OpenAIAPI api, string prompt, string openAiAPI)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type",
                    "application/json; charset=utf-8");
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer "+openAiAPI);

                var requestBody = new
                {
                    prompt = prompt,
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
                    return data?.data[0].url;
                }
            }

            return "Error";
        }

        private async Task<(double, double)> GetLocationFromIp()
        {
            string ip_address = await new HttpClient().GetStringAsync("https://api.ipify.org");
            string url = $"https://ipapi.co/{ip_address}/latlong/";
            using var client = new HttpClient();
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var latlong = responseBody.Split(',');
            return (double.Parse(latlong[0]), double.Parse(latlong[1]));
        }

        private async Task<string> GetWeather(double lon, double lat, string openweather_api_key)
        {
            string latitude = lat.ToString();
            string longitude = lon.ToString();
            try
            {
                string url = $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={openweather_api_key}";
                using var client = new HttpClient();
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}\n\nDetails: {ex.ToString()}");
                return ""; // or handle the error in an appropriate way
            }

        }

        private async Task<string> generatePrompt(OpenAIAPI client, string location, string weather) {
       
            var parameters = new CompletionRequest
            {
                Model = "gpt-3.5-turbo-instruct",
                Prompt = $"image generation prompt for a painting of {location} with this weather: {weather}",
                MaxTokens = 300
            };

            var response = await client.Completions.CreateCompletionAsync(parameters);
            return response.ToString();
        }

        private void setWallpaper(string imageurl, string imageFolder)
        {
            string filename = DateTime.Now.ToString("yyyy-MM-dd") + ".jpg";
            string path = $"{imageFolder}\\{filename}";
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(imageurl, path);
            }

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            static extern int SystemParametersInfo(
                                    int uAction, int uParam,
                                    string lpvParam, int fuWinIni);

           SystemParametersInfo(20, 0, path, 1 | 2);

    }
    }
}
