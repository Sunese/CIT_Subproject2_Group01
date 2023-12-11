using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models;

public class SimiliarMoviesResult
{
    public string TitleID { get; set; }
    public string PrimaryTitle { get; set; }
    public string StartYear { get; set; }
    public int? RunTimeMinutes { get; set; }
}