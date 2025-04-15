using Ecommerce.DAL.Db;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Ecommerce.BLL.Services;

var builder = WebApplication.CreateBuilder(args);

// Get the connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configure the DbContext to use the connection string
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

//JWT configuration
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddScoped<EmailService>();

//// Configure CORS
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowFrontend",
//        policy =>
//        {
//            policy.WithOrigins("http://localhost:4200")
//                  .AllowAnyHeader()
//                  .AllowAnyMethod();
//        });
//});

//app.UseCors("AllowFrontend");


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

