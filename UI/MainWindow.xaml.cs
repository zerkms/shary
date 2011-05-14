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

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Screenshots.Fullscreen _fullscreenWindow;
        private HotKey _sshHotkey;

        public MainWindow()
        {
            InitializeComponent();

            Loaded += (s, e) =>
                {
                    _sshHotkey = new HotKey(ModifierKeys.Alt | ModifierKeys.Shift, Keys.R, this);
                    _sshHotkey.HotKeyPressed += k => ShowFullscreenWindow();
                };
        }

        public void ShowFullscreenWindow()
        {
            if (_fullscreenWindow != null && _fullscreenWindow.IsLoaded) return;
                
            _fullscreenWindow = new Screenshots.Fullscreen();
            _fullscreenWindow.TakeScreenshot();
        }

        private void TakeScreenshotButton_Click(object sender, RoutedEventArgs e)
        {
            ShowFullscreenWindow();
        }
    }
}
