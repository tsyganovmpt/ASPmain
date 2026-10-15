using ApiAsp0.DTO.DTOcart;
using ApiAsp0.DTO.DTOcartItem;
using ApiAsp0.DTO.DTOcategory;
using ApiAsp0.DTO.DTOorder;
using ApiAsp0.DTO.DTOorderItem;
using ApiAsp0.DTO.DTOproduct;
using ApiAsp0.DTO.DTOrole;
using ApiAsp0.DTO.DTOuser;
using ApiAsp0.Models;
using ApiAsp0.DTO.DTOreservation;
using AutoMapper;

namespace ApiAsp0.Mapping;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Role, RoleDto>()
            .ForMember(d => d.UsersCount, o => o.MapFrom(s => s.Users.Count));
        CreateMap<CreateRoleDto, Role>();
        CreateMap<UpdateRoleDto, Role>();
        CreateMap<PatchRoleDto, Role>()
            .ForAllMembers(o => o.Condition((src, dest, value) => value != null));

        CreateMap<User, UserDto>()
            .ForMember(d => d.RoleName, o => o.MapFrom(s => s.Role.RoleName));
        CreateMap<CreateUserDto, User>()
            .ForMember(d => d.IsActive, o => o.MapFrom(_ => true));
        CreateMap<UpdateUserDto, User>();
        CreateMap<PatchUserDto, User>()
            .ForAllMembers(o => o.Condition((src, dest, value) => value != null));

        CreateMap<Category, CategoryDto>()
            .ForMember(d => d.ProductsCount, o => o.MapFrom(s => s.Products.Count));
        CreateMap<CreateCategoryDto, Category>();
        CreateMap<UpdateCategoryDto, Category>();
        CreateMap<PatchCategoryDto, Category>()
            .ForAllMembers(o => o.Condition((src, dest, value) => value != null));

        CreateMap<Product, ProductDto>()
            .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.CategoryName));
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();
        CreateMap<PatchProductDto, Product>()
            .ForAllMembers(o => o.Condition((src, dest, value) => value != null));

        CreateMap<Cart, CartDto>()
            .ForMember(d => d.UserLogin, o => o.MapFrom(s => s.Users.LoginUser))
            .ForMember(d => d.ItemsCount, o => o.MapFrom(s => s.CartItems.Count));
        CreateMap<CreateCartDto, Cart>();
        CreateMap<UpdateCartDto, Cart>();
        CreateMap<PatchCartDto, Cart>()
            .ForAllMembers(o => o.Condition((src, dest, value) => value != null));

        CreateMap<CartItem, CartItemDto>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.ProductName))
            .ForMember(d => d.ProductPrice, o => o.MapFrom(s => s.Product.ProductPrice))
            .ForMember(d => d.CartOwnerLogin, o => o.MapFrom(s => s.Cart.Users.LoginUser));
        CreateMap<CreateCartItemDto, CartItem>();
        CreateMap<UpdateCartItemDto, CartItem>();
        CreateMap<PatchCartItemDto, CartItem>()
            .ForAllMembers(o => o.Condition((src, dest, value) => value != null));

        CreateMap<Order, OrderDto>()
            .ForMember(d => d.UserLogin, o => o.MapFrom(s => s.Users.LoginUser))
            .ForMember(d => d.ItemsCount, o => o.MapFrom(s => s.OrderItems.Count));
        CreateMap<CreateOrderDto, Order>();
        CreateMap<UpdateOrderDto, Order>();
        CreateMap<PatchOrderDto, Order>()
            .ForAllMembers(o => o.Condition((src, dest, value) => value != null));

        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.CartItem.Product.ProductName))
            .ForMember(d => d.OrderOwnerLogin, o => o.MapFrom(s => s.Order.Users.LoginUser));
        CreateMap<CreateOrderItemDto, OrderItem>();
        CreateMap<UpdateOrderItemDto, OrderItem>();
        CreateMap<PatchOrderItemDto, OrderItem>()
            .ForAllMembers(o => o.Condition((src, dest, value) => value != null));

        CreateMap<ProductReservation, ReservationDto>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.ProductName))
            .ForMember(d => d.UserLogin, o => o.MapFrom(s => s.User.LoginUser));
    }
}
