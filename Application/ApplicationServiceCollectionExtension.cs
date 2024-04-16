using Application.Repositories;
using Application.Repositories.CartItemStorage;
using Application.Repositories.CartStorage;
using Application.Repositories.OrderItemStorage;
using Application.Repositories.ProductStorage;
using Application.Repositories.UserStorage;
using Application.Services.CartItemServices;
using Application.Services.CartServices;
using Application.Services.OrderItemServices;
using Application.Services.OrderServices;
using Application.Services.ProductServices;
using Application.Services.UserServices;
using DatabaseAccess;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ApplicationServiceCollectionExtension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {


            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<ICartRepository, CartRepository>();
            services.AddTransient<ICartService, CartService>();
            services.AddTransient<ICartItemRepository, CartItemRepository>();
            services.AddTransient<ICartItemService, CartItemService>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IOrderItemRepository, OrderItemRepository>();
            services.AddTransient<IOrderItemService, OrderItemService>();
            services.AddTransient<ISqlDataAccess, SqlDataAccess>();
            
            services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Singleton);

          

            return services;
        }
    }
}
