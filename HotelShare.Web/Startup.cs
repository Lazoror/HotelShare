using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using HotelShare.DAL.Data;
using HotelShare.Infrastructure.AutofacModules;
using HotelShare.Web.AutofacModules;
using HotelShare.Web.MapperModules;
using HotelShare.Web.Settings;
using HotelShare.Web.Settings.API;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Globalization;
using System.Text;

namespace HotelShare.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // DB Configuration
            var connection = Configuration.GetConnectionString("MyConnection");
            services.AddDbContext<HotelContext>
                (options => options.UseSqlServer(connection));

            ConfigureAuthorization(services);

            // Auto Mapper Configuration
            var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            ConfigureLocalization(services);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Autofac configuration
            var builder = ConfigureAutofac(services);
            var container = builder.Build();

            return container.Resolve<IServiceProvider>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("uk-UA")
            };

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseRequestLocalization(locOptions.Value);
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("uk-UA"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
        }

        private void ConfigureLocalization(IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMvc()
                .AddDataAnnotationsLocalization()
                .AddViewLocalization();

            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
            });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("uk-UA")
                };

                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
        }

        private ContainerBuilder ConfigureAutofac(IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new ServicesModule());
            builder.RegisterModule(new DALModule());
            builder.RegisterModule(new InfrastructureModule());
            builder.RegisterModule(new WebModule());
            builder.Populate(services);

            return builder;
        }

        private void ConfigureAuthorization(IServiceCollection services)
        {
            var cookieAuthSettings = AddCookieAuthSettings(services);
            var apiAuthSettings = AddApiAuthSettings(services);

            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(options =>
                {
                    options.SlidingExpiration = true;
                    options.LoginPath = "/account/login";
                    options.AccessDeniedPath = "/account/accessDenied";
                    options.ExpireTimeSpan = TimeSpan.FromSeconds(cookieAuthSettings.ExpirationTimeInSeconds);
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(apiAuthSettings.Secret)),
                        ValidIssuer = apiAuthSettings.Issuer,
                        ValidateIssuer = true,
                        ValidateAudience = false
                    };
                });

            services.AddAuthorization();
        }

        private CookieAuthSettings AddCookieAuthSettings(IServiceCollection services)
        {
            var settingsSection = Configuration.GetSection(nameof(CookieAuthSettings));
            services.Configure<CookieAuthSettings>(settingsSection);

            return settingsSection.Get<CookieAuthSettings>();
        }

        private ApiAuthSettings AddApiAuthSettings(IServiceCollection services)
        {
            var authSettingsSection = Configuration.GetSection(nameof(ApiAuthSettings));
            services.Configure<ApiAuthSettings>(authSettingsSection);

            return authSettingsSection.Get<ApiAuthSettings>();
        }
    }
}
