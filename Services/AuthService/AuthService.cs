
using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Services.AuthService
{
    public class AuthService:IAuthService{

        private readonly DataContext context;
        private readonly IConfiguration configuration;
        public AuthService(DataContext _context,IConfiguration _configuration)
        {   
            context = _context;
             configuration = _configuration;
        }
        
         public async Task<string?> RegisterAsync(UserDto request)
{
    try
    {
        // Check if user already exists
        if (await context.Users.AnyAsync(u => u.Username == request.Username))
        {
            return null; // Or return an error message string like "User already exists"
        }

        var user = new User();
        var hashedPassword = new PasswordHasher<User>()
            .HashPassword(user, request.Password);

        user.Username = request.Username;
        user.Password = hashedPassword;

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return CreateToken(user);
    }
    catch (Exception ex)
    {
        // Log the exception (consider using a logging framework like Serilog or NLog)
        Console.WriteLine($"Error occurred during registration: {ex.Message}");
        
         throw new Exception("An error occurred during registration.", ex);
    }
}


          public async  Task<string?> LoginAsync(UserDto request){
                 var user = await context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user is null)
            {
                return null;
            }
            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.Password, request.Password)
                == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return CreateToken(user);

            }


             private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                audience: configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}