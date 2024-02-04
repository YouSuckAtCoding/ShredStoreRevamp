using ShredStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShredStoreApiTests
{
    public static class Utility
    {
        public static string SetGet_Or_DeleteUrl(int id, string endpoint)
        {
            string url = endpoint;
            url = url.Replace( "{id}", $"{id}");
            url = url.Replace("{id:int}", $"{id}");
            return url;
        }
        
    }
}
