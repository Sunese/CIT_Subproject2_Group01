using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Context;

public class ImdbContextOptions
{
    public static string ImdbContext = "ImdbContext";
    public string ConnectionString { get; set; }
}