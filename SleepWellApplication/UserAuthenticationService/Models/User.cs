namespace UserAuthenticationService.Models
{
    public class User
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public string? Role { get; set; }
    }

    public record UserRequest(string UserName, string Password);
    public record UserResponse(string UserName, string Token, int ExpiresIn);
}
