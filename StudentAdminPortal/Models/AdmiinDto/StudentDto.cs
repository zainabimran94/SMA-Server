namespace StudentAdminPortal.Models.AdmiinDto
{
    public class StudentDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required List<CoursesDto> Courses { get; set; }
    public required List<ReminderDto> Reminders { get; set; }
    public required List<SemestersDto> Semesters {get; set;}
}

    public class SemestersDto
    {
       public Guid SemesterId { get; set; }
       public required int Semester { get; set; }
    }

    public class CoursesDto
{
    public Guid CourseId { get; set; }
    public required string Subject { get; set; }
}

public class ReminderDto
    {
        public Guid Id { get; set; }
        public required string Message { get; set; }
        public DateTime Date { get; set; }
       
    }
}