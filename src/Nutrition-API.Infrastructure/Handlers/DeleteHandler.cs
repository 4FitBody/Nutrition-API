namespace Nutrition_API.Infrastructure.Handlers;

using System.Threading;
using System.Threading.Tasks;
using Nutrition_API.Core.Repositories;
using Nutrition_API.Infrastructure.Commands;
using MediatR;

public class DeleteHandler : IRequestHandler<DeleteCommand>
{
    private readonly IFoodRepository foodRepository;

    public DeleteHandler(IFoodRepository foodRepository) => this.foodRepository = foodRepository;

    public async Task Handle(DeleteCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request.Id);

        await this.foodRepository.DeleteAsync((int)request.Id);
    }
}