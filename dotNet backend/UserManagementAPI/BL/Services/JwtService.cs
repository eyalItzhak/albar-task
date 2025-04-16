using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UserManagementAPI.DAL.Dtos.Results;
using UserManagementAPI.DAL.Models;

namespace UserManagementAPI.BL.Services
{
    public interface IJwtService
    {
        Result<string> GenerateToken(User user); // החזר Result<string> במקום string
        Result<ClaimsPrincipal> ValidateToken(string token);
    }

    public class JwtService : IJwtService
    {
        private readonly string _jwtKey;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;

        public JwtService(string jwtKey, string jwtIssuer, string jwtAudience)
        {
            _jwtKey = jwtKey;
            _jwtIssuer = jwtIssuer;
            _jwtAudience = jwtAudience;
        }

        public Result<string> GenerateToken(User user)
        {
            try
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.Username)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _jwtIssuer,
                    audience: _jwtAudience,
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: creds);

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                return Result<string>.Success(tokenString);
            }
            catch (Exception ex)
            {
                return Result<string>.Failure($"Error generating token: {ex.Message}");
            }
        }

        public Result<ClaimsPrincipal> ValidateToken(string token)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _jwtIssuer,
                    ValidAudience = _jwtAudience,
                    IssuerSigningKey = key
                };

                var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                return Result<ClaimsPrincipal>.Success(claimsPrincipal);
            }
            catch (Exception)
            {
                return Result<ClaimsPrincipal>.Failure("Invalid token");
            }
        }
    }
}
