using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using Domain.Models;

namespace Application.Services;

public interface IImdbService
{
    IList<TitleDTO> GetTitles(int num);
    TitleDTO GetTitle(string id);
}