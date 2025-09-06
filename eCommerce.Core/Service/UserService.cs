using AutoMapper;
using eCommerce.Core.DTO;
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
    public async Task<AuthenticationResponse?> Login(LoginRequest loginRequest)
    {
        ApplicationUser user =   await
        _usersRepository.GetUserByEmailAndPassword(loginRequest.Email,loginRequest.Password);
        if (user == null)
        {
            return null;
        }
        //  return new AuthenticationResponse(user.UserId, user.Email, user.PersonName,user.Gender,"token",Sucess: true);

        var roles = "Admin";
        var token = "";

        return _mapper.Map<AuthenticationResponse>(user) with
      {
          Sucess =true,
          Token = token
        }; 
    }

    public async Task<AuthenticationResponse?> Register(RegisterRequest registerRequest)
    {
        ApplicationUser user = new ApplicationUser()
        {
            PersonName = registerRequest.PersonName, Email = registerRequest.Email,
            Password = registerRequest.Password, Gender = registerRequest.Gender.ToString()
        };

        ApplicationUser? registeredUser = await
         _usersRepository.AddUser(user);
        if (registeredUser == null)
        {
            return null;
            
        }
        return _mapper.Map<AuthenticationResponse?>(user) with
        {
            Sucess = true,
            Token = "token"
        };
    //    return new AuthenticationResponse(registeredUser.UserId,registeredUser.Email,registeredUser.PersonName,registeredUser.Gender, "token",true);
    }
}
