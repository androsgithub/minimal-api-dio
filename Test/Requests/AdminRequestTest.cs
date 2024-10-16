using System.Net;
using System.Text;
using System.Text.Json;
using asp_minimals_apis.Domain.Entities;
using asp_minimals_apis.Domain.ModelViews;
using asp_minimals_apis.DTOs;
using Test.Helpers;

namespace Test.Requests;

[TestClass]
public class AdminRequestTest
{
    [ClassInitialize]
    public static void ClassInit(TestContext testContext)
    {
        Setup.ClassInit(testContext);
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
        var loginDTO = new LoginDTO { Email = "admin@test.com", Password = "admin" };

        var content = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8, "Application/json");

        // Act -- set properties
        var response = await Setup.client.PostAsync("/admin/login", content);

        // Assert 
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadAsStringAsync();
        var adminLoggedIn = JsonSerializer.Deserialize<AdminLoggedIn>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.IsNotNull(adminLoggedIn?.Email);
        Assert.IsNotNull(adminLoggedIn.Profile);
        Assert.IsNotNull(adminLoggedIn.Token);

    }

}