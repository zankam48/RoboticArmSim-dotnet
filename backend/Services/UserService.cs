using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.Data;
using RoboticArmSim.Models;

namespace RoboticArmSim.Services;

public class UserService 
{
    // db context readonly
    private readonly string _jwtSecretKey;
    public UserService(IConfiguration configuration)
    {
        // db context
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

    
}