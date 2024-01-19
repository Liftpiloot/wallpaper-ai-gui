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
            string imageFolder = Properties.Settings.Default.image_folder;
            if (imageFolder == "")
            {
                MessageBox.Show("Please select a folder to save the images");
                return;
            }

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
            wallpaper.generateWallpaper(openai_api_key, weather_api_key, imageFolder);
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

        private void folderSelect_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string folder = folderBrowserDialog.SelectedPath;
                Properties.Settings.Default.image_folder = folder;
                Properties.Settings.Default.Save();
            }

        }
    }
}