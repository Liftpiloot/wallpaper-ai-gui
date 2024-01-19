using Microsoft.Win32;
using static WallpaperAI.Properties.Settings;

namespace WallpaperAI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            runOnStartButton.Checked = Default.run_on_start;
        }
        

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void runButton_Click(object sender, EventArgs e)
        {
            saveButton.PerformClick();
            string imageFolder = Default.image_folder;
            if (imageFolder == "")
            {
                MessageBox.Show("Please select a folder to save the images");
                return;
            }

            string openaiApiKey = Default.openai_api_key;
            if (openaiApiKey == "")
            {
                openaiApiKey = openai_api.Text;
                Default.openai_api_key = openaiApiKey;
                Default.Save();
            }
            string weatherApiKey = Default.weather_api_key;
            if (weatherApiKey == "")
            {
                weatherApiKey = weather_api.Text;
                Default.weather_api_key = weatherApiKey;
                Default.Save();
            }
            if (openaiApiKey == "" || weatherApiKey == "")
            {
                MessageBox.Show("Please enter your API keys");
                return;
            }
            Wallpaper wallpaper = new Wallpaper();
            wallpaper.GenerateWallpaper(openaiApiKey, weatherApiKey, imageFolder);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Default.run_on_start = runOnStartButton.Checked;
            Default.Save();
            // start the application on startup if true
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (runOnStartButton.Checked)
            {
                rk.SetValue("AI Wallpaper", Application.ExecutablePath);
            }
            else
            {
                rk.DeleteValue("AI Wallpaper", false);
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            // save the keys if they are not empty
            string openaiApiKey = openai_api.Text;
            string weatherApiKey = weather_api.Text;
            if (openaiApiKey != "")
            {
                Default.openai_api_key = openaiApiKey;
                Default.Save();
            }
            if (weatherApiKey != "")
            {
                Default.weather_api_key = weatherApiKey;
                Default.Save();
            }
        }

        private void folderSelect_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string folder = folderBrowserDialog.SelectedPath;
                Default.image_folder = folder;
                Default.Save();
            }

        }
    }
}