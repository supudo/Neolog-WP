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
    public partial class Words : NeologBasePage
    {
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
            string n = "";
            if (NavigationContext.QueryString.TryGetValue("nid", out n))
            {
                int nid = int.Parse(n);
                this.wordsList.ItemsSource = App.DbViewModel.GetWords(nid);
            }
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