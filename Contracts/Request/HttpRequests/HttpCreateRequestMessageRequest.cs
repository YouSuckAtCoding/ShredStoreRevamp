using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Request.HttpRequests
{
    public class HttpCreateRequestMessageRequest
    {
        public Uri route { get; set; }
        public HttpMethod Method { get; set; }
        public string Token { get; set; }
        public string Content { get; set; }
        

    }
}
