﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Application.Models;
using MyBlog.Application.Services;
using System.Security.Claims;

namespace MyBlog.WebApp.Controllers
{
    public class AccountController(UserService userService) : Controller
    {
        private readonly UserService _userService = userService;

        public async Task<IActionResult> Login(Application.Models.LoginRequest loginRequest)
        {
            if (loginRequest.UserLogin != null && loginRequest.Password != null)
            {
                UserViewModel user = await _userService.FindByLogin(loginRequest.UserLogin);

                if (await _userService.ValidatePassword(user, loginRequest.Password))
                {
                    List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                    };

                    foreach (var role in user.Roles)
                    {
                        claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Name));
                    }

                    ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(new ClaimsPrincipal(identity));
                    return Ok($"Привет, {user.FirstName} {user.LastName}");
                }
                else
                {
                    return BadRequest("Неверный логин или пароль.");
                }
            }

            return BadRequest();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            string currentUserName = HttpContext.User.FindFirstValue(ClaimsIdentity.DefaultNameClaimType) ?? String.Empty;
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok($"Возвращайтесь, {currentUserName}");
        }

    }
}