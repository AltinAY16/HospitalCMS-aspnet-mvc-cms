using App.Data;
using App.Web.Mvc.Services;
using App.Web.Mvc.Services.Concrete;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//builder.Services.AddSession();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    string? connStr = builder.Configuration.GetConnectionString("DBConStr"); // Builder konfig�rasyonu i�erisinde "DBConStr" appsettings.json de�erini oku.

    options.UseSqlServer(connStr);
});

builder.Services.AddScoped<ICategoryService, CategoryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

// JWT - Token ve Cookie ara�t�r.

//app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>(); // Uygulama aya�a kalkt���nda, belirtilen Database'i getir.

    var db = dbContext.Database; 

    if(!await db.CanConnectAsync()) // E�er ilgili database'yi bulam�yorsan 
    {
        await db.EnsureCreatedAsync();

        // TODO: e�er veritaban� s�f�rdan olu�turulunca
        // i�erisindeki baz� tablolarda kay�t olmas� gerekiyorsa
        // burada seed yap�lmal�

    }
}

// Category ve Services(Hospital Services) k�sm�n� g�ncelle.

app.Run();
