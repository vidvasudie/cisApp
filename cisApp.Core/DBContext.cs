using System;
using System.Collections.Generic;
using System.Text;

namespace cisApp.Core
{
    public class DBContext
    {
        private CAppContext _db;
        public DBContext()
        {
            _db = new CAppContext();
            cAppContext = _db;
        }
        public CAppContext cAppContext { get; }


    }
}
