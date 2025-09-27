using AutoMapper;
using BCrypt.Net;
using eCommerce.Core.DTO;
using eCommerce.Core.DTO.Customer;
using eCommerce.Core.Entities;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Core.ServiceContracts;
using System.Data;

namespace eCommerce.Core.Service;

public class UserService : IUsersService
{
    private readonly IjwtRepository _jwt;
    private readonly IUsersRepository _usersRepository;
    private readonly IMapper _mapper;
    public UserService(IUsersRepository _user,IMapper mapper, IjwtRepository jwt)
    {
        this._usersRepository = _user;
        this._mapper = mapper;
        _jwt = jwt;
    }

    public async Task<bool> DeleteCustomer(Guid userId)
    {
        await _usersRepository.DeleteCustomer(userId);
        return true;
    }

    public async Task<IEnumerable<CustomerDTO>> GetAllCustomer(string role)
    {
        var result  = await _usersRepository.GetAllCustomer(role);
        return result;
    }

    public async Task<CustomerDTO?> GetCustomerById(Guid userId, string role)
    {
        var rsult  = await _usersRepository.GetCustomerById(userId,role);
        return rsult;
    }

    //public async Task<IEnumerable<CustomerDTO>> GetUserByID(Guid userId)
    //{
    //    var result = await _usersRepository.GetCustomerById(userId);
    //    return (IEnumerable<CustomerDTO>)result;
    //}

    public async Task<AuthenticationResponse?> Login(LoginRequest loginRequest)
    {
        ApplicationUser user =   await  _usersRepository.GetUserByEmailAndPassword(loginRequest.Email,loginRequest.Password);
        if (user == null)
        {
            return null;
        }
        //  return new AuthenticationResponse(user.UserId, user.Email, user.PersonName,user.Gender,"token",Sucess: true);

        var roles = "Admin";
        var token = "";

        return _mapper.Map<AuthenticationResponse>(user) with
      {
          Success =true,
          Token = token
        }; 
    }

    public async Task<AuthenticationResponse?> Register(RegisterRequest registerRequest)
    {
        ApplicationUser user = new ApplicationUser
        {
            FullName = registerRequest.FullName,
            Email = registerRequest.Email,
            Phone = registerRequest.Phone,
            Role = registerRequest.Role ?? "Customer",
            IsActive = true,
            Status = "Active",

            PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password)
        };

        ApplicationUser? registeredUser = await _usersRepository.AddUser(user);

        if (registeredUser == null)
        {
            return null;
            
        }
        return _mapper.Map<AuthenticationResponse?>(user) with
        {
            Success = true,
            Token = "token"
        };
    //    return new AuthenticationResponse(registeredUser.UserId,registeredUser.Email,registeredUser.PersonName,registeredUser.Gender, "token",true);
    }

    public async Task<bool> UpdateCustomer(Guid custId, EditCustomer customer)
    {
       await _usersRepository.UpdateCustomer(custId,customer);
        return true;
    }
}
