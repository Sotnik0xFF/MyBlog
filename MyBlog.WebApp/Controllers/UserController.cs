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

    


    
}
