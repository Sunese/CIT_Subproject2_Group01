using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Models;

public class TitleSearchResultDTO
{
    public string TitleID { get; set; }
    public string PrimaryTitle { get; set; }
    public TitleDTO Title { get; set; }
}