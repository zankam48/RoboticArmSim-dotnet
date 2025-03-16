using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity.Data;
using RoboticArmSim.Models;

namespace RoboticArmSim.Services;

public class UserService 
{
    private readonly ApplicationDbContext _context;
    private readonly string _jwtSecretKey;
    public UserService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _jwtSecretKey = configuration["Jwt:SecretKey"];
    }

    public async Task<bool> RegisterUserAsync(User user)
    {
        if (await _context.Users.AnyAsync(u => u.Username == user.Username))
        {
            return false;
        }

        user.PasswordHash = HashPassword(user.PasswordHash);
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<string> AuthenticateAsync(LoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
        if (user == null ||!VerifyPassword(user.PasswordHash, request.Password))
        {
            return null;
        }
        return GenerateJwtToken(user);
    }

    public async Task<bool> AssignControlAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return false;

        var users = await _context.Users.ToListAsync();
        users.ForEach(u => u.HasControl = false);

        user.HasControl = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public List<User> GetActiveUsers()
    {
        return _context.Users.Where(u => u.HasControl).ToList();
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    private bool VerifyPassword(string password, string storedHash)
    {
        return HashPassword(password) == storedHash;
    }

    private string GenerateJwtToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: "RoboticArmSim",
            audience: "RoboticArmUsers",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
}