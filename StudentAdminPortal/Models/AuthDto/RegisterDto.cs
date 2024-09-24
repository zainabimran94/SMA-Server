namespace StudentAdminPortal.Models.AuthDto
{
    public class RegisterDto
 {
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Name { get; set; } 
    public required string Role { get; set; }
    public required string UserName { get; set; }

 }
}