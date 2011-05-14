using System.Windows;

namespace Screenshots
{
    /// <summary>
    /// Interaction logic for Background.xaml
    /// </summary>
    public partial class Background : Window
    {
        public Background()
        {
            InitializeComponent();
        }

        public void SetDimensions(int x, int y, int width, int height)
        {
            this.Width = width;
            this.Height = height;
            this.Top = y;
            this.Left = x;
        }
    }
}
