using Microsoft.AspNetCore.Mvc;
using MyBlog.Application.Models;
using MyBlog.Application.Services;
using MyBlog.Domain.Interfaces;

namespace MyBlog.WebApp.Controllers;

public class RoleController(RoleService roleService) : Controller
{
    private readonly RoleService _roleService = roleService;

    public async Task<IActionResult> All()
    {
        IEnumerable<RoleDTO> roles = await _roleService.FindAll();
        return View(roles);
    }
}
