using System.ComponentModel;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Helpers;
using SchoolApp.Interfaces.Services;
using SchoolApp.Models;
using SchoolApp.Services;

var builder = WebApplication.CreateBuilder(args);

var options = new JsonSerializerOptions
{
    Converters =
    {
        new EnumConverter<Level>(),
        new EnumConverter<EnrollmentType>()
    }
};


var environment = builder.Environment.EnvironmentName;
builder.Configuration.AddEnvironmentVariables().AddJsonFile($"appsettings.{environment}.json");

var databaseProvider = builder.Configuration["DatabaseProvider"];
var connectionStrings = builder.Configuration.GetSection("ConnectionStrings");

builder.Services.AddRazorPages(opt =>
{
    opt.Conventions.AddPageApplicationModelConvention("/Pages/YourPage", model =>
    {
        model.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
    });
});
builder.Services.AddScoped<EnrollmentService>();
builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddSingleton(options);
builder.Services.AddSingleton<JsonDataLoader>();
builder.Services.AddSingleton<DbInitializer>();

builder.Services.AddDbContext<DefaultContext>(opt =>
{
    switch (databaseProvider)
    {
        case "SQLite":
            opt.UseSqlite(connectionStrings["SQLite"]);
            break;
        case "AzureSQL":
            opt.UseSqlServer(connectionStrings["AzureSQL"]);
            break;
        default:
            throw new InvalidOperationException("Unsupported database provider configured.");
    }
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DefaultContext>();
    var dbInitializer = services.GetRequiredService<DbInitializer>();

    switch (databaseProvider)
    {
        case "SQLite":
            context.Database.EnsureCreated();
            dbInitializer.Initialize(context);
            break;
        case "AzureSQL":
            context.Database.Migrate();
            break;
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();