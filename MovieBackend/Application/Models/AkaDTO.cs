using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Type = Domain.Models.Type;

namespace Application.Models;

public class AkaDTO
{
    public string TitleId { get; set; }
    public int Ordering { get; set; }
    public Title Title { get; set; }
    public string TitleName { get; set; }
    public string Region { get; set; }
    public string Attribtues { get; set; }
    public string Language { get; set; }
    public bool IsOriginalTitle { get; set; }
    public List<Type> Types { get; set; }
}