namespace StudentAdminPortal.Models.Entities
{
    public class Course
{
    public Guid Id { get; set; }
    public required string Subject { get; set; }

    public  ICollection<StudentDetails> StudentDetails { get; set; } = new List<StudentDetails>();
    public  ICollection<Semesters> Semesters { get; set; } = new List<Semesters>();
       
}
}