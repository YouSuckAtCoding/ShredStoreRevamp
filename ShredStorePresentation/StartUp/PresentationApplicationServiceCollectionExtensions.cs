using ShredStorePresentation.Services.CartItemServices;
using ShredStorePresentation.Services.CartServices;
using ShredStorePresentation.Services.Images;
using ShredStorePresentation.Services.ProductServices;
using ShredStorePresentation.Services.UserService;

namespace ShredStorePresentation.StartUp
{
    public static class PresentationApplicationServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, WebApplicationBuilder builder)
        {
            ConfigurationManager configuration = builder.Configuration;

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddControllersWithViews();
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(600);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.AddTransient<IUserHttpService, UserHttpService>();
            services.AddTransient<IProductHttpService, ProductHttpService>();
            services.AddSingleton<IImageService, ImageService>();
            services.AddTransient<ICartHttpService, CartHttpService>();
            services.AddTransient<ICartItemHttpService, CartItemHttpService>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddSingleton<IProductFactory, ConcreteProductFactory>();
            //services.AddSingleton<IUserFactory, ConcreteUserFactory>();
            //services.AddSingleton<ICartFactory, ConcreteCartFactory>();
            //services.AddSingleton<ICartItemFactory, ConcreteCartItemFactory>();
            //services.AddSingleton<EmailSender>();
            //services.AddSingleton<MiscellaneousUtilityClass>();


            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis");
                options.InstanceName = "ShredStore_";
            });




            return services;

        }
    }
}
