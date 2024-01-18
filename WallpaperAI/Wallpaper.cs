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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static WallpaperAI.WeatherData;

namespace WallpaperAI
{
    internal class Wallpaper
    {
        public async Task generateWallpaper(string openAiAPI, string weatherAPI)
        {
            var (lat, lon) = await GetLocationFromIp();
            MessageBox.Show($"Your location is {lat}, {lon}");

            string weather = await GetWeather(lon, lat, weatherAPI);
            WeatherData.Root weatherJson = JsonConvert.DeserializeObject<WeatherData.Root>(weather);
            string city = weatherJson.name;
            string country = weatherJson.sys.country;
            string location = $"{city} in {country}";
            MessageBox.Show($"Your location is {location}");

            OpenAIAPI client = new OpenAIAPI(openAiAPI);
            string response = await generatePrompt(client, location, weather);
            MessageBox.Show(response);

            string imageUrl = await generateImage(client, response, openAiAPI);
            MessageBox.Show(imageUrl);

            setWallpaper(imageUrl);
        }

        private async Task<string> generateImage(OpenAIAPI client, string prompt, string openAiAPI)
        {
            string url = "https://api.openai.com/v1/images/generations";
            string body = $"{{\"prompt\": \"{prompt}\", \"n\": 1, \"size\": \"1792x1024\", \"model\": \"dall-e-3\", \"quality\": \"standard\"}}";

            var data = Encoding.ASCII.GetBytes(body);

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;
            request.Headers.Add("Authorization", "Bearer " + openAiAPI);
            Debug.WriteLine($"Request URL: {url}");
            Debug.WriteLine($"Request Headers: {request.Headers}");
            Debug.WriteLine($"Request Body: {body}");

            try
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                var response = (HttpWebResponse)request.GetResponse();
                Debug.WriteLine($"Response Status Code: {response.StatusCode}");
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                Debug.WriteLine($"Response Body: {responseString}");
                return responseString; 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return ""; // or handle the error in an appropriate way
            }


        }

        public async Task<(double, double)> GetLocationFromIp()
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

        public async Task<string> GetWeather(double lon, double lat, string openweather_api_key)
        {
            string latitude = lat.ToString();
            string longitude = lon.ToString();
            try
            {
                string url = $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={openweather_api_key}";
                using var client = new HttpClient();
                Debug.WriteLine(url);
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

        public void setWallpaper(string imageurl)
        {
            string path = @"C:\Users\Public\Pictures\wallpaper.jpg";
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
