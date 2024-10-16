using System.Net;
using System.Text;
using System.Text.Json;
using asp_minimals_apis.Domain.Entities;
using asp_minimals_apis.Domain.ModelViews;
using asp_minimals_apis.DTOs;
using Test.Helpers;

namespace Test.Requests;

[TestClass]
public class VehicleRequestTest
{
    private string? Token;
    [ClassInitialize]
    public async Task ClassInit(TestContext testContext)
    {
        Setup.ClassInit(testContext);
        var loginDTO = new LoginDTO { Email = "admin@test.com", Password = "admin" };
        var content = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8, "Application/json");
        var response = await Setup.client.PostAsync("/admin/login", content);
        var result = await response.Content.ReadAsStringAsync();
        var adminLoggedIn = JsonSerializer.Deserialize<AdminLoggedIn>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        Token = adminLoggedIn?.Token;
    }
    [ClassCleanup]
    public static void ClassCleanup()
    {
        Setup.ClassCleanup();
    }

    [TestMethod]
    public async Task TestGetSetProperties()
    {
        // Arrange -- variables to use in the test
        var vehicleDTO = new VehicleDTO { Name = "Uno", Marca = "Fiat", Ano = 1996 };

        var content = new StringContent(JsonSerializer.Serialize(vehicleDTO), Encoding.UTF8, "Application/json");
        content.Headers.Add("Authentication", $"Bearer {Token}");

        // Act -- set properties
        var response = await Setup.client.PostAsync("/vehicle", content);

        // Assert 
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

        var result = await response.Content.ReadAsStringAsync();
        var vehicle = JsonSerializer.Deserialize<Vehicle>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.IsNotNull(vehicle?.Id);
        Assert.AreEqual(vehicleDTO.Name, vehicle.Name);
        Assert.AreEqual(vehicleDTO.Marca, vehicle.Marca);
        Assert.AreEqual(vehicleDTO.Ano, vehicle.Ano);

    }

}