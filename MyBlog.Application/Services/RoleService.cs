﻿using MyBlog.Application.Models;
using MyBlog.Domain.Interfaces;
using MyBlog.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Application.Services;

public class RoleService(IRoleRepository roleRepository)
{
    private readonly IRoleRepository _roleRepository = roleRepository;

    public async Task<IEnumerable<RoleViewModel>> FindAll()
    {
        IEnumerable<Role> roles = await _roleRepository.FindAll();
        List<RoleViewModel> roleModels = new();
        foreach (Role role in roles)
        {
            roleModels.Add(Map(role));
        }
        return roleModels;
    }

    private RoleViewModel Map(Role role)
    {
        return new RoleViewModel(role.Id, role.Name, role.Description);
    }
}
