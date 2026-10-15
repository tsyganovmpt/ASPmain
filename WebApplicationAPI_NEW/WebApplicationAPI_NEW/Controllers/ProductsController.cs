using ApiAsp0.Models;
using ApiAsp0.Services;
using ApiAsp0.DTO.DTOproduct;
using Microsoft.AspNetCore.Mvc;

namespace ApiAsp0.Controllers;

[Route("api/[controller]")]
public sealed class ProductsController : CrudControllerBase<Product, ProductDto, CreateProductDto, UpdateProductDto, PatchProductDto>
{
    public ProductsController(ICrudService<Product, ProductDto, CreateProductDto, UpdateProductDto, PatchProductDto> service)
        : base(service) { }
}
