﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SportsStore.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace SportsStore
{
    public class Startup
    {
        IConfigurationRoot configuration;
        public Startup(IHostingEnvironment env)
        {
            configuration = new ConfigurationBuilder().SetBasePath(env.ContentRootPath).AddJsonFile("appsettings.json").Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration["Data:SportsStoreProducts:ConnectionString"]));
            services.AddDbContext<AppIdentityDbContext>(options =>
            options.UseSqlServer(configuration["Data:SportsStoreIdentity:ConnectionString"]));
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>();
            services.AddTransient<IProductRepository, EFProductRepository>();
            services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IOrderRepository,EFOrderRepository>();
            services.AddMvc();
            services.AddMemoryCache();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            
            app.UseStaticFiles();
            app.UseSession();
            app.UseIdentity();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "Error",
                    template: "Error",
                    defaults: new { controller = "Error", action = "Error" });

                routes.MapRoute(
                    name: null,
                    template: "{category}/Page{page:int}",
                    defaults: new { controller = "Product", action = "List" });

                routes.MapRoute(
                   name: null,
                   template: "Page{page:int}",
                   defaults: new { controller = "Product", action = "List", page = 1 });

                routes.MapRoute(
                   name: null,
                   template: "{category}",
                   defaults: new { controller = "Product", action = "List", page = 1 });

                routes.MapRoute(
                    name: null,
                    template: null,
                    defaults: new { controller = "Product", action = "List", page = 1 });

                routes.MapRoute(
                    name: null,
                    template: "{controller}/{action}/{id?}"
                    );
            });
            SeedData.EnsurePopulated(app);
            IdentitySeedData.EnsurePopulated(app);  
        }
    }
}
