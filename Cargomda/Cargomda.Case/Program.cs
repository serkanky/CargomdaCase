using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete;
using DataAccess.EntityFramework;
using Entity.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies;
using Redis.Cache;
using Serilog;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddRazorPages(); //Razor sayfalarýný kullanýlabilir hale getirme

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme) //Kimlik doðrulama ayarý ve þema bilgisi
    .AddCookie(x =>
    {
        x.LoginPath = "/Login/Index"; // Giriþ sayfasý yolu
        x.LogoutPath = "/Login/LogOut"; // Oturum kapatma sayfasý yolu
    });

var logger = new LoggerConfiguration()             //Log Structure ayarlarý
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .CreateLogger();


string redisConnectionString = builder.Configuration.GetSection("CacheOptions")["Url"]; // builder.Configuration ile Configuration'a eriþin

var redis = ConnectionMultiplexer.Connect(redisConnectionString);
builder.Services.AddSingleton<RedisService>(sp => new RedisService(redis.GetDatabase()));


builder.Logging.ClearProviders();    //Loglama varsayýlaný kaldýrýr. 

builder.Logging.AddSerilog(logger); //Serilog günlük yönetimi yetkilendirme.

builder.Services.AddDbContext<Context>();  //Ef Core veritabaný sýnýfý belirtme.

builder.Services.AddScoped<ICategoryDal, EfCategoryDal>();                    // Dependency Injection. Veritabaný iþlemleri için servis eklemeleri. Veirtabaný katmaný ile uygulama katmaný iletiþimi.
builder.Services.AddScoped<ICategoryService, CategoryManager>();

builder.Services.AddScoped<IProductDal, EfProductDal>();
builder.Services.AddScoped<IProductService, ProductManager>();


builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<Context>();            //Kimlik doðrulama ayarlarý.


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/ErrorPage/NotFound", "?code={0}");   //Durum kodlarýnýn yönlendirmesi

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();      

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
