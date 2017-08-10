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
            services.AddDbContext<TradeOffContext>(o => o.UseSqlServer(connectionString)).AddIdentity<User, IdentityRole>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddTransient<TradeOffIdentitySeeder>();
            services.AddIdentity<User, IdentityRole>()
       .AddEntityFrameworkStores<TradeOffContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, TradeOffContext tradeOffContext, TradeOffIdentitySeeder identitySeeder)
        {
            app.UseIdentity();
            app.UseMvc();

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

            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }
            identitySeeder.SeedUserIdenities();
            tradeOffContext.EnsureSeedDataForContext();

            app.UseStatusCodePages();

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}
