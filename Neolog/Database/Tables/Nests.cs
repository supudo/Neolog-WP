using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Neolog.Database.Tables
{
    [Table]
    public class Nests : INotifyPropertyChanged, INotifyPropertyChanging
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

        private string _title;
        [Column]
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    NotifyPropertyChanging("Title");
                    _title = value;
                    NotifyPropertyChanged("Title");
                }
            }
        }

        private int _orderPos;
        [Column]
        public int OrderPos
        {
            get { return _orderPos; }
            set
            {
                if (_orderPos != value)
                {
                    NotifyPropertyChanging("OrderPos");
                    _orderPos = value;
                    NotifyPropertyChanged("OrderPos");
                }
            }
        }

        [Column(IsVersion = true)]
        private Binary _version;

        private EntitySet<Words> _words;
        [Association(Storage = "_words", OtherKey = "RefNestId", ThisKey = "Id")]
        public EntitySet<Words> Words
        {
            get { return this._words; }
            set { this._words.Assign(value); }
        }

        public Nests()
        {
            _words = new EntitySet<Words>(
                new Action<Words>(this.attach_Words),
                new Action<Words>(this.detach_Words)
                );
        }

        private void attach_Words(Words ent)
        {
            NotifyPropertyChanging("Words");
            ent.Nest = this;
        }

        private void detach_Words(Words ent)
        {
            NotifyPropertyChanging("Words");
            ent.Nest = null;
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
