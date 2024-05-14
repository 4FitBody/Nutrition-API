namespace Nutrition_API.Infrastructure.Commands;

using MediatR;

public class ApproveCommand : IRequest
{
    public int? Id { get; set; }

    public ApproveCommand(int? id)
    {
        this.Id = id;
    }

    public ApproveCommand() { }

}