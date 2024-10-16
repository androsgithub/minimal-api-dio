
using System.Reflection;
using asp_minimals_apis.Domain.Entities;
using asp_minimals_apis.Domain.Services;
using asp_minimals_apis.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace Test.Domain.Services
{
    [TestClass]
    public class AdminServiceTest
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
        public void TestSaveAdmin()
        {
            // Arrange -- variables to use in the test
            var context = CreateInfraDbContextTest();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE admins");
            var admin = new Admin();
            admin.Id = 1;
            admin.Email = "test@test.com";
            admin.Password = "test";
            admin.Profile = "Admin";

            var adminService = new AdminService(context);

            // Act -- set properties
            adminService.Add(admin);
            var adminDb = adminService.GetById(admin.Id);

            // Assert -- validate the information
            Assert.AreEqual(1, adminService.All(null).Count);
            Assert.AreEqual(1, adminDb!.Id);
            Assert.AreEqual("test@test.com", adminDb!.Email);
            Assert.AreEqual("test", adminDb!.Password);
            Assert.AreEqual("Admin", adminDb!.Profile);
        }
    }
}