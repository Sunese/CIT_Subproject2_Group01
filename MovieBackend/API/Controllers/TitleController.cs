using Application.Models;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class TitleController : ControllerBase
{

    private readonly ILogger<TitleController> _logger;
    private readonly ITitleService _titleService;

    public TitleController(ILogger<TitleController> logger, ITitleService titleService)
    {
        _logger = logger;
        _titleService = titleService;
    }

    [HttpGet]
    public IList<TitleDTO> Get(int startyear = 1, int startmonth = 1, int startday = 1, int endyear = 9999, int endmonth = 12, int endday = 31, int count = 10)
    {
        DateTime startDate = new DateTime(startyear, startmonth, startday);
        DateTime endDate = new DateTime(endyear, endmonth, endday);
        return _titleService.Get(startDate, endDate, count);
    }

    [HttpGet("{id}")]
    public TitleDTO Get(string id)
    {
        return _titleService.GetTitle(id);
    }

    // Title/GetFeature
    [HttpGet(nameof(GetFeature))]
    public IList<TitleDTO> GetFeature(int year = 00, int month = 00, int count = 10)
    {
        return _titleService.GetFeature(year, month, count);
    }

    // Title/GetPopular
    [HttpGet(nameof(GetPopular))]
    public IList<TitleDTO> GetPopular(int year = 1, int month = 1, int day = 1, int count = 10)
    {
        DateTime datetime = new DateTime(year, month, day);
        return _titleService.GetPopular(datetime, count);
    }
}