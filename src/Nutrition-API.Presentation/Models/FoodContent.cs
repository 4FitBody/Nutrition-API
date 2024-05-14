namespace Nutrition_API.Presentation.Models;

using Nutrition_API.Core.Models;

public class FoodContent
{
    public Food? Food { get; set; }
    public string? ImageFileName { get; set; }
    public byte[]? ImageFileContent { get; set; }
    public string? VideoFileName { get; set; }
    public byte[]? VideoFileContent { get; set; }
}
