namespace Nutrition_API.Infrastructure.Queries;

using Nutrition_API.Core.Models;
using MediatR;

public class GetAllQuery : IRequest<IEnumerable<Food>>
{

}