using System;
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
                        NestId = t.NestId,
                        Title = t.Title,
                        OrderPos = t.OrderPos,
                    }).OrderBy(t => t.OrderPos).ToList();
        }

        public Nest GetNest(int nid)
        {
            return nlDB.Nests.Where(t => t.NestId == nid).Select(t => new Nest
            {
                Id = t.Id,
                Title = t.Title,
                OrderPos = t.OrderPos,
            }).FirstOrDefault();
        }
        #endregion

        #region Words
        public void AddWord(Words ent)
        {
            var exists = from t in nlDB.Words
                         where t.WordId == ent.WordId
                         select t;
            if (exists.Count() == 0)
            {
                ent.Nest = nlDB.Nests.Where(t => t.NestId == ent.NestId).FirstOrDefault();
                nlDB.Words.InsertOnSubmit(ent);
                AllWords.Add(ent);
            }
            else
            {
                Words t = exists.FirstOrDefault();
                t.AddedAtDate = ent.AddedAtDate;
                t.AddedBy = ent.AddedBy;
                t.AddedByEmail = ent.AddedByEmail;
                t.AddedByUrl = ent.AddedByUrl;
                t.CommentsCount = ent.CommentsCount;
                t.Derivatives = ent.Derivatives;
                t.Description = ent.Description;
                t.Ethimology = ent.Ethimology;
                t.Example = ent.Example;
                t.NestId = ent.NestId;
                t.WordComments = ent.WordComments;
                t.WordContent = ent.WordContent;
                t.Nest = nlDB.Nests.Where(t2 => t2.NestId == ent.NestId).FirstOrDefault();
                t.RefNestId = t.Nest.Id;
            }
            nlDB.SubmitChanges();
        }

        public void AddWordComment(WordComments ent)
        {
            var exists = from t in nlDB.WordComments
                         where t.WordCommentId == ent.WordCommentId
                         select t;
            if (exists.Count() == 0)
            {
                ent.Word = nlDB.Words.Where(t => t.WordId == ent.WordId).FirstOrDefault();
                nlDB.WordComments.InsertOnSubmit(ent);
                AllWordComments.Add(ent);
            }
            else
            {
                WordComments t = exists.FirstOrDefault();
                t.Author = ent.Author;
                t.Comment = ent.Comment;
                t.CommentDate = ent.CommentDate;
                t.WordId = ent.WordId;
                ent.Word = nlDB.Words.Where(t2 => t2.WordId == ent.WordId).FirstOrDefault();
                t.RefWordId = t.Word.Id;
            }
            nlDB.SubmitChanges();
        }

        public List<Word> GetWordsForNest(int nestId)
        {
            return nlDB.Words.Where(t => t.NestId == nestId).Select(t => new Word
            {
                Id = t.Id,
                AddedAtDate = t.AddedAtDate,
                AddedBy = t.AddedBy,
                AddedByEmail = t.AddedByEmail,
                AddedByUrl = t.AddedByUrl,
                CommentsCount = t.CommentsCount,
                Derivatives = t.Derivatives,
                Description = t.Description,
                Ethimology = t.Ethimology,
                Example = t.Example,
                NestId = t.NestId,
                WordContent = t.WordContent,
                WordId = t.WordId
            }).OrderBy(t => t.WordContent).ToList();
        }

        public List<Word> GetWordsForLetter(string letter)
        {
            return nlDB.Words.Where(t => t.WordContent.ToLower().StartsWith(letter.ToLower())).Select(t => new Word
            {
                Id = t.Id,
                AddedAtDate = t.AddedAtDate,
                AddedBy = t.AddedBy,
                AddedByEmail = t.AddedByEmail,
                AddedByUrl = t.AddedByUrl,
                CommentsCount = t.CommentsCount,
                Derivatives = t.Derivatives,
                Description = t.Description,
                Ethimology = t.Ethimology,
                Example = t.Example,
                NestId = t.NestId,
                WordContent = t.WordContent,
                WordId = t.WordId
            }).OrderBy(t => t.WordContent).ToList();
        }

        public Word GetWord(int wid)
        {
            return nlDB.Words.Where(t => t.WordId == wid).Select(t => new Word
            {
                Id = t.Id,
                AddedAtDate = t.AddedAtDate,
                AddedBy = t.AddedBy,
                AddedByEmail = t.AddedByEmail,
                AddedByUrl = t.AddedByUrl,
                CommentsCount = t.CommentsCount,
                Derivatives = t.Derivatives,
                Description = t.Description,
                Ethimology = t.Ethimology,
                Example = t.Example,
                NestId = t.NestId,
                WordContent = t.WordContent,
                WordId = t.WordId
            }).FirstOrDefault();
        }

        public List<WordComment> GetWordComments(int wid)
        {
            return nlDB.WordComments.Where(t => t.WordId == wid).Select(t => new WordComment
            {
                Id = t.Id,
                Author = t.Author,
                Comment = t.Comment,
                CommentDate = t.CommentDate,
                CommentDateString = AppSettings.DoLongDate(t.CommentDate, false, true),
                WordCommentId = t.WordCommentId,
                WordId = t.WordId
            }).OrderByDescending(t => t.CommentDate).ToList();
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
