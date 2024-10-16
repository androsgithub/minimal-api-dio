using asp_minimals_apis.Domain.Entities;

namespace Test.Domain.Entities;

[TestClass]
public class AdminTest
{
    [TestMethod]
    public void TestGetSetProperties()
    {
        // Arrange -- variables to use in the test
        var admin = new Admin();

        // Act -- set properties
        admin.Id = 1;
        admin.Email = "test@test.com";
        admin.Password = "test";
        admin.Profile = "Editor";

        // Assert -- validate the information
        Assert.AreEqual(1, admin.Id);
        Assert.AreEqual("test@test.com", admin.Email);
        Assert.AreEqual("test", admin.Password);
        Assert.AreEqual("Editor", admin.Profile);
    }
}