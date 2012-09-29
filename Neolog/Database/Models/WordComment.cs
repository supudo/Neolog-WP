using System;

namespace Neolog.Database.Models
{
    public class WordComment
    {
        private int _id;
        public int Id
        {
            get { return this._id; }
            set { this._id = value; }
        }

        private int _wordCommentId;
        public int WordCommentId
        {
            get { return this._wordCommentId; }
            set { this._wordCommentId = value; }
        }

        private int _wordId;
        public int WordId
        {
            get { return this._wordId; }
            set { this._wordId = value; }
        }

        private string _comment;
        public string Comment
        {
            get { return this._comment; }
            set { this._comment = value; }
        }

        private string _author;
        public string Author
        {
            get { return this._author; }
            set { this._author = value; }
        }

        private DateTime _commentDate;
        public DateTime CommentDate
        {
            get { return this._commentDate; }
            set { this._commentDate = value; }
        }
    }
}
