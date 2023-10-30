namespace Application.Models;
    
public class UserDTO
{
	public string UserName { get; set; } = String.Empty;
	public string Password { get; set; } = String.Empty;
	public string Email { get; set; } = String.Empty;
	public string Salt { get; set; } = String.Empty;
	public string Role { get; set; } = "User";
}