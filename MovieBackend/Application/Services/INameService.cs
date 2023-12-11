using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Models;

namespace Application.Services;

public interface INameService
{
    bool NameExists(string nameID, out NameDTO foundName);
    (IList<NameDTO>, int) GetNames(int page, int pageSize);
    NameDTO GetName(string nameID);
    NameRatingDTO GetRating(string nameID);
    IList<ProfessionDTO> GetPrimaryProfessions(string nameID);
    (IList<KnownForTitlesDTO>, int) GetKnownForTitles(string nameID, int page, int pageSize);
    (IList<PrincipalDTO>, int) GetPrincipals(string nameID, int page, int pageSize);
}