using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Models;

namespace Application.Services;

public interface INameService
{
    NameDTO GetName(string nameId);
    NameRatingDTO GetRating(string nameId);
}