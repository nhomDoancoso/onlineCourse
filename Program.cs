using Microsoft.EntityFrameworkCore;
using test.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("MySqlConn");

builder.Services.AddDbContext<CourseOnlineContext>(options => 
{
   options.UseMySql(connectionString,  ServerVersion.AutoDetect(connectionString)); 
});

builder.Services.AddCors(p => p.AddPolicy("Cors_App", builder =>
{
    builder.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("Cors_App");
app.UseRouting();
app.UseAuthorization();
app.UseAuthentication();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
