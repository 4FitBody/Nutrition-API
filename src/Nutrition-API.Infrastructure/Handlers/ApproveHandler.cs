namespace Nutrition_API.Infrastructure.Handlers;

using Nutrition_API.Core.Repositories;
using Nutrition_API.Infrastructure.Commands;
using MediatR;

public class ApproveHandler : IRequestHandler<ApproveCommand>
{
    private readonly IFoodRepository foodRepository;

    public ApproveHandler(IFoodRepository foodRepository) => this.foodRepository = foodRepository;

    public async Task Handle(ApproveCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request.Id);

        await this.foodRepository.ApproveAsync((int)request.Id);
    }
}