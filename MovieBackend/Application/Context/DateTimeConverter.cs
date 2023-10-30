using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Application.Context;

internal class DateTimeConverter : ValueConverter<DateTime?, string?>
{
    public DateTimeConverter() : base(
         v => v.ToString(),
         v => StringToDateTimeConverter(v))
    {

    }

    private static DateTime? StringToDateTimeConverter(string? v)
    {
        if ( v == null || v == "N/A") 
        {
        return null;
        }
        // format returned is: YYYY-MM-DDTHH:MM:SS
        return DateTime.Parse(v).Date;
    }
}
