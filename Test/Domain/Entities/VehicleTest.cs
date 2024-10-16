
using asp_minimals_apis.Domain.Entities;

namespace Test.Domain.Entities
{
    [TestClass]
    public class VehicleTest
    {
        [TestMethod]
        public void TestGetSetProperties()
        {
            // Arrange -- variables to use in the test
            var vehicle = new Vehicle();

            // Act -- set properties
            vehicle.Id = 1;
            vehicle.Name = "Corsa";
            vehicle.Marca = "Chevrolet";
            vehicle.Ano = 2012;


            // Assert -- validate the information
            Assert.AreEqual(1, vehicle.Id);
            Assert.AreEqual("Corsa", vehicle.Name);
            Assert.AreEqual("Chevrolet", vehicle.Marca);
            Assert.AreEqual(2012, vehicle.Ano);

        }
    }
}