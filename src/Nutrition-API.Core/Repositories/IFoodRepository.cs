namespace Nutrition_API.Core.Repositories;

using Nutrition_API.Core.Models;

public interface IFoodRepository
{
    Task<IEnumerable<Food>?> GetAllAsync();
    Task CreateAsync(Food food);
    Task DeleteAsync(int id);
    Task UpdateAsync(int id, Food food);
    Task<Food> GetByIdAsync(int id);
    Task ApproveAsync(int id);
}
