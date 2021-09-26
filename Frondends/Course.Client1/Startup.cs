using Course.Client1.ConfigurationOptions;
using Course.Client1.ConfigurationOptions.Abstract;
using Course.Client1.Extensions;
using Course.Client1.Helpers;
using Course.Client1.Services;
using Course.Client1.Services.Abstract;
using Course.Client1.Services.Abstract.MicroserviceAbstract;
using Course.Client1.Services.Mİcroservices;
using Course.Client1.TokenHandler;
using Course.Shared.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Client1
{
    public class Startup
    {
        public ServiceApiSettings serviceApiSettings;
        public Startup(IConfiguration configuration)
        {
             
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddHttpContextAccessor();
            services.AddAccessTokenManagement();
            services.AddScoped<ClientCredentialsTokenHandler>();
            services.AddScoped<PasswordTokenHandler>();
            services.AddScoped<IClientCredentialsTokenService, ClientCredentialsTokenService>();
            services.AddScoped<ISharedIdentityService, SharedIdentityService>();
            services.AddSingleton<PhotoHelper>();
            //HttpClient
            services.AddHttpClientServices(Configuration);

            // ConfigureSettings
            services.Configure<ClientSettings>(Configuration.GetSection("ClientSettings"));
            services.AddSingleton<IClientSettings>(opt =>
            {
                ClientSettings clientSettings = opt.GetRequiredService<IOptions<ClientSettings>>().Value;
                return clientSettings;
            });

            services.Configure<ServiceApiSettings>(Configuration.GetSection("ServiceSettings"));
            services.AddSingleton<IServiceApiSettings>(opt =>
            {
                return opt.GetRequiredService<IOptions<ServiceApiSettings>>().Value;
            });
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opt =>
             {
                 opt.LoginPath = "/Auth/Signin";
                 opt.AccessDeniedPath = "/Auth/AccessDenied";
                 opt.LogoutPath = "/Home/Index";
                 opt.ExpireTimeSpan = TimeSpan.FromDays(60);
                 opt.Cookie.Name = "KrcWebCookie";
                 opt.SlidingExpiration = true;
             });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
