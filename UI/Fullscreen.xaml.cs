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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace UI
{
    /// <summary>
    /// Interaction logic for Fullscreen.xaml
    /// </summary>
    public partial class Fullscreen : Window
    {
        private DispatcherTimer timer;

        public Fullscreen()
        {
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            timer.Tick += Tick;
            InitializeComponent();

            ForegroundRect.Width = SystemParameters.PrimaryScreenWidth;
            ForegroundRect.Height = SystemParameters.PrimaryScreenHeight;
        }

        public void SetImage(BitmapSource img)
        {
            ScreenshotImage.Source = img;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timer.IsEnabled = true;
        }

        private void Tick(object sender, EventArgs e)
        {
            ForegroundRect.Opacity += 0.05;

            if (ForegroundRect.Opacity >= 0.7)
            {
                timer.IsEnabled = false;
                ScreenshotImage.Visibility = Visibility.Visible;
            }
        }
    }
}
