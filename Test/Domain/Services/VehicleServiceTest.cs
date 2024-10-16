
using System.Reflection;
using asp_minimals_apis.Domain.Entities;
using asp_minimals_apis.Domain.Services;
using asp_minimals_apis.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace Test.Domain.Services
{
    [TestClass]
    public class VehicleServiceTest
    {
        private InfraDbContext CreateInfraDbContextTest()
        {
            var asemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Path.GetFullPath(Path.Combine(asemblyPath ?? "", "..", "..", ".."));
            var builder = new ConfigurationBuilder()
            .SetBasePath(path ?? Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
            var configuration = builder.Build();
            return new InfraDbContext(configuration);
        }
        [TestMethod]
        public void TestSaveVehicle()
        {
            // Arrange -- variables to use in the test
            var context = CreateInfraDbContextTest();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE vehicles");
            var vehicle = new Vehicle();
            vehicle.Id = 1;
            vehicle.Name = "Corsa";
            vehicle.Marca = "Chevrolet";
            vehicle.Ano = 2012;

            var vehicleService = new VehicleService(context);

            // Act -- set properties
            vehicleService.Add(vehicle);
            var vehicleDb = vehicleService.GetById(vehicle.Id);

            // Assert -- validate the information
            Assert.AreEqual(1, vehicleService.All(null).Count);
            Assert.AreEqual(1, vehicleDb!.Id);
            Assert.AreEqual("Corsa", vehicleDb!.Name);
            Assert.AreEqual("Chevrolet", vehicleDb!.Marca);
            Assert.AreEqual(2012, vehicleDb!.Ano);

        }
    }
}