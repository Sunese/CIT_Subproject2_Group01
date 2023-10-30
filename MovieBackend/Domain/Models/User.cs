using System;
namespace Domain.Models;

public class User
{
	public string UserName { get; set; } = String.Empty;
	public string Password { get; set; } = String.Empty;
	public string Email { get; set; } = String.Empty;
	public string Salt { get; set; } = String.Empty;
	public string Role { get; set; } = "User";
}