namespace Nutrition_API.Infrastructure.Handlers;

using Nutrition_API.Infrastructure.Queries;
using Nutrition_API.Core.Models;
using MediatR;
using Nutrition_API.Core.Repositories;

public class GetAllHandler : IRequestHandler<GetAllQuery, IEnumerable<Food>>
{
    private readonly IFoodRepository foodRepository;

    public GetAllHandler(IFoodRepository foodRepository) => this.foodRepository = foodRepository;

    public async Task<IEnumerable<Food>> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        var food = await this.foodRepository.GetAllAsync();

        if (food is null)
        {
            return Enumerable.Empty<Food>();
        }

        return food;
    }
}