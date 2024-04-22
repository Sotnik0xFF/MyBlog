using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Application.Exceptions;
using MyBlog.Application.Models;
using MyBlog.Application.Services;
using MyBlog.WebApp.Models;
using System.Security.Claims;

namespace MyBlog.WebApp.Controllers;

public class AccountController(UserService userService, RoleService roleService) : Controller
{
    private readonly UserService _userService = userService;
    private readonly RoleService _roleService = roleService;

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
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (KeyNotFoundException)
            {
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
                await _userService.Create(request);

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
            return RedirectToAction("All");
        }
        catch (KeyNotFoundException)
        {
            return BadRequest($"Пользователь [Id = {id}] не найден.");
        }
    }

    public async Task<ActionResult> Details(long id)
    {
        try
        {
            return Json(await _userService.FindById(id));
        }
        catch (KeyNotFoundException)
        {
            return BadRequest($"Пользователь [Id = {id}] не найден.");
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
            return BadRequest($"Пользователь [Id = {id}] не найден.");
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
                return BadRequest($"Пользователь [Id = {model.Id}] не найден.");
            }
        }

        model.UserRoleNames ??= new List<string>();
        return View(model);
    }
}




