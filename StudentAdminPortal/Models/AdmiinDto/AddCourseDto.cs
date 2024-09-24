
namespace StudentAdminPortal.Models.AdmiinDto
{
    public class AddCourseDto
    {
       public required string Subject { get; set; }
       public required List<Guid> SemesterIds {get; set;}

    }
}
