namespace Application.Models;
    
public class TitleBookmarkDTO
{
	// public string Username { get; set; }
	public string TitleID { get; set; }
	public DateTime Timestamp { get; set; }
	public string Notes { get; set; } = string.Empty; // Default to empty string
	public TitleDTO Title { get; set; }
}