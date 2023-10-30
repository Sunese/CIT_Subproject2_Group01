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
    IList<TitleDTO> Get(DateTime startdatetime, DateTime enddatetime, int num, bool isAdult = false);
    TitleDTO GetTitle(string id, bool isAdult = false);
    IList<TitleDTO> GetFeature(int year, int month, int num, bool isAdult = false);
    IList<TitleDTO> GetPopular(DateTime datetime, int num, bool isAdult = false);
}