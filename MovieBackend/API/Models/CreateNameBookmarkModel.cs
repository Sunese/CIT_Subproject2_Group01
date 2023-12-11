using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models;

public class CreateNameBookmarkModel
{
    public string NameID { get; set; }
    public string Notes { get; set; }
}