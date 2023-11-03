using System;
namespace Domain.Models;

public class Search
{
	public string Username { get; set; }
	public string Query { get; set; }
	public DateTime Timestamp { get; set; }
}

