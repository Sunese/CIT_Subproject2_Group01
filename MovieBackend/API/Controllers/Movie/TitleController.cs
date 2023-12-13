using Application.Models;
using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Movie;

[ApiController]
[Route("api/v1/title")]
public class TitleController : MovieBaseController
{
    private readonly ILogger<TitleController> _logger;
    private readonly ITitleService _titleService;

    public TitleController(ILogger<TitleController> logger, ITitleService titleService, LinkGenerator linkGenerator) : base(linkGenerator)
    {
        _logger = logger;
        _titleService = titleService;
    }

    [HttpGet(Name = nameof(GetTitles))]
    public IActionResult GetTitles(int? startYear, int? startMonth, int? startDay, int? endYear,
        int? endMonth, int? endDay,  bool isAdult = false, int pageSize = 10, int page = 0)
    {
        DateOnly startDate = DateOnly.MinValue;
        DateOnly endDate = DateOnly.MaxValue;
        if (startYear.HasValue)
        {
            startDate = new(startYear.Value, startMonth ?? 1, startDay ?? 1);
        }
        if (endYear.HasValue)
        {
            endDate = new(endYear.Value, endMonth ?? 1, endDay ?? 1);
        }
        var (titles, total) = _titleService.GetTitles(startDate, endDate, pageSize, page, isAdult);
        var items = titles.Select(CreateTitlePageItem);
        var result = Paging(items, total, page, pageSize, nameof(GetTitles));
        return Ok(result);
    }

    [HttpGet("{id}", Name = nameof(GetTitle))]
    public IActionResult GetTitle(string id)
    {
        if (!_titleService.TitleExists(id, out var titleDTO))
        {
            return NotFound();
        }
        return Ok(titleDTO);
    }

    [HttpGet("featured", Name = nameof(GetFeaturedTitles))]
    public IActionResult GetFeaturedTitles(int page = 0, int pageSize = 10)
    {
        var (titles, total) = _titleService.GetFeatured(page, pageSize);
        var items = titles.Select(CreateTitlePageItem);
        return Ok(Paging(items, total, page, pageSize, nameof(GetFeaturedTitles)));
    }

    [HttpGet("popular", Name = nameof(GetPopularTitles))]
    public IActionResult GetPopularTitles(int page = 0, int pageSize = 10)
    {
        var (titles, total) = _titleService.GetPopular(page, pageSize);
        var items = titles.Select(CreateTitlePageItem);
        return Ok(Paging(items, total, page, pageSize, nameof(GetPopularTitles)));
    }

    [HttpGet("{id}/rating", Name = nameof(GetTitleRating))]
    public IActionResult GetTitleRating(string id)
    {
        var rating = _titleService.GetRating(id);
        if (rating == null)
        {
            return NotFound();
        }
        return Ok(rating);
    }

    // title/rating?OrderByHighestRating=true&count=10&days=30
    // where OrderBy can be AverageRating or NumVotes
    // count is the number of titles to return
    // all query parameters are optional
    [HttpGet("rating", Name = nameof(GetTitleRatings))]
    public IActionResult GetTitleRatings(int page = 0, int pageSize = 10, bool orderByHighestRating = true, int? days = null)
    {
        var (ratings, total) = _titleService.GetRatings(page, pageSize, orderByHighestRating, days);
        var items = ratings.Select(CreateTitleRatingPageItem);
        return Ok(Paging(items, total, page, pageSize, nameof(GetTitleRatings)));
    }

    // title/{id}/popularActors
    [HttpGet("{id}/popularActors", Name = nameof(GetPopularActorsFromTitle))]
    public IActionResult GetPopularActorsFromTitle(string id, int page = 0, int pageSize = 10)
    {
        var (popularActors, total) = _titleService.GetPopularActors(id, page, pageSize);
        var items = popularActors.Select(CreatePopularActorPageItem);
        return Ok(Paging(items, total, page, pageSize, nameof(GetPopularActorsFromTitle), new RouteValueDictionary { { "id", id } }));
    }


