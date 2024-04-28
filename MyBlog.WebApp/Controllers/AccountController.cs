using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Application.Exceptions;
using MyBlog.Application.Models;
using MyBlog.Application.Services;
using MyBlog.Domain.Models;
using MyBlog.WebApp.Models;
using System.Security.Claims;

namespace MyBlog.WebApp.Controllers;

public class AccountController(UserService userService, RoleService roleService, ILogger<AccountController> logger) : Controller
{
    private readonly UserService _userService = userService;
    private readonly RoleService _roleService = roleService;
    private readonly ILogger<AccountController> _logger = logger;

    [Authorize(Roles = "Администратор")]
    public async Task<IActionResult> All()
    {
        IEnumerable<UserDTO> users = await _userService.FindAll();
        return View(users);
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        if (loginRequest.Email != null && loginRequest.Password != null)
        {
            try
            {
                UserDTO user = await _userService.FindByEmail(loginRequest.Email);
                if (await _userService.ValidatePassword(user, loginRequest.Password))
                {
                    List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                    };

                    foreach (var role in user.Roles)
                    {
                        claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Name));
                    }

                    ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(new ClaimsPrincipal(identity));
                    _logger.LogInformation($"Пользователь ID {user.Id}: успешный вход.");
                    return RedirectToAction("Index", "Home");
                }
                else 
                {
                    _logger.LogInformation($"Пользователь ID {user.Id}: Неверный пароль.");
                }
            }
            catch (KeyNotFoundException)
            {
                _logger.LogInformation($"Пользователь {loginRequest.Email} не найден.");
            }
        }
        ModelState.AddModelError("", "Неверный логин или пароль.");
        return View(loginRequest);
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(AccountRegisterViewModel accountRegisterViewModel)
    {
        if (ModelState.IsValid)
        {
            CreateUserRequest request = new(
                accountRegisterViewModel.FirstName,
                accountRegisterViewModel.LastName,
                accountRegisterViewModel.Password,
                accountRegisterViewModel.Email);
            try
            {
                UserDTO createdUser = await _userService.Create(request);
                _logger.LogInformation($"Зарегистрирован новый пользователь {createdUser.Email}.");
                LoginRequest loginRequest = new LoginRequest()
                {
                    Email = accountRegisterViewModel.Email,
                    Password = accountRegisterViewModel.Password
                };
                return await Login(loginRequest);
            }
            catch (EntityAlreadyExistsException)
            {
                ModelState.AddModelError("Email", "E -Mail уже занят.");
            }
        }

        return View(accountRegisterViewModel);
    }

    [Authorize(Roles = "Администратор")]
    [HttpGet]
    public async Task<IActionResult> Delete(long id)
    {
        try
        {
            UserDTO user = await _userService.Delete(id);
            _logger.LogInformation($"Пользователь ID {id} удален");
            return RedirectToAction("All");
        }
        catch (KeyNotFoundException)
        {
            string errorMessage = $"Пользователь [Id = {id}] не найден.";
            _logger.LogError(errorMessage);
            return BadRequest(errorMessage);
        }
    }

    public async Task<ActionResult> Details(long id)
    {
        try
        {
            UserDTO user = await _userService.FindById(id);
            UserDetailsViewModel userDetailsViewModel = new UserDetailsViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                RoleNames = user.Roles.Select(role => role.Name)
            };
            return View(userDetailsViewModel);
        }
        catch (KeyNotFoundException)
        {
            string errorMessage = $"Пользователь [Id = {id}] не найден.";
            _logger.LogError(errorMessage);
            return BadRequest(errorMessage);
        }
    }

    
    [HttpGet]
    public async Task<IActionResult> Edit(long id)
    {
        try
        {
            UserDTO user = await _userService.FindById(id);

            EditUserViewModel model = new EditUserViewModel()
            {
                Id = id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                AllRoleNames = _roleService.FindAll().Result.Select(r => r.Name),
                UserRoleNames = user.Roles.Select(r => r.Name)
            };
            return View(model);
        }
        catch(KeyNotFoundException)
        {
            string errorMessage = $"Пользователь [Id = {id}] не найден.";
            _logger.LogError(errorMessage);
            return BadRequest(errorMessage);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditUserViewModel model)
    {
        IEnumerable<RoleDTO> allRoles = await _roleService.FindAll();

        model.AllRoleNames = allRoles.Select(r => r.Name);

        if (ModelState.IsValid)
        {
            try
            {
                List<RoleDTO> userRoles = new List<RoleDTO>();
                foreach (string userRoleName in model.UserRoleNames)
                {
                    userRoles.Add(allRoles.First(r => r.Name == userRoleName));
                }
                UpdateUserRequest request = new(
                    model.Id,
                    model.FirstName,
                    model.LastName,
                    model.NewPassword,
                    userRoles);

                await _userService.Update(request);
                return RedirectToAction("All");
            }
            catch (Exception)
            {
                string errorMessage = $"Пользователь [Id = {model.Id}] не найден.";
                _logger.LogError(errorMessage);
                return BadRequest(errorMessage);
            }
        }

        model.UserRoleNames ??= new List<string>();
        return View(model);
    }
}




