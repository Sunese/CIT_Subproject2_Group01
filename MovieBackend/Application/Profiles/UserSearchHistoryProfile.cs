using Application.Models;
using AutoMapper;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Profiles;

public class UserSearchHistoryProfile : Profile
{
    public UserSearchHistoryProfile()
    {
        CreateMap<Search, UserSearchHistoryDTO>();
    }
}