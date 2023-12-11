using System;
using System.ComponentModel.DataAnnotations;
namespace Domain.Models;

public class TitleBookmark
{
	public string Username { get; set; }
	public string TitleID { get; set; }
	public DateTime Timestamp { get; set; }
	public string Notes { get; set; } = string.Empty; // Default to empty string
	public Title Title { get; set; }
}