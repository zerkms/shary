using System.Windows;

namespace Screenshots
{
    /// <summary>
    /// Interaction logic for Background.xaml
    /// </summary>
    public partial class Background : Window
    {
        private const int backgroundPadding = 30;

        public Background()
        {
            InitializeComponent();
        }

        public void SetDimensions(int x, int y, int width, int height)
        {
            this.Width = width + backgroundPadding * 2;
            this.Height = height + backgroundPadding * 2;
            this.Top = y - backgroundPadding;
            this.Left = x - backgroundPadding;
        }
    }
}
