namespace Nutrition_API.Infrastructure.Commands;

using MediatR;

public class DeleteCommand : IRequest
{
    public int? Id { get; set; }

    public DeleteCommand(int? id) => this.Id = id;

    public DeleteCommand() { }
}
