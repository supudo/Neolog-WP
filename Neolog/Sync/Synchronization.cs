﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Xml.Linq;
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
            ServiceOpTexts,
            ServiceOpNests,
            ServiceOpSendWord,
            ServiceOpWords
        }

        BackgroundWorker bgWorker;
        private int nestId;
        private string letter;

        #region Constructor
        public Synchronization()
        {
            this._networkHelper = new NetworkHelper();
            this._networkHelper.DownloadComplete += new NetworkHelper.EventHandler(_networkHelper_DownloadComplete);
            this._networkHelper.DownloadInBackgroundComplete +=new NetworkHelper.EventHandler(_networkHelper_DownloadInBackgroundComplete);
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
                case ServiceOp.ServiceOpNests:
                    this.doNests(xmlContent);
                    break;
                case ServiceOp.ServiceOpWords:
                    this.doWords(xmlContent);
                    break;
                default:
                    SyncComplete(this, new NeologEventArgs(false, "", ""));
                    break;
            }
        }
        #endregion

        #region Public
        public void DoSendWord(Dictionary<string, string> postParams)
        {
            this.currentOp = ServiceOp.ServiceOpSendWord;
            this._networkHelper.uploadURL(AppSettings.ServicesURL + "?action=SendWord", postParams);
        }

        public void DoGetWordsForNestInBackground(int nid)
        {
            this.nestId = nid;
            this.letter = "";
            this.bgWorker = new BackgroundWorker();
            RunProcess();
        }

        public void DoGetWordsForLetterInBackground(string letter)
        {
            this.nestId = 0;
            this.letter = letter;
            this.bgWorker = new BackgroundWorker();
            RunProcess();
        }

        private void RunProcess()
        {
            this.bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
            this.bgWorker.RunWorkerAsync();
        }

        void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (this.nestId > 0)
                this._networkHelper.downloadURL(AppSettings.ServicesURL + "?action=FetchWordsForNest&nestID=" + this.nestId, true);
            else
                this._networkHelper.downloadURL(AppSettings.ServicesURL + "?action=FetchWordsForLetter&letter=" + this.letter, true);
        }
        #endregion

        #region Handle URLs
        private void syncTexts()
        {
            this.currentOp = ServiceOp.ServiceOpTexts;
            this._networkHelper.downloadURL(AppSettings.ServicesURL + "?action=GetContent");
        }

        private void syncNests()
        {
            this.currentOp = ServiceOp.ServiceOpNests;
            this._networkHelper.downloadURL(AppSettings.ServicesURL + "?action=GetNests");
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

        void _networkHelper_DownloadInBackgroundComplete(object sender, NeologEventArgs e)
        {
            if (!e.IsError)
            {
                this.currentOp = ServiceOp.ServiceOpWords;
                this.dispatchDownload(e.XmlContent);
            }
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
            var ents = from ent in doc.Descendants("cnabout")
                       select new Texts
                       {
                           TextId = int.Parse(ent.Attribute("id").Value),
                           Title = "...", // seriously?!?!
                           Content = ent.Value
                       };
            using (NeologDataContext db = new NeologDataContext(AppSettings.DBConnectionString))
            {
                foreach (Texts t in ents)
                    App.DbViewModel.AddText(t);
            }
            this.syncNests();
        }

        private void doNests(string xmlContent)
        {
            XDocument doc = XDocument.Parse(xmlContent);
            var ents = from ent in doc.Descendants("nest")
                       select new Nests
                       {
                           NestId = int.Parse(ent.Attribute("id").Value),
                           OrderPos = int.Parse(ent.Attribute("ord").Value),
                           Title = ent.Element("nn").Value,
                       };
            using (NeologDataContext db = new NeologDataContext(AppSettings.DBConnectionString))
            {
                foreach (Nests t in ents)
                    App.DbViewModel.AddNest(t);
            }
            this.SynchronizationComplete();
        }

        private void doWords(string xmlContent)
        {
            XDocument doc = XDocument.Parse(xmlContent);
            var ents = from ent in doc.Descendants("wrd")
                       select new Words
                       {
                           WordId = int.Parse(ent.Attribute("id").Value),
                           NestId = int.Parse(ent.Element("wnid").Value),
                           CommentsCount = int.Parse(ent.Element("wcms").Value),
                           AddedBy = ent.Element("wnm").Value,
                           AddedByEmail = ent.Element("wem").Value,
                           AddedByUrl = ent.Element("wurl").Value,
                           Description = ent.Element("wdsc").Value,
                           Ethimology = ent.Element("wet").Value,
                           Example = ent.Element("wex").Value,
                           WordContent = ent.Element("wwrd").Value,
                           AddedAtDate = DateTime.ParseExact(ent.Element("wdt").Value + " 00:00:00", AppSettings.DateTimeFormat, null),
                       };
            using (NeologDataContext db = new NeologDataContext(AppSettings.DBConnectionString))
            {
                foreach (Words t in ents)
                    App.DbViewModel.AddWord(t);
            }
            this.SynchronizationComplete();
        }
        #endregion

        #region Parsers
        #endregion
    }
}
