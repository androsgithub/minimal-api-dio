using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_minimals_apis.Domain.ModelViews
{
    public class AdminLoggedIn
    {
        public string Email { get; set; } = default!;
        public string Profile { get; set; } = default!;
        public string Token { get; set; } = default!;
    }
}