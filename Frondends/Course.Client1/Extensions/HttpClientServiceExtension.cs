using Course.Client1.ConfigurationOptions;
using Course.Client1.ConfigurationOptions.Abstract;
using Course.Client1.Services;
using Course.Client1.Services.Abstract;
using Course.Client1.Services.Abstract.MicroserviceAbstract;
using Course.Client1.Services.Mİcroservices;
using Course.Client1.TokenHandler;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Client1.Extensions
{
    public static class HttpClientServiceExtension
    {

        public static void AddHttpClientServices(this IServiceCollection services,IConfiguration configuration )
        {
            var serviceApiSettings = configuration.GetSection("ServiceSettings").Get<ServiceApiSettings>();


            services.AddHttpClient<IClientSettings, ClientSettings>();
            services.AddHttpClient<IIdentityService, IdentityService>();

            services.AddHttpClient<IDiscountService, DiscountService>(opt =>
            {
                opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.DiscountService.Path}");
            }).AddHttpMessageHandler<PasswordTokenHandler>();

            services.AddHttpClient<IBasketService, BasketService>(opt =>
            {
                opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.BasketService.Path}");
            }).AddHttpMessageHandler<PasswordTokenHandler>();
            services.AddHttpClient<IPhotoService, PhotoService>(opt =>
            {
                opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.PhotoService.Path}");
            }).AddHttpMessageHandler<ClientCredentialsTokenHandler>();
            services.AddHttpClient<ICatalogService, CatalogService>(opt =>
            {
                opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.CatalogService.Path}");
            }).AddHttpMessageHandler<ClientCredentialsTokenHandler>();
            services.AddHttpClient<IUserService, UserService>(opt =>
            {
                opt.BaseAddress = new Uri(serviceApiSettings.BaseUrl);
            }).AddHttpMessageHandler<PasswordTokenHandler>();
            services.AddHttpClient<IPaymentService, PaymentService>(opt =>
            {
                opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.PaymentService.Path}");
            }).AddHttpMessageHandler<PasswordTokenHandler>();
            services.AddHttpClient<IOrderService, OrderService>(opt =>
            {
                opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.OrderService.Path}");
            }).AddHttpMessageHandler<PasswordTokenHandler>();
        }
    }
}
