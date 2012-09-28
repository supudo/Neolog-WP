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
            base.SyncComplete += new NeologBasePage.EventHandler(MainPage_SyncComplete);
            base.DoSync();
        }

        void MainPage_SyncComplete(object sender, NeologEventArgs e)
        {
            base.BuildApplicationBar();
            NavigationService.Navigate(new Uri("/Views/Nests.xaml", UriKind.Relative));
        }
    }
}