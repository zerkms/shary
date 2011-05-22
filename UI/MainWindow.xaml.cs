using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Runtime.InteropServices;
using System.ComponentModel;

using Hotkeys;
using System.Windows.Forms;
using Storages;

using Configuration;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Screenshots.Fullscreen _fullscreenWindow;
        private Config _config;

        public MainWindow()
        {
            InitializeComponent();
            _config = ((App)App.Current).Config;
        }

        public void SetConfig(Config config)
        {
            _config = config;
        }

        public void ShowFullscreenWindow()
        {
            if (_fullscreenWindow != null && _fullscreenWindow.IsLoaded) return;

            var factory = new Factory();
            var storage = factory.Storage(_config);

            _fullscreenWindow = new Screenshots.Fullscreen();
            _fullscreenWindow.Captured += (s, e) =>
            {
                storage.Store(e.CapturedImage);
                image1.Source = e.CapturedImage;
            };

            _fullscreenWindow.TakeScreenshot();
        }

        private void TakeScreenshotButton_Click(object sender, RoutedEventArgs e)
        {
            ShowFullscreenWindow();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.Background = new System.Windows.Media.SolidColorBrush(Colors.Green);
        }
    }
}
