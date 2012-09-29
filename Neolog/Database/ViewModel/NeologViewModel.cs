using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Neolog.Database.Context;
using Neolog.Database.Models;
using Neolog.Database.Tables;

namespace Neolog.Database.ViewModel
{
    public class NeologViewModel : INotifyPropertyChanged
    {
        private NeologDataContext nlDB;

        public NeologViewModel(string nlDBConnectionString)
        {
            nlDB = new NeologDataContext(nlDBConnectionString);
        }

        #region Observables
        private ObservableCollection<Nests> _allNests;
        public ObservableCollection<Nests> AllNests
        {
            get { return _allNests; }
            set
            {
                _allNests = value;
                NotifyPropertyChanged("AllNests");
            }
        }

        private ObservableCollection<Texts> _allTexts;
        public ObservableCollection<Texts> AllTexts
        {
            get { return _allTexts; }
            set
            {
                _allTexts = value;
                NotifyPropertyChanged("AllTexts");
            }
        }

        private ObservableCollection<WordComments> _allWordComments;
        public ObservableCollection<WordComments> AllWordComments
        {
            get { return _allWordComments; }
            set
            {
                _allWordComments = value;
                NotifyPropertyChanged("AllWordComments");
            }
        }

        private ObservableCollection<Words> _allWords;
        public ObservableCollection<Words> AllWords
        {
            get { return _allWords; }
            set
            {
                _allWords = value;
                NotifyPropertyChanged("AllWords");
            }
        }

        public void InitObservables()
        {
            AllNests = new ObservableCollection<Nests>();
            AllTexts = new ObservableCollection<Texts>();
            AllWordComments = new ObservableCollection<WordComments>();
            AllWords = new ObservableCollection<Words>();
        }
        #endregion

        #region Texts
        public void AddText(Texts ent)
        {
            var exists = from t in nlDB.Texts
                         where t.TextId == ent.TextId
                         select t;
            if (exists.Count() == 0)
            {
                nlDB.Texts.InsertOnSubmit(ent);
                AllTexts.Add(ent);
            }
            else
            {
                Texts t = exists.FirstOrDefault();
                t.Title = ent.Title;
                t.Content = ent.Content;
            }
            nlDB.SubmitChanges();
        }

        public void DeleteText(Texts ent)
        {
            AllTexts.Remove(ent);
            nlDB.Texts.DeleteOnSubmit(ent);
            nlDB.SubmitChanges();
        }

        public string GetTextContent(int tid)
        {
            return (from t in nlDB.Texts where t.TextId == tid select new { t.Content }).FirstOrDefault().Content;
        }
        #endregion

        #region Nests
        public void AddNest(Nests ent)
        {
            var exists = from t in nlDB.Nests
                         where t.NestId == ent.NestId
                         select t;
            if (exists.Count() == 0)
            {
                nlDB.Nests.InsertOnSubmit(ent);
                AllNests.Add(ent);
            }
            else
            {
                Nests t = exists.FirstOrDefault();
                t.Title = ent.Title;
                t.OrderPos = ent.OrderPos;
            }
            nlDB.SubmitChanges();
        }

        public void DeleteNest(Nests ent)
        {
            AllNests.Remove(ent);
            nlDB.Nests.DeleteOnSubmit(ent);
            nlDB.SubmitChanges();
        }

        public List<Nest> GetNests()
        {
            return nlDB.Nests.Select(t => new Nest
                    {
                        Id = t.Id,
                        Title = t.Title,
                        OrderPos = t.OrderPos,
                    }).OrderBy(t => t.OrderPos).ToList();
        }
        #endregion

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
