namespace Nutrition_API.Infrastructure.Commands;

using MediatR;
using Nutrition_API.Core.Models;

public class CreateCommand :IRequest
{
    public Food? Food { get; set; }

    public CreateCommand(Food? food) => this.Food = food;

    public CreateCommand() {}
}
