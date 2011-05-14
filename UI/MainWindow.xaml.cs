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

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Screenshots.Fullscreen _fullscreenWindow;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void ShowFullscreenWindow()
        {
            if (_fullscreenWindow != null && _fullscreenWindow.IsLoaded) return;

            var storage = new ClipboardStorage();
            _fullscreenWindow = new Screenshots.Fullscreen();
            _fullscreenWindow.Captured += (s, e) =>
            {
                storage.Store(e.CapturedImage);
            };

            _fullscreenWindow.TakeScreenshot();
        }

        private void TakeScreenshotButton_Click(object sender, RoutedEventArgs e)
        {
            ShowFullscreenWindow();
        }
    }
}
