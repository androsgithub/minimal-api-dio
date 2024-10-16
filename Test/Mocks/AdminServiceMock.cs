
using asp_minimals_apis.Domain.Entities;
using asp_minimals_apis.Domain.Interfaces;
using asp_minimals_apis.DTOs;

namespace Test.Mocks
{
    public class AdminServiceMock : IAdminService
    {
        private static List<Admin> admins = new List<Admin>(){
            new Admin { Id = 1, Email = "admin@test.com", Password = "admin", Profile = "Admin" },
            new Admin { Id = 2, Email = "editor@test.com", Password = "editor", Profile = "Editor" }
        };

        public Admin Add(Admin admin)
        {
            admin.Id = admins.Count + 1;
            admins.Add(admin);
            return admin;
        }

        public List<Admin> All(int? page)
        {
            var _admins = admins;
            int itemsPerPage = 10;
            if (page != null)
            {
                _admins = admins.Skip(((int)page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
            }
            return _admins;
        }

        public Admin? GetById(int id)
        {
            return admins.Find(a => a.Id == id);
        }

        public Admin? Login(LoginDTO loginDTO)
        {
            return admins.Find(a => a.Email == loginDTO.Email && a.Password == loginDTO.Password);
        }
    }
}