namespace StudentAdminPortal.Models.Entities
{
    public class StudentSemester
{
    public Guid StudentId { get; set; }
    public Student? Student { get; set; }
    
    public Guid SemesterId { get; set; }
    public Semesters? Semester { get; set; }
    
}
}