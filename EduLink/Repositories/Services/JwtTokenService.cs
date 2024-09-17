using EduLink.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EduLink.Repositories.Services
{
    public class JwtTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly SignInManager<User> _signInManager;

        public JwtTokenService(IConfiguration configuration, SignInManager<User> signInManager)
        {
            _configuration = configuration;
            _signInManager = signInManager;
        }


        public static TokenValidationParameters ValidateToken(IConfiguration configuration)
        {
            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = GetSecurityKey(configuration),
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = true,
                ValidateIssuer = false,
                ValidateAudience = false
            };

        }


        private static SecurityKey GetSecurityKey(IConfiguration configuration)
        {
            var secretKey = configuration["JWT:SecretKey"];
            if (secretKey == null)
            {
                throw new InvalidOperationException("Jwt Secret key is not exsist");
            }

            var secretBytes = Encoding.UTF8.GetBytes(secretKey);

            return new SymmetricSecurityKey(secretBytes);
        }


        public async Task<string> GenerateToken(User user, TimeSpan expiryDate)
        {
            var userPrincliple = await _signInManager.CreateUserPrincipalAsync(user);
            if (userPrincliple == null)
            {
                return null;
            }

            var signInKey = GetSecurityKey(_configuration);

            var token = new JwtSecurityToken
                (
                expires: DateTime.UtcNow + expiryDate,
                signingCredentials: new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256),
                claims: userPrincliple.Claims
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> GenerateTokenWithRoleData(User user, string role, object roleData, TimeSpan expiryDate)
        {
            var userPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
            if (userPrincipal == null)
            {
                return null;
            }

            var claims = userPrincipal.Claims.ToList();

            // Add custom claims based on role data
            if (role == "Volunteer" && roleData is Volunteer volunteer)
            {
                claims.Add(new Claim("VolunteerID", volunteer.VolunteerID.ToString()));
                claims.Add(new Claim("StudentID", volunteer.StudentID.ToString()));
                claims.Add(new Claim("Availability", volunteer.Availability.ToString()));
                claims.Add(new Claim("SkillDescription", volunteer.SkillDescription ?? ""));
                // Add other relevant claims...
            }
            else if (role == "Student" && roleData is Student student)
            {
                claims.Add(new Claim("StudentID", student.StudentID.ToString()));
                // Add other relevant claims...
            }

            var signInKey = GetSecurityKey(_configuration);
            var token = new JwtSecurityToken(
                expires: DateTime.UtcNow + expiryDate,
                signingCredentials: new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256),
                claims: claims
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> GenerateTokenWithAdminClaims(User user, TimeSpan expiryDuration)
        {
            // Create user claims
            var userPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
            if (userPrincipal == null)
            {
                return null;
            }

            var claims = userPrincipal.Claims.ToList();

            // Add custom admin-specific claims
            claims.Add(new Claim("IsAdmin", user.IsAdmin.ToString()));

            var signInKey = GetSecurityKey(_configuration);

            var jwtToken = new JwtSecurityToken(
                expires: DateTime.UtcNow.Add(expiryDuration),
                signingCredentials: new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256),
                claims: claims
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }


    }
}
