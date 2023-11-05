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

builder.Services.AddRazorPages(); //Razor sayfalar�n� kullan�labilir hale getirme

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme) //Kimlik do�rulama ayar� ve �ema bilgisi
    .AddCookie(x =>
    {
        x.LoginPath = "/Login/Index"; // Giri� sayfas� yolu
        x.LogoutPath = "/Login/LogOut"; // Oturum kapatma sayfas� yolu
    });

var logger = new LoggerConfiguration()             //Log Structure ayarlar�
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .CreateLogger();


string redisConnectionString = builder.Configuration.GetSection("CacheOptions")["Url"]; // builder.Configuration ile Configuration'a eri�in

var redis = ConnectionMultiplexer.Connect(redisConnectionString);
builder.Services.AddSingleton<RedisService>(sp => new RedisService(redis.GetDatabase()));


builder.Logging.ClearProviders();    //Loglama varsay�lan� kald�r�r. 

builder.Logging.AddSerilog(logger); //Serilog g�nl�k y�netimi yetkilendirme.

builder.Services.AddDbContext<Context>();  //Ef Core veritaban� s�n�f� belirtme.

builder.Services.AddScoped<ICategoryDal, EfCategoryDal>();                    // Dependency Injection. Veritaban� i�lemleri i�in servis eklemeleri. Veirtaban� katman� ile uygulama katman� ileti�imi.
builder.Services.AddScoped<ICategoryService, CategoryManager>();

builder.Services.AddScoped<IProductDal, EfProductDal>();
builder.Services.AddScoped<IProductService, ProductManager>();


builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<Context>();            //Kimlik do�rulama ayarlar�.


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/ErrorPage/NotFound", "?code={0}");   //Durum kodlar�n�n y�nlendirmesi

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();      

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
