using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public interface IUserRatingService
{
    (IList<UserTitleRatingDTO> Ratings, int Total) GetUserTitleRatings(string username, int page, int pageSize);
    bool TryGetUserTitleRating(string username, string titleID, out UserTitleRatingDTO? foundTitleRating);
    bool CreateUserTitleRating(UserTitleRatingDTO userTitleRatingDTO);
    bool UserTitleRatingExists(string username, string titleID, out UserTitleRatingDTO? userTitleRating);
    bool DeleteUserTitleRating(UserTitleRatingDTO userTitleRatingDTO);
    bool ReplaceUserTitleRating(UserTitleRatingDTO oldRating, UserTitleRatingDTO newRating);
}