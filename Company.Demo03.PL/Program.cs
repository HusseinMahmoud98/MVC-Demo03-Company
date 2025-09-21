using Company.BLL.Interfaces;
using Company.BLL.Repositories;
using Company.DAL.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the cont ainer.
builder.Services.AddControllersWithViews(); //Register Built-in MVC Services
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>(); //Allow DI For DepartmentRepository
builder.Services.AddDbContext<CompanyDBContext>(options => //Allow DI For CompanyDbContext
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
 
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
