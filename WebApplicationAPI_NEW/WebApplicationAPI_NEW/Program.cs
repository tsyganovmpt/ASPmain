using ApiAsp0.DTO.DTOcart;
using ApiAsp0.DTO.DTOcartItem;
using ApiAsp0.DTO.DTOcategory;
using ApiAsp0.DTO.DTOorder;
using ApiAsp0.DTO.DTOorderItem;
using ApiAsp0.DTO.DTOproduct;
using ApiAsp0.DTO.DTOrole;
using ApiAsp0.DTO.DTOuser;
using ApiAsp0.Mapping;
using ApiAsp0.Models;
using ApiAsp0.Repositories;
using ApiAsp0.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApiAsp0;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<AspfisrtContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("con")));

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        RegisterCrudServices(builder.Services);
        builder.Services.AddScoped<IReservationService, ReservationService>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }

    private static void RegisterCrudServices(IServiceCollection services)
    {
        services.AddScoped<ICrudService<Role, RoleDto, CreateRoleDto, UpdateRoleDto, PatchRoleDto>>(sp =>
            new CrudService<Role, RoleDto, CreateRoleDto, UpdateRoleDto, PatchRoleDto>(
                sp.GetRequiredService<IRepository<Role>>(), sp.GetRequiredService<AutoMapper.IMapper>(),
                nameof(Role.IdRole), e => e.IdRole, "Users"));

        services.AddScoped<ICrudService<User, UserDto, CreateUserDto, UpdateUserDto, PatchUserDto>>(sp =>
            new CrudService<User, UserDto, CreateUserDto, UpdateUserDto, PatchUserDto>(
                sp.GetRequiredService<IRepository<User>>(), sp.GetRequiredService<AutoMapper.IMapper>(),
                nameof(User.IdUsers), e => e.IdUsers, "Role"));

        services.AddScoped<ICrudService<Category, CategoryDto, CreateCategoryDto, UpdateCategoryDto, PatchCategoryDto>>(sp =>
            new CrudService<Category, CategoryDto, CreateCategoryDto, UpdateCategoryDto, PatchCategoryDto>(
                sp.GetRequiredService<IRepository<Category>>(), sp.GetRequiredService<AutoMapper.IMapper>(),
                nameof(Category.IdCategory), e => e.IdCategory, "Products"));

        services.AddScoped<ICrudService<Product, ProductDto, CreateProductDto, UpdateProductDto, PatchProductDto>>(sp =>
            new CrudService<Product, ProductDto, CreateProductDto, UpdateProductDto, PatchProductDto>(
                sp.GetRequiredService<IRepository<Product>>(), sp.GetRequiredService<AutoMapper.IMapper>(),
                nameof(Product.IdProduct), e => e.IdProduct, "Category"));

        services.AddScoped<ICrudService<Cart, CartDto, CreateCartDto, UpdateCartDto, PatchCartDto>>(sp =>
            new CrudService<Cart, CartDto, CreateCartDto, UpdateCartDto, PatchCartDto>(
                sp.GetRequiredService<IRepository<Cart>>(), sp.GetRequiredService<AutoMapper.IMapper>(),
                nameof(Cart.IdCart), e => e.IdCart, "Users", "CartItems"));

        services.AddScoped<ICrudService<CartItem, CartItemDto, CreateCartItemDto, UpdateCartItemDto, PatchCartItemDto>>(sp =>
            new CrudService<CartItem, CartItemDto, CreateCartItemDto, UpdateCartItemDto, PatchCartItemDto>(
                sp.GetRequiredService<IRepository<CartItem>>(), sp.GetRequiredService<AutoMapper.IMapper>(),
                nameof(CartItem.IdCartItem), e => e.IdCartItem, "Product", "Cart.Users"));

        services.AddScoped<ICrudService<Order, OrderDto, CreateOrderDto, UpdateOrderDto, PatchOrderDto>>(sp =>
            new CrudService<Order, OrderDto, CreateOrderDto, UpdateOrderDto, PatchOrderDto>(
                sp.GetRequiredService<IRepository<Order>>(), sp.GetRequiredService<AutoMapper.IMapper>(),
                nameof(Order.IdOrder), e => e.IdOrder, "Users", "OrderItems"));

        services.AddScoped<ICrudService<OrderItem, OrderItemDto, CreateOrderItemDto, UpdateOrderItemDto, PatchOrderItemDto>>(sp =>
            new CrudService<OrderItem, OrderItemDto, CreateOrderItemDto, UpdateOrderItemDto, PatchOrderItemDto>(
                sp.GetRequiredService<IRepository<OrderItem>>(), sp.GetRequiredService<AutoMapper.IMapper>(),
                nameof(OrderItem.IdOrderItem), e => e.IdOrderItem, "CartItem.Product", "Order.Users"));
    }
}
