using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using BookStore.DataAccess.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//builder.Services.AddDbContext<ApplicationDbContent>(options=>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//var keyVaultUrl = new Uri(builder.Configuration.GetSection("KeyVaultURL").Value!);
//var azureCredential = new DefaultAzureCredential();
//builder.Configuration.AddAzureKeyVault(keyVaultUrl, azureCredential);

//var cs = builder.Configuration.GetSection("ConnectionStrings--DefaultConnection").Value;
//builder.Services.AddDbContext<ApplicationDbContent>(options => options.UseSqlServer(cs));


var keyVaultUrl = new Uri(builder.Configuration.GetSection("KeyVaultURL").Value!);
var azureCredential = new DefaultAzureCredential();
builder.Configuration.AddAzureKeyVault(keyVaultUrl, azureCredential);

var cs = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(cs))
{
    Console.WriteLine("Connection string is null or empty.");
}
else
{
    builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(cs));
}

builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

