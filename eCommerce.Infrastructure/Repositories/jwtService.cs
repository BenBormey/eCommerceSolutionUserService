using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using eCommerce.Core.Options;
using eCommerce.Core.RepositoryContracts;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

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
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(_keyBytes),
                ValidateIssuer = !string.IsNullOrWhiteSpace(_opts.Issuer),
                ValidateAudience = !string.IsNullOrWhiteSpace(_opts.Audience),
                ValidIssuer = _opts.Issuer,
                ValidAudience = _opts.Audience,
                ClockSkew = TimeSpan.FromSeconds(5)
            };
        }

        public string GenerateToken(Guid userId, string username, string roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, username ?? string.Empty),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, username ?? string.Empty)
            };

            if (roles != null)
            {
              
                    if (!string.IsNullOrWhiteSpace(roles))
                    {
                        claims.Add(new Claim(ClaimTypes.Role, roles));
                    }
                
            }

            var creds = new SigningCredentials(new SymmetricSecurityKey(_keyBytes), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: string.IsNullOrWhiteSpace(_opts.Issuer) ? null : _opts.Issuer,
                audience: string.IsNullOrWhiteSpace(_opts.Audience) ? null : _opts.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(_opts.AccessTokenMinutes > 0 ? _opts.AccessTokenMinutes : 60),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return null;
            var handler = new JwtSecurityTokenHandler();
            try
            {
                var principal = handler.ValidateToken(token, _validationParams, out var validatedToken);
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
    }
}
