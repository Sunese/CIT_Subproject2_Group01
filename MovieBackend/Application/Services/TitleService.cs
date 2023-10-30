using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Context;
using Application.Models;
using AutoMapper;
using Domain.Models;

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

    public IList<TitleDTO> Get(DateTime startdatetime, DateTime enddatetime, int num, bool isAdult = false)
    {
        var titles = new List<Title>();
        // default input of start- and enddatetime
        if (startdatetime.Year == 1 && enddatetime.Year == 9999)
        {
            titles = _imdbContext.Titles
                .Where(t => t.IsAdult == isAdult)
                .Take(num)
                .ToList();
            return _mapper.Map<IList<TitleDTO>>(titles);
        }
        titles = _imdbContext.Titles
            .Where(t => 
                t.IsAdult == isAdult &&
                t.Released.HasValue &&
                t.Released.Value.Date >= startdatetime.Date &&
                t.Released.Value.Date <= enddatetime.Date)
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
        // need refactor
        if (year > 1 && month == 1)
        {
            titles = _imdbContext.Titles
                .Where(t =>
                        t.Released.HasValue &&
                        t.Released.Value.Year == year)
                .Take(num)
                .ToList();

            return _mapper.Map<IList<TitleDTO>>(titles);
        }
        if (year > 1 && month > 1)
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

    // will get popular movies with a period of time based on year or month.
    public IList<TitleDTO> GetPopular(DateTime datetime,  int num, bool isAdult = false)
    {
        throw new NotImplementedException();
    }


}