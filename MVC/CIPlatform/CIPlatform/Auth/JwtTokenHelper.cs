using CIPlatform.Entities.Auth;
using CIPlatform.Entities.ViewModels;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace CIPlatform.Auth
{
    public static class JwtTokenHelper
    {
        public static string GenerateToken(JwtSetting jwtSetting, SessionDetailsViewModel volunteer)
        {
            if (jwtSetting == null)
                return string.Empty;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
            new Claim(ClaimTypes.Name, volunteer.FullName),

            new Claim(ClaimTypes.NameIdentifier, volunteer.FullName),
            new Claim(ClaimTypes.Role, volunteer.Role),

            new Claim("CustomClaimForUser", JsonSerializer.Serialize(volunteer))
            }; // Additional Claims

            var token = new JwtSecurityToken(

            jwtSetting.Issuer,

            jwtSetting.Audience,

            claims,

            expires: DateTime.UtcNow.AddMinutes(15),

            signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}