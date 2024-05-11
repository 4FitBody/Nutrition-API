using Microsoft.OpenApi.Models;
using Nutrition_API.Infrastructure.Data;
using Nutrition_API.Infrastructure.Repositories;
using Nutrition_API.Core.Repositories;
using Nutrition_API.Presentation.Options;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var blobImageOptionsSection = builder.Configuration.GetSection("BlobImageOptions");

var blobImageOptions = blobImageOptionsSection.Get<BlobOptions>() ?? throw new Exception("Couldn't create blob image options object");

builder.Services.Configure<BlobOptions>(blobImageOptionsSection);

var blobVideoOptionsSection = builder.Configuration.GetSection("BlobVideoOptions");

var blobVideoOptions = blobVideoOptionsSection.Get<BlobOptions>() ?? throw new Exception("Couldn't create blob video options object");

builder.Services.Configure<BlobOptions>(blobVideoOptionsSection);

var infrastructureAssembly = typeof(FoodDbContext).Assembly;

builder.Services.AddMediatR(configurations =>
{
    configurations.RegisterServicesFromAssembly(infrastructureAssembly);
});

builder.Services.AddScoped<IFoodRepository, FoodSqlRepository>();

builder.Services.AddAuthorization();

var connectionString = builder.Configuration.GetConnectionString("FoodDb");

builder.Services.AddDbContext<FoodDbContext>(dbContextOptionsBuilder =>
{
    dbContextOptionsBuilder.UseNpgsql(connectionString, o =>
    {
        o.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
    });
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "4FitBody (api for working staff)",
        Version = "v1"
    });
});

builder.Services.AddAuthentication();

builder.Services.AddAuthorization();

builder.Services.AddCors(options => {
    options.AddPolicy("BlazorWasmPolicy", corsBuilder => {
        corsBuilder
            .WithOrigins("http://localhost:5160")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseCors("BlazorWasmPolicy");

app.Run();