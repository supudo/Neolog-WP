using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Xml.Linq;
using System.Threading;
using Neolog.Database.Context;
using Neolog.Database.Tables;
using Neolog.Utilities.Network;
using Neolog.Utilities;

namespace Neolog.Sync
{
    public class Synchronization
    {
        public delegate void EventHandler(Object sender, NeologEventArgs e);
        public event EventHandler SyncComplete;
        public event EventHandler SyncError;

        NetworkHelper _networkHelper;

        private ServiceOp currentOp;
        enum ServiceOp
        {
            ServiceOpTexts
        }

        BackgroundWorker bgWorker;

        #region Constructor
        public Synchronization()
        {
            this._networkHelper = new NetworkHelper();
            this._networkHelper.DownloadComplete += new NetworkHelper.EventHandler(_networkHelper_DownloadComplete);
            this._networkHelper.DownloadError += new NetworkHelper.EventHandler(_networkHelper_DownloadError);
        }
        #endregion

        #region Dispatcher
        private void dispatchDownload(string xmlContent)
        {
            switch (this.currentOp)
            {
                case ServiceOp.ServiceOpTexts:
                    this.doTexts(xmlContent);
                    break;
                default:
                    SyncComplete(this, new NeologEventArgs(false, "", ""));
                    break;
            }
        }
        #endregion

        #region Public
        #endregion

        #region Handle URLs
        private void syncTexts()
        {
            this.currentOp = ServiceOp.ServiceOpTexts;
            this._networkHelper.InBackground = false;
            this._networkHelper.downloadURL(AppSettings.ServicesURL + "?action=getTextContent");
        }
        #endregion

        #region Events
        private void SynchronizationComplete()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                SyncComplete(this, new NeologEventArgs(false, "", ""));
            });
        }

        void _networkHelper_DownloadComplete(object sender, NeologEventArgs e)
        {
            if (!e.IsError)
                this.dispatchDownload(e.XmlContent);
        }

        void _networkHelper_DownloadError(object sender, NeologEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                SyncError(this, new NeologEventArgs(true, e.ErrorMessage, ""));
            });
        }
        #endregion

        #region Initial Synchronization
        public void StartSync()
        {
            this.syncTexts();
        }

        private void doTexts(string xmlContent)
        {
            XDocument doc = XDocument.Parse(xmlContent);
            var texts = from txt in doc.Descendants("tctxt")
                        select new Texts
                        {
                            TextId = int.Parse(txt.Attribute("id").Value),
                            Title = txt.Element("tctitle").Value,
                            Content = txt.Element("tccontent").Value,
                        };
            using (NeologDataContext db = new NeologDataContext(AppSettings.DBConnectionString))
            {
                foreach (Texts t in texts)
                    App.DbViewModel.AddText(t);
            }
            this.SynchronizationComplete();
        }
        #endregion

        #region Parsers
        #endregion
    }
}
