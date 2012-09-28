using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using Neolog.Database.Tables;

namespace Neolog.Database.Context
{
    public class NeologDataContext : DataContext
    {
        public NeologDataContext(string connectionString) : base(connectionString)
        {
        }

        public Table<Texts> Texts;
    }
}
