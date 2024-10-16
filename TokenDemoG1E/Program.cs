using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using TokenDemoG1E.Data.Context;
using TokenDemoG1E.Data.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region DB

builder.Services.AddDbContext<EmployeeContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("con"));
});

#endregion

#region Identity
builder.Services.AddIdentity<Employee, IdentityRole>(options =>
{
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 5;

}).AddEntityFrameworkStores<EmployeeContext>();

#endregion

#region Authentication

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Default";
    options.DefaultChallengeScheme = "Default";
})
    .AddJwtBearer("Default", options =>
    {
        var keyString = builder.Configuration.GetValue<string>("SecretKey");
        var keyInBytes = Encoding.ASCII.GetBytes(keyString);
        var key = new SymmetricSecurityKey(keyInBytes);
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = key,
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

#endregion

#region Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CEO",
        p => p.RequireClaim(ClaimTypes.Role, "CEO")
        );
});

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
