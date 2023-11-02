using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public interface IUserRatingService
{
    IList<UserTitleRatingDTO> GetUserTitleRatings(string username, int num);
}