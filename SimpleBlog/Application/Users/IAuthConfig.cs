using Microsoft.IdentityModel.Tokens;

namespace SimpleBlog.Application.Users;

public interface IAuthConfig
{
    SigningCredentials SigningCredentials { get; }
    string Issuer { get; }
    string Audience { get; }
    string PublicKeyPem { get; }
    int ExpirationTimeInHours { get; }
}
