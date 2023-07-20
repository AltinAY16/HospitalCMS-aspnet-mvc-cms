using App.Data;
using App.Data.Entity;
using App.Service.Abstract;
using App.Service.Concrete;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(); // E�er sonsuz d�ng� hatas� al�rsan AddJsonOptions(IgnoreCycles) kullan
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
	string? connStr = builder.Configuration.GetConnectionString("DBConStr"); // Builder konfig�rasyonu i�erisinde "DBConStr" appsettings.json de�erini oku.

	options.UseSqlServer(connStr);
});

builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>(); // Uygulama aya�a kalkt���nda, belirtilen Database'i getir.

	var db = dbContext.Database;

	if (!await db.CanConnectAsync()) // E�er ilgili database'yi bulam�yorsan 
	{

		DbSeeder.Seed(dbContext);
		// TODO: e�er veritaban� s�f�rdan olu�turulunca
		// i�erisindeki baz� tablolarda kay�t olmas� gerekiyorsa
		// burada seed yap�lmal�

		await db.EnsureCreatedAsync();
	}
}

app.Run();
