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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Neolog.Sync;
using Neolog.Utilities;

namespace Neolog.Views
{
    public partial class SendComment : NeologBasePage
    {
        private Synchronization syncManager;
        private int wordID;

        public SendComment()
        {
            InitializeComponent();
            this.LayoutRoot.Background = new SolidColorBrush(AppSettings.BackgroundColor);
            this.pageTitle.Text = AppResources.appName;
            this.lblComment.Text = AppResources.your_Comment;
            this.lblAuthor.Text = AppResources.your_CommentAuhor;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string wid = "";
            if (NavigationContext.QueryString.TryGetValue("wid", out wid))
                this.wordID = int.Parse(wid);
            else
                NavigationService.GoBack();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void post_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.syncManager == null)
                    this.syncManager = new Synchronization();
                this.syncManager.SyncError += new Synchronization.EventHandler(syncManager_SyncError);
                this.syncManager.SyncComplete += new Synchronization.EventHandler(syncManager_SyncComplete);

                if (!this.txtAuthor.Text.Trim().Equals("") && !this.txtComment.Text.Trim().Equals(""))
                {
                    Dictionary<string, string> postParams = new Dictionary<string, string>();
                    postParams.Add("w", "" + this.wordID);
                    postParams.Add("author", this.txtAuthor.Text);
                    postParams.Add("comment", this.txtComment.Text);
                    this.syncManager.DoSendComment(postParams);
                }
                else
                    MessageBox.Show(AppResources.comment_Empty);
            }
            catch
            {
                MessageBox.Show(AppResources.generalError);
            }
        }

        void syncManager_SyncComplete(object sender, NeologEventArgs e)
        {
            NavigationService.GoBack();
        }

        void syncManager_SyncError(object sender, NeologEventArgs e)
        {
            try
            {
                if (e.IsError)
                    MessageBox.Show(e.ErrorMessage);
            }
            catch
            {
                MessageBox.Show(AppResources.generalError);
            }
        }
    }
}