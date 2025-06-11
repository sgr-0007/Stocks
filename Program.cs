using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using dotNET8;
using dotNET8.Data;
using dotNET8.Interfaces;
using dotNET8.Repository;

var builder = WebApplication.CreateBuilder(args);

// ------------- Services --------------------------------------------------
builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDBContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddApiVersioning(opt =>
    {
        opt.DefaultApiVersion = new ApiVersion(1, 0);
        opt.AssumeDefaultVersionWhenUnspecified = true;
        opt.ReportApiVersions = true;
        opt.ApiVersionReader = ApiVersionReader.Combine(
            new UrlSegmentApiVersionReader(),                  // /v{version}/...
            new HeaderApiVersionReader("X-Api-Version"),
            new QueryStringApiVersionReader("api-version"));
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

var app = builder.Build();

// ------------- Pipeline --------------------------------------------------
if (app.Environment.IsDevelopment())
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
