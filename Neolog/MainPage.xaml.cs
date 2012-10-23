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
using System.Threading;
using System.ComponentModel;
using Neolog.Utilities;

namespace Neolog
{
    public partial class MainPage : NeologBasePage
    {
        public MainPage()
        {
            InitializeComponent();
            this.LayoutRoot.Background = new SolidColorBrush(AppSettings.BackgroundColor);
            base.SyncComplete += new NeologBasePage.EventHandler(syncComplete);
            base.SyncError += new EventHandler(syncComplete);
            base.DoSync();
        }

        void syncComplete(object sender, NeologEventArgs e)
        {
            base.BuildApplicationBar();
            NavigationService.Navigate(new Uri("/Views/NestsAndLetters.xaml", UriKind.Relative));
        }
    }
}