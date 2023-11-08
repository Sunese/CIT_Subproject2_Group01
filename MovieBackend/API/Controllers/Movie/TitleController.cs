using Application.Models;
using Application.Services;
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

        return Ok(Paging(items, total, page, pageSize, nameof(GetTitles)));
    }

    [HttpGet("{id}", Name = nameof(GetById))]
    public IActionResult GetById(string id)
    {
        if (!_titleService.TitleExists(id, out var titleDTO))
        {
            return NotFound("Title does not exist");
        }
        return Ok(titleDTO);
    }

    [HttpGet("featured", Name = nameof(GetFeatured))]
    public IActionResult GetFeatured(int page = 0, int pageSize = 10)
    {
        var (titles, total) = _titleService.GetFeatured(page, pageSize);
        var items = titles.Select(CreateTitlePageItem);
        return Ok(Paging(items, total, page, pageSize, nameof(GetFeatured)));
    }

    [HttpGet("popular", Name = nameof(GetPopular))]
    public IActionResult GetPopular(int page = 0, int pageSize = 10)
    {
        var (titles, total) = _titleService.GetPopular(page, pageSize);
        var items = titles.Select(CreateTitlePageItem);
        return Ok(Paging(items, total, page, pageSize, nameof(GetPopular)));
    }

    [HttpGet("{id}/rating", Name = nameof(GetRating))]
    public IActionResult GetRating(string id)
    {
        var rating = _titleService.GetRating(id);
        if (rating == null)
        {
            return NotFound("Title does not have a rating");
        }
        return Ok(rating);
    }

    // title/rating?OrderByHighestRating=true&count=10&days=30
    // where OrderBy can be AverageRating or NumVotes
    // count is the number of titles to return
    // all query parameters are optional
    [HttpGet("rating", Name = nameof(GetRatings))]
    public IActionResult GetRatings(int page = 0, int pageSize = 10, bool orderByHighestRating = true, int? days = null)
    {
        var (ratings, total) = _titleService.GetRatings(page, pageSize, orderByHighestRating, days);
        var items = ratings.Select(CreateTitleRatingPageItem);
        return Ok(Paging(items, total, page, pageSize, nameof(GetRatings)));
    }

    // title/{id}/popularActors
    [HttpGet("{id}/popularActors", Name = nameof(GetPopularActors))]
    public IActionResult GetPopularActors(string id, int page = 0, int pageSize = 10)
    {
        var (popularActors, total) = _titleService.GetPopularActors(id, page, pageSize);
        var items = popularActors.Select(CreatePopularActorPageItem);
        return Ok(Paging(items, total, page, pageSize, nameof(GetPopularActors), id));
    }


    [HttpGet("{id}/aka", Name = nameof(GetAkas))]
    public IActionResult GetAkas(string id, int page = 0, int pageSize = 10)
    {
        var (akas, total) = _titleService.GetAkas(id, page, pageSize);
        var items = akas.Select(CreateAkaPageItem);
        return Ok(Paging(items, total, page, pageSize, nameof(GetAkas), id));
    }

    [HttpGet("{id}/similarmovies", Name = nameof(GetSimiliarMovies))]
    public IActionResult GetSimiliarMovies(string id, int page = 0, int pageSize = 10)
    {
        var (similiarMovies, total) = _titleService.GetSimiliarMovies(id, page, pageSize);
        var items = similiarMovies.Select(CreateSimiliarMoviePageItem);
        return Ok(Paging(items, total, page, pageSize, nameof(GetSimiliarMovies), id));
    }

    private object CreateTitlePageItem(TitleDTO title)
    {
        return new
        {
            Url = GetUrl(nameof(GetById), new { id = title.TitleID }),
            Name = title.PrimaryTitle,
            Released = title.Released,
            Poster = title.Poster
        };
    }

    private object CreateTitleRatingPageItem(TitleRatingDTO titleRatingDTO)
    {
        return new
        {
            Url = GetUrl(nameof(GetRating), new { id = titleRatingDTO.TitleID }),
            PrimaryTitle = titleRatingDTO.PrimaryTitle,
            AverageRating = titleRatingDTO.AverageRating,
            NumVotes = titleRatingDTO.NumVotes
        };
    }

    private object CreatePopularActorPageItem(PopularActorsDTO popularActorsDTO)
    {
        return new
        {
            Url = GetUrl("GetName", new { id = popularActorsDTO.NameId }),
            Name = popularActorsDTO.PrimaryName,
            Rating = popularActorsDTO.Rating,
        };
    }

    private object CreateAkaPageItem(AkaDTO akaDTO)
    {
        return new
        {
            Url = GetUrl(nameof(GetAkas), new { id = akaDTO.TitleId }),
            TitleName = akaDTO.TitleName,
            Language = akaDTO.Language,
            Region = akaDTO.Region
        };
    }

    private object CreateSimiliarMoviePageItem(SimiliarMoviesResultDTO similiarMovieDTO)
    {
        return new
        {
            Url = GetUrl(nameof(GetById), new { id = similiarMovieDTO.TitleId }),
            Title = similiarMovieDTO.PrimaryTitle
        };
    }
}