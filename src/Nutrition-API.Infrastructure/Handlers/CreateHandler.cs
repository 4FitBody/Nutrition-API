namespace Nutrition_API.Infrastructure.Handlers;

using System.Threading;
using System.Threading.Tasks;
using Nutrition_API.Core.Repositories;
using Nutrition_API.Infrastructure.Commands;
using MediatR;

public class CreateHandler : IRequestHandler<CreateCommand>
{
    private readonly IFoodRepository foodRepository;

    public CreateHandler(IFoodRepository foodRepository) => this.foodRepository = foodRepository;

    public async Task Handle(CreateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request.Food);

        await this.foodRepository.CreateAsync(request.Food);
    }
}