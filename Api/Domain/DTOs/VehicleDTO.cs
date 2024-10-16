namespace asp_minimals_apis.DTOs
{
    public record VehicleDTO
    {
        public string Name { get; set; } = default!;
        public string Marca { get; set; } = default!;
        public int Ano { get; set; } = default!;
    }

}