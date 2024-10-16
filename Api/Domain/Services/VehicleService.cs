using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asp_minimals_apis.Domain.Entities;
using asp_minimals_apis.Domain.Interfaces;
using asp_minimals_apis.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace asp_minimals_apis.Domain.Services
{
    public class VehicleService : IVehicleService
    {

        private readonly InfraDbContext _ctx;
        public VehicleService(InfraDbContext ctx)
        {
            _ctx = ctx;
        }
        public List<Vehicle> All(int? page, string? nome = null, string? marca = null)
        {
            var q = _ctx.Vehicles.AsQueryable();
            if (!string.IsNullOrEmpty(nome))
            {
                q = q.Where(v => EF.Functions.Like(v.Name.ToLower(), $"%{nome}%"));
            }
            int itemsPerPage = 10;
            if (page != null)
            {
                q = q.Skip(((int)page - 1) * itemsPerPage).Take(itemsPerPage);
            }
            return q.ToList();
        }
        public Vehicle? GetById(int id)
        {
            var vehicleDb = _ctx.Vehicles.Find(id);

            return vehicleDb;
        }
        public void Add(Vehicle vehicle)
        {
            _ctx.Vehicles.Add(vehicle);
            _ctx.SaveChanges();

        }
        public void Update(Vehicle vehicle)
        {
            _ctx.Vehicles.Update(vehicle);
            _ctx.SaveChanges();
        }
        public void Delete(Vehicle vehicle)
        {
            _ctx.Vehicles.Remove(vehicle);
            _ctx.SaveChanges();
        }
    }
}