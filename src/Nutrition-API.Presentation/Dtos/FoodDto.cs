namespace Nutrition_API.Presentation.Dtos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class FoodDto
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? VideoUrl { get; set; }

    public string? ImageUrl { get; set; }

    public string? Diet { get; set; }

    public bool IsApproved { get; set; }
}
