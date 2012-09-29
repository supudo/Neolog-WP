using System;

namespace Neolog.Database.Models
{
    public class Word
    {
        private int _id;
        public int Id
        {
            get { return this._id; }
            set { this._id = value; }
        }

        private int _wordId;
        public int WordId
        {
            get { return this._wordId; }
            set { this._wordId = value; }
        }

        private int _nestId;
        public int NestId
        {
            get { return this._nestId; }
            set { this._nestId = value; }
        }

        private int _commentsCount;
        public int CommentsCount
        {
            get { return this._commentsCount; }
            set { this._commentsCount = value; }
        }

        private string _example;
        public string Example
        {
            get { return this._example; }
            set { this._example = value; }
        }

        private string _ethimology;
        public string Ethimology
        {
            get { return this._ethimology; }
            set { this._ethimology = value; }
        }

        private string _addedBy;
        public string AddedBy
        {
            get { return this._addedBy; }
            set { this._addedBy = value; }
        }

        private string _addedByUrl;
        public string AddedByUrl
        {
            get { return this._addedByUrl; }
            set { this._addedByUrl = value; }
        }

        private string _addedByEmail;
        public string AddedByEmail
        {
            get { return this._addedByEmail; }
            set { this._addedByEmail = value; }
        }

        private string _wordContent;
        public string WordContent
        {
            get { return this._wordContent; }
            set { this._wordContent = value; }
        }

        private string _description;
        public string Description
        {
            get { return this._description; }
            set { this._description = value; }
        }

        private string _derivatives;
        public string Derivatives
        {
            get { return this._derivatives; }
            set { this._derivatives = value; }
        }

        private DateTime _addedAtDate;
        public DateTime AddedAtDate
        {
            get { return this._addedAtDate; }
            set { this._addedAtDate = value; }
        }
    }
}
