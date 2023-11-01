﻿using Application.Models;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class TitleController : ControllerBase
{

    private readonly ILogger<TitleController> _logger;
    private readonly ITitleService _titleService;

    public TitleController(ILogger<TitleController> logger, ITitleService titleService)
    {
        _logger = logger;
        _titleService = titleService;
    }

    [HttpGet]
    public IList<TitleDTO> Get(int startYear = 1, int startMonth = 1, int startDay = 1, int endYear = 9999, int endMonth = 12, int endDay = 31, int num = 10)
    {
        DateTime startDate = new(startYear, startMonth, startDay);
        DateTime endDate = new(endYear, endMonth, endDay);
        return _titleService.Get(startDate, endDate, num);
    }

    [HttpGet("{id}")]
    public TitleDTO Get(string id)
    {
        return _titleService.GetTitle(id);
    }

    // Title/GetFeature
    [HttpGet(nameof(GetFeature))]
    public IList<TitleDTO> GetFeature(int year = 0, int month = 0, int count = 10)
    {
        return _titleService.GetFeature(year, month, count);
    }

    // Title/GetPopular
    [HttpGet(nameof(GetPopular))]
    public IList<TitleDTO> GetPopular(int year = 1, int month = 1, int day = 1, int count = 10)
    {
        DateTime datetime = new(year, month, day);
        return _titleService.GetPopular(datetime, count);
    }
}