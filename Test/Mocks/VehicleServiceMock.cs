


using asp_minimals_apis.Domain.Entities;
using asp_minimals_apis.Domain.Interfaces;

namespace Test.Mocks
{
    public class VehicleServiceMock : IVehicleService
    {
        private static List<Vehicle> vehicles = new List<Vehicle>()
        {
            new Vehicle() { Id = 1, Name = "Corsa", Marca="Chevrolet", Ano=2012},
            new Vehicle() { Id = 2, Name = "Gol", Marca="Volkswagen", Ano=2015},
        };
        public void Add(Vehicle vehicle)
        {
            vehicle.Id = vehicles.Count + 1;
            vehicles.Add(vehicle);
        }

        public List<Vehicle> All(int? page, string? nome = null, string? marca = null)
        {
            var _vehicles = vehicles;
            int itemsPerPage = 10;
            if (page != null)
            {
                _vehicles = vehicles.Skip(((int)page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
            }
            return _vehicles;
        }

        public void Delete(Vehicle vehicle)
        {
            vehicles.Remove(vehicle);
        }

        public void Update(Vehicle vehicle)
        {
            var vehicleDb = vehicles.Find(a => a.Id == vehicle.Id);
            if (vehicleDb == null) return;
            vehicles.Remove(vehicleDb);
            vehicles.Add(vehicle);
        }

        Vehicle? IVehicleService.GetById(int id)
        {
            return vehicles.Find(a => a.Id == id);
        }
    }
}