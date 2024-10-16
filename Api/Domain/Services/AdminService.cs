

using System.Data.Common;
using asp_minimals_apis.Domain.Entities;
using asp_minimals_apis.Domain.Interfaces;
using asp_minimals_apis.DTOs;
using asp_minimals_apis.Infrastructure.Db;
using Microsoft.EntityFrameworkCore.Query;

namespace asp_minimals_apis.Domain.Services
{
    public class AdminService : IAdminService
    {
        private readonly InfraDbContext _ctx;
        public AdminService(InfraDbContext ctx)
        {
            _ctx = ctx;
        }

        public Admin? Login(LoginDTO loginDTO)
        {
            var admin = _ctx.Admins.Where(a => a.Email == loginDTO.Email && a.Password == loginDTO.Password).FirstOrDefault();
            return admin;
        }
        public Admin Add(Admin admin)
        {
            _ctx.Admins.Add(admin);
            _ctx.SaveChanges();
            return admin;
        }

        public List<Admin> All(int? page)
        {
            var q = _ctx.Admins.AsQueryable();

            int itemsPerPage = 10;
            if (page != null)
            {
                q = q.Skip(((int)page - 1) * itemsPerPage).Take(itemsPerPage);
            }
            return q.ToList();
        }

        public Admin? GetById(int id)
        {
            return _ctx.Admins.Find(id);
        }
    }
}