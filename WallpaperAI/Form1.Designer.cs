namespace WallpaperAI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            openai_api = new TextBox();
            weather_api = new TextBox();
            label2 = new Label();
            label3 = new Label();
            runButton = new Button();
            runOnStartButton = new CheckBox();
            label4 = new Label();
            label5 = new Label();
            saveButton = new Button();
            folderSelect = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(125, 29);
            label1.Name = "label1";
            label1.Size = new Size(530, 54);
            label1.TabIndex = 0;
            label1.Text = "Ai weather wallpaper creator";
            label1.Click += label1_Click;
            // 
            // openai_api
            // 
            openai_api.Location = new Point(324, 141);
            openai_api.Name = "openai_api";
            openai_api.Size = new Size(350, 27);
            openai_api.TabIndex = 1;
            // 
            // weather_api
            // 
            weather_api.Location = new Point(324, 184);
            weather_api.Name = "weather_api";
            weather_api.Size = new Size(350, 27);
            weather_api.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(125, 141);
            label2.Name = "label2";
            label2.Size = new Size(114, 20);
            label2.TabIndex = 3;
            label2.Text = "OpenAI API key:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(125, 184);
            label3.Name = "label3";
            label3.Size = new Size(185, 20);
            label3.TabIndex = 4;
            label3.Text = "OpenWeatherMap API key:";
            label3.Click += label3_Click;
            // 
            // runButton
            // 
            runButton.Location = new Point(580, 289);
            runButton.Name = "runButton";
            runButton.Size = new Size(94, 29);
            runButton.TabIndex = 5;
            runButton.Text = "Run";
            runButton.UseVisualStyleBackColor = true;
            runButton.Click += runButton_Click;
            // 
            // runOnStartButton
            // 
            runOnStartButton.AutoSize = true;
            runOnStartButton.Location = new Point(324, 243);
            runOnStartButton.Name = "runOnStartButton";
            runOnStartButton.Size = new Size(127, 24);
            runOnStartButton.TabIndex = 6;
            runOnStartButton.Text = "Run on startup";
            runOnStartButton.UseVisualStyleBackColor = true;
            runOnStartButton.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(457, 244);
            label4.Name = "label4";
            label4.Size = new Size(232, 20);
            label4.TabIndex = 7;
            label4.Text = "(OpenAI will charge for every run)";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(125, 102);
            label5.Name = "label5";
            label5.Size = new Size(274, 20);
            label5.TabIndex = 8;
            label5.Text = "*Only fill in the api keys on first time use";
            label5.Click += label5_Click;
            // 
            // saveButton
            // 
            saveButton.Location = new Point(471, 289);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(94, 29);
            saveButton.TabIndex = 9;
            saveButton.Text = "Save";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += saveButton_Click;
            // 
            // folderSelect
            // 
            folderSelect.Location = new Point(249, 289);
            folderSelect.Name = "folderSelect";
            folderSelect.Size = new Size(202, 29);
            folderSelect.TabIndex = 10;
            folderSelect.Text = "Select image save location";
            folderSelect.UseVisualStyleBackColor = true;
            folderSelect.Click += folderSelect_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(folderSelect);
            Controls.Add(saveButton);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(runOnStartButton);
            Controls.Add(runButton);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(weather_api);
            Controls.Add(openai_api);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox openai_api;
        private TextBox weather_api;
        private Label label2;
        private Label label3;
        public Button runButton;
        private CheckBox runOnStartButton;
        private Label label4;
        private Label label5;
        private Button saveButton;
        private Button folderSelect;
    }
}