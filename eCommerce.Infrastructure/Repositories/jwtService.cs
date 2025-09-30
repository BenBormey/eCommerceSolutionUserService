using eCommerce.Core.Options;
using eCommerce.Core.RepositoryContracts;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Numerics;
using System.Security.Claims;
using System.Text;

namespace eCommerce.Core.Service
{
    public class JwtService : IjwtRepository
    {
        private readonly JwtOptions _opts;
        private readonly byte[] _keyBytes;
        private readonly TokenValidationParameters _validationParams;

        public JwtService(IOptions<JwtOptions> options)
        {
            _opts = options.Value ?? new JwtOptions();
            _keyBytes = Encoding.UTF8.GetBytes(_opts.SecretKey ?? string.Empty);

            _validationParams = new TokenValidationParameters
            {
                // Signature / issuer / audience
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(_keyBytes),
                ValidateIssuer = !string.IsNullOrWhiteSpace(_opts.Issuer),
                ValidateAudience = !string.IsNullOrWhiteSpace(_opts.Audience),
                ValidIssuer = _opts.Issuer,
                ValidAudience = _opts.Audience,

                // Lifetime
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(30),

                // Map which claim types represent name/role in resulting principal
                NameClaimType = ClaimTypes.Name,
                RoleClaimType = ClaimTypes.Role
            };
        }


        public string GenerateToken(Guid userId, string username, string roles)
            => GenerateTokenCore(userId, username, email: null, roles);

        public ClaimsPrincipal? ValidateToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return null;

            var handler = new JwtSecurityTokenHandler();
            try
            {
                var principal = handler.ValidateToken(token, _validationParams, out var validatedToken);

                // ensure alg is HS256 (same as we sign with)
                if (validatedToken is JwtSecurityToken jwt &&
                    jwt.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.OrdinalIgnoreCase))
                {
                    return principal;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public Guid? GetUserIdFromToken(string token)
        {
            var principal = ValidateToken(token);
            if (principal is null) return null;

            var id =
                principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ??
                principal.FindFirst("userId")?.Value;

            return Guid.TryParse(id, out var guid) ? guid : null;
        }

    
        private string GenerateTokenCore(Guid userId, string username, string? email,string? roles)
        {
            var now = DateTime.UtcNow;
            var expires = now.AddMinutes(_opts.AccessTokenMinutes > 0 ? _opts.AccessTokenMinutes : 60);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, userId.ToString()),       
                new(ClaimTypes.NameIdentifier, userId.ToString()),      
                new(JwtRegisteredClaimNames.UniqueName, username ?? string.Empty),
                new(ClaimTypes.Name, username ?? string.Empty),
                new(ClaimTypes.Role, roles ?? string.Empty),
          
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iat, ToUnixTime(now), ClaimValueTypes.Integer64)
            };

            if (!string.IsNullOrWhiteSpace(email))
            {
                claims.Add(new(JwtRegisteredClaimNames.Email, email));
                claims.Add(new(ClaimTypes.Email, email));
            }

            //if (roles != null)
            //{
            //    foreach (var r in roles)
            //    {
            //        var role = (r ?? string.Empty).Trim();
            //        if (!string.IsNullOrWhiteSpace(role))
            //            claims.Add(new Claim(ClaimTypes.Role, role));
            //    }
            //}

            var creds = new SigningCredentials(new SymmetricSecurityKey(_keyBytes), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: string.IsNullOrWhiteSpace(_opts.Issuer) ? null : _opts.Issuer,
                audience: string.IsNullOrWhiteSpace(_opts.Audience) ? null : _opts.Audience,
                claims: claims,
                notBefore: now,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static IEnumerable<string> ParseRolesCsv(string? csv)
            => string.IsNullOrWhiteSpace(csv)
                ? Array.Empty<string>()
                : csv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        private static string ToUnixTime(DateTime utc)
            => ((long)(utc - DateTime.UnixEpoch).TotalSeconds).ToString();
    }
}




