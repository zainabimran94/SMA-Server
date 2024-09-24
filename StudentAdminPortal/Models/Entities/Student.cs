namespace StudentAdminPortal.Models.Entities
{
    public class Student
    {
       public Guid Id {get; set;}
       public required string Name {get; set;}
       public required string Email {get; set;}
       public string ? PasswordHash {get; set;}
       // Foreign key to ApplicationUser
        public string ? ApplicationUserId { get; set; }
        public  ApplicationUser ? ApplicationUser { get; set; } 
       

       public ICollection<StudentDetails> StudentDetails{ get; set; } = new List<StudentDetails>();
       public ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();
       public ICollection<StudentSemester>CurrentSemester { get; set; } = new List<StudentSemester>();
    }

    
}