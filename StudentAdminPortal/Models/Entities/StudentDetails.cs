namespace StudentAdminPortal.Models.Entities
{
    public class StudentDetails
{
    public Guid StudentId { get; set; }
    public required Student Student { get; set; }

    public Guid CourseId { get; set; }
    public required Course Course { get; set; }
    public Guid SemesterId { get; set; }
    public required Semesters Semester { get; set; }
}

}