using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Application.Models;
using MyBlog.Application.Services;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyBlog.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class RoleController(RoleService roleService) : ControllerBase
{
    private readonly RoleService _roleService = roleService;

    /// <summary>
    /// Возвращает список всех ролей.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RoleDTO>), StatusCodes.Status200OK)]
    public async Task<Ok<IEnumerable<RoleDTO>>> Get()
    {
        return TypedResults.Ok(await _roleService.FindAll());
    }

}
