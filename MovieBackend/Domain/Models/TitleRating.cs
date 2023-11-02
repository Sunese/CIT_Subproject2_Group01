using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models;

public class TitleRating
{
    public string TitleID { get; set; }
    public double AverageRating { get; set; }
    public int NumVotes { get; set; }
}