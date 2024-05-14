namespace Nutrition_API.Infrastructure.Handlers;

using System.Threading;
using System.Threading.Tasks;
using Nutrition_API.Core.Repositories;
using Nutrition_API.Infrastructure.Commands;
using MediatR;

public class UpdateHandler : IRequestHandler<UpdateCommand>
{
    private readonly IFoodRepository foodRepository;

    public UpdateHandler(IFoodRepository foodRepository) => this.foodRepository = foodRepository;

    public async Task Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request.Id);

        ArgumentNullException.ThrowIfNull(request.Food);

        await this.foodRepository.UpdateAsync((int)request.Id, request.Food);
    }
}