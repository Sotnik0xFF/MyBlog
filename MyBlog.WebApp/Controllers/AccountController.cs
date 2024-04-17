﻿using Microsoft.AspNetCore.Authentication;
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

    [Authorize(Roles = "Администратор")]
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
            UpdateUserRequest updateUserRequest = new() { Id = user.Id, FirstName = user.FirstName, LastName = user.LastName, Roles = user.Roles };

            EditUserViewModel model = new EditUserViewModel() { Request = updateUserRequest, AllRoles = await _roleService.FindAll()};
            return View(model);
        }
        catch(KeyNotFoundException)
        {
            return BadRequest($"Пользователь [Id = {id}] не найден.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditUserViewModel model, List<string> roles)
    {
        model.AllRoles = await _roleService.FindAll();
        model.Request.Id = model.Id;

        List<RoleViewModel> userRoles = new List<RoleViewModel>();
        foreach (string roleName in roles) 
        {
            userRoles.Add(model.AllRoles.First(r => r.Name == roleName));
        }
        model.Request.Roles = userRoles;
        try
        {
            await _userService.Update(model.Request);
            return RedirectToAction("Index", "Home");
        }
        catch (Exception)
        {
            return BadRequest($"Пользователь [Id = {model.Request.Id}] не найден.");
        }
    }
}




