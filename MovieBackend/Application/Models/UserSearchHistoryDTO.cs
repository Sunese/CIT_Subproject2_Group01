using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Models;

public class UserSearchHistoryDTO
{
    public string Username { get; set; } = null!;
    public string Query { get; set; } = null!;
    public DateTime Timestamp { get; set; }
    public string SearchType { get; set; } = null!;
}