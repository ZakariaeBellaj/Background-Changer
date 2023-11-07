using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace backgroundChanger
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        private static extern int SystemParametersInfo(uint action, uint uParam, string vParam, uint winIni);

        private const uint SPI_SETWALL = 0x0014;
        private const uint SPIF_UPDATE = 0x01;
        private const uint SPIF_SENDCHANGE = 0x02;

        private List<string> imagePaths = new List<string>();
        private int currentImageIndex = 0;
        private Timer wallpaperTimer = new Timer();
        private int rotationDelay = 5000;

        public Form1()
        {
            InitializeComponent();
            InitializeTimer();
        }
        private void InitializeTimer()
        {
            wallpaperTimer.Interval = rotationDelay;
            wallpaperTimer.Tick += ChangeWallpaper;
        }

        private void ChangeWallpaper(object sender, EventArgs e)
        {
            if (imagePaths.Count > 0)
            {
                currentImageIndex = (currentImageIndex + 1) % imagePaths.Count;
                SetWallpaper(imagePaths[currentImageIndex]);
            }
        }

        private void SetWallpaper(string imagePath)
        {
            int result = SystemParametersInfo(SPI_SETWALL, 0, imagePath, SPIF_UPDATE | SPIF_SENDCHANGE);

            if (result == 0)
            {
                MessageBox.Show("Failed to set the desktop wallpaper.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
                Title = "Select Images for Wallpaper"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                imagePaths.AddRange(openFileDialog.FileNames);
                // If the timer is not running, start it when the user adds images.
                if (!wallpaperTimer.Enabled)
                {
                    wallpaperTimer.Start();
                }
            }
        }


        
    }
}
