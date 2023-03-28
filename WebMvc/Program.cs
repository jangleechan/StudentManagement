using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Web;
using StudentManagement.Middlewares;
using StudentManagement.Models;
using WebMvc.Models;
using WebMvc.Utility;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{

    var builder = WebApplication.CreateBuilder(args);

    //获取数据库连接字符串
    var connectionString = builder.Configuration.GetConnectionString("StudentDBConnection");

    //注册EFcoresqlserver
    builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

    builder.Services.AddScoped<IStudentRepository, SQLStudentRepository>();
    builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddErrorDescriber<CustomIdentityErrorDescriber>()
        .AddEntityFrameworkStores<AppDbContext>();
    builder.Services.Configure<IdentityOptions>(options =>
    {
        options.Password.RequiredLength = 6;
        //options.Password.RequiredUniqueChars = 3;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
    });


   
    // Add services to the container.
    builder.Services.AddControllersWithViews(
        config =>
        {
            var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            config.Filters.Add(new AuthorizeFilter(policy));
        }
        ).AddXmlSerializerFormatters();

    //NLog:Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var app = builder.Build();
    //var app = builder.ConfigureServices();



    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        //app.UseExceptionHandler("/Home/Error");
        //app.UseStatusCodePagesWithRedirects("/Error/{0}");
        app.UseDeveloperExceptionPage();

    }
    else
    {
        app.UseExceptionHandler("/Error"); //拦截我们的异常
        app.UseStatusCodePagesWithReExecute("/Error/{0}"); //拦截404找不到页面信息
    }

    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    app.UseAuthentication();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception exception)
{

    logger.Error(exception,"Stopped program because of exception");
    throw;
} 
finally
{
    NLog.LogManager.Shutdown();
}


