using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity.Data;
using RoboticArmSim.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using RoboticArmSim.Data;
using RoboticArmSim.DTOs;


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

    private bool VerifyPassword(string hash, string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }


    // public async Task<bool> RegisterUserAsync(User user)
    // {
    //     if (await _context.Users.AnyAsync(u => u.Username == user.Username))
    //     {
    //         return false;
    //     }

    //     user.PasswordHash = HashPassword(user.PasswordHash);
    //     await _context.Users.AddAsync(user);
    //     await _context.SaveChangesAsync();
    //     return true;
    // }

    public async Task<bool> IsUniqueUserAsync(string username)
    {
        return !await _context.Users.AnyAsync(u => u.Username == username);
    }

    public async Task<UserDTO?> RegisterUserAsync(RegisterationRequestDTO model)
    {
        var user = new User
        {
            Username = model.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password)
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return _mapper.Map<UserDTO>(user);
    }

    public async Task<string> AuthenticateAsync(RoboticArmSim.DTOs.LoginRequest request)
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
        users.ForEach(u => u.IsControlling = false);

        user.IsControlling = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public List<User> GetActiveUsers()
    {
        return _context.Users.Where(u => u.IsControlling).ToList();
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