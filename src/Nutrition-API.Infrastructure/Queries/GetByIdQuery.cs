namespace Nutrition_API.Infrastructure.Queries;

using Nutrition_API.Core.Models;
using MediatR;

public class GetByIdQuery : IRequest<Food>
{
    public int? Id { get; set; }

    public GetByIdQuery(int? Id)
    {
        this.Id = Id;
    }

    public GetByIdQuery() { }
}
