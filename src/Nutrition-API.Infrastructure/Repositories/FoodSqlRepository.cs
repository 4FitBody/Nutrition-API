namespace Nutrition_API.Infrastructure.Repositories;

using Nutrition_API.Core.Repositories;
using Nutrition_API.Core.Models;
using Nutrition_API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class FoodSqlRepository : IFoodRepository
{
    private readonly FoodDbContext dbContext;

    public FoodSqlRepository(FoodDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task ApproveAsync(int id)
    {
        var searchedFood = await this.dbContext.Food.FirstOrDefaultAsync(exercise => exercise.Id == id);
    
        searchedFood!.IsApproved = true;

        await this.dbContext.SaveChangesAsync();
    }

    public async Task CreateAsync(Food food)
    {
        await this.dbContext.Food.AddAsync(food);

        await this.dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var foodToDelete = await this.dbContext.Food.FirstOrDefaultAsync(food => food.Id == id);

        this.dbContext.Remove<Food>(foodToDelete!);

        await this.dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Food>?> GetAllAsync()
    {
        var food = this.dbContext.Food.AsEnumerable();

        return food;
    }

    public async Task<Food> GetByIdAsync(int id)
    {
        var food = await this.dbContext.Food.FirstOrDefaultAsync(f => f.Id == id);

        return food;
    }

    public async Task UpdateAsync(int id, Food food)
    {
        var oldFood = await this.dbContext.Food.FirstOrDefaultAsync(f => f.Id == id);

#pragma warning disable CS8602
        oldFood.Name = food.Name;
#pragma warning restore CS8602
        oldFood.Diet = food.Diet;
        oldFood.Description = food.Description;
        oldFood.ImageUrl = food.ImageUrl;
        oldFood.VideoUrl = food.VideoUrl;
        oldFood.IsApproved = food.IsApproved;

        await this.dbContext.SaveChangesAsync();
    }
}
