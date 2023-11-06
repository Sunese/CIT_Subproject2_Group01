using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Models;

public class PopularActorsResult
{
    public string NameId { get; set; }
    public string PrimaryName { get; set; }
    public int Rating { get; set; }
}