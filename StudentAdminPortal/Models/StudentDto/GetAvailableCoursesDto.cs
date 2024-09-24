namespace StudentAdminPortal.Models.StudentDto
{
    public class GetAvailableCoursesDto
    {
       public required List<CourseDto> Courses { get; set; }
    }  
     public class CourseDto
    {
    public Guid CourseId { get; set; }
    public required string Subject { get; set; }
    }
}
