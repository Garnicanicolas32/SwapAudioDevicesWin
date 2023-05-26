using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using AudioSwitcher.AudioApi.CoreAudio;

namespace SwapAudioTrack
{
    public partial class MainWindow : Window
    {
        private CoreAudioController controller = new CoreAudioController();
        private NotifyIcon notifyIcon;

        public MainWindow()
        {
            InitializeComponent();
            WindowState = WindowState.Minimized;

            InitializeNotifyIcon();

            Hide();
        }

        private void InitializeNotifyIcon()
        {
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = new System.Drawing.Icon("YourIconFile.ico");
            notifyIcon.Text = "Audio Device App";

            
            notifyIcon.ContextMenuStrip = CreateContextMenu();
            notifyIcon.Visible = true;

            notifyIcon.MouseClick += NotifyIcon_MouseClick;
        }

        private void NotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                notifyIcon.ContextMenuStrip = CreateContextMenu();
            }
        }

        private ContextMenuStrip CreateContextMenu()
        {
            var contextMenu = new ContextMenuStrip();
            List<CoreAudioDevice> audioDevices = controller.GetPlaybackDevices(AudioSwitcher.AudioApi.DeviceState.Active).ToList();

            foreach (var device in audioDevices)
            {
                var menuItem = new ToolStripMenuItem(device.Name);
                menuItem.Click += (sender, e) => controller.DefaultPlaybackDevice = device;
                contextMenu.Items.Add(menuItem);
            }

            var exitMenuItem = new ToolStripMenuItem("Exit");
            exitMenuItem.ForeColor = System.Drawing.Color.Red;
            exitMenuItem.Click += (sender, e) => ExitApplication();
            contextMenu.Items.Add(exitMenuItem);

            return contextMenu;
        }


        private void ExitApplication()
        {
            notifyIcon.Visible = false;
            notifyIcon.Dispose();
            System.Windows.Application.Current.Shutdown();
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
                Hide();

            base.OnStateChanged(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            ExitApplication();
            base.OnClosing(e);
        }
    }
}
