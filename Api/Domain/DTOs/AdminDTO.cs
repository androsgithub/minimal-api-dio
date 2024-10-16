
using asp_minimals_apis.Domain.Enums;

namespace asp_minimals_apis.DTOs
{
    public class AdminDTO
    {

        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public Profile? Profile { get; set; } = default!;

    }
}