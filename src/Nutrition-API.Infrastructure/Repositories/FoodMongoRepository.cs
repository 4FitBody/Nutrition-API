namespace Nutrition_API.Infrastructure.Repositories;

using Nutrition_API.Core.Repositories;
using Nutrition_API.Core.Models;
using MongoDB.Driver;

public class FoodMongoRepository : IFoodRepository
{
    private readonly IMongoDatabase foodDb;
    private readonly IMongoCollection<Food> collection;

    public FoodMongoRepository(string connectionString, string db, string collection)
    {
        var client = new MongoClient(connectionString);

        this.foodDb = client.GetDatabase(db);

        this.collection = this.foodDb.GetCollection<Food>(collection);
    }

    public async Task ApproveAsync(int id)
    {
        var update = Builders<Food>.Update.Set(_ => _.IsApproved, false);

        var options = new FindOneAndUpdateOptions<Food>();

        await this.collection.FindOneAndUpdateAsync<Food>(food => food.Id == id, update, options);
    }

    public async Task CreateAsync(Food food)
    {
        var lastIndex = this.collection.Find(e => e.Id >= 0).ToList().Last().Id;

        food.Id = lastIndex + 1;

        await this.collection.InsertOneAsync(food);
    }

    public async Task DeleteAsync(int id)
    {
        await this.collection.FindOneAndDeleteAsync(food => food.Id == id);
    }

    public async Task<IEnumerable<Food>?> GetAllAsync()
    {
        var food = await this.collection.FindAsync(f => f.IsApproved == true);

        var allFood = food.ToList();

        return allFood;
    }

    public async Task<Food> GetByIdAsync(int id)
    {
        var food = await this.collection.FindAsync(f => f.Id == id);

        var searchedFood = food.FirstOrDefault();

        return searchedFood;
    }

    public async Task UpdateAsync(int id, Food food)
    {
        food.Id = id;
        
        var oldFood = await this.GetByIdAsync(id);

        food.ImageUrl = oldFood.ImageUrl;

        food.VideoUrl = oldFood.VideoUrl;

        await this.collection.ReplaceOneAsync<Food>(filter: f => f.Id == id, replacement: food);
    }
}
