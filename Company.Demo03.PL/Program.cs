using Company.BLL;
using Company.BLL.Interfaces;
using Company.BLL.Repositories;
using Company.DAL.Data.Contexts;
using Company.DAL.Models;
using Company.Demo03.PL.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the cont ainer.
builder.Services.AddControllersWithViews(); //Register Built-in MVC Services
//builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>(); //Allow DI For DepartmentRepository
//builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(m => m.AddProfile(new EmployeeProfile()));

//Life Time
//builder.Services.AddScoped(); //Create object life time per request => then become unreachable object
//builder.Services.AddTransient(); //Create object life time per operation
//builder.Services.AddSingleton(); //Create object life time per Application

builder.Services.AddDbContext<CompanyDBContext>(options => //Allow DI For CompanyDbContext
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
 
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<CompanyDBContext>();


builder.Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = "/Account/SignIn";
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

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
   // .WithStaticAssets();


app.Run();
 