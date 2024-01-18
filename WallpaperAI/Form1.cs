namespace WallpaperAI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
            string openai_api_key = Properties.Settings.Default.openai_api_key;
            if (openai_api_key == "")
            {
                openai_api_key = openai_api.Text;
                Properties.Settings.Default.openai_api_key = openai_api_key;
                Properties.Settings.Default.Save();
            }
            string weather_api_key = Properties.Settings.Default.weather_api_key;
            if (weather_api_key == "")
            {
                weather_api_key = weather_api.Text;
                Properties.Settings.Default.weather_api_key = weather_api_key;
                Properties.Settings.Default.Save();
            }
            if (openai_api_key == "" || weather_api_key == "")
            {
                MessageBox.Show("Please enter your API keys");
                return;
            }
            Wallpaper wallpaper = new Wallpaper();
            wallpaper.generateWallpaper(openai_api_key, weather_api_key);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            // save the keys if they are not empty
            string openai_api_key = openai_api.Text;
            string weather_api_key = weather_api.Text;
            if (openai_api_key != "")
            {
                Properties.Settings.Default.openai_api_key = openai_api_key;
                Properties.Settings.Default.Save();
            }
            if (weather_api_key != "")
            {
                Properties.Settings.Default.weather_api_key = weather_api_key;
                Properties.Settings.Default.Save();
            }
        }
    }
}