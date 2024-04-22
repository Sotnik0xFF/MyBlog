using MyBlog.Application.Exceptions;
using MyBlog.Application.Models;
using MyBlog.Domain.Interfaces;
using MyBlog.Domain.Models;

namespace MyBlog.Application.Services;

public class UserService(IUserRepository userRepository, IRoleRepository roleRepository)
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IRoleRepository _roleRepository = roleRepository;

    public async Task<UserDTO> Create(CreateUserRequest createUserRequest)
    {
        Role userRole = await _roleRepository.GetUserRole();

        User? user = null;

        user = await _userRepository.FindByEmail(createUserRequest.Email);
        if (user != null)
            throw new EntityAlreadyExistsException();

        user = new User(
            createUserRequest.Password,
            createUserRequest.FirstName,
            createUserRequest.LastName,
            createUserRequest.Email);
        user.AddRole(userRole);

        _userRepository.Add(user);
        await _userRepository.UnitOfWork.SaveChangesAsync();

        return Map(user);
    }

    public async Task<UserDTO> FindById(long id)
    {
        User? user = await _userRepository.FindById(id);
        if (user == null)
            throw new KeyNotFoundException(nameof(id));

        return Map(user);
    }

    public async Task<UserDTO> FindByEmail(string email)
    {
        User? user = await _userRepository.FindByEmail(email);
        if (user == null)
            throw new KeyNotFoundException(nameof(email));

        return Map(user);
    }

    public async Task<IEnumerable<UserDTO>> FindAll()
    {
        var users = await _userRepository.FindAll();

        List<UserDTO> results = new();
        foreach (var user in users)
        {
            UserDTO userDetails = Map(user);
            results.Add(userDetails);
        }

        return results;
    }

    public async Task<UserDTO> Delete(long id)
    {
        User? user = await _userRepository.FindById(id);

        if (user == null)
            throw new KeyNotFoundException(nameof(id));

        _userRepository.Delete(user);
        await _userRepository.UnitOfWork.SaveChangesAsync();
        return Map(user);
    }

    public async Task<UserDTO> Update(UpdateUserRequest updateUserRequest)
    {
        User? updatingUser = await _userRepository.FindById(updateUserRequest.Id);
        if (updatingUser == null)
            throw new KeyNotFoundException(nameof(updateUserRequest.Id));

        updatingUser.FirstName = updateUserRequest.FirstName;
        updatingUser.LastName = updateUserRequest.LastName;
        if (!String.IsNullOrEmpty(updateUserRequest.NewPassword))
        {
            updatingUser.SetNewPassword(updateUserRequest.NewPassword);
        }

        updatingUser.ClearRoles();
        IEnumerable<Role> allRoles = await _roleRepository.FindAll();
        foreach (var userRoleModel in updateUserRequest.Roles)
        {
            Role role = allRoles.First(r => r.Id == userRoleModel.Id);
            updatingUser.AddRole(role);
        }

        _userRepository.Update(updatingUser);
        await _userRepository.UnitOfWork.SaveChangesAsync();
        return Map(updatingUser);
    }

    public async Task<bool> ValidatePassword(UserDTO userDetails, string password)
    {
        User? user = await _userRepository.FindById(userDetails.Id);
        if (user == null)
            return false;

        return user.Password == password;
    }

    private UserDTO Map(User user)
    {
        UserDTO userDTO = userDTO = new(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.Roles.Select(r => new RoleDTO(r.Id, r.Name, r.Description)).ToArray());
        return userDTO;
    }
}
