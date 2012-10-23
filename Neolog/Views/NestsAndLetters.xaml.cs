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
using Neolog.Utilities;

namespace Neolog.Views
{
    public partial class NestsAndLetters : NeologBasePage
    {
        public NestsAndLetters()
        {
            InitializeComponent();
            this.LayoutRoot.Background = new SolidColorBrush(AppSettings.BackgroundColor);
            this.Loaded += new RoutedEventHandler(Nests_Loaded);
        }

        void Nests_Loaded(object sender, RoutedEventArgs e)
        {
            base.BuildApplicationBar();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.nlPanorama.Title = AppResources.appName;
            this.panNests.Header = AppResources.menu_Nests;
            this.panLetters.Header = AppResources.menu_Letters;
            this.listNests.ItemsSource = App.DbViewModel.GetNests();
            this.listLetters.ItemsSource = AppSettings.CyrillicLetters;
        }

        private void Nests_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.listNests.SelectedIndex == -1)
                return;
            int nid = ((Neolog.Database.Models.Nest)e.AddedItems[0]).NestId;
            NavigationService.Navigate(new Uri("/Views/Words.xaml?l=&nid=" + nid, UriKind.Relative));
            this.listNests.SelectedIndex = -1;
        }

        private void Letters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.listLetters.SelectedIndex == -1)
                return;
            NavigationService.Navigate(new Uri("/Views/Words.xaml?nid=0&l=" + AppSettings.CyrillicLetters[this.listLetters.SelectedIndex], UriKind.Relative));
            this.listLetters.SelectedIndex = -1;
        }
    }
}