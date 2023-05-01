using Blazored.LocalStorage;
using Blazored.Modal;
using Blazored.Modal.Services;
using BlazorTest.Client.Utils;
using BlazorTest.Server.Extensions;
using BlazorTest.Server.Extensions.Insfrastruce;
using BlazorTest;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MealOrdering.Server.Data.Context;
using BlazorTest.Server.Services.Infrastruce;
using BlazorTest.Server.Services.Services;
using MealOrdering.Server.Services.Services;
using MealOrdering.Server.Services.Infrastruce;

namespace BlazorTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            builder.Services.AddBlazoredModal();
            builder.Services.ConfigureMapping();
            builder.Services.AddScoped<IUserService,UserService>();
            builder.Services.AddTransient<ISupplierService, SupplierService>();
            builder.Services.AddTransient<IOrderService, OrderService>();
            builder.Services.AddTransient<IValidationService, ValidationService>();
            builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddDbContext<MealOrderinDbContext>(config =>
            {
                config.UseNpgsql("databaseAdress");
            });
            //bu ifaed datetime to timezone without date donusturme isi yapiyor.
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(options =>
           {
               options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   ValidIssuer = builder.Configuration["JwtIssuer"],
                   ValidAudience = builder.Configuration["JwtAudience"],
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSecurityKey"]))
               };
           });
            builder.Services.AddBlazoredLocalStorage();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();
            //app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();
            app.MapControllers();
            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}