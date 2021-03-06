﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Neolog.Utilities
{
    public class NeologEventArgs : EventArgs
    {
        private AppSettings.ServiceOp serviceOp;
        private bool isError;
        private string errorMessage;
        private string xmlContent;

        public NeologEventArgs(bool ise, string eMsg, string xml)
        {
            this.isError = ise;
            this.errorMessage = eMsg;
            this.xmlContent = xml;
        }

        public NeologEventArgs(bool ise, string eMsg, string xml, AppSettings.ServiceOp sOp)
        {
            this.isError = ise;
            this.errorMessage = eMsg;
            this.xmlContent = xml;
            this.serviceOp = sOp;
        }

        public bool IsError
        {
            get { return this.isError; }
        }

        public string ErrorMessage
        {
            get { return this.errorMessage; }
        }

        public string XmlContent
        {
            get { return this.xmlContent; }
        }

        public AppSettings.ServiceOp ServiceOp
        {
            get { return this.serviceOp; }
        }
    }
}
