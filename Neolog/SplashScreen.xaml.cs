using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Neolog.Sync;
using Neolog.Utilities;

namespace Neolog
{
    public partial class SplashScreen : UserControl
    {
        public delegate void EventHandler(Object sender, NeologEventArgs e);
        public event EventHandler SplashComplete;
        public event EventHandler SplashError;

        private Synchronization syncManager;

        public SplashScreen()
        {
            InitializeComponent();
            this.txtLoading.Text = AppResources.loading;
            this.LayoutRoot.Background = new SolidColorBrush(AppSettings.BackgroundColor);

            if (this.syncManager == null)
                this.syncManager = new Synchronization();
            this.syncManager.SyncError += new Synchronization.EventHandler(syncManager_SyncError);
            this.syncManager.SyncComplete += new Synchronization.EventHandler(syncManager_SyncComplete);
        }

        public void startSync()
        {
            try
            {
                this.syncManager.StartSync();
            }
            catch
            {
                MessageBox.Show(AppResources.generalError);
            }
        }

        void syncManager_SyncComplete(object sender, NeologEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                SplashComplete(this, new NeologEventArgs(e.IsError, e.ErrorMessage, e.XmlContent));
            });
        }

        void syncManager_SyncError(object sender, NeologEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                SplashError(this, new NeologEventArgs(e.IsError, e.ErrorMessage, e.XmlContent));
            });
        }
    }
}