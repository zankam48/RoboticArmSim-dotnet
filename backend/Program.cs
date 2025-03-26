using Microsoft.EntityFrameworkCore;
using RoboticArmSim.Data;
using AutoMapper;
using RoboticArmSim;
using RoboticArmSim.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
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


var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors("AllowAllOrigins");

app.MapControllers();
app.MapHub<RoboticArmHub>("/robotArmHub");

app.Run();