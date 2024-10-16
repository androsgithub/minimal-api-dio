using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asp_minimals_apis.Domain.Enums;

namespace asp_minimals_apis.Domain.ModelViews
{
    public record AdminModelView
    {

        public int Id { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Profile { get; set; } = default!;

    }
}