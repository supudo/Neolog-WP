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
    public partial class SendWord : NeologBasePage
    {
        public SendWord()
        {
            InitializeComponent();
            this.LayoutRoot.Background = new SolidColorBrush(AppSettings.BackgroundColor);
            this.pageTitle.Text = AppResources.menu_Send;
            this.Loaded += new RoutedEventHandler(SendWord_Loaded);
        }

        void SendWord_Loaded(object sender, RoutedEventArgs e)
        {
            base.BuildApplicationBar();
        }
    }
}