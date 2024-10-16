using asp_minimals_apis.Domain.Entities;

namespace asp_minimals_apis.Domain.Interfaces
{
    public interface IVehicleService
    {
        List<Vehicle> All(int? page, string? nome = null, string? marca = null);
        Vehicle? GetById(int id);
        void Add(Vehicle vehicle);

        void Update(Vehicle vehicle);

        void Delete(Vehicle vehicle);

    }
}