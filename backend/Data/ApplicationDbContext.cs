using Microsoft.EntityFrameworkCore;
using RoboticArmSim.Models;

namespace RoboticArmSim.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}
    public DbSet<User> Users {get; set;}
    public DbSet<RobotArm> RobotArms {get; set;}
    public DbSet<MovementCommand> MovementCommands {get; set;}
    public DbSet<MovementLog> MovementLogs {get; set;}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Ignore<MovementCommand>();
    }

}