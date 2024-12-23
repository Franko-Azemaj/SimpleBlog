using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SimpleBlog.Application.Users;

public class AuthService
{
    public const string ClaimUserId = "UserId";

    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
    private readonly IAuthConfig _authConfig;
    private readonly TimeSpan _expirationDuration = TimeSpan.FromHours(16);
    private readonly UsersService _usersService;

    public AuthService(IAuthConfig authConfig, UsersService usersService)
    {
        _authConfig = authConfig;
        _expirationDuration = TimeSpan.FromDays(authConfig.ExpirationTimeInHours);
        _usersService = usersService;
    }

    public string GetPublicKeyPem()
    {
        return _authConfig.PublicKeyPem.Replace("\n", "").Replace("\r", "");
    }


    public async Task<string?> AuthenticateUserAsync(string email, string password)
    {
        try
        {
            var user = await _usersService.GetUserByCredentialsAsync(email, password);

            if (user is null)
                return null;


            var claims = new List<Claim>()
            {
                new Claim(ClaimUserId, user.Id.ToString()),
                new Claim("jti", Guid.NewGuid().ToString()),
            };

            var jwtString = GenerateJWT(claims);
            return jwtString;
        }
        catch (Exception)
        {
            throw;
        }
    }


    private string GenerateJWT(IEnumerable<Claim> claims)
    {
        var token = new JwtSecurityToken(
                            issuer: _authConfig.Issuer,
                            audience: _authConfig.Audience,
                            claims: claims,
                            notBefore: DateTime.Now,
                            expires: DateTime.Now.Add(_expirationDuration),
                            signingCredentials: _authConfig.SigningCredentials);

        var jwtString = _jwtSecurityTokenHandler.WriteToken(token);
        return jwtString;
    }

}
