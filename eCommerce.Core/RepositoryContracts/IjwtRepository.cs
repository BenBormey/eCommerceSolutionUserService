using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.RepositoryContracts
{
    public interface IjwtRepository
    {

        /// Generate JWT token from given user info and roles
        /// </summary>
        /// <param name="userId">Unique user id</param>
        /// <param name="username">Username or email</param>
        /// <param name="roles">List of roles</param>
        /// <returns>JWT token as string</returns>
        string GenerateToken(Guid userId, string username, string roles);

        /// <summary>
        /// Validate JWT token and return principal (claims identity)
        /// </summary>
        /// <param name="token">JWT token</param>
        /// <returns>ClaimsPrincipal if valid, otherwise null</returns>
        ClaimsPrincipal? ValidateToken(string token);

        /// <summary>
        /// Extract userId from token claims
        /// </summary>
        Guid? GetUserIdFromToken(string token);

    }
}
