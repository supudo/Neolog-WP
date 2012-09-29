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
using Microsoft.Phone.Controls;
using Neolog.Utilities;
using Neolog.Utilities.Extensions;

namespace Neolog.Views
{
    public partial class Nests : NeologBasePage
    {
        public Nests()
        {
            InitializeComponent();
            this.LayoutRoot.Background = new SolidColorBrush(AppSettings.BackgroundColor);
            this.pageTitle.Text = AppResources.menu_Nests;
            this.Loaded += new RoutedEventHandler(Nests_Loaded);
        }

        void Nests_Loaded(object sender, RoutedEventArgs e)
        {
            base.BuildApplicationBar();
            this.nestsList.ItemsSource = App.DbViewModel.GetNests();
        }

        private void Nests_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.nestsList.SelectedIndex == -1)
                return;
            //int oid = ((BombaJob.Database.Tables.JobOffers)e.AddedItems[0]).OfferId;
            //bool humanYn = ((BombaJob.Database.Tables.JobOffers)e.AddedItems[0]).HumanYn;
            //NavigationService.Navigate(new Uri("/Views/OfferDetails.xaml?oid=" + oid + "&h=" + humanYn, UriKind.Relative));
            this.nestsList.SelectedIndex = -1;
        }
    }
}