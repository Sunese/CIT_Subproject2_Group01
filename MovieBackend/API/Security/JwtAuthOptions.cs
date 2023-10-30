namespace API.Security;

public class JwtAuthOptions
{
    public static string JwtAuth = "JwtAuth";
    public string SecretKey { get; set; } = string.Empty;
}