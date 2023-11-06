using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql.Replication.PgOutput.Messages;
using AutoMapper;
using Domain.Models;
using Application.Models;

namespace Application.Profiles;

public class CoplayerProfile : Profile
{
    public CoplayerProfile()
    {
        CreateMap<CoPlayers, CoPlayersDTO>();
    }
}