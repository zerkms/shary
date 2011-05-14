using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Threading;

namespace UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private HotKey _sshHotkey;
        private System.Windows.Forms.NotifyIcon _niNotifyIcon;
        private System.Windows.Forms.ContextMenuStrip _mnuStripMenu;
        private Mutex _singleInstanceMutex;

        public App()
        {
            SingleInstanceCheck();
            InitializeHotKeys();
            InitializeComponent();
            InitializeTrayIcon();
        }

        private void InitializeTrayIcon()
        {
            InitializeNotifyMenu();
            _niNotifyIcon = new System.Windows.Forms.NotifyIcon();
            _niNotifyIcon.Text = "Shary";
            _niNotifyIcon.BalloonTipTitle = "Shary";
            _niNotifyIcon.Visible = true;
            _niNotifyIcon.ContextMenuStrip = _mnuStripMenu;
            _niNotifyIcon.Icon = UI.Properties.Resources.Icon;

            Exit += (s, e) =>
            {
                HideNotifyIcon();
            };
        }

        private void InitializeNotifyMenu()
        {
            _mnuStripMenu = new System.Windows.Forms.ContextMenuStrip();

            _mnuStripMenu.Items.Add(new System.Windows.Forms.ToolStripMenuItem("Exit", null, (s, e) =>
            {
                Application.Current.Shutdown();
            }));
        }

        private void HideNotifyIcon()
        {
            _niNotifyIcon.Visible = false;
        }

        private void InitializeHotKeys()
        {
            _sshHotkey = new HotKey(ModifierKeys.Alt | ModifierKeys.Shift, System.Windows.Forms.Keys.R);
            _sshHotkey.HotKeyPressed += k => ((MainWindow)this.MainWindow).ShowFullscreenWindow();

            Exit += (s, e) =>
            {
                _sshHotkey.Dispose();
            };
        }

        private void SingleInstanceCheck()
        {
            _singleInstanceMutex = new Mutex(false, "Shary");

            var hasSignal = _singleInstanceMutex.WaitOne(1000, false);

            if (hasSignal)
            {
                Exit += (s, e) =>
                {
                    _singleInstanceMutex.ReleaseMutex();
                    _singleInstanceMutex.Dispose();
                };
            }
            else
            {
                MessageBox.Show("Shary is already running", "Shary", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }
    }
}
