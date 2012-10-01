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
using Neolog.Sync;
using Neolog.Utilities;
using Neolog.Utilities.Extensions;

namespace Neolog.Views
{
    public partial class SendWord : NeologBasePage
    {
        private Synchronization syncManager;

        public SendWord()
        {
            InitializeComponent();
            this.LayoutRoot.Background = new SolidColorBrush(AppSettings.BackgroundColor);
            this.pageTitle.Text = AppResources.menu_Send;
            this.Loaded += new RoutedEventHandler(SendWord_Loaded);
        }

        void SendWord_Loaded(object sender, RoutedEventArgs e)
        {
            this.lblName.Text = AppResources.send_Name;
            this.lblEmail.Text = AppResources.send_Email;
            this.lblURL.Text = AppResources.send_URL;
            this.lblWord.Text = AppResources.send_Word;
            this.lblNest.Text = AppResources.send_Nest;
            this.ddNests.ItemsSource = App.DbViewModel.GetNests();
            this.ddNests.FullModeHeader = AppResources.send_Nest;
            this.lblDescription.Text = AppResources.send_Description;
            this.lblExample.Text = AppResources.send_Example;
            this.lblEthimology.Text = AppResources.send_Ethimology;
            if (AppSettings.ConfPrivateData)
            {
                this.txtEmail.Text = AppSettings.ConfPDEmail;
                this.txtName.Text = AppSettings.ConfPDName;
                this.txtURL.Text = AppSettings.ConfPDURL;
            }
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/NestsAndLetters.xaml", UriKind.Relative));
        }

        private void post_Click(object sender, EventArgs e)
        {
            string name = this.txtName.Text;
            string email = this.txtEmail.Text;
            string url = this.txtURL.Text;
            string word = this.txtWord.Text;
            string description = this.txtDescription.Text;
            string example = this.txtExample.Text;
            string ethimology = this.txtEthimology.Text;

            int nestId = ((Neolog.Database.Models.Nest)this.ddNests.SelectedItem).NestId;
            
            bool hasError = true;
            if (name.Trim().Equals(""))
                this.txtName.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            else if (description.Trim().Equals(""))
                this.txtDescription.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            else if (word.Trim().Equals(""))
                this.txtWord.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            else if (example.Trim().Equals(""))
                this.txtExample.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            else
                hasError = false;

            if (hasError)
                MessageBox.Show(AppResources.missingFields);
            else
            {
                if (this.syncManager == null)
                    this.syncManager = new Synchronization();
                this.syncManager.SyncError += new Synchronization.EventHandler(syncManager_SyncError);
                this.syncManager.SyncComplete += new Synchronization.EventHandler(syncManager_SyncComplete);

                Dictionary<string, string> postParams = new Dictionary<string, string>();
                postParams.Add("added_by", name);
                postParams.Add("added_by_email", email);
                postParams.Add("added_by_url", url);
                postParams.Add("word", word);
                postParams.Add("nest", "" + nestId);
                postParams.Add("word_desc", description);
                postParams.Add("example", example);
                postParams.Add("ethimology", ethimology);
                this.syncManager.DoSendWord(postParams);

                AppSettings.ConfPDEmail = "";
                AppSettings.ConfPDName = "";
                AppSettings.ConfPDURL = "";
                if (AppSettings.ConfPrivateData)
                {
                    AppSettings.ConfPDEmail = email;
                    AppSettings.ConfPDName = name;
                    AppSettings.ConfPDURL = url;
                }
            }
        }

        void syncManager_SyncComplete(object sender, NeologEventArgs e)
        {
            MessageBox.Show(AppResources.thankYou);
            this.txtName.Text = "";
            this.txtEmail.Text = "";
            this.txtURL.Text = "";
            this.txtWord.Text = "";
            this.ddNests.SelectedIndex = 0;
            this.txtDescription.Text = "";
            this.txtExample.Text = "";
            this.txtEthimology.Text = "";
        }

        void syncManager_SyncError(object sender, NeologEventArgs e)
        {
            if (e.IsError)
                MessageBox.Show(e.ErrorMessage);
        }
    }
}