using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using Neolog.Utilities;
using Neolog.Utilities.Controls;
using Neolog.Sync;

namespace Neolog.Views
{
    public partial class Words : NeologBasePage
    {
        public delegate void EventHandler(Object sender, NeologEventArgs e);
        public event EventHandler SyncComplete;
        public event EventHandler SyncError;

        private Popup popup;
        private SplashScreen splashScreen;

        Synchronization syncManager;

        private int nestID = 0;
        private string letter = "";

        public Words()
        {
            InitializeComponent();
            this.LayoutRoot.Background = new SolidColorBrush(AppSettings.BackgroundColor);
            this.pageTitle.Text = AppResources.menu_Words;
            this.Loaded += new RoutedEventHandler(Words_Loaded);
        }

        void Words_Loaded(object sender, RoutedEventArgs e)
        {
            base.BuildApplicationBar();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string n = "", l = "";
            if (NavigationContext.QueryString.TryGetValue("nid", out n) && NavigationContext.QueryString.TryGetValue("l", out l))
            {
                this.nestID = int.Parse(n);
                this.letter = l;
                if (this.syncManager == null)
                    this.syncManager = new Synchronization();
                this.syncManager.SyncComplete += new Synchronization.EventHandler(syncManager_SyncComplete);
                this.syncManager.SyncError += new Synchronization.EventHandler(syncManager_SyncError);
                if (this.nestID > 0)
                {
                    this.pageTitle.Text = App.DbViewModel.GetNest(this.nestID).Title;
                    this.syncManager.DoGetWordsForNestInBackground(this.nestID);
                }
                else
                {
                    this.pageTitle.Text = letter.ToUpper();
                    this.syncManager.DoGetWordsForLetterInBackground(this.letter);
                }
            }
        }

        void syncManager_SyncComplete(object sender, NeologEventArgs e)
        {
            if (this.nestID > 0)
                this.wordsList.ItemsSource = App.DbViewModel.GetWordsForNest(this.nestID);
            else
                this.wordsList.ItemsSource = App.DbViewModel.GetWordsForLetter(this.letter);
        }

        void syncManager_SyncError(object sender, NeologEventArgs e)
        {
            if (e.IsError)
                MessageBox.Show(e.ErrorMessage);
        }

        private void Words_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.wordsList.SelectedIndex == -1)
                return;
            int nid = ((Neolog.Database.Models.Word)e.AddedItems[0]).NestId;
            int wid = ((Neolog.Database.Models.Word)e.AddedItems[0]).WordId;
            NavigationService.Navigate(new Uri("/Views/WordDetails.xaml?nid=" + nid + "&wid=" + wid, UriKind.Relative));
            this.wordsList.SelectedIndex = -1;
        }
    }
}