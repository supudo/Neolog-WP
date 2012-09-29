using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Neolog.Database.Tables
{
    [Table]
    public class WordComments : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private int _id;
        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    NotifyPropertyChanging("Id");
                    _id = value;
                    NotifyPropertyChanged("Id");
                }
            }
        }

        private int _wordCommentId;
        [Column]
        public int WordCommentId
        {
            get { return _wordCommentId; }
            set
            {
                if (_wordCommentId != value)
                {
                    NotifyPropertyChanging("WordCommentId");
                    _wordCommentId = value;
                    NotifyPropertyChanged("WordCommentId");
                }
            }
        }

        private int _wordId;
        [Column]
        public int WordId
        {
            get { return _wordId; }
            set
            {
                if (_wordId != value)
                {
                    NotifyPropertyChanging("WordId");
                    _wordId = value;
                    NotifyPropertyChanged("WordId");
                }
            }
        }

        private string _comment;
        [Column]
        public string Comment
        {
            get { return _comment; }
            set
            {
                if (_comment != value)
                {
                    NotifyPropertyChanging("Comment");
                    _comment = value;
                    NotifyPropertyChanged("Comment");
                }
            }
        }

        private string _author;
        [Column]
        public string Author
        {
            get { return _author; }
            set
            {
                if (_author != value)
                {
                    NotifyPropertyChanging("Author");
                    _author = value;
                    NotifyPropertyChanged("Author");
                }
            }
        }

        private DateTime _commentDate;
        [Column]
        public DateTime CommentDate
        {
            get { return _commentDate; }
            set
            {
                if (_commentDate != value)
                {
                    NotifyPropertyChanging("CommentDate");
                    _commentDate = value;
                    NotifyPropertyChanged("CommentDate");
                }
            }
        }

        [Column(IsVersion = true)]
        private Binary _version;

        private int _refWordId;
        [Column]
        public int RefWordId
        {
            get { return _refWordId; }
            set
            {
                if (_refWordId != value)
                {
                    NotifyPropertyChanging("RefWordId");
                    _refWordId = value;
                    NotifyPropertyChanged("RefWordId");
                }
            }
        }

        private EntityRef<Words> _word;
        [Association(Storage = "_word", ThisKey = "RefWordId", OtherKey = "Id", IsForeignKey = true)]
        public Words Word
        {
            get { return _word.Entity; }
            set
            {
                NotifyPropertyChanging("Word");
                _word.Entity = value;
                if (value != null)
                    _wordId = value.WordId;
                NotifyPropertyChanging("Word");
            }
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region INotifyPropertyChanging Members
        public event PropertyChangingEventHandler PropertyChanging;
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
        }
        #endregion
    }
}
