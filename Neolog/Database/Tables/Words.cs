using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Neolog.Database.Tables
{
    [Table]
    public class Words : INotifyPropertyChanged, INotifyPropertyChanging
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

        private int _nestId;
        [Column]
        public int NestId
        {
            get { return _nestId; }
            set
            {
                if (_nestId != value)
                {
                    NotifyPropertyChanging("NestId");
                    _nestId = value;
                    NotifyPropertyChanged("NestId");
                }
            }
        }

        private int _commentsCount;
        [Column]
        public int CommentsCount
        {
            get { return _commentsCount; }
            set
            {
                if (_commentsCount != value)
                {
                    NotifyPropertyChanging("CommentsCount");
                    _commentsCount = value;
                    NotifyPropertyChanged("CommentsCount");
                }
            }
        }

        private string _example;
        [Column]
        public string Example
        {
            get { return _example; }
            set
            {
                if (_example != value)
                {
                    NotifyPropertyChanging("Example");
                    _example = value;
                    NotifyPropertyChanged("Example");
                }
            }
        }

        private string _ethimology;
        [Column]
        public string Ethimology
        {
            get { return _ethimology; }
            set
            {
                if (_ethimology != value)
                {
                    NotifyPropertyChanging("Ethimology");
                    _ethimology = value;
                    NotifyPropertyChanged("Ethimology");
                }
            }
        }

        private string _addedBy;
        [Column]
        public string AddedBy
        {
            get { return _addedBy; }
            set
            {
                if (_addedBy != value)
                {
                    NotifyPropertyChanging("AddedBy");
                    _addedBy = value;
                    NotifyPropertyChanged("AddedBy");
                }
            }
        }

        private string _addedByUrl;
        [Column]
        public string AddedByUrl
        {
            get { return _addedByUrl; }
            set
            {
                if (_addedByUrl != value)
                {
                    NotifyPropertyChanging("AddedByUrl");
                    _addedByUrl = value;
                    NotifyPropertyChanged("AddedByUrl");
                }
            }
        }

        private string _addedByEmail;
        [Column]
        public string AddedByEmail
        {
            get { return _addedByEmail; }
            set
            {
                if (_addedByEmail != value)
                {
                    NotifyPropertyChanging("AddedByEmail");
                    _addedByEmail = value;
                    NotifyPropertyChanged("AddedByEmail");
                }
            }
        }

        private string _wordContent;
        [Column]
        public string WordContent
        {
            get { return _wordContent; }
            set
            {
                if (_wordContent != value)
                {
                    NotifyPropertyChanging("WordContent");
                    _wordContent = value;
                    NotifyPropertyChanged("WordContent");
                }
            }
        }

        private string _description;
        [Column]
        public string Description
        {
            get { return _description; }
            set
            {
                if (_wordContent != value)
                {
                    NotifyPropertyChanging("Description");
                    _description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }

        private string _derivatives;
        [Column]
        public string Derivatives
        {
            get { return _derivatives; }
            set
            {
                if (_wordContent != value)
                {
                    NotifyPropertyChanging("Derivatives");
                    _derivatives = value;
                    NotifyPropertyChanged("Derivatives");
                }
            }
        }

        private DateTime _addedAtDate;
        [Column]
        public DateTime AddedAtDate
        {
            get { return _addedAtDate; }
            set
            {
                if (_addedAtDate != value)
                {
                    NotifyPropertyChanging("AddedAtDate");
                    _addedAtDate = value;
                    NotifyPropertyChanged("AddedAtDate");
                }
            }
        }

        [Column(IsVersion = true)]
        private Binary _version;

        private EntitySet<WordComments> _wordComments;
        [Association(Storage = "_wordComments", OtherKey = "RefWordId", ThisKey = "Id")]
        public EntitySet<WordComments> WordComments
        {
            get { return this._wordComments; }
            set { this._wordComments.Assign(value); }
        }

        public Words()
        {
            _wordComments = new EntitySet<WordComments>(
                new Action<WordComments>(this.attach_WordComments),
                new Action<WordComments>(this.detach_WordComments)
                );
        }

        private void attach_WordComments(WordComments ent)
        {
            NotifyPropertyChanging("WordComments");
            ent.Word = this;
        }

        private void detach_WordComments(WordComments ent)
        {
            NotifyPropertyChanging("WordComments");
            ent.Word = null;
        }

        private int _refNestId;
        [Column]
        public int RefNestId
        {
            get { return _refNestId; }
            set
            {
                if (_refNestId != value)
                {
                    NotifyPropertyChanging("RefNestId");
                    _refNestId = value;
                    NotifyPropertyChanged("RefNestId");
                }
            }
        }

        private EntityRef<Nests> _nest;
        [Association(Storage = "_nest", ThisKey = "RefNestId", OtherKey = "Id", IsForeignKey = true)]
        public Nests Nest
        {
            get { return _nest.Entity; }
            set
            {
                NotifyPropertyChanging("Nest");
                _nest.Entity = value;
                if (value != null)
                    _wordId = value.NestId;
                NotifyPropertyChanging("Nest");
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
