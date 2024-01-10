using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShredStoreTests
{
    public class Utility
    {
        public string CreateQueryForStoredProcedureCheck(string spName)
        {
            string str = @"SELECT name
                     FROM sys.objects
                     WHERE type = 'P' AND name = '" + spName + "';";
            return str;
        }
    }
}
