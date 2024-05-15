using MongoDB.Driver;
using Microsoft.OpenApi.Models;
using Nutrition_API.Infrastructure.Repositories;
using Nutrition_API.Core.Repositories;
using Nutrition_API.Presentation.Options;
using Nutrition_API.Core.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetSection("FoodDb").Value;

var database = builder.Configuration.GetSection("Database").Value;

var collection = builder.Configuration.GetSection("Collection").Value;

var blobImageOptionsSection = builder.Configuration.GetSection("BlobImageOptions");

var blobImageOptions = blobImageOptionsSection.Get<BlobOptions>() ?? throw new Exception("Couldn't create blob image options object");

builder.Services.Configure<BlobOptions>(blobImageOptionsSection);

var blobVideoOptionsSection = builder.Configuration.GetSection("BlobVideoOptions");

var blobVideoOptions = blobVideoOptionsSection.Get<BlobOptions>() ?? throw new Exception("Couldn't create blob video options object");

builder.Services.Configure<BlobOptions>(blobVideoOptionsSection);

var infrastructureAssembly = typeof(FoodMongoRepository).Assembly;

builder.Services.AddMediatR(configurations =>
{
    configurations.RegisterServicesFromAssembly(infrastructureAssembly);
});

builder.Services.AddSingleton<IFoodRepository>(provider =>
{
    if (string.IsNullOrWhiteSpace(connectionString) || string.IsNullOrWhiteSpace(database) || string.IsNullOrWhiteSpace(collection))
    {
        throw new Exception($"Not Found");
    }

    return new FoodMongoRepository(connectionString, database, collection);
});

builder.Services.AddAuthorization();

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

builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorWasmPolicy", corsBuilder =>
    {
        corsBuilder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var client = new MongoClient(connectionString);

    var foodDb = client.GetDatabase(database);

    var foodCollection = foodDb.GetCollection<Food>(collection);
}


app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseCors("BlazorWasmPolicy");

app.Run();
