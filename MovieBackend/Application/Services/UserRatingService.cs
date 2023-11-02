using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Context;
using Application.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class UserRatingService : IUserRatingService
{
    private readonly ImdbContext _imdbContext;
    private readonly IMapper _mapper;

    public UserRatingService(ImdbContext imdbContext, IMapper mapper)
    {
        _imdbContext = imdbContext;
        _mapper = mapper;
    }
    public IList<UserTitleRatingDTO> GetUserTitleRatings(string username, int num)
    {
        var userRatings = _imdbContext.UserRatings
            .Include(t => t.Title)
            .Where(ur => ur.Username == username)
            .Take(num)
            .ToList();

        return _mapper.Map<IList<UserTitleRatingDTO>>(userRatings);
    }
}