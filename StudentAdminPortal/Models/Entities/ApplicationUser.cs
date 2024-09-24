using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using StudentAdminPortal.Models.Entities;

namespace StudentAdminPortal.Models
{
    public partial class ApplicationUser : IdentityUser
    {
        // Add any additional properties here
        public required string FullName { get; set; }
         public bool IsAdmin { get; set; }
        public bool IsStudent { get; set; }

        // Navigation properties
        public Student? Student { get; set; }
        
        public Admin? Admin {get; set;}
       
    }
    
}
