using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Threading;
using System.IO;

using Configuration;

using SWF = System.Windows.Forms;

namespace UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string APPLICATION_NAME = "Shary";

        private HotKey _sshHotkey;
        private System.Windows.Forms.NotifyIcon _niNotifyIcon;
        private System.Windows.Forms.ContextMenuStrip _mnuStripMenu;
        private Mutex _singleInstanceMutex;

        private Config _config;
        public Config Config { get { return _config; } }

        public App()
        {
            if (SingleInstanceCheck())
            {
                InitializeConfig();
                InitializeHotKeys();
                InitializeComponent();
                InitializeTrayIcon();
            }
        }

        private void InitializeConfig()
        {
            _config = new Config(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), APPLICATION_NAME));
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
            var kc = new SWF.KeysConverter();
            var mkc = new ModifierKeysConverter();

            _sshHotkey = new HotKey((ModifierKeys)mkc.ConvertFromString(_config.Hotkeys.Select.Modifiers), (SWF.Keys)kc.ConvertFromString(_config.Hotkeys.Select.Key));
            _sshHotkey.HotKeyPressed += k => ((MainWindow)this.MainWindow).ShowFullscreenWindow();

            Exit += (s, e) =>
            {
                _sshHotkey.Dispose();
            };
        }

        private bool SingleInstanceCheck()
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

            return hasSignal;
        }
    }
}
