using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Response
{
    public class UsersResponse
    {
        public required IEnumerable<UserResponse> Items { get; init; } = Enumerable.Empty<UserResponse>();
    }
}
