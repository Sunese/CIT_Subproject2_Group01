using Domain.Models;

namespace Application.Models;
    
public class NameBookmarkDTO
{
    public string Username { get; set; }
    public string NameId { get; set; }
    public DateTime Timestamp { get; set; }
    public string Notes { get; set; } = string.Empty; // Default to empty string
    public NameDTO Name { get; set; }
}