using Application.Models;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class TitleController : ControllerBase
{

    private readonly ILogger<TitleController> _logger;
    private readonly IImdbService _imdbService;

    public TitleController(ILogger<TitleController> logger, IImdbService imdbService)
    {
        _logger = logger;
        _imdbService = imdbService;
    }

    [HttpGet]
    public IList<TitleDTO> Get()
    {
        return _imdbService.GetTitles(10);
    }

    [HttpGet("{id}")]
    public TitleDTO Get(string id)
    {
        return _imdbService.GetTitle(id);
    }
}