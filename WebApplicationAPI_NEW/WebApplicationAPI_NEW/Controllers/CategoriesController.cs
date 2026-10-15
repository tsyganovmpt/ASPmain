using ApiAsp0.Models;
using ApiAsp0.Services;
using ApiAsp0.DTO.DTOcategory;
using Microsoft.AspNetCore.Mvc;

namespace ApiAsp0.Controllers;

[Route("api/[controller]")]
public sealed class CategoriesController : CrudControllerBase<Category, CategoryDto, CreateCategoryDto, UpdateCategoryDto, PatchCategoryDto>
{
    public CategoriesController(ICrudService<Category, CategoryDto, CreateCategoryDto, UpdateCategoryDto, PatchCategoryDto> service)
        : base(service) { }
}
