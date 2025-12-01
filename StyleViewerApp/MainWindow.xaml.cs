using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Diagnostics;

namespace StyleViewerApp
{
    public partial class MainWindow : Window
    {
        private string _currentThemeName = "Aero";

        public MainWindow()
        {
            InitializeComponent();
            // Default Blue Title Bar
            StyleTitleBar("Default");
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (ChkSplash != null && ChkSplash.IsChecked == true)
            {
                SplashGrid.Visibility = Visibility.Visible;
                await Task.Delay(3000);
                SplashGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                SplashGrid.Visibility = Visibility.Collapsed;
            }
        }

        // Window Chrome
        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) this.DragMove();
        }
        private void BtnClose_Click(object sender, RoutedEventArgs e) => Close();
        private void BtnMinimize_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
        private void BtnMaximize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = (WindowState == WindowState.Maximized) ? WindowState.Normal : WindowState.Maximized;
        }

        // Navigation
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            ThemeGrid.Visibility = Visibility.Collapsed;
            SettingsGrid.Visibility = Visibility.Collapsed;
            HomeGrid.Visibility = Visibility.Visible;
            BtnStopVideo_Click(null, null);
        }
        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            HomeGrid.Visibility = Visibility.Collapsed;
            ThemeGrid.Visibility = Visibility.Collapsed;
            SettingsGrid.Visibility = Visibility.Visible;
        }
        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UpdateWindow updatePopup = new UpdateWindow();
                updatePopup.Owner = this;
                updatePopup.ShowDialog();
            }
            catch
            {
                MessageBox.Show("Please create 'UpdateWindow.xaml' first.");
            }
        }

        // Themes
        private void BtnClassic_Click(object sender, RoutedEventArgs e) => SwitchTheme("Classic", "CLASSIC STYLE");
        private void BtnLuna_Click(object sender, RoutedEventArgs e) => SwitchTheme("Luna", "LUNA THEME");
        private void BtnRoyale_Click(object sender, RoutedEventArgs e) => SwitchTheme("Royale", "MEDIA CENTER");
        private void BtnAero_Click(object sender, RoutedEventArgs e) => SwitchTheme("Aero", "AERO GLASS");
        private void BtnMetro_Click(object sender, RoutedEventArgs e) => SwitchTheme("Metro", "METRO UI");
        private void BtnDefault_Click(object sender, RoutedEventArgs e) => SwitchTheme("Default", "SYSTEM DEFAULT");

        private void SwitchTheme(string themeKey, string title)
        {
            _currentThemeName = themeKey;
            ApplyTheme(themeKey);
            StyleTitleBar(themeKey);
            if (TxtThemeTitle != null) TxtThemeTitle.Text = title;
            HomeGrid.Visibility = Visibility.Collapsed;
            SettingsGrid.Visibility = Visibility.Collapsed;
            ThemeGrid.Visibility = Visibility.Visible;
        }

        private void ApplyTheme(string theme)
        {
            var appResources = Application.Current.Resources;
            appResources.MergedDictionaries.Clear();
            Uri themeUri = null;
            switch (theme)
            {
                case "Classic": themeUri = new Uri("pack://application:,,,/PresentationFramework.Classic;component/themes/classic.xaml"); break;
                case "Luna": themeUri = new Uri("pack://application:,,,/PresentationFramework.Luna;component/themes/luna.normalcolor.xaml"); break;
                case "Royale": themeUri = new Uri("pack://application:,,,/PresentationFramework.Royale;component/themes/royale.normalcolor.xaml"); break;
                case "Aero": themeUri = new Uri("pack://application:,,,/PresentationFramework.Aero;component/themes/aero.normalcolor.xaml"); break;
                case "Metro": themeUri = new Uri("pack://application:,,,/PresentationFramework.Aero2;component/themes/aero2.normalcolor.xaml"); break;
                case "Default": return;
            }
            if (themeUri != null)
            {
                try
                {
                    ResourceDictionary newTheme = new ResourceDictionary();
                    newTheme.Source = themeUri;
                    appResources.MergedDictionaries.Add(newTheme);
                }
                catch { }
            }
        }

        // VISUAL STYLING FOR WINDOW (The Fix)
        private void StyleTitleBar(string theme)
        {
            Brush winBg, winFg;
            Thickness winBorder;
            CornerRadius winRadius;
            string buttonStyleKey = "BtnStyleAero"; // Fallback

            switch (theme)
            {
                case "Classic":
                    winBg = new SolidColorBrush(Color.FromRgb(192, 192, 192));
                    winFg = Brushes.Black;
                    winBorder = new Thickness(3);
                    winRadius = new CornerRadius(0);
                    buttonStyleKey = "BtnStyleClassic";
                    break;

                case "Luna":
                    var lunaGrad = new LinearGradientBrush();
                    lunaGrad.StartPoint = new Point(0, 0); lunaGrad.EndPoint = new Point(0, 1);
                    lunaGrad.GradientStops.Add(new GradientStop(Color.FromRgb(0, 88, 238), 0.0));
                    lunaGrad.GradientStops.Add(new GradientStop(Color.FromRgb(0, 30, 160), 1.0));
                    winBg = lunaGrad; winFg = Brushes.White; winBorder = new Thickness(1); winRadius = new CornerRadius(10, 10, 0, 0);
                    buttonStyleKey = "BtnStyleLuna";
                    break;

                case "Royale":
                    var royaleGrad = new LinearGradientBrush();
                    royaleGrad.StartPoint = new Point(0, 0); royaleGrad.EndPoint = new Point(0, 1);
                    royaleGrad.GradientStops.Add(new GradientStop(Color.FromRgb(220, 245, 255), 0.0));
                    royaleGrad.GradientStops.Add(new GradientStop(Color.FromRgb(129, 198, 221), 0.2));
                    royaleGrad.GradientStops.Add(new GradientStop(Color.FromRgb(58, 147, 195), 1.0));
                    winBg = royaleGrad; winFg = Brushes.Black; winBorder = new Thickness(1); winRadius = new CornerRadius(8, 8, 0, 0);
                    buttonStyleKey = "BtnStyleLuna"; // Use Luna buttons
                    break;

                case "Aero":
                    var aeroGrad = new LinearGradientBrush();
                    aeroGrad.StartPoint = new Point(0, 0); aeroGrad.EndPoint = new Point(0, 1);
                    aeroGrad.GradientStops.Add(new GradientStop(Color.FromRgb(60, 60, 60), 0.0));
                    aeroGrad.GradientStops.Add(new GradientStop(Color.FromRgb(0, 0, 0), 1.0));
                    winBg = aeroGrad; winFg = Brushes.White; winBorder = new Thickness(1); winRadius = new CornerRadius(5);
                    buttonStyleKey = "BtnStyleAero";
                    break;

                default: // Metro/Default
                    winBg = Brushes.White; winFg = Brushes.Black; winBorder = new Thickness(1); winRadius = new CornerRadius(0);
                    buttonStyleKey = "BtnStyleAero";
                    break;
            }

            TitleBar.Background = winBg;
            TitleText.Foreground = winFg;
            WindowFrame.BorderThickness = winBorder;
            WindowFrame.CornerRadius = winRadius;

            // Apply Button Skin
            if (TitleBar.Children.Count > 2 && TitleBar.Children[2] is StackPanel buttonPanel)
            {
                Style selectedStyle = this.Resources[buttonStyleKey] as Style;
                if (selectedStyle != null)
                {
                    foreach (var child in buttonPanel.Children)
                    {
                        if (child is Button btn)
                        {
                            btn.Style = selectedStyle;
                            if ((theme == "Metro" || theme == "Default" || theme == "Royale") && buttonStyleKey == "BtnStyleAero")
                            {
                                btn.Foreground = Brushes.Black;
                                if (btn.Content.ToString() == "r") btn.Foreground = Brushes.White;
                            }
                        }
                    }
                }
            }
        }

        // Video
        private void BtnPlayStartup_Click(object sender, RoutedEventArgs e) => PlayVideo("start");
        private void BtnPlayShutdown_Click(object sender, RoutedEventArgs e) => PlayVideo("shutdown");
        private void BtnStopVideo_Click(object sender, RoutedEventArgs e)
        {
            if (BootPlayer != null) { BootPlayer.Stop(); BootPlayer.Source = null; }
            if (VideoStatusText != null) VideoStatusText.Visibility = Visibility.Visible;
        }
        private void PlayVideo(string type)
        {
            if (VideoStatusText != null) VideoStatusText.Visibility = Visibility.Collapsed;
            string fileName = $"{_currentThemeName}_{type}.mp4";
            string fullPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Videos", fileName);
            if (System.IO.File.Exists(fullPath)) { BootPlayer.Source = new Uri(fullPath); BootPlayer.Play(); }
            else { MessageBox.Show("Video missing: " + fileName); if (VideoStatusText != null) VideoStatusText.Visibility = Visibility.Visible; }
        }
    }
}