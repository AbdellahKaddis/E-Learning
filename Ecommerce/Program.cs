using Ecommerce.DAL.Db;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Ecommerce.BLL.Services;
using Ecommerce.DAL.Repositories;
<<<<<<< HEAD
using Ecommerce.DAL.SignarHub;
=======
using QuestPDF.Infrastructure;
>>>>>>> aaec8d4816efbb6e32d967fa847ef4a5999e8e7c

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

builder.Services.AddSignalR();



builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<EmailService>();

builder.Services.AddScoped<RoleRepository>();
builder.Services.AddScoped<RoleService>();

builder.Services.AddScoped<ParentService>();
builder.Services.AddScoped<ParentRepository>();

builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<StudentRepository>();
builder.Services.AddScoped<ClassRepository>();
builder.Services.AddScoped<ClasseService>();


builder.Services.AddScoped<LocationService>();
builder.Services.AddScoped<LocationRepository>();


builder.Services.AddScoped<ScheduleService>();
builder.Services.AddScoped<ScheduleRepository>();
builder.Services.AddScoped<LessonProgressService>();
builder.Services.AddScoped<LessonProgressRepository>();

builder.Services.AddScoped<ChatMessageRepository>();
builder.Services.AddScoped<ChatMessageService>();
builder.Services.AddScoped<LevelRepository>();
builder.Services.AddScoped<LevelService>();

//// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins(
                            "http://localhost:4200",
                            "http://localhost:57671"
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials());
});





// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddScoped<CategoryRepositories>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<CourseRepository>();
builder.Services.AddScoped<CourseService>();



builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNameCaseInsensitive = true);// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

QuestPDF.Settings.License = LicenseType.Community;


var app = builder.Build();


app.UseCors("AllowFrontend");
app.UseDeveloperExceptionPage(); 

app.MapHub<ChatHub>("/chathub");
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

