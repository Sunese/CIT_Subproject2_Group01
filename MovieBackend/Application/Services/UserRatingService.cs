using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Context;
using Application.Models;
using AutoMapper;
using Domain.Models;
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

    public bool CreateUserTitleRating(UserTitleRatingDTO userTitleRatingDTO)
    {
        var userTitleRating = _mapper.Map<UserTitleRating>(userTitleRatingDTO);
        var rowsAffected = _imdbContext.Database.ExecuteSqlInterpolated(
            $"CALL rate({userTitleRating.Username}, {userTitleRating.TitleId}, {userTitleRating.Rating})");
        // TODO: rowsAffected returns -1 even though the rating was created
        // for now we just return true and rely on an exception being thrown
        // if something went wrong
        return true;
    }

    public IList<UserTitleRatingDTO> GetUserTitleRatings(string username)
    {
        var userRatings = _imdbContext.UserTitleRatings
            // TODO: consider setting up a materialized view for titles
            // with ratings and genres to avoid having to do this
            // every time
            .Include(userRating => userRating.Title).ThenInclude(title => title.TitleRating)
            .Include(userRating => userRating.Title).ThenInclude(title => title.Genres)
            .Where(ur => ur.Username == username)
            .ToList();

        return _mapper.Map<IList<UserTitleRatingDTO>>(userRatings);
    }

    public bool UserTitleRatingExists(string username, string titleId, out UserTitleRatingDTO? userTitleRatingDTO)
    {
        var userTitleRating = _imdbContext.UserTitleRatings
            .FirstOrDefault(ur =>
                ur.Username == username
                &&
                ur.TitleId == titleId);
        _imdbContext.Entry(userTitleRating).State = EntityState.Detached;
        userTitleRatingDTO = _mapper.Map<UserTitleRatingDTO>(userTitleRating);
        return userTitleRating != null;
    }

    public bool DeleteUserTitleRating(UserTitleRatingDTO userTitleRatingDTO)
    {
        var userTitleRating = _mapper.Map<UserTitleRating>(userTitleRatingDTO);
        _imdbContext.UserTitleRatings.Remove(userTitleRating);
        return _imdbContext.SaveChanges() > 0;
    }
}