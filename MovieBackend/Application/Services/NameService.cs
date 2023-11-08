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

    public (IList<KnownForTitlesDTO>, int) GetKnownForTitles(string nameId, int page, int pageSize)
    {
        var knownForTitles = _imdbContext.Names
            .Where(n => n.NameId == nameId)
            .Include(n => n.KnownForTitles)
            .ThenInclude(t => t.Genres)
            .SelectMany(n => n.KnownForTitles);
        var paged = knownForTitles
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToList();
        return (_mapper.Map<List<KnownForTitlesDTO>>(paged), knownForTitles.Count());
    }

    public IList<ProfessionDTO> GetPrimaryProfessions(string nameId)
    {
        var primaryProfessions = _imdbContext.Names
            .Where(n => n.NameId == nameId)
            .Include(n => n.PrimaryProfessions)
            .SelectMany(n => n.PrimaryProfessions)
            .ToList();
        return _mapper.Map<List<ProfessionDTO>>(primaryProfessions);
    }

    public (IList<PrincipalDTO>, int) GetPrincipals(string nameId, int page, int pageSize)
    {
        var principals = _imdbContext.Names
            .Where(n => n.NameId == nameId)
            .Include(n => n.Principals).ThenInclude(p => p.Title)
            .Include(n => n.Principals).ThenInclude(p => p.Characters)
            .SelectMany(n => n.Principals);
        var paged = principals
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToList();
        return (_mapper.Map<List<PrincipalDTO>>(paged), principals.Count());
    }
}