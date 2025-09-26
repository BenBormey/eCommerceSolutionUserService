using BCrypt.Net;
using Dapper;
using eCommerce.Core.DTO;
using eCommerce.Core.DTO.Customer;
using eCommerce.Core.Entities;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Infrastructure.DbContext;
using System.Text.RegularExpressions;

namespace eCommerce.Infrastructure.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly DapperDbContext _context;
    public UsersRepository(DapperDbContext context) => _context = context;

    // Small helper to check if string looks like BCrypt hash
    private static bool LooksLikeBcrypt(string? s) =>
        !string.IsNullOrWhiteSpace(s) && Regex.IsMatch(s, @"^\$2[aby]\$\d{2}\$[./A-Za-z0-9]{53}$");

    // ✅ Add new user with hashed password
    public async Task<ApplicationUser?> AddUser(ApplicationUser user)
    {
        user.UserId = Guid.NewGuid();



        // Hash password if plain text was provided
        if (!LooksLikeBcrypt(user.PasswordHash))
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
        }

        const string sql = @"
INSERT INTO public.users
    (user_id, full_name, email, phone, password_hash, role, profile_image, status, created_at)
VALUES
    (@UserId, @FullName, @Email, @Phone, @PasswordHash, @Role, @ProfileImage, @Status, NOW())
RETURNING
    user_id       AS ""UserId"",
    full_name     AS ""FullName"",
    email,
    phone,
    password_hash AS ""PasswordHash"",
    role,
    profile_image AS ""ProfileImage"",
    status,
    created_at    AS ""CreatedAt"";";

        return await _context.DbConnection.QueryFirstOrDefaultAsync<ApplicationUser>(sql, user);

    }

    // ✅ Login user with email + password check
    public async Task<ApplicationUser?> GetUserByEmailAndPassword(string? email, string? password)
    {
        const string sql = @"
SELECT 
    user_id       AS ""UserId"",
    full_name     AS ""FullName"",
    email,
    phone as Phone,
    password_hash AS ""PasswordHash"",
    role,
    profile_image AS ""ProfileImage"",
    is_active     AS ""IsActive"",
    status,
    created_at    AS ""CreatedAt""
FROM public.users
WHERE lower(email) = lower(@Email)
LIMIT 1;";

        var user = await _context.DbConnection.QueryFirstOrDefaultAsync<ApplicationUser>(sql, new { Email = email });
        if (user is null) return null;

        // BCrypt verify
        if (!string.IsNullOrEmpty(password) && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return user;

        return null;
    }

    // Roles handling (if you’re using UserRoles / Roles tables)
    public async Task<bool> AddUserRoleAsync(Guid userId, Guid roleId)
    {
        const string sql = @"
INSERT INTO public.""UserRoles"" (""UserID"", ""RoleID"")
VALUES (@UserId, @RoleId)
ON CONFLICT DO NOTHING;";

        var rows = await _context.DbConnection.ExecuteAsync(sql, new { UserId = userId, RoleId = roleId });
        return rows > 0;
    }

    public async Task<Guid?> GetRoleIdByNameAsync(string roleName)
    {
        const string sql = @"SELECT ""RoleID"" FROM public.""Roles"" WHERE ""Name"" = @Name LIMIT 1;";
        return await _context.DbConnection.ExecuteScalarAsync<Guid?>(sql, new { Name = roleName });
    }

    public async Task<IEnumerable<string>> GetRolesAsync(Guid userId)
    {
        const string sql = @"
SELECT r.""Name""
FROM public.""UserRoles"" ur
JOIN public.""Roles"" r ON r.""RoleID"" = ur.""RoleID""
WHERE ur.""UserID"" = @UserId;";

        return await _context.DbConnection.QueryAsync<string>(sql, new { UserId = userId })
               ?? Enumerable.Empty<string>();
    }

    public async Task<IEnumerable<CustomerDTO>> GetAllCustomer()
    {
        var sql = @"
select 
user_id as UserId,
full_name as FullName,
email as Email,
phone as Phone,
role as Role,
profile_image as ProfileImage,
status as Status 

from public.users where role = 'Customer'
order by full_name;";
        var result  = await _context.DbConnection.QueryAsync<CustomerDTO>(sql);
        return result;
    }

    public Task<CustomerDTO?> GetCustomerById(Guid userId)
    {
        var sql = $@"select 
user_id as UserId,
full_name as FullName,
email as Email,
phone as Phone,
role as Role,
profile_image as ProfileImage,
status as Status 

from public.users where role = 'Customer' and user_id = '51ee687a-8553-4be0-97a3-7d32feded0a6'
order by full_name;";
        var result =  _context.DbConnection.QueryFirstOrDefaultAsync<CustomerDTO>(sql);
        return result;
    }

    public async Task<bool> UpdateCustomer(CustomerDTO customer)
    {
       var sql = $@"update  public.users set 
	full_name = '{customer.FullName}',
	email = '{customer.Email}',
	phone = '{customer.Phone}',
	role = '{customer.Role}',
	profile_image = '{customer.ProfileImage}',
	status = '{customer.Status}' 
	where userid = '{customer.UserId}'
;";
        var rows = await _context.DbConnection.ExecuteAsync(sql);
        return rows > 0;

    }

    public async Task<bool> DeleteCustomer(Guid userId)
    {
        var sql = $@"
delete from public.users where ""role"" = 'Customer' and user_id ='{userId}'
";
        var rows = await _context.DbConnection.ExecuteAsync(sql);
        return rows > 0;
    }
}
