using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("/api/v1/name")]
public class NameController : ControllerBase
{
    private readonly INameService _nameService;
    public NameController(INameService nameService)
    {
        _nameService = nameService;
    }

    // Get name by id
    [HttpGet("{nameId}")]
    public IActionResult Get(string nameId)
    {
        var name = _nameService.GetName(nameId);
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
}