using System;
using System.Drawing;
using System.Windows.Forms;

namespace CheckersWindowUI
{
    public partial class StartForm : Form
    {
        private readonly SettingsForm r_FormSettings;
        private readonly PictureBox r_PictureBoxStartScreen;

        public StartForm()
        {
            this.Text = "Checkers";
            r_FormSettings = new SettingsForm();
            r_PictureBoxStartScreen = new PictureBox();
            initializeControls();
        }

        private void initializeControls()
        {
            ((System.ComponentModel.ISupportInitialize)this.r_PictureBoxStartScreen).BeginInit();
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.StartPosition = FormStartPosition.CenterScreen;
            initializePictureBox();
            this.ClientSize = new Size(410, 246);
            this.Controls.Add(r_PictureBoxStartScreen);
            this.Name = "Start Form";
            ((System.ComponentModel.ISupportInitialize)this.r_PictureBoxStartScreen).EndInit();
            this.ResumeLayout(false);
            r_PictureBoxStartScreen.Click += pictureBoxStartScreen_Pressed;
        }

        private void initializePictureBox()
        {
            r_PictureBoxStartScreen.Image = global::CheckersWindowUI.Properties.Resources.startimage;
            r_PictureBoxStartScreen.Location = new Point(0, 0);
            r_PictureBoxStartScreen.Name = "startScreen";
            r_PictureBoxStartScreen.Size = new Size(410, 246);
            r_PictureBoxStartScreen.SizeMode = PictureBoxSizeMode.StretchImage;
            r_PictureBoxStartScreen.TabIndex = 0;
            r_PictureBoxStartScreen.TabStop = false;
        }

        private void pictureBoxStartScreen_Pressed(object sender, EventArgs e)
        {
            this.Hide();
            r_FormSettings.ShowDialog();
        }

        private void StartForm_Load(object sender, EventArgs e)
        {
        }
    }
}
