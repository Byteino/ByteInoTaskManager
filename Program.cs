using AutoMapper;
using ByteInoTaskManager.Data;
using ByteInoTaskManager.Mappings;
using ByteInoTaskManager.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;



var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllersWithViews();



var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
var configExpression = new MapperConfigurationExpression();
configExpression.AddProfile<MappingProfile>();

var configuration = new MapperConfiguration(configExpression, loggerFactory);
var mapper = new Mapper(configuration);

builder.Services.AddSingleton<IMapper>(mapper);


builder.Services.AddDbContext<ApplicationDbContext>(option => option
    .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{

    options.AccessDeniedPath = "/Index";
    options.ExpireTimeSpan = TimeSpan.FromHours(6);
    options.SlidingExpiration = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.LogoutPath = "/Index";


});


var app = builder.Build();


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();




app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    await DbSeeder.SeedAdminAsync(userManager, roleManager);
}


app.Run();
