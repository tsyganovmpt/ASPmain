using ApiAsp0.Models;
using ApiAsp0.Services;
using ApiAsp0.DTO.DTOorderItem;
using Microsoft.AspNetCore.Mvc;

namespace ApiAsp0.Controllers;

[Route("api/[controller]")]
public sealed class OrderItemsController : CrudControllerBase<OrderItem, OrderItemDto, CreateOrderItemDto, UpdateOrderItemDto, PatchOrderItemDto>
{
    public OrderItemsController(ICrudService<OrderItem, OrderItemDto, CreateOrderItemDto, UpdateOrderItemDto, PatchOrderItemDto> service)
        : base(service) { }
}
