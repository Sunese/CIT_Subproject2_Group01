using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    [HttpGet("{nameId}/rating")]
    public IActionResult GetRating(string nameId)
    {
        var nameRating = _nameService.GetRating(nameId);
        if (nameRating == null)
        {
            return NotFound("Name does not have a rating");
        }
        return Ok(nameRating);
    }

    // Get primary professions by name id
    [HttpGet("{nameId}/primaryProfessions")]
    public IActionResult GetPrimaryProfessions(string nameId)
    {
        var primaryProfessions = _nameService.GetPrimaryProfessions(nameId);
        if (primaryProfessions == null)
        {
            return NotFound("Name does not have any primary professions");
        }
        return Ok(primaryProfessions);
    }

    // Get known for titles by name id
    [HttpGet("{nameId}/knownForTitles")]
    public IActionResult GetKnownForTitles(string nameId)
    {
        var knownForTitles = _nameService.GetKnownForTitles(nameId);
        if (knownForTitles == null)
        {
            return NotFound("Name does not have any known for titles");
        }
        return Ok(knownForTitles);
    }

    // Get principals by name id
    [HttpGet("{nameId}/principals")]
    public IActionResult GetPrincipals(string nameId)
    {
        var principals = _nameService.GetPrincipals(nameId);
        if (principals == null)
        {
            return NotFound("Name does not have any principals");
        }
        return Ok(principals);
    }
}