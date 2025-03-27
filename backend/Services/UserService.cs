using System.Text;
using RoboticArmSim.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using RoboticArmSim.Data;
using RoboticArmSim.DTOs;
using AutoMapper;
using RoboticArmSim.Repositories;

namespace RoboticArmSim.Services;

public class UserService 
{
    // private readonly ApplicationDbContext _context;
    private readonly IUserRepository _userRepository;

    private readonly string _jwtSecretKey;
    private readonly IMapper _mapper;
    public UserService(IUserRepository userRepository, IConfiguration configuration, IMapper mapper)
    {
        _userRepository = userRepository;
        _jwtSecretKey = configuration["Jwt:SecretKey"];
        _mapper = mapper;
    }
  
    public async Task<bool> IsUniqueUserAsync(string username)
    {
        return await _userRepository.IsUniqueUsernameAsync(username);
    }

    public async Task<UserDTO?> RegisterUserAsync(RegisterationRequestDTO model)
    {
        // if (await _context.Users.AnyAsync(u => u.Username == model.Username));
        var user = new User
        {
            Username = model.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
            Role = model.Role,
            ConnectedAt = DateTime.UtcNow
        };

        await _userRepository.AddUserAsync(user);
        await _userRepository.SaveChangesAsync();

        return _mapper.Map<UserDTO>(user);
    }

    public async Task<LoginResponseDTO> AuthenticateAsync(LoginRequestDTO request)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username!);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return null;

        return new LoginResponseDTO
        {
            User = _mapper.Map<UserDTO>(user),
            Token = GenerateJwtToken(user)
        };
    }

    public async Task<bool> AssignControlAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) return false;

        var users = await _userRepository.GetAllAsync();
        users.ForEach(u => u.IsControlling = false);

        user.IsControlling = true;
        await _userRepository.SaveChangesAsync();
        return true;
    }

    public List<UserDTO> GetActiveUsers()
    {
        var activeUsers = _userRepository.GetAllAsync()
            .Result.Where(u => u.IsControlling).ToList();
        return _mapper.Map<List<UserDTO>>(activeUsers);
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


