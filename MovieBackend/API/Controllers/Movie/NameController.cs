using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Models;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Movie;

[ApiController]
[Route("/api/v1/name")]
public class NameController : MovieBaseController
{
    private readonly INameService _nameService;
    public NameController(INameService nameService, LinkGenerator linkGenerator) : base(linkGenerator)
    {
        _nameService = nameService;
    }

    [HttpGet(Name = nameof(GetNames))]
    public IActionResult GetNames(int page = 0, int pageSize = 10)
    {
        var (names, total) = _nameService.GetNames(page, pageSize);
        var items = names.Select(CreateNamePageItem);
        return Ok(Paging(items, total, page, pageSize, nameof(GetNames)));
    }

    // Get name by id
    [HttpGet("{id}", Name = nameof(GetName))]
    public IActionResult GetName(string id)
    {
        var name = _nameService.GetName(id);
        if (name == null)
        {
            return NotFound();
        }
        return Ok(name);
    }

    // Get name rating by id
    [HttpGet("{id}/rating", Name = nameof(GetNameRating))]
    public IActionResult GetNameRating(string id)
    {
        var nameRating = _nameService.GetRating(id);
        if (nameRating == null)
        {
            return NotFound();
        }
        return Ok(nameRating);
    }

    // Get primary professions by name id
    [HttpGet("{id}/primaryProfessions", Name = nameof(GetPrimaryProfessions))]
    public IActionResult GetPrimaryProfessions(string id)
    {
        // names can have at most 3 professions, hence no need for paging
        // on another note, professions don't have any references to any pther entities either
        var primaryProfessions = _nameService.GetPrimaryProfessions(id);
        if (primaryProfessions == null)
        {
            return NoContent();
        }
        return Ok(primaryProfessions);
    }

    // Get known for titles by name id
    [HttpGet("{id}/knownForTitles", Name = nameof(GetKnownForTitles))]
    public IActionResult GetKnownForTitles(string id, int page = 0, int pageSize = 10)
    {
        var (knownForTitles, total) = _nameService.GetKnownForTitles(id, page, pageSize);
        if (knownForTitles == null)
        {
            return NotFound();
        }
        var items = knownForTitles.Select(CreateKnownForTitlePageItem);
            return Ok(Paging(items, total, page, pageSize, nameof(GetKnownForTitles), new RouteValueDictionary { { "id", id } }));
    }

    // Get principals by name id
    [HttpGet("{id}/principals", Name = nameof(GetPrincipals))]
    public IActionResult GetPrincipals(string id, int page = 0, int pageSize = 10)
    {
        var (principals, total) = _nameService.GetPrincipals(id, page, pageSize);
        if (principals == null)
        {
            return NotFound();
        }
        var items = principals.Select(CreatePrincipalPageItem);
        return Ok(Paging(items, total, page, pageSize, nameof(GetPrincipals), new RouteValueDictionary { { "id", id } }));
    }

    private object CreateNamePageItem(NameDTO nameDTO)
    {
        return new
        {
            NameID = nameDTO.NameID,
            Url = GetUrl(nameof(GetName), new { id = nameDTO.NameID }),
            Name = nameDTO.PrimaryName,
            BirthYear = nameDTO.BirthYear,
            DeathYear = nameDTO.DeathYear
        };
    }

    private object CreateKnownForTitlePageItem(KnownForTitlesDTO knownForTitles)
    {
        return new
        {
            TitleID = knownForTitles.TitleID,
            Poster = knownForTitles.Poster,
            Url = GetUrl("GetTitle", new { id = knownForTitles.TitleID }),
            PrimaryTitle = knownForTitles.PrimaryTitle,
            TitleType = knownForTitles.TitleType,
            Genres = knownForTitles.Genres.Select(g => g.GenreName)
        };
    }

    private object CreatePrincipalPageItem(PrincipalDTO principal)
    {
        return new
        {
            TitleID = principal.Title.TitleID,
            Url = GetUrl("GetTitle", new { id = principal.Title.TitleID }),
            Category = principal.Category,
            Job = principal.Job,
            Characters = principal.Characters.Select(c => c.CharacterName)
        };
    }
}