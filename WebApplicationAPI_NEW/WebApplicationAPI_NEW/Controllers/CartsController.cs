using ApiAsp0.Models;
using ApiAsp0.Services;
using ApiAsp0.DTO.DTOcart;
using Microsoft.AspNetCore.Mvc;

namespace ApiAsp0.Controllers;

[Route("api/[controller]")]
public sealed class CartsController : CrudControllerBase<Cart, CartDto, CreateCartDto, UpdateCartDto, PatchCartDto>
{
    public CartsController(ICrudService<Cart, CartDto, CreateCartDto, UpdateCartDto, PatchCartDto> service)
        : base(service) { }
}
