using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace SimpleBlog.Application.Users;

public record class JwtOptions(
    string Issuer,
    string Audience,
    string SigningKey
);

public sealed class AuthConfig : IAuthConfig
{
    public SigningCredentials SigningCredentials { get; }

    public string Issuer { get; }
    public string Audience { get; }
    public string PublicKeyPem { get; }
    public int ExpirationTimeInHours { get; }

    public AuthConfig(ECDsa privateKey, int expirationTimeInHours, string issuer, string audience)
    {
        PublicKeyPem = privateKey.ExportSubjectPublicKeyInfoPem();
        SigningCredentials = new SigningCredentials(new ECDsaSecurityKey(privateKey), SecurityAlgorithms.EcdsaSha512);
        Issuer = issuer;
        Audience = audience;
        ExpirationTimeInHours = expirationTimeInHours;
    }
}

public static class JwtAuthExtensions
{
    public static void AddAuth(this IServiceCollection services, ConfigurationManager config)
    {
        // Generate Public and Private Keys
        var key = ECDsa.Create(ECCurve.NamedCurves.nistP521);
        var publicKey = key.ExportSubjectPublicKeyInfoPem();
        var privateKey = key.ExportPkcs8PrivateKeyPem();

        var privatePkcs8Pem = """
            -----BEGIN PRIVATE KEY-----
            MIHuAgEAMBAGByqGSM49AgEGBSuBBAAjBIHWMIHTAgEBBEIAgQygOUrcMw4qB3q3
            CzuPFgYAMFQsUDczYxiL5tm1eXX8sqzjSmo2BR2I5WvUd0cJxufqi41nbzWXqW45
            2gIWgLyhgYkDgYYABAEMVwfgJRQdQ1Uw+XSbcIv+BGTlvic2PWTxp3iug4AhXQsy
            8JLZ9Q5+fPj6y1XEJ/3iYDOovnQT2OkR52gcMXe1OAD8d+LDIPslCwQ+RHjyUdTp
            GwCCRJ3BcIHtd0iLav+1npUd7WKVEwokmo/KMLkP+qx95g2Io9WVrfPpZXIO/cFu
            UQ==
            -----END PRIVATE KEY-----
            """;

        key.ImportFromPem(privatePkcs8Pem);


        var y = key.ExportSubjectPublicKeyInfoPem();
        var y2 = key.ExportPkcs8PrivateKeyPem();

        var jwtOptions = config.GetRequiredSection("JwtOptions")
                               .Get<JwtOptions>();
        ArgumentNullException.ThrowIfNull(jwtOptions);

        //var keyBytes = Encoding.UTF8.GetBytes(jwtOptions.SigningKey);
        //var jwtSymmetricKey = new SymmetricSecurityKey(keyBytes);
        var expirationTimeInHours = 16;
#if DEBUG
        expirationTimeInHours = 7 * 24;
#endif
        var authConfig = new AuthConfig(key, expirationTimeInHours, jwtOptions.Issuer, jwtOptions.Audience);
        services.AddScoped<AuthService>(s =>
        {
            var usersSErvice = s.GetRequiredService<UsersService>();
            return new AuthService(authConfig, usersSErvice);
        });

        //services.AddScoped<AuthManager>();
        //services.AddScoped<IAuthManager>(s => s.GetRequiredService<AuthManager>());

        //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience,
                        IssuerSigningKey = new ECDsaSecurityKey(key),
                    };
                    options.MapInboundClaims = false;
                });

    }
}

