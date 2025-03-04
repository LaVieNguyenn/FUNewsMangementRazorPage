using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Team_07_PRN222_A02.BLL.Mapping;
using Team_07_PRN222_A02.BLL.Services.CategoryService;
using Team_07_PRN222_A02.BLL.Services.NewsArticleService;
using Team_07_PRN222_A02.BLL.Services.SystemAccountService;
using Team_07_PRN222_A02.DAL.Models;
using Team_07_PRN222_A02.DAL.Repositories.AccountRepository;
using Team_07_PRN222_A02.DAL.Repositories.CategoryRepository;
using Team_07_PRN222_A02.DAL.Repositories.NewsRepository;
using Team_07_PRN222_A02.DAL.UnitOfWork;

namespace Team_07_PRN222_A02
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<FunewsManagementContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionStringDB")));
            // Add services to the container.
            builder.Services.AddRazorPages();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Authentication/Login";
        options.AccessDeniedPath = "/";
    });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("StaffOnly", policy => policy.RequireRole("Staff"));
                options.AddPolicy("AdminOrStaff", policy => policy.RequireRole("Admin", "Staff"));
            });
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<ISystemAccountService, SystemAccountService>();

            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();

            builder.Services.AddScoped<INewsRepository, NewRepository>();
            builder.Services.AddScoped<INewArticleService, NewArticleService>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
