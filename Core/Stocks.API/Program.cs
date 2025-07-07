using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stocks.API;
using Stocks.API.Data;
using Stocks.API.Interfaces;
using Stocks.API.Models;
using Stocks.API.Repository;

var builder = WebApplication.CreateBuilder(args);

// ------------- Services --------------------------------------------------
builder.Services.AddControllers();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 12;
})
.AddEntityFrameworkStores<ApplicationDBContext>();

builder.Services
    .AddApiVersioning(opt =>
    {
        opt.DefaultApiVersion = new ApiVersion(1, 0);
        opt.AssumeDefaultVersionWhenUnspecified = true;
        opt.ReportApiVersions = true;
        opt.ApiVersionReader = ApiVersionReader.Combine(
            new UrlSegmentApiVersionReader(),                  // /v{version}/...
            new HeaderApiVersionReader("X-Api-Version"));
    })
    .AddApiExplorer(opt =>
    {
        opt.GroupNameFormat = "'v'VVV";                       // v1, v1.1, v2
        opt.SubstituteApiVersionInUrl = true;
    });

// Configure Swagger to work with API versioning
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();                  // keeps minimal-API discovery on

builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();


var app = builder.Build();

// ------------- Pipeline --------------------------------------------------
// Enable Swagger in both Development and Production
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    app.UseSwagger();
    app.UseSwaggerUI(ui =>
    {
        foreach (var desc in provider.ApiVersionDescriptions)
        {
            ui.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json",
                               $"Stocks API {desc.GroupName.ToUpperInvariant()}");
        }
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
