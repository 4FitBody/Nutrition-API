namespace Nutrition_API.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using Nutrition_API.Core.Models;

public class FoodDbContext : DbContext
{
    public DbSet<Food> Food { get; set; }

    public FoodDbContext(DbContextOptions<FoodDbContext> options) : base(options) { }
}
