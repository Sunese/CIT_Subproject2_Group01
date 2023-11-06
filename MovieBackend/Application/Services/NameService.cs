using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Context;
using Application.Models;
using AutoMapper;

namespace Application.Services;

public class NameService : INameService
{
    private readonly ImdbContext _imdbContext;
    private readonly IMapper _mapper;
    public NameService(
        ImdbContext imdbContext,
        IMapper mapper)
    {
        _imdbContext = imdbContext;
        _mapper = mapper;

    }

    public NameDTO GetName(string nameId)
    {
        var name = _imdbContext.Names
            .Where(n => n.NameId == nameId)
            .FirstOrDefault();
        return _mapper.Map<NameDTO>(name);
    }

    public NameRatingDTO GetRating(string nameId)
    {
        var nameRating = _imdbContext.NameRatings
            .Where(nr => nr.NameId == nameId)
            .FirstOrDefault();
        return _mapper.Map<NameRatingDTO>(nameRating);
    }
}