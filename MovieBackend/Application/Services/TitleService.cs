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
        // remove tracking
        _imdbContext.Entry(title).State = EntityState.Detached;
        titleDTO = _mapper.Map<TitleDTO>(title);
        return title != null;
    }

    public IList<TitleDTO> Get(DateOnly startDateTime, DateOnly endDateTime, int count, bool isAdult = false)
    {
        var titles = new List<Title>();
        // default input of start- and endDateTime
        if (startDateTime.Year == 1 && endDateTime.Year == 9999)
        {
            titles = _imdbContext.Titles
                .Include(t => t.Genres)  
                .Where(t => t.IsAdult == isAdult)
                .Take(count)
                .ToList();
            return _mapper.Map<IList<TitleDTO>>(titles);
        }
        titles = _imdbContext.Titles
            .Include(t => t.Genres)
            .Where(t => 
                t.IsAdult == isAdult &&
                t.Released.HasValue &&
                t.Released.Value.Year >= startDateTime.Year &&
                t.Released.Value.Year <= endDateTime.Year)
            .Take(count)
            .ToList();
        return _mapper.Map<IList<TitleDTO>>(titles);
    }

    public TitleDTO GetTitle(string id, bool isAdult = false)
    {
        var title = _imdbContext.Titles
            .Where(t => t.TitleID == id && t.IsAdult == isAdult)
            .Include(t => t.Genres)
            .Include(t => t.TitleRating);
        return _mapper.Map<TitleDTO>(title.FirstOrDefault());
    }

    // will get a number of current month and/or year of featured movies
    public IList<TitleDTO> GetFeature(int year, int month, int count, bool isAdult = false)
    {
        var titles = new List<Title>();
        
        if (year > 0 && month == 0)
        {
            titles = _imdbContext.Titles
                .Where(t =>
                    t.IsAdult == isAdult &&
                    t.Released.HasValue &&
                    t.Released.Value.Year == year)
                .Take(count)
                .ToList();

            return _mapper.Map<IList<TitleDTO>>(titles);
        }
        if (year > 0 && month > 0)
        {
            titles = _imdbContext.Titles
                .Where(t => 
                        t.Released.HasValue &&
                        t.Released.Value.Year == year &&
                        t.Released.Value.Month == month)
                .Take(count)
                .ToList();

            return _mapper.Map<IList<TitleDTO>>(titles);
        }
        titles = _imdbContext.Titles.Take(count).ToList();
        return _mapper.Map<IList<TitleDTO>>(titles);
    }

    // will get popular movies with a period of time based on year or month, requires ratings to work
    public IList<TitleDTO> GetPopular(DateOnly datetime, int count, bool isAdult = false)
    {
        throw new NotImplementedException();
    }

    public TitleRatingDTO? GetRating(string id)
    {
        var title = _imdbContext.TitleRatings.Find(id);
        return _mapper.Map<TitleRatingDTO>(title);
    }

    // Returns N amount of titles ordered by ratings, between input day and today.
    // If no day is inputted, then it returns all titles and its ratings between year 0 and today.
    public IList<TitleDTO> GetRatings(bool orderByHighestRating, int count, int? days)
    {
        IList<Title> titles;
        var today = DateOnly.FromDateTime(DateTime.Now);
        var limit = DateOnly.MinValue;
        if (days.HasValue)
        {
            limit = today.AddDays(-days.Value);
        }
        if (orderByHighestRating)
        {
                titles = _imdbContext.Titles
                    .Include(t => t.TitleRating)
                    .Where(t => t.TitleRating != null)
                    .Where(t => t.Released >= limit)
                    .Where(t => t.Released <= today)
                    .OrderByDescending(t => t.TitleRating.AverageRating)
                    .Take(count)
                    .ToList();
        }
        else
        {
            titles = _imdbContext.Titles
                    .Include(t => t.TitleRating)
                    .Where(t => t.TitleRating != null)
                    .Where(t => t.Released >= limit)
                    .Where(t => t.Released <= today)
                    .Take(count)
                    .ToList();
        }
        return _mapper.Map<IList<TitleDTO>>(titles);
    }

    public IList<PopularActorsDTO> GetPopularActors(string titleId)
    {
        var actors = _imdbContext.PopularActorsResults
            .FromSqlInterpolated($"SELECT * FROM popular_actors({titleId})")
            .ToList();
        return _mapper.Map<IList<PopularActorsDTO>>(actors);
    }

    public IList<AkaDTO> GetAkas(string id, int ordering)
    {
        if (ordering != 0)
        {
            return _mapper.Map<IList<AkaDTO>>(
                _imdbContext.Aka
                .Where(ta => ta.TitleId == id && ta.Ordering == ordering)
                .Include(ty => ty.Types)
                .ToList());
        }

        return _mapper.Map<IList<AkaDTO>>(
            _imdbContext.Aka
                .Where(ta => ta.TitleId == id)
                .Include(ty => ty.Types)
                .ToList());
    }

    public IList<SimiliarMoviesResultDTO> GetSimiliarMovies(string titleId)
    {
        return _mapper.Map<IList<SimiliarMoviesResultDTO>>(
            _imdbContext.SimiliarMoviesResult
                .FromSqlInterpolated($"SELECT * FROM similar_movies({titleId})"))
                .ToList();
    }
}