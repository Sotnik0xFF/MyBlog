using MyBlog.Application.Exceptions;
using MyBlog.Application.Models;
using MyBlog.Domain.Interfaces;
using MyBlog.Domain.Models;

namespace MyBlog.Application.Services;

public class UserService(IUserRepository userRepository, IRoleRepository roleRepository)
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IRoleRepository _roleRepository = roleRepository;

    public async Task<UserDetails> Create(CreateUserRequest createUserRequest)
    {
        Role userRole = await _roleRepository.GetUserRole();

        User? user = null;

        user = await _userRepository.FindByLogin(createUserRequest.Login);
        if (user != null)
            throw new EntityAlreadyExistsException();

        user = new User(
            createUserRequest.Login,
            createUserRequest.Password,
            createUserRequest.FirstName,
            createUserRequest.LastName,
            createUserRequest.Email);
        user.AddRole(userRole);

        _userRepository.Add(user);
        await _userRepository.UnitOfWork.SaveChangesAsync();

        return Map(user);
    }

    public async Task<UserDetails> FindById(long id)
    {
        User? user = await _userRepository.FindById(id);
        if (user == null)
            throw new KeyNotFoundException(nameof(id));

        return Map(user);
    }

    public async Task<UserDetails> FindByLogin(string login)
    {
        User? user = await _userRepository.FindByLogin(login);
        if (user == null)
            throw new KeyNotFoundException(nameof(login));

        return Map(user);
    }

    public async Task<IEnumerable<UserDetails>> FindAll()
    {
        var users = await _userRepository.FindAll();

        List<UserDetails> results = new();
        foreach (var user in users)
        {
            UserDetails userDetails = Map(user);
            results.Add(userDetails);
        }

        return results;
    }

    public async Task<UserDetails> Delete(long id)
    {
        User? user = await _userRepository.FindById(id);

        if (user == null)
            throw new KeyNotFoundException(nameof(id));

        _userRepository.Delete(user);
        await _userRepository.UnitOfWork.SaveChangesAsync();
        return Map(user);
    }

    public async Task<UserDetails> Update(UpdateUserRequest updateUserRequest)
    {
        User? user = await _userRepository.FindById(updateUserRequest.Id);
        if (user == null)
            throw new KeyNotFoundException(nameof(updateUserRequest.Id));

        user.FirstName = updateUserRequest.FirstName;
        user.LastName = updateUserRequest.LastName;
        _userRepository.Update(user);
        await _userRepository.UnitOfWork.SaveChangesAsync();
        return Map(user);
    }

    public async Task<bool> ValidatePassword(UserDetails userDetails, string password)
    {
        User? user = await _userRepository.FindById(userDetails.Id);
        if (user == null)
            return false;

        return user.Password == password;
    }

    private UserDetails Map(User user)
    {
        UserDetails userDetails = userDetails = new()
        {
            Id = user.Id,
            Login = user.Login,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Roles = user.Roles.Select(r => r.Name).ToArray()
        };
        return userDetails;
    }
}
