using Microsoft.EntityFrameworkCore;
using RoboticArmSim.Data;
using AutoMapper;
using RoboticArmSim;
using RoboticArmSim.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using RoboticArmSim.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

// fluent api
// builder.Services.AddFluentValidationAutoValidation();
// builder.Services.AddFluentValidationClientsideAdapters();
// builder.Services.AddTransient<IValidator<Registration>, RegistrationValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingProfile));

// services
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RobotArmService>();
builder.Services.AddScoped<MovementLogService>();

builder.Services.AddSignalR();

builder.Services.AddCors(options => 
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});


// var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:SecretKey"]);


var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors("AllowAllOrigins");

app.MapControllers();
app.MapHub<RoboticArmHub>("/robotArmHub");

app.Run();