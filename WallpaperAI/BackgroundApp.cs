using System.Net.NetworkInformation;

namespace WallpaperAI;

internal partial class BackgroundApp: ApplicationContext
{
    private readonly NotifyIcon trayIcon;
    
    public BackgroundApp()
    {
        trayIcon = new()
        {
            Icon = Properties.Resources.icon,
            Text = "Ai wallpapers",
            Visible = true
        };

        ContextMenuStrip contextMenu = new();

        ToolStripMenuItem openSettings = new()
        {
            Text = "Settings"
        };
        openSettings.Click += OpenSettingsMenu;
        contextMenu.Items.Add(openSettings);

        ToolStripMenuItem exitApplication = new()
        {
            Text = "Close"
        };
        exitApplication.Click += new EventHandler(CloseApplication);
        contextMenu.Items.Add(exitApplication);

        trayIcon.ContextMenuStrip = contextMenu;
        
        // Generate wallpaper if run on start is enabled
        if (Properties.Settings.Default.run_on_start)
        { 
            //wait for internet connection
            while (!NetworkInterface.GetIsNetworkAvailable())
            {
                Thread.Sleep(1000);
            }
            // Generate wallpaper
            run_wallpaper();
        }
    }

    private async Task run_wallpaper()
    {
        string imageFolder = Properties.Settings.Default.image_folder;
        if (imageFolder == "")
        {
            MessageBox.Show("Please select a folder to save the images");
            return;
        }

        string openaiApiKey = Properties.Settings.Default.openai_api_key;
        if (openaiApiKey == "")
        {
            openaiApiKey = Properties.Settings.Default.openai_api_key;
            Properties.Settings.Default.openai_api_key = openaiApiKey;
            Properties.Settings.Default.Save();
        }
        string weatherApiKey = Properties.Settings.Default.weather_api_key;
        if (weatherApiKey == "")
        {
            weatherApiKey = Properties.Settings.Default.weather_api_key;
            Properties.Settings.Default.weather_api_key = weatherApiKey;
            Properties.Settings.Default.Save();
        }
        if (openaiApiKey == "" || weatherApiKey == "")
        {
            MessageBox.Show("Please enter your API keys");
            return;
        }
        Wallpaper wallpaper = new();
        await wallpaper.GenerateWallpaper(openaiApiKey, weatherApiKey, imageFolder);
    }

    private void OpenSettingsMenu(object sender, EventArgs e)
    {
        Form1 form = new();
        form.Show();
    }
    public void CloseApplication(object sender, EventArgs e)
    {
        DialogResult messageBox = MessageBox.Show("Are you sure you want to close Ai wallpapers", "Close App?", MessageBoxButtons.YesNo);
        if(messageBox.Equals(DialogResult.Yes))
        {
            Dispose();
            Environment.Exit(1);
            Application.Exit();
        }
    }
}