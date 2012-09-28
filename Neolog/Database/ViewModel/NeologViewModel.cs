using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Neolog.Database.Context;
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

        public void InitObservables()
        {
            AllTexts = new ObservableCollection<Texts>();
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
