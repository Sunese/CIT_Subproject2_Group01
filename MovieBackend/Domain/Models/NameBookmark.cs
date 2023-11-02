using System;
namespace Domain.Models;

public class NameBookmark
{
	public string Username { get; set; }
	public string NameId { get; set; }
	public DateTime Timestamp { get; set; }
	public string Notes { get; set; } = string.Empty; // Default to empty string
	public Name Name { get; set; }
}

