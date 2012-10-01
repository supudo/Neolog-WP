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
using Neolog.Utilities.Twitter;
using Hammock;
using Hammock.Authentication.OAuth;
using Hammock.Web;

namespace Neolog.Views
{
    public partial class ShareTwitter : NeologBasePage
    {
        public ShareTwitter()
        {
            InitializeComponent();
        }

        private void wbTwitter_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void wbTwitter_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
        }
    }
}