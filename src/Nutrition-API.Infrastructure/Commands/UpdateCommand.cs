namespace Nutrition_API.Infrastructure.Commands;

using MediatR;
using Nutrition_API.Core.Models;

public class UpdateCommand : IRequest
{
    public int? Id { get; set; }
    public Food? Food { get; set; }

    public UpdateCommand(int? id, Food? food)
    {
        this.Id = id;

        this.Food = food;
    }

    public UpdateCommand() { }
}
