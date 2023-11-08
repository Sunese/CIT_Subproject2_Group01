using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models;

public class CreateNameBookmarkModel
{
    public string NameId { get; set; }
    public string Notes { get; set; }
}