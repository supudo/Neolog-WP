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

namespace Neolog.Views
{
    public partial class Settings : NeologBasePage
    {
        public Settings()
        {
            InitializeComponent();
            this.LayoutRoot.Background = new SolidColorBrush(AppSettings.BackgroundColor);
            this.pageTitle.Text = AppResources.menu_Settings;
            this.Loaded += new RoutedEventHandler(Settings_Loaded);
        }

        void Settings_Loaded(object sender, RoutedEventArgs e)
        {
            this.chkPrivateData.Content = AppResources.conf_PrivateData;
            this.chkPrivateData.IsChecked = AppSettings.ConfPrivateData;

            this.chkWordSync.Content = AppResources.conf_WordSync;
            this.chkWordSync.IsChecked = AppSettings.ConfWordSync;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            AppSettings.ConfPrivateData = (bool)this.chkPrivateData.IsChecked;
            AppSettings.ConfWordSync = (bool)this.chkWordSync.IsChecked;

            if (!AppSettings.ConfPrivateData)
            {
                AppSettings.ConfPDEmail = "";
                AppSettings.ConfPDName = "";
            }

            NavigationService.GoBack();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}