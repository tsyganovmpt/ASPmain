using ApiAsp0.Models;
using ApiAsp0.Services;
using ApiAsp0.DTO.DTOuser;
using Microsoft.AspNetCore.Mvc;

namespace ApiAsp0.Controllers;

[Route("api/[controller]")]
public sealed class UsersController : CrudControllerBase<User, UserDto, CreateUserDto, UpdateUserDto, PatchUserDto>
{
    public UsersController(ICrudService<User, UserDto, CreateUserDto, UpdateUserDto, PatchUserDto> service)
        : base(service) { }
}
