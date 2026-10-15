using ApiAsp0.Models;
using ApiAsp0.Services;
using ApiAsp0.DTO.DTOcartItem;
using Microsoft.AspNetCore.Mvc;

namespace ApiAsp0.Controllers;

[Route("api/[controller]")]
public sealed class CartItemsController : CrudControllerBase<CartItem, CartItemDto, CreateCartItemDto, UpdateCartItemDto, PatchCartItemDto>
{
    public CartItemsController(ICrudService<CartItem, CartItemDto, CreateCartItemDto, UpdateCartItemDto, PatchCartItemDto> service)
        : base(service) { }
}
