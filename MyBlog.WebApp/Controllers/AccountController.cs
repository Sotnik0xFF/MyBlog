using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Application.Exceptions;
using MyBlog.Application.Models;
using MyBlog.Application.Services;
using System.Security.Claims;

namespace MyBlog.WebApp.Controllers;

public class AccountController(UserService userService) : Controller
{
    private readonly UserService _userService = userService;

    public async Task<IActionResult> All()
    {
        IEnumerable<UserViewModel> users = await _userService.FindAll();
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
                UserViewModel user = await _userService.FindByEmail(loginRequest.Email);
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
        string currentUserName = HttpContext.User.FindFirstValue(ClaimsIdentity.DefaultNameClaimType) ?? String.Empty;
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(CreateUserRequest createUserRequest)
    {
        try
        {
            await _userService.Create(createUserRequest);
            LoginRequest loginRequest = new LoginRequest() { Email = createUserRequest.Email, Password = createUserRequest.Password };
            return await Login(loginRequest);
        }
        catch (EntityAlreadyExistsException)
        {
            ModelState.AddModelError("", "E-Mail уже занят.");
        }

        return View(createUserRequest);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(long id)
    {
        try
        {
            UserViewModel user = await _userService.Delete(id);
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
            UserViewModel user = await _userService.FindById(id);
            UpdateUserRequest updateUserRequest = new() { Id = user.Id, FirstName = user.FirstName, LastName = user.LastName };
            return View(updateUserRequest);
        }
        catch(KeyNotFoundException)
        {
            return BadRequest($"Пользователь [Id = {id}] не найден.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UpdateUserRequest updateUserRequest)
    {
        try
        {
            await _userService.Update(updateUserRequest);
            return RedirectToAction("Index", "Home");
        }
        catch (Exception)
        {
            return BadRequest($"Пользователь [Id = {updateUserRequest.Id}] не найден.");
        }
    }
}




