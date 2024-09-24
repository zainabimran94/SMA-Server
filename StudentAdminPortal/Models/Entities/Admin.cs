namespace StudentAdminPortal.Models.Entities
{
    public class Admin
{
    public Guid Id { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }
    public string ? PasswordHash { get; set; } // For authentication
    // Foreign key to ApplicationUser
     public string ?ApplicationUserId { get; set; }
     public  ApplicationUser ? ApplicationUser { get; set; }
    

       
}
}