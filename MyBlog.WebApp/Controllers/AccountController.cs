using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Application.Exceptions;
using MyBlog.Application.Models;
using MyBlog.Application.Services;
using System.Security.Claims;

namespace MyBlog.WebApp.Controllers
{
    public class AccountController(UserService userService) : Controller
    {
        private readonly UserService _userService = userService;

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

    }
}
