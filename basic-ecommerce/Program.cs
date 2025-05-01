
using basic_ecommerce.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Connection string'i kullanarak DbContext yapılandırması
builder.Services.AddDbContext<EcommerceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Veritabanı bağlantısının çalışıp çalışmadığını test edebiliriz
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<EcommerceContext>();
    dbContext.Database.EnsureCreated(); // Veritabanını oluşturur (varsa günceller)
}

app.Run();