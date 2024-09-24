namespace StudentAdminPortal.Models.Entities
{
   public class Semesters
{
    public Guid SemesterId { get; set; }
    public  int Semester { get; set; }

    public ICollection<StudentSemester> StudentSemester { get; set; } = new List<StudentSemester>();
    public  ICollection<StudentDetails> StudentDetails { get; set; } = new List<StudentDetails>();  
}
}