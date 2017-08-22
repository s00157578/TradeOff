using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TradeOff.API.Services;
using TradeOff.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TradeOff.API
{
    public class Startup
    {
        public static IConfigurationRoot Configuration;
        public static IHostingEnvironment Env;
        public Startup(IHostingEnvironment env)
        {         
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appSettings.Json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appSettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            Env = env;
            Configuration = builder.Build();

        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<JWTSettings>(Configuration.GetSection("JWTSettings"));
            services.Configure<IdentityOptions>(options =>
            {
                // avoid redirecting REST clients on 401
                options.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = ctx =>
                    {
                        ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        return Task.FromResult(0);
                    }
                };
            });
            services.AddMvc(o =>
                {
                    if (Env.IsDevelopment())
                    {
                        //o.SslPort = 44367;
                    }
                    //o.Filters.Add(new RequireHttpsAttribute());
                })
                .AddMvcOptions(o => o.OutputFormatters.Add(
                    new XmlDataContractSerializerOutputFormatter()));            
            var connectionString = Configuration["connectionStrings:tradeOffDBConnectionString"];
            services.AddDbContext<TradeOffContext>(o => o.UseSqlServer(connectionString)).AddIdentity<IdentityUser, IdentityRole>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddTransient<TradeOffIdentitySeeder>();
            services.AddIdentity<IdentityUser, IdentityRole>()
       .AddEntityFrameworkStores<TradeOffContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, TradeOffContext tradeOffContext, TradeOffIdentitySeeder identitySeeder)
        {
            app.UseIdentity();
            //for validating tokens on login
            var secretKey = Configuration.GetSection("JWTSettings:SecretKey").Value;
            var issuer = Configuration.GetSection("JWTSettings:Issuer").Value;
            var audience = Configuration.GetSection("JWTSettings:Audience").Value;
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = issuer,

                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = audience
            };
            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                TokenValidationParameters = tokenValidationParameters
            });

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AutomaticAuthenticate = false,
                AutomaticChallenge = false
            });

            app.UseMvc();
            //mapping models to entities and vice versa
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Entities.Product, Models.ProductModel>(); //Source followed by destination
                cfg.CreateMap<Entities.Category, Models.CategoryModel>();
                cfg.CreateMap<Entities.ProductImage, Models.ProductImageModel>();
                cfg.CreateMap<Models.ProductCreateModel, Entities.Product>();
                cfg.CreateMap<Models.ProductImageCreateModel, Entities.ProductImage>();

                cfg.CreateMap<Models.ProductImageUpdateModel, Entities.ProductImage>();
                cfg.CreateMap<Entities.ProductImage, Models.ProductImageUpdateModel>();

                cfg.CreateMap<Models.ProductUpdateModel, Entities.Product>();
                cfg.CreateMap<Entities.Product, Models.ProductUpdateModel>();
            });
            //log to console, if development use exception page
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }
            //seed data
            identitySeeder.SeedUserIdenities();
            tradeOffContext.EnsureSeedDataForContext();

            app.UseStatusCodePages();
            
        }
    }
}