    [HttpGet("{id}/aka", Name = nameof(GetTitleAkas))]
    public IActionResult GetTitleAkas(string id, int page = 0, int pageSize = 10)
    {
        var (akas, total) = _titleService.GetAkas(id, page, pageSize);
        var items = akas.Select(CreateAkaPageItem);
        return Ok(Paging(items, total, page, pageSize, nameof(GetTitleAkas), new RouteValueDictionary { { "id", id } }));
    }

    [HttpGet("{id}/similarmovies", Name = nameof(GetSimiliarMovies))]
    public IActionResult GetSimiliarMovies(string id, int page = 0, int pageSize = 10)
    {
        var (similiarMovies, total) = _titleService.GetSimiliarMovies(id, page, pageSize);
        var items = similiarMovies.Select(CreateSimiliarMoviePageItem);
        return Ok(Paging(items, total, page, pageSize, nameof(GetSimiliarMovies), new RouteValueDictionary { { "id", id } }));
    }

    [HttpGet("{id}/directors", Name = nameof(GetDirectors))]
    public IActionResult GetDirectors(string id)
    {
        var directors = _titleService.GetDirectors(id);
        if (directors == null)
        {
            return NotFound();
        }
        return Ok(directors.Select(d => new
        {
            Url = Url.Link("GetName", new { id = d.NameID }),
            PrimaryName = d.Name.PrimaryName,
            NameID = d.Name.NameID
        }));
    }

    [HttpGet("{id}/writers", Name = nameof(GetWriters))]
    public IActionResult GetWriters(string id)
    {
        var writers = _titleService.GetWriters(id);
        if (writers == null)
        {
            return NotFound();
        }
        return Ok(writers.Select(w => new {
            Url = Url.Link("GetName", new { id = w.NameID }),
            PrimaryName = w.Name.PrimaryName,
            NameID = w.Name.NameID}));
    }

    private object CreateTitlePageItem(TitleDTO title)
    {
        return new
        {
            TitleID = title.TitleID,
            Url = GetUrl(nameof(GetTitle), new { id = title.TitleID }),
            Name = title.PrimaryTitle,
            Released = title.Released,
            Poster = title.Poster
        };
    }

    private object CreateTitleRatingPageItem(TitleRatingDTO titleRatingDTO)
    {
        return new
        {
            TitleID = titleRatingDTO.TitleID,
            Url = GetUrl(nameof(GetTitleRating), new { id = titleRatingDTO.TitleID }),
            PrimaryTitle = titleRatingDTO.PrimaryTitle,
            AverageRating = titleRatingDTO.AverageRating,
            NumVotes = titleRatingDTO.NumVotes
        };
    }

    private object CreatePopularActorPageItem(PopularActorsDTO popularActorsDTO)
    {
        return new
        {
            NameID = popularActorsDTO.NameID,
            Url = GetUrl("GetName", new { id = popularActorsDTO.NameID }),
            Name = popularActorsDTO.PrimaryName,
            Rating = popularActorsDTO.Rating,
        };
    }

    private object CreateAkaPageItem(AkaDTO akaDTO)
    {
        return new
        {
            TitleID = akaDTO.TitleID,
            Url = GetUrl(nameof(GetTitleAkas), new { id = akaDTO.TitleID }),
            TitleName = akaDTO.TitleName,
            Language = akaDTO.Language,
            Region = akaDTO.Region
        };
    }

    private object CreateSimiliarMoviePageItem(SimiliarMoviesResultDTO similiarMovieDTO)
    {
        return new
        {
            TitleID = similiarMovieDTO.TitleID,
            Url = GetUrl(nameof(GetTitle), new { id = similiarMovieDTO.TitleID }),
            Title = similiarMovieDTO.PrimaryTitle
        };
    }
}