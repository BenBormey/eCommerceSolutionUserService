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
