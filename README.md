# Create an AI generated wallpaper based on the weather
Create and set a wallpaper based on your location and weather data

Allow it to run on startup to have a brand new wallpaper every day

Example wallpaper (village in the netherlands, cloudy winter day):

<img src="https://github.com/Liftpiloot/wallpaper-ai-gui/assets/119590103/0d79f589-32d2-4ce3-bc08-0cbe8a7ae736" alt="drawing" width="600"/>
<div></div>
<img src="https://github.com/Liftpiloot/wallpaper-ai-gui/assets/119590103/46258d2a-dd90-47a2-81a4-02a4bec29c3e" alt="openweather logo" width="150"/>

![OpenAI](https://a11ybadges.com/badge?logo=openai)


-Create openAI API key: https://platform.openai.com/docs/quickstart?context=python

-Create free openweathermap API key: https://openweathermap.org/api

Save these keys in the settings form, by right clicking on the tray icon

## note:
- API
  - OpenAI API costs about $0.08-$0.10 per request (wallpaper)
  - OpenWeather api is free (upto 1000 requests per day)
  - OpenAI might refuse requests, if you have a daily limit set (that is too low)
- Conflicts
  - Other wallpaper applications might conflict, disable them (wallpaper engine)
  - Not tested on windows versions other than windows 11
- Manual
  - Wallpaper AI opens in the background, right click the icon in the tray menu to open the menu
  - To disable run on start, uncheck the option in the settings (closing the app won't disable it)
  - Wallpapers are overwritten if created on the same day, rename or move them to keep them
  - Generation may take a minute or two, spamming the run button will result in multiple requests running at once
 
## about:
This c# application estimates your location using your ip adress. This information is passed to openweather, which returns the current weather information. 

This information is used by gpt 3.5 to create an image generation prompt based on your location and weather situation. Dalle-3 uses this prompt to create a wallpaper that should more or less represent the view in your area. This will sometimes be a photorealistic painting, but may result in a more abstract drawing. 

- Donations: https://www.buymeacoffee.com/liftpiloot
