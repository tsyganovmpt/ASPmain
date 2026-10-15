using ApiAsp0.Models;
using ApiAsp0.Services;
using ApiAsp0.DTO.DTOrole;
using Microsoft.AspNetCore.Mvc;

namespace ApiAsp0.Controllers;

[Route("api/[controller]")]
public sealed class RolesController : CrudControllerBase<Role, RoleDto, CreateRoleDto, UpdateRoleDto, PatchRoleDto>
{
    public RolesController(ICrudService<Role, RoleDto, CreateRoleDto, UpdateRoleDto, PatchRoleDto> service)
        : base(service) { }
}
