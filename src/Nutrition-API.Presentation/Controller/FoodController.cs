namespace Nutrition_API.Presentation.Controller;

using MediatR;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Nutrition_API.Core.Models;
using Microsoft.Extensions.Options;
using Nutrition_API.Infrastructure.Commands;
using Nutrition_API.Infrastructure.Queries;
using Nutrition_API.Infrastructure.Services;
using Nutrition_API.Presentation.Dtos;
using Nutrition_API.Presentation.Models;
using Nutrition_API.Presentation.Options;

[ApiController]
[Route("api/[controller]/[action]")]
public class FoodController : ControllerBase
{
    private readonly ISender sender;
    private readonly BlobContainerService blobContainerService;

    public FoodController(ISender sender, IOptions<BlobOptions> blobOptions)
    {
        this.sender = sender;

        this.blobContainerService = new BlobContainerService(blobOptions.Value.Url, blobOptions.Value.ContainerName);
    }

    [HttpGet]
    [ActionName("Index")]
    public async Task<IActionResult> GetAll()
    {
        var getAllQuery = new GetAllQuery();

        var foods = await this.sender.Send(getAllQuery);

        return base.Ok(foods.Where(food => food.IsApproved));
    }

    [HttpGet]
    [Route("/api/[controller]/[action]/{id}")]
    public async Task<IActionResult> Details(int id)
    {
        var getByIdQuery = new GetByIdQuery(id);

        var food = await this.sender.Send(getByIdQuery);

        return base.Ok(food);
    }

    [HttpPost]
    public async Task<IActionResult> Create(object foodContentJson)
    {
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.None,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented
        };

        var foodContent = JsonConvert.DeserializeObject<FoodContent>(foodContentJson.ToString()!, settings);  

        var imageFileName = foodContent!.ImageFileName;

        var imageFileData = foodContent.ImageFileContent;

        string imageRawPath = Guid.NewGuid().ToString() + imageFileName;

        var imagePath = imageRawPath.Replace(" ", "%20");

        var videoFileName = foodContent!.VideoFileName;

        var videoFileData = foodContent.VideoFileContent;

        string videoRawPath = Guid.NewGuid().ToString() + videoFileName;

        var videoPath = imageRawPath.Replace(" ", "%20");

        var food = new Food
        {
            Name = foodContent.Food!.Name,
            Description = foodContent.Food.Description,
            Diet = foodContent.Food.Diet,
            IsApproved = false,
            ImageUrl = "https://4fitbodystorage.blob.core.windows.net/images/" + imagePath,
            VideoUrl = "https://4fitbodystorage.blob.core.windows.net/videos/" + videoPath
        };

        await this.blobContainerService.UploadAsync(new MemoryStream(imageFileData!), imageRawPath);
        await this.blobContainerService.UploadAsync(new MemoryStream(videoFileData!), videoRawPath);

        var createCommand = new CreateCommand(food);

        await this.sender.Send(createCommand);

        return base.Ok();
    }

    [HttpDelete]
    [Route("/api/[controller]/[action]/{id}")]
    public async Task<IActionResult> Delete(int? id)
    {
        var createCommand = new DeleteCommand(id);

        await this.sender.Send(createCommand);

        return base.RedirectToAction(actionName: "Index");
    }

    [HttpPut]
    [Route("/api/[controller]/[action]/{id}")]
    public async Task<IActionResult> Update(int? id, [FromBody] FoodDto foodDto)
    {
        var food = new Food
        {
            Name = foodDto.Name,
            Description = foodDto.Description,
            Diet = foodDto.Diet,
            IsApproved = false,
        };

        var createCommand = new UpdateCommand(id, food);

        await this.sender.Send(createCommand);

        return base.RedirectToAction(actionName: "Index");
    }
}