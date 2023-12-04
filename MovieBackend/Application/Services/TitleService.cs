using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Context;
using Application.Models;
using AutoMapper;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class TitleService : ITitleService
{
    private readonly ImdbContext _imdbContext;
    private readonly IMapper _mapper;
    public TitleService(ImdbContext imdbContext, IMapper mapper)
    {
        _imdbContext = imdbContext;
        _mapper = mapper;
    }

    public bool TitleExists(string id, out TitleDTO? titleDTO)
    {
        var title = _imdbContext.Titles.Find(id);
        if (title is null)
        {
            titleDTO = null;
            return false;
        }
        titleDTO = GetTitle(id);
        // remove tracking
        _imdbContext.Titles.Entry(title).State = EntityState.Detached;
        return true;
    }

    public (IList<TitleDTO>, int) GetTitles(DateOnly startDateTime, DateOnly endDateTime, int pageSize, int page, bool isAdult)
    {
        var filtered = _imdbContext.Titles
            .Include(t => t.Genres)
            .Where(t => 
                //t.IsAdult == isAdult && // Until we implement child accounts, this does not matter and will filter away a lot of data
                t.Released.HasValue && // NOTE: we strip away all movies that are NOT present in OMDb here
                t.Released.Value.Year >= startDateTime.Year &&
                t.Released.Value.Year <= endDateTime.Year);
        var paged = filtered
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToList();
        return (_mapper.Map<IList<TitleDTO>>(paged), filtered.Count());
    }

    public TitleDTO GetTitle(string id, bool isAdult = false)
    {
        var title = _imdbContext.Titles
            .Where(t => 
                t.TitleID == id)
            //&& t.IsAdult == isAdult) // Until we implement child accounts, this does not matter and will filter away a lot of data
            .Include(t => t.Genres)
            .Include(t => t.TitleRating);
        return _mapper.Map<TitleDTO>(title.FirstOrDefault());
    }

    // Get featured movies each month
    // NOTE: for now we simply define "featured" as movies released this month
    public (IList<TitleDTO>, int) GetFeatured(int page, int pageSize)
    {
        var month = DateTime.Now.Month;
        var year = DateTime.Now.Year;
        var filteredTitles = _imdbContext.Titles
                .Where(t => 
                        t.Released.HasValue && 
                        t.Released.Value.Month == month &&
                        t.Released.Value.Year == year)
                .OrderByDescending(t => t.Released);

        var paged = filteredTitles
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToList();

        return (_mapper.Map<IList<TitleDTO>>(paged), filteredTitles.Count());
    }

    // will get the best rated movies from the last month
    public (IList<TitleDTO>, int) GetPopular(int page, int pageSize)
    {
        var filteredTitles = _imdbContext.Titles
                .Include(t => t.TitleRating)
                .Where(t =>
                        t.Released.HasValue && 
                        t.Released.Value <= DateOnly.FromDateTime(DateTime.Now.AddDays(30)) &&
                        t.Released.Value >= DateOnly.FromDateTime(DateTime.Now.AddDays(-30)))
                .OrderByDescending(t => t.TitleRating.AverageRating);

        var paged = filteredTitles
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToList();

        return (_mapper.Map<IList<TitleDTO>>(paged), filteredTitles.Count());
    }

    public TitleRatingDTO? GetRating(string id)
    {
        var title = _imdbContext.Titles
            .Where(t => t.TitleID == id)
            .Include(t => t.TitleRating)
            .FirstOrDefault();
        return _mapper.Map<TitleRatingDTO>(title);
    }

    // Returns N amount of titles ordered by ratings, between input day and today.
    // If no day is inputted, then it returns all titles and its ratings between year 0 and today.
    public (IList<TitleRatingDTO>, int) GetRatings(int page, int pageSize, bool orderByHighestRating, int? days)
    {
        IEnumerable<Title> filtered;
        var today = DateOnly.FromDateTime(DateTime.Now);
        var limit = DateOnly.MinValue;
        if (days.HasValue)
        {
            limit = today.AddDays(-days.Value);
        }
        if (orderByHighestRating)
        {
                filtered = _imdbContext.Titles
                    .Include(t => t.TitleRating)
                    .Where(t => t.TitleRating != null)
                    .Where(t => t.Released >= limit)
                    .Where(t => t.Released <= today)
                    .OrderByDescending(t => t.TitleRating.AverageRating)
                    .ThenByDescending(t => t.TitleRating.NumVotes);
        }
        else
        {
            filtered = _imdbContext.Titles
                    .Include(t => t.TitleRating)
                    .Where(t => t.TitleRating != null)
                    .Where(t => t.Released >= limit)
                    .Where(t => t.Released <= today)
                    .OrderByDescending(t => t.TitleRating.AverageRating)
                    .ThenByDescending(t => t.TitleRating.NumVotes);
        }
        var paged = filtered
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToList();
        return (_mapper.Map<IList<TitleRatingDTO>>(paged), filtered.Count());
    }

    public (IList<PopularActorsDTO>, int) GetPopularActors(string titleId, int page, int pageSize)
    {
        var actors = _imdbContext.PopularActorsResults
            .FromSqlInterpolated($"SELECT * FROM popular_actors({titleId})");
        var paged = actors
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToList();
        return (_mapper.Map<IList<PopularActorsDTO>>(paged), actors.Count());
    }

    public (IList<AkaDTO>, int) GetAkas(string id, int page, int pageSize)
    {
        var filtered = _imdbContext.Aka
            .Where(ta => ta.TitleId == id)
            .Include(ty => ty.Types);
        var paged = filtered
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToList();

        return (_mapper.Map<IList<AkaDTO>>(paged), filtered.Count());
    }

    public (IList<SimiliarMoviesResultDTO>, int) GetSimiliarMovies(string titleId, int page, int pageSize)
    {
        var similiarMovies = _imdbContext.SimiliarMoviesResult
            .FromSqlInterpolated($"SELECT * FROM similar_movies({titleId})");
        var paged = similiarMovies
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToList();
        return (_mapper.Map<IList<SimiliarMoviesResultDTO>>(paged), similiarMovies.Count());
    }

    public (IList<TitleDTO> titles, int count) GetTvSeries(int page, int pageSize)
    {
        var filtered = _imdbContext.Titles
            .Include(t => t.Genres)
            .Where(t => t.TitleType == "tvSeries");
        var paged = filtered
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToList();
        return (_mapper.Map<IList<TitleDTO>>(paged), filtered.Count());
    }

    public (IList<EpisodeDTO>, int count) GetEpisodes(string id, int page, int pageSize)
    {
        var filtered = _imdbContext.Episodes
            .Include(e => e.Title)
            .Include(e => e.ParentTitle)
            .Where(e => e.ParentTitleID == id)
            .OrderBy(e => e.SeasonNumber)
            .ThenBy(e => e.EpisodeNumber);
        var paged = filtered
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToList();
        return (_mapper.Map<IList<EpisodeDTO>>(paged), filtered.Count());
    }

    public bool IsTvSeries(string id)
    {
        var title = _imdbContext.Titles.Find(id);
        if (title is null)
        {
            return false;
        }
        return title.TitleType == "tvSeries";
    }

    public IList<WriterDTO> GetWriters(string id)
    {
        var writers = _imdbContext.Writers
            .Where(w => w.TitleID == id)
            .Include(w => w.Name)
            .Include(w => w.Title)
            .ToList();
        return _mapper.Map<IList<WriterDTO>>(writers);
    }

    public IList<DirectorDTO> GetDirectors(string id)
    {
        var directors = _imdbContext.Directors
            .Where(d => d.TitleID == id)
            .Include(d => d.Name)
            .Include(d => d.Title)
            .ToList();
        return _mapper.Map<IList<DirectorDTO>>(directors);
    }
}