using Karako�.Bussiness.concrete;
using Karako�.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddScoped<LoginManager>();
builder.Services.AddScoped<CalisanManager>();
builder.Services.AddScoped<OrganizerManager>();
builder.Services.AddScoped<AdminManager>();

builder.Services.AddDbContext<ResulContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Session'� ekle
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session s�resi
    options.Cookie.HttpOnly = true; // Cookie'yi HTTP istekleri i�in k�s�tla
    options.Cookie.IsEssential = true; // Uygulama i�in gerekli oldu�unu belirt
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Login/Index");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Session middleware'ini ekle
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Kay�tOl}/{id?}");

app.Run();
