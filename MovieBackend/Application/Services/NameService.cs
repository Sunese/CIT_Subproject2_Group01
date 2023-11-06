using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Context;
using Application.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

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

    public List<KnownForTitlesDTO> GetKnownForTitles(string nameId)
    {
        var knownForTitles = _imdbContext.Names
            .Where(n => n.NameId == nameId)
            .Include(n => n.KnownForTitles)
            .ThenInclude(t => t.Genres)
            .SelectMany(n => n.KnownForTitles)
            .ToList();
        return _mapper.Map<List<KnownForTitlesDTO>>(knownForTitles);
    }

    public List<ProfessionDTO> GetPrimaryProfessions(string nameId)
    {
        var primaryProfessions = _imdbContext.Names
            .Where(n => n.NameId == nameId)
            .Include(n => n.PrimaryProfessions)
            .SelectMany(n => n.PrimaryProfessions)
            .ToList();
        return _mapper.Map<List<ProfessionDTO>>(primaryProfessions);
    }

    public List<PrincipalDTO> GetPrincipals(string nameId)
    {
        var principals = _imdbContext.Names
            .Where(n => n.NameId == nameId)
            .Include(n => n.Principals).ThenInclude(p => p.Title)
            .Include(n => n.Principals).ThenInclude(p => p.Characters)
            .SelectMany(n => n.Principals)
            .ToList();
        return _mapper.Map<List<PrincipalDTO>>(principals);
    }
}