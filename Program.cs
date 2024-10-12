var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => new { Response = "Hello World!" });
app.MapPost("/login", (asp_minimals_apis.DTOs.LoginDTO loginDTO) =>
{
    if (loginDTO.Email == "admin@test.com" && loginDTO.Password == "12345")
    {
        return Results.Ok("sucess");
    }
    else
    {
        return Results.Unauthorized();
    }
});


app.Run();


