using ApiAsp0.Models;
using ApiAsp0.Services;
using ApiAsp0.DTO.DTOorder;
using Microsoft.AspNetCore.Mvc;

namespace ApiAsp0.Controllers;

[Route("api/[controller]")]
public sealed class OrdersController : CrudControllerBase<Order, OrderDto, CreateOrderDto, UpdateOrderDto, PatchOrderDto>
{
    public OrdersController(ICrudService<Order, OrderDto, CreateOrderDto, UpdateOrderDto, PatchOrderDto> service)
        : base(service) { }
}
