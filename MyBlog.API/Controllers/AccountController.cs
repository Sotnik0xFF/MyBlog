﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Application.Exceptions;
using MyBlog.Application.Models;
using MyBlog.Application.Services;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyBlog.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController(UserService userService) : ControllerBase
{
    private readonly UserService _userService = userService;

    [HttpGet]
    public async Task<Ok<IEnumerable<UserDTO>>> GetAll()
    {
        var users = await _userService.FindAll();
        return TypedResults.Ok(users);
    }

    [HttpGet("{id:long}")]
    public async Task<Results<Ok<UserDTO>, NotFound>> GetById(long id)
    {
        try
        {
            UserDTO user = await _userService.FindById(id);
            return TypedResults.Ok(user);
        }
        catch (KeyNotFoundException)
        {
            return TypedResults.NotFound();
        }
    }

    [HttpPost]
    [Route("register")]
    public async Task<Results<Ok, BadRequest<string>>> Register([FromBody] CreateUserRequest createUserRequest)
    {
        try
        {
            UserDTO createdUser = await _userService.Create(createUserRequest);
            return TypedResults.Ok();
        }
        catch (EntityAlreadyExistsException)
        {
            return TypedResults.BadRequest("E-Mail уже занят.");
        }
    }

    [HttpPut]
    public async Task<Results<Ok, NotFound>> Put([FromBody] UpdateUserRequest updateUserRequest)
    {
        try
        {
            await _userService.Update(updateUserRequest);
            return TypedResults.Ok();
        }
        catch (KeyNotFoundException)
        {
            return TypedResults.NotFound();
        }
        
    }

    // DELETE api/<AccountController>/5
    [HttpDelete("{id:long}")]
    public async Task<Results<Ok, NotFound>> Delete(long id)
    {
        try
        {
            await _userService.Delete(id);
            return TypedResults.Ok();
        }
        catch (KeyNotFoundException)
        {
            return TypedResults.NotFound();
        }
    }

    [HttpPost]
    [Route("login")]
    public async Task<Results<Ok, NotFound, BadRequest<string>>> LogIn([FromBody] LoginRequest loginRequest)
    {
        try
        {
            UserDTO user = await _userService.FindByEmail(loginRequest.Email);
            if (await _userService.ValidatePassword(user, loginRequest.Password))
            {
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email)
                };

                foreach (var role in user.Roles)
                {
                    claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Name));
                }

                ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(new ClaimsPrincipal(identity));
                return TypedResults.Ok();
            }
            else
            {
                return TypedResults.BadRequest("Неверный логин или пароль.");
            }
        }
        catch (KeyNotFoundException)
        {
            return TypedResults.NotFound();
        }
    }

    [HttpPost]
    [Route("logout")]
    public async Task LogOut()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}
