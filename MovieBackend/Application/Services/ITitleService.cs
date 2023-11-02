using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using Domain.Models;

namespace Application.Services;

public interface ITitleService
{
    IList<TitleDTO> Get(DateOnly startdatetime, DateOnly enddatetime, int count, bool isAdult = false);
    TitleDTO GetTitle(string id, bool isAdult = false);
    IList<TitleDTO> GetFeature(int year, int month, int count, bool isAdult = false);
    IList<TitleDTO> GetPopular(DateOnly datetime, int count, bool isAdult = false);
    TitleRatingDTO? GetRating(string id);
    IList<TitleDTO> GetRatings(bool orderByHighestRating, int count, int? days);
}