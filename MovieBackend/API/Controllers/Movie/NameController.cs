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

    // Get name by id
    [HttpGet("{id}", Name = nameof(GetName))]
    public IActionResult GetName(string id)
    {
        var name = _nameService.GetName(id);
        if (name == null)
        {
            return NotFound("Name does not exist");
        }
        return Ok(name);
    }

    // Get name rating by id
    [HttpGet("{id}/rating", Name = nameof(GetRating))]
    public IActionResult GetRating(string id)
    {
        var nameRating = _nameService.GetRating(id);
        if (nameRating == null)
        {
            return NotFound("Name does not have a rating");
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
            return NotFound("Name does not have any primary professions");
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
            return NotFound("Name does not have any \"known for\" titles");
        }
        var items = knownForTitles.Select(CreateKnownForTitlePageItem);
        return Ok(Paging(items, total, page, pageSize, nameof(GetKnownForTitles), id));
    }

    // Get principals by name id
    [HttpGet("{id}/principals", Name = nameof(GetPrincipals))]
    public IActionResult GetPrincipals(string id, int page = 0, int pageSize = 10)
    {
        var (principals, total) = _nameService.GetPrincipals(id, page, pageSize);
        if (principals == null)
        {
            return NotFound("Name does not have any principals");
        }
        var items = principals.Select(CreatePrincipalPageItem);
        return Ok(Paging(items, total, page, pageSize, nameof(GetPrincipals), id));
    }

    private object CreateKnownForTitlePageItem(KnownForTitlesDTO knownForTitles)
    {
        return new
        {
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
            Url = GetUrl("GetTitle", new { id = principal.Title.TitleID }),
            Category = principal.Category,
            Job = principal.Job,
            Characters = principal.Characters.Select(c => c.CharacterName)
        };
    }
}