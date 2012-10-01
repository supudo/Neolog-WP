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

namespace Neolog.Utilities.Controls
{
    public partial class LoadingPopup : UserControl
    {
        public delegate void EventHandler(Object sender, NeologEventArgs e);
        public event EventHandler LoadingComplete;
        public event EventHandler LoadingError;

        Synchronization syncManager;

        private int nestID = 0;
        private string letter = "";

        public LoadingPopup()
        {
            InitializeComponent();

            if (this.syncManager == null)
                this.syncManager = new Synchronization();
            this.syncManager.SyncError += new Synchronization.EventHandler(syncManager_SyncError);
            this.syncManager.SyncComplete += new Synchronization.EventHandler(syncManager_SyncComplete);
        }

        public void StartLoading(int nid, string l)
        {
            this.nestID = nid;
            this.letter = l;

            if (this.nestID > 0)
                this.syncManager.DoGetWordsForNestInBackground(this.nestID);
            else
                this.syncManager.DoGetWordsForLetterInBackground(this.letter);
        }

        void syncManager_SyncComplete(object sender, NeologEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                LoadingComplete(this, new NeologEventArgs(e.IsError, e.ErrorMessage, e.XmlContent));
            });
        }

        void syncManager_SyncError(object sender, NeologEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                LoadingError(this, new NeologEventArgs(e.IsError, e.ErrorMessage, e.XmlContent));
            });
        }
    }
}
