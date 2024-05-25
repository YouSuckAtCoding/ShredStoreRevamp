using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Request.JwtRequests
{
    public class JwtGenerateRequest
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public Dictionary<string, object> Claims { get; set; } = new();
    }
}
