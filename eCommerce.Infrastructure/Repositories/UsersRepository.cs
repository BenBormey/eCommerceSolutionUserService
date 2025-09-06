using Dapper;
using eCommerce.Core.DTOl;
using eCommerce.Core.Entities;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Infrastructure.DbContext;

namespace eCommerce.Infrastructure.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly DapperDbContext _context;
    public UsersRepository(DapperDbContext context)
    {
        this._context = context;
        
    }
    public async Task<ApplicationUser?> AddUser(ApplicationUser user)
    {
        // Generate a new unique user ID for the user
        user.UserId = Guid.NewGuid();
        string  query = "INSERT INTO public.\"Users\"(\"UserID\", \"Email\", \"PersonName\", \"Gender\", \"Password\") VALUES(@UserID,@Email,@PersonName,@Gender,@Password)";
      
   int rowCountAffected  =    await  _context.DbConnection.ExecuteAsync(query,user);

        if (rowCountAffected > 0)
        {
            return user;
        }
        else
        {
            return null;
        }

        
       
    }

    public async Task<bool> AddUserRoleAsync(Guid userId, Guid roleId)
    {
   
        const string sql = @"
INSERT INTO public.""UserRoles"" (""UserID"", ""RoleID"")
VALUES (@UserId, @RoleId) ON CONFLICT DO NOTHING;";

        var rows = await _context.DbConnection.ExecuteAsync(sql, new { UserId = userId, RoleId = roleId });
        return rows > 0;
    }

    

    public async Task<Guid?> GetRoleIdByNameAsync(string roleName)
    {
   
        const string sql = @"
SELECT ""RoleID""
FROM public.""Roles""
WHERE ""Name"" = @Name
LIMIT 1;";

        return await _context.DbConnection.ExecuteScalarAsync<Guid?>(sql, new { Name = roleName });
    }

    

    public async Task<IEnumerable<string>> GetRolesAsync(Guid userId)
    {
        const string sql = @"
SELECT r.""Name""
FROM public.""UserRoles"" ur
JOIN public.""Roles"" r ON r.""RoleID"" = ur.""RoleID""
WHERE ur.""UserID"" = @UserId;";

        var roles = await _context.DbConnection.QueryAsync<string>(sql, new { UserId = userId });
        return roles ?? Enumerable.Empty<string>();
    }

    public async Task<ApplicationUser?> GetUserByEmailAndPassword(string? email, string? password)
    {
        return new ApplicationUser()
        {
            UserId = Guid.NewGuid(),
            Email = email,

            Password = password,
            PersonName = "Person Name",
            Gender = GenderOption.Male.ToString()
        };
    }
}
