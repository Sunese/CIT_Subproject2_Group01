using Application.Models;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Movie;

[ApiController]
[Route("api/v1/tvseries")]
public class TvSeriesController : MovieBaseController
{
    private readonly ILogger<TitleController> _logger;
    private readonly ITitleService _titleService;

    public TvSeriesController(ILogger<TitleController> logger, ITitleService titleService, LinkGenerator linkGenerator) : base(linkGenerator)
    {
        _logger = logger;
        _titleService = titleService;
    }

    [HttpGet(Name = nameof(GetAllTvSeries))]
    public IActionResult GetAllTvSeries(int page = 0, int pageSize = 10)
    {
        DateOnly startDate = DateOnly.MinValue;
        DateOnly endDate = DateOnly.MaxValue;
        var (titles, total) = _titleService.GetTvSeries(page, pageSize);
        var items = titles.Select(CreateTvSeriesPageItem);
        var result = Paging(items, total, page, pageSize, nameof(GetAllTvSeries));
        return Ok(result);
    }

    [HttpGet("{id}", Name = nameof(GetTvSeries))]
    public IActionResult GetTvSeries(string id, int page = 0, int pageSize = 10)
    {
        if (!IsValidTitleID(id))
        {
            return BadRequest();
        }
        if (!_titleService.TitleExists(id, out var titleDTO))
        {
            return NotFound();
        }
        if (!_titleService.IsTvSeries(id))
        {
            return BadRequest();
        }
        var (episodes, total) = _titleService.GetEpisodes(id, page, pageSize);
        var items = episodes.Select(CreateEpisodePageItem);
        var result = Paging(items, total, page, pageSize, nameof(GetTvSeries), new RouteValueDictionary { { "id", id } });
        return Ok(result);
    }

    private object CreateEpisodePageItem(EpisodeDTO episode)
    {
        return new
        {
            TitleID = episode.TitleID,
            Url = GetUrl("GetTitle", new { id = episode.TitleID }),
            Name = episode.Title.PrimaryTitle,
            Released = episode.Title.Released,
            Poster = episode.Title.Poster,
            EpisodeNumber = episode.EpisodeNumber,
            SeasonNumber = episode.SeasonNumber
        };
    }

    private object CreateTvSeriesPageItem(TitleDTO title)
    {
        return new
        {
            TitleID = title.TitleID,
            Url = GetUrl("GetTitle", new { id = title.TitleID }),
            Episodes = GetUrl(nameof(GetTvSeries), new { id = title.TitleID }),
            Name = title.PrimaryTitle,
            Released = title.Released,
            Poster = title.Poster
        };
    }
}