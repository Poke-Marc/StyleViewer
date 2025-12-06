using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;

namespace StyleViewerApp
{
    // The error says "UpdateWindow", so this class MUST be named UpdateWindow
    public partial class UpdateWindow : Window
    {
        public UpdateWindow()
        {
            InitializeComponent();
        }

        // ========================================================
        // THIS IS THE MISSING METHOD CAUSING THE ERROR
        // ========================================================
        private async void Window_ContentRendered(object sender, EventArgs e)
        {
            await RunUpdateProcess();
        }
        // ========================================================

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private async Task RunUpdateProcess()
        {
            // 1. Start Animation
            StatusText.Text = "Checking for updates...";
            UpdateProgress.IsIndeterminate = true;

            await Task.Delay(3000); // Wait 3 seconds

            // 2. Stop Animation (Green Bar)
            StatusText.Text = "Redirecting to github to view updates...";
            UpdateProgress.IsIndeterminate = false; // Stops the loop
            UpdateProgress.Value = 100;             // Fills it green

            await Task.Delay(1500);

            // 3. Open Browser
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://github.com/WindowsSe7en/StyleViewer", // Your Link
                    UseShellExecute = true
                });
            }
            catch { }

            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}