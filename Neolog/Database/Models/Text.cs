using System;

namespace Neolog.Database.Models
{
    public class Text
    {
        private int _id;
        public int Id
        {
            get { return this._id; }
            set { this._id = value; }
        }

        private int _textId;
        public int TextId
        {
            get { return this._textId; }
            set { this._textId = value; }
        }

        private string _title;
        public string Title
        {
            get { return this._title; }
            set { this._title = value; }
        }

        private string _content;
        public string Content
        {
            get { return this._content; }
            set { this._content = value; }
        }  
    }
}
