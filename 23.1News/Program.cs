using _23._1News.Data;
using _23._1News.Helpers;
using _23._1News.Models.Db;
using _23._1News.Services.Abstract;
using _23._1News.Services.Implement;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;


namespace _23._1News
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            builder.Services.AddControllersWithViews();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            //Services:
            builder.Services.AddScoped<IArticleService, ArticleService>();
            builder.Services.AddScoped<IAdminService, AdminService>();

            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
            builder.Services.AddScoped<ISubscriptionTypeService, SubscriptionTypeService>();
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();



            builder.Services.AddScoped<IYahooFinanceService, YahooFinanceService>();
            builder.Services.AddHttpClient("YahooFinance", config =>
            {

                config.BaseAddress = new(builder.Configuration["MyYahooFinanceAPIAddress"]);
                config.DefaultRequestHeaders.Add("X-RapidAPI-Key", builder.Configuration["YahooApiKey"]);
                config.DefaultRequestHeaders.Add("X-RapidAPI-Host", builder.Configuration["YahooApiHost"]);

            });



            builder.Services.AddScoped<IElectricityService, ElectricityService>();
            builder.Services.AddHttpClient("electricityPrice", config =>
            {

                config.BaseAddress = new(builder.Configuration["MyElectricityPriceAPIAddress"]);

            });

            builder.Services.AddScoped<IWeatherService, WeatherService>();
            builder.Services.AddHttpClient("weatherForecast", config =>
            {

                config.BaseAddress = new(builder.Configuration["MyWeatherAPIAddress"]);

            });


            builder.Services.AddScoped<IExchangeRatesService, ExchangeRatesServices>();
            builder.Services.AddHttpClient("dailyPrices", config =>
            {
                config.BaseAddress = new(builder.Configuration["ExchangeRateAPIAddress"]);
            });


            //builder.Services.AddScoped<ICategoryService, CategoryService>();
            //builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();

            //builder.Services.AddScoped<IWeatherService, WeatherService>();

            
               
           
          builder.Services.AddSingleton<IEmailConfiguration>(
                builder.Configuration.GetSection("EmailConfiguration")
                .Get<EmailConfiguration>());


                     

            builder.Services.AddTransient<IEmailSender, EmailHelper>();
            builder.Services.AddTransient<IEmailHelper, EmailHelper>();

            //Sessions for weekly newsletter in my pages
            builder.Services.AddSession();


            var app = builder.Build();

            //Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
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
            app.UseSession();
            app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();



            app.Run();
        }
    }

}
