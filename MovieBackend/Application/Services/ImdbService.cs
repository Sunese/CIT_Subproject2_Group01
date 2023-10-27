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

public class ImdbService : IImdbService
{
    private readonly ImdbContext _imdbContext;
    private readonly IMapper _mapper;
    public ImdbService(ImdbContext imdbContext, IMapper mapper)
    {
        _imdbContext = imdbContext;
        _mapper = mapper;
    }
    public IList<TitleDTO> GetTitles(int num)
    {
        var titles = _imdbContext.Titles.Take(num).ToList();
        return _mapper.Map<IList<TitleDTO>>(titles);
    }

    public TitleDTO GetTitle(string id)
    {
        var title = _imdbContext.Titles.FirstOrDefault(t => t.TitleID == id);
        return _mapper.Map<TitleDTO>(title);
    }
}