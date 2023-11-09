using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Models;

namespace Application.Services;

public interface INameService
{
    (IList<NameDTO>, int) GetNames(int page, int pageSize);
    NameDTO GetName(string nameId);
    NameRatingDTO GetRating(string nameId);
    IList<ProfessionDTO> GetPrimaryProfessions(string nameId);
    (IList<KnownForTitlesDTO>, int) GetKnownForTitles(string nameId, int page, int pageSize);
    (IList<PrincipalDTO>, int) GetPrincipals(string nameId, int page, int pageSize);
}