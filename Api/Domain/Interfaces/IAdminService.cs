using asp_minimals_apis.Domain.Entities;
using asp_minimals_apis.DTOs;

namespace asp_minimals_apis.Domain.Interfaces
{
    public interface IAdminService
    {

        public Admin? Login(LoginDTO loginDTO);
        public Admin Add(Admin admin);
        public List<Admin> All(int? page);

        public Admin? GetById(int id);

    }
}