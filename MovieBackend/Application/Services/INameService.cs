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
    List<ProfessionDTO> GetPrimaryProfessions(string nameId);
    List<KnownForTitlesDTO> GetKnownForTitles(string nameId);
    List<PrincipalDTO> GetPrincipals(string nameId);
}