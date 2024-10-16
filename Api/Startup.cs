
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using asp_minimals_apis.Domain.Entities;
using asp_minimals_apis.Domain.Enums;
using asp_minimals_apis.Domain.Interfaces;
using asp_minimals_apis.Domain.ModelViews;
using asp_minimals_apis.Domain.Services;
using asp_minimals_apis.DTOs;
using asp_minimals_apis.Infrastructure.Db;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace asp_minimals_apis
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            key = Configuration.GetSection("Jwt").ToString() ?? "12345";
        }
        public IConfiguration Configuration { get; set; }
        private string key;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});
            services.AddAuthorization();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IVehicleService, VehicleService>();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement{
        {new OpenApiSecurityScheme{
            Reference = new OpenApiReference{
                Type= ReferenceType.SecurityScheme,
                Id="Bearer"
            }
        },new string[ ]{}}
                });
            });


            services.AddDbContext<InfraDbContext>(options =>
            {
                var connectionString = Configuration.GetConnectionString("MySql")?.ToString();
                options.UseMySql(connectionString,
                ServerVersion.AutoDetect(connectionString));
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {


            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {

                #region Home
                endpoints.MapGet("/", () => Results.Json(new Home())).AllowAnonymous().WithTags("Home");
                #endregion

                #region Admin
                string GenerateJwtToken(Admin admin)
                {
                    if (string.IsNullOrEmpty(key))
                    {
                        return string.Empty;
                    }
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                    var claims = new List<Claim>()
                        {
                            new Claim("Email", admin.Email),
                            new Claim(ClaimTypes.Role, admin.Profile),
                            new Claim("Profile", admin.Profile)
                        };
                    var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddMinutes(1), signingCredentials: credentials);
                    return new JwtSecurityTokenHandler().WriteToken(token);
                }
                endpoints.MapPost("/admin/login", ([FromBody] LoginDTO loginDTO, IAdminService adminService) =>
                {
                    var admin = adminService.Login(loginDTO);
                    if (admin != null)
                    {
                        var token = GenerateJwtToken(admin);
                        return Results.Ok(new AdminLoggedIn
                        {
                            Email = admin.Email,
                            Profile = admin.Profile,
                            Token = token
                        });
                    }
                    else
                    {
                        return Results.Unauthorized();
                    }
                })
                .AllowAnonymous()
                .WithTags("Admin"); ;

                endpoints.MapPost("/admin", ([FromBody] AdminDTO adminDTO, IAdminService adminService) =>
                {
                    var validation = new ValidationErrors { Messages = new List<string>() };
                    if (string.IsNullOrEmpty(adminDTO.Email))
                    {
                        validation.Messages.Add("Email cannot be empty");
                    }
                    if (string.IsNullOrEmpty(adminDTO.Password))
                    {
                        validation.Messages.Add("Password cannot be empty");
                    }
                    if (adminDTO.Profile == null)
                    {
                        validation.Messages.Add("Profile cannot be empty");
                    }
                    if (validation.Messages.Count > 0) return Results.BadRequest(validation);

                    var admin = new Admin
                    {
                        Email = adminDTO.Email,
                        Password = adminDTO.Password,
                        Profile = adminDTO.Profile.ToString() ?? Profile.Editor.ToString(),
                    };
                    adminService.Add(admin);
                    return Results.Created($"/admin/{admin.Id}", admin);
                })
                .RequireAuthorization()
                .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" })
                .WithTags("Admin");

                endpoints.MapGet("/admin", ([FromQuery] int? page, IAdminService adminService) =>
                {
                    var data = adminService.All(page);
                    var admins = data.Select(admin => new AdminModelView
                    {
                        Id = admin.Id,
                        Email = admin.Email,
                        Profile = admin.Profile
                    });
                    return Results.Json(new { data = admins, page, });
                })
                .RequireAuthorization()
                .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" })
                .WithTags("Admin");

                endpoints.MapGet("/admin/{id}", ([FromRoute] int id, IAdminService adminService) =>
                {
                    var admin = adminService.GetById(id);

                    if (admin == null) return Results.NotFound();

                    return Results.Ok(new AdminModelView
                    {
                        Id = admin.Id,
                        Email = admin.Email,
                        Profile = admin.Profile
                    });

                })
                .RequireAuthorization()
                .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" })
                .WithTags("Admin");

                #endregion

                #region Vehicle
                ValidationErrors validaDTO(VehicleDTO vehicleDTO)
                {
                    var validation = new ValidationErrors { Messages = new List<string>() };
                    if (string.IsNullOrEmpty(vehicleDTO.Name)) validation.Messages.Add("Name cannot be empty");
                    if (string.IsNullOrEmpty(vehicleDTO.Marca)) validation.Messages.Add("Marca cannot be empty");
                    if (vehicleDTO.Ano < 1950 || vehicleDTO.Ano > DateTime.Now.Year)
                        validation.Messages.Add($"Ano must be between 1950 and {DateTime.Now.Year}");
                    return validation;
                }

                endpoints.MapPost("/vehicle", ([FromBody] VehicleDTO vehicleDTO, IVehicleService vehicleService) =>
                {
                    var validation = validaDTO(vehicleDTO);
                    if (validation.Messages.Count > 0)
                    {
                        return Results.BadRequest(validation);
                    }

                    var vehicle = new Vehicle
                    {
                        Name = vehicleDTO.Name,
                        Marca = vehicleDTO.Marca,
                        Ano = vehicleDTO.Ano,
                    };
                    vehicleService.Add(vehicle);
                    return Results.Created($"/vehicle/{vehicle.Id}", vehicle);

                })
                .RequireAuthorization()
                .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin,Editor" })
                .WithTags("Vehicle");

                endpoints.MapGet("/vehicle", (int page, string? name, string? marca, IVehicleService vehicleService) =>
                {
                    var vehicles = vehicleService.All(page: page, nome: name, marca: marca);
                    return Results.Json(new { data = vehicles, page, search = new { name, marca } });

                })
                .RequireAuthorization()
                .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin,Editor" })
                .WithTags("Vehicle");

                endpoints.MapGet("/vehicle/{id}", ([FromRoute] int id, IVehicleService vehicleService) =>
                {
                    var vehicle = vehicleService.GetById(id);
                    if (vehicle == null) return Results.NotFound();

                    return Results.Ok(vehicle);

                })
                .RequireAuthorization()
                .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin,Editor" })
                .WithTags("Vehicle");

                endpoints.MapPut("/vehicle/{id}", ([FromRoute] int id, [FromBody] VehicleDTO vehicleDTO, IVehicleService vehicleService) =>
                {

                    var validation = validaDTO(vehicleDTO);
                    if (validation.Messages.Count > 0)
                    {
                        return Results.BadRequest(validation);
                    }

                    var vehicle = vehicleService.GetById(id);
                    if (vehicle == null) return Results.NotFound();

                    vehicle.Name = vehicleDTO.Name;
                    vehicle.Marca = vehicleDTO.Marca;
                    vehicle.Ano = vehicleDTO.Ano;

                    vehicleService.Update(vehicle);

                    return Results.Ok(vehicle);

                })
                .RequireAuthorization()
                .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" })
                .WithTags("Vehicle");

                endpoints.MapDelete("/vehicle/{id}", ([FromRoute] int id, IVehicleService vehicleService) =>
                {
                    var vehicle = vehicleService.GetById(id);
                    if (vehicle == null) return Results.NotFound();

                    vehicleService.Delete(vehicle);

                    return Results.NoContent();

                })
                .RequireAuthorization()
                .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" })
                .WithTags("Vehicle");
                #endregion

            });

        }
    }
}