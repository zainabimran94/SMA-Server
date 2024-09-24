namespace StudentAdminPortal.Models.StudentDto
{
    public class CourseSelectionDto
    {
        public required Guid StudentId { get; set; }
        public required List<Guid> CourseId { get; set; } 
        public required Guid SemesterId { get; set; }
    }
}