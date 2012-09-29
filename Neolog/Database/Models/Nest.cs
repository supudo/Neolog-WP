using System;

namespace Neolog.Database.Models
{
    public class Nest
    {
        private int _id;
        public int Id
        {
            get { return this._id; }
            set { this._id = value; }
        }

        private int _nestId;
        public int NestId
        {
            get { return this._nestId; }
            set { this._nestId = value; }
        }

        private int _orderPos;
        public int OrderPos
        {
            get { return this._orderPos; }
            set { this._orderPos = value; }
        }

        private string _title;
        public string Title
        {
            get { return this._title; }
            set { this._title = value; }
        }
    }
}
