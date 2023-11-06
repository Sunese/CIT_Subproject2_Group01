using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models;

public class AkaDTO
{
    public string TitleId { get; set; }
    public int Ordering { get; set; }
    public string Title { get; set; }
    public string Region { get; set; }
    public string Attribtues { get; set; }
    public string Language { get; set; }
    public bool IsOridinalTitle { get; set; }
}