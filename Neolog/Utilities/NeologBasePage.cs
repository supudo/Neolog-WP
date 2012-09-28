using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Neolog.Utilities
{
    public class NeologBasePage : PhoneApplicationPage
    {
        public delegate void EventHandler(Object sender, NeologEventArgs e);
        public event EventHandler SyncComplete;
        public event EventHandler SyncError;

        private Popup popup;
        private SplashScreen splashScreen;

        public NeologBasePage()
        {
            SupportedOrientations = SupportedPageOrientation.Portrait | SupportedPageOrientation.Landscape;
        }

        public void DoSync()
        {
            if (this.ApplicationBar != null)
                this.ApplicationBar.IsVisible = false;
            this.ShowSyncPopup();
        }

        private void ShowSyncPopup()
        {
            if (this.splashScreen == null)
                this.splashScreen = new SplashScreen();
            this.splashScreen.SplashError += new SplashScreen.EventHandler(splashScreen_SplashError);
            this.splashScreen.SplashComplete += new SplashScreen.EventHandler(splashScreen_SplashComplete);

            this.popup = new Popup();
            this.popup.Child = this.splashScreen;
            this.popup.IsOpen = true;
            this.splashScreen.startSync();
        }

        void splashScreen_SplashComplete(object sender, NeologEventArgs e)
        {
            this.Dispatcher.BeginInvoke(() =>
            {
                this.popup.IsOpen = false;
                if (this.ApplicationBar != null)
                    this.ApplicationBar.IsVisible = true;
                try
                {
                    SyncComplete(this, new NeologEventArgs(e.IsError, "", ""));
                }
                catch { }
            });
        }

        void splashScreen_SplashError(object sender, NeologEventArgs e)
        {
            if (e.IsError)
                MessageBox.Show(e.ErrorMessage);
            this.ApplicationBar.IsVisible = true;
            this.Dispatcher.BeginInvoke(() =>
            {
                SyncError(this, new NeologEventArgs(e.IsError, e.ErrorMessage, ""));
            });
        }

        public void BuildApplicationBar()
        {
            this.ApplicationBar = new ApplicationBar();

            /*
            ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("images/menu/tb-newest.png", UriKind.Relative));
            appBarButton.Text = AppResources.menu_Newest;
            appBarButton.Click += new System.EventHandler(menuNewest_Click);
            this.ApplicationBar.Buttons.Add(appBarButton);

            ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.menu_Post);
            appBarMenuItem.Click += new System.EventHandler(menuPost_Click);
            this.ApplicationBar.MenuItems.Add(appBarMenuItem);
             * */
        }

        void menuNewest_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Newest.xaml", UriKind.Relative));
        }

        void menuPost_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Post.xaml", UriKind.Relative));
        }
    }
}
