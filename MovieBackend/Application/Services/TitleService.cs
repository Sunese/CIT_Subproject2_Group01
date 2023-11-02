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

    public IList<TitleDTO> Get(DateOnly startDateTime, DateOnly endDateTime, int num, bool isAdult = false)
    {
        var titles = new List<Title>();
        // default input of start- and endDateTime
        if (startDateTime.Year == 1 && endDateTime.Year == 9999)
        {
            titles = _imdbContext.Titles
                .Include(t => t.Genres)
                .Where(t => t.IsAdult == isAdult)
                .Take(num)
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
            .Take(num)
            .ToList();
        return _mapper.Map<IList<TitleDTO>>(titles);
    }

    public TitleDTO GetTitle(string id, bool isAdult = false)
    {
        var title = _imdbContext.Titles.FirstOrDefault(t => t.TitleID == id && t.IsAdult == isAdult);
        return _mapper.Map<TitleDTO>(title);
    }

    // will get a number of current month and/or year of featured movies
    public IList<TitleDTO> GetFeature(int year, int month, int num, bool isAdult = false)
    {
        var titles = new List<Title>();
        
        if (year > 0 && month == 0)
        {
            titles = _imdbContext.Titles
                .Where(t =>
                    t.IsAdult == isAdult &&
                    t.Released.HasValue &&
                    t.Released.Value.Year == year)
                .Take(num)
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
                .Take(num)
                .ToList();

            return _mapper.Map<IList<TitleDTO>>(titles);
        }
        titles = _imdbContext.Titles.Take(num).ToList();
        return _mapper.Map<IList<TitleDTO>>(titles);
    }

    // will get popular movies with a period of time based on year or month, requires ratings to work
    public IList<TitleDTO> GetPopular(DateOnly datetime, int num, bool isAdult = false)
    {
        throw new NotImplementedException();
    }

    public TitleRatingDTO? GetRating(string id)
    {
        var title = _imdbContext.TitleRatings.Find(id);
        return _mapper.Map<TitleRatingDTO>(title);
    }

    public IList<TitleDTO> GetRatings(bool orderByHighestRating, int num, int? days)
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
                    .Take(num)
                    .ToList();
        }
        else
        {
            titles = _imdbContext.Titles
                    .Include(t => t.TitleRating)
                    .Where(t => t.TitleRating != null)
                    .Where(t => t.Released >= limit)
                    .Where(t => t.Released <= today)
                    .Take(num)
                    .ToList();
        }
        return _mapper.Map<IList<TitleDTO>>(titles);
    }
}