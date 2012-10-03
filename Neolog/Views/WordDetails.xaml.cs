using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Shell;
using Neolog.Database.Models;
using Neolog.Sync;
using Neolog.Utilities;
using Neolog.Utilities.Controls;

namespace Neolog.Views
{
    public partial class WordDetails : NeologBasePage
    {
        private Synchronization syncManager;
        private Word currentWord;

        public WordDetails()
        {
            InitializeComponent();
            this.LayoutRoot.Background = new SolidColorBrush(AppSettings.BackgroundColor);
            this.pageTitle.Text = AppResources.appName;
            this.Loaded += new RoutedEventHandler(WordDetails_Loaded);

            if (this.syncManager == null)
                this.syncManager = new Synchronization();
            this.syncManager.SyncComplete +=new Synchronization.EventHandler(syncManager_SyncComplete);
            this.syncManager.SyncError +=new Synchronization.EventHandler(syncManager_SyncError);
        }

        void WordDetails_Loaded(object sender, RoutedEventArgs e)
        {
            this.BuildApplicationBar();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string nid = "", wid = "";
            if (NavigationContext.QueryString.TryGetValue("nid", out nid) && NavigationContext.QueryString.TryGetValue("wid", out wid))
            {
                int wordID = int.Parse(wid);
                int nestID = int.Parse(nid);
                this.syncManager.DoGetWordCommentsInBackground(wordID);
                this.currentWord = App.DbViewModel.GetWord(wordID);
                this.pageTitle.Text = this.currentWord.WordContent;

                this.txtDescription.Text = this.currentWord.Description;

                this.lblExample.Text = AppResources.word_Example;
                this.txtExample.Text = this.currentWord.Example;

                this.lblEthimology.Text = AppResources.word_Ethimology;
                this.txtEthimology.Text = this.currentWord.Ethimology;

                this.txtURL.Visibility = System.Windows.Visibility.Visible;
                if (!this.currentWord.AddedByUrl.Trim().Equals(""))
                {
                    this.txtURL.Content = this.currentWord.AddedByUrl;
                    this.txtURL.Click += new RoutedEventHandler(txtURL_Click);
                }
                else
                    this.txtURL.Visibility = System.Windows.Visibility.Collapsed;

                this.txtEmail.Visibility = System.Windows.Visibility.Visible;
                if (!this.currentWord.AddedByEmail.Trim().Equals(""))
                {
                    this.txtEmail.Content = this.currentWord.AddedByEmail;
                    this.txtEmail.Click += new RoutedEventHandler(txtEmail_Click);
                }
                else
                    this.txtEmail.Visibility = System.Windows.Visibility.Collapsed;

                this.txtAuthorAndDate.Text = this.currentWord.AddedBy + " @ " + AppSettings.DoLongDate(this.currentWord.AddedAtDate, false, true);
            }
            else
                NavigationService.Navigate(new Uri("/Views/Newest.xaml", UriKind.Relative));
        }

        void txtEmail_Click(object sender, RoutedEventArgs e)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();
            emailComposeTask.Subject = AppResources.appName;
            emailComposeTask.Body = " " + this.currentWord.WordContent + " \n";
            emailComposeTask.To = this.currentWord.AddedByEmail;
            emailComposeTask.Show();
        }

        void txtURL_Click(object sender, RoutedEventArgs e)
        {
            WebBrowserTask webbrowser = new WebBrowserTask();
            webbrowser.Uri = new Uri(this.currentWord.AddedByUrl);
            webbrowser.Show();
        }

        #region Sync
        void syncManager_SyncComplete(object sender, NeologEventArgs e)
        {
        }

        void syncManager_SyncError(object sender, NeologEventArgs e)
        {
        }
        #endregion

        #region Application bar
        private new void BuildApplicationBar()
        {
            this.ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("images/menu/tb-back.png", UriKind.Relative));
            appBarButton.Text = "back";
            appBarButton.Click += new System.EventHandler(back_Click);
            this.ApplicationBar.Buttons.Add(appBarButton);

            appBarButton = new ApplicationBarIconButton(new Uri("images/menu/tb-share.png", UriKind.Relative));
            appBarButton.Text = "share";
            appBarButton.Click += new System.EventHandler(share_Click);
            this.ApplicationBar.Buttons.Add(appBarButton);

            appBarButton = new ApplicationBarIconButton(new Uri("images/menu/tb-send-comment.png", UriKind.Relative));
            appBarButton.Text = AppResources.sendComment;
            appBarButton.Click += new System.EventHandler(word_sendComment);
            this.ApplicationBar.Buttons.Add(appBarButton);

            ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.comments);
            appBarMenuItem.Click += new System.EventHandler(word_viewComments);
            this.ApplicationBar.MenuItems.Add(appBarMenuItem);
        }

        private void back_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void share_Click(object sender, EventArgs e)
        {
            this.share();
        }

        private void shareFacebook_Click(object sender, EventArgs e)
        {
            this.shareFacebook();
        }

        private void shareTwitter_Click(object sender, EventArgs e)
        {
            this.shareTwitter();
        }

        private void word_viewComments(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/WordComments.xaml?wid=" + this.currentWord.WordId, UriKind.Relative));
        }

        private void word_sendComment(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/SendComment.xaml?wid=" + this.currentWord.WordId, UriKind.Relative));
        }
        #endregion

        #region Share
        private void share()
        {
            ShareLinkTask shareLinkTask = new ShareLinkTask();
            shareLinkTask.Title = this.currentWord.WordContent;
            shareLinkTask.LinkUri = new Uri("http://www.neolog.bg/word/" + this.currentWord.WordId, UriKind.Absolute);
            shareLinkTask.Message = this.currentWord.Description;
            shareLinkTask.Show();
        }
        #endregion

        #region Facebook
        private void shareFacebook()
        {
            NavigationService.Navigate(new Uri("/Views/ShareFacebook.xaml?wid=" + this.currentWord.WordId, UriKind.Relative));
        }
        #endregion

        #region Twitter
        private void shareTwitter()
        {
            NavigationService.Navigate(new Uri("/Views/ShareTwitter.xaml?wid=" + this.currentWord.WordId, UriKind.Relative));
        }
        #endregion
    }
}