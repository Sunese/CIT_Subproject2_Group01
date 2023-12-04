using Application.Models;
using AutoMapper;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Profiles;

public class UserTitleRatingProfile : Profile
{
    public UserTitleRatingProfile()
    {
        CreateMap<UserTitleRating, UserTitleRatingDTO>();
        CreateMap<UserTitleRatingDTO, UserTitleRating>();
    }
}
