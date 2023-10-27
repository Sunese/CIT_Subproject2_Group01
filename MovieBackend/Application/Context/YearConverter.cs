using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Execution;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Application.Context;

public class YearConverter : ValueConverter<int?, string>
{
    public YearConverter() : base(
               v => v.ToString(),
               v => YearStringToInt(v))
    {

    }

    private static int? YearStringToInt(string? v)
    {
        if (v is null)
        {
            return null;
        }

        if (int.TryParse(v, out var result))
        {
            return result;
        }

        return null;
    }   
}