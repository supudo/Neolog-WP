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
    public partial class WordComments : NeologBasePage
    {
        public WordComments()
        {
            InitializeComponent();
            this.LayoutRoot.Background = new SolidColorBrush(AppSettings.BackgroundColor);
            this.pageTitle.Text = AppResources.appName;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string wid = "";
            if (NavigationContext.QueryString.TryGetValue("wid", out wid))
            {
                int wordID = int.Parse(wid);
            }
        }

        private void back_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void sendComment_Click(object sender, EventArgs e)
        {
        }
    }
}