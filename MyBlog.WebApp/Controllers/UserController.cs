using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Application.Exceptions;
using MyBlog.Application.Models;
using MyBlog.Application.Services;
using MyBlog.Domain.Models;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace MyBlog.WebApp.Controllers;

public class UserController(UserService userService) : Controller
{
    private readonly UserService _userService = userService;
    private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    [Authorize]
    public async Task<ActionResult> Index()
    {
        return Json(await _userService.FindAll(), _jsonOptions);
    }

    [HttpPost]
    public async Task<ActionResult> Create(CreateUserRequest createUserRequest)
    {
        try
        {
            return Json(await _userService.Create(createUserRequest), _jsonOptions);
        }
        catch (EntityAlreadyExistsException)
        {
            return BadRequest("Пользователь с таким логином или e-mail уже существует.");
        }
    }

    [Authorize(Roles = "Администратор")]
    public async Task<ActionResult> Delete(long id)
    {
        try
        {
            UserViewModel user = await _userService.Delete(id);
            return Ok($"Пользователь {user.Login} удален.");
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
            return Json(await _userService.FindById(id), _jsonOptions);
        }
        catch (KeyNotFoundException)
        {
            return BadRequest($"Пользователь [Id = {id}] не найден.");
        }
    }

    public async Task<ActionResult> Update(UpdateUserRequest updateUserRequest)
    {
        try
        {
            await _userService.Update(updateUserRequest);
            return Ok("Информация о пользователе обновлена.");
        }
        catch (Exception)
        {
            return BadRequest($"Пользователь [Id = {updateUserRequest.Id}] не найден.");
        }
        
    }
}
