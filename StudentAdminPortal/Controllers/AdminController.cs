using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAdminPortal.Data;
using StudentAdminPortal.Models.AdmiinDto;
using StudentAdminPortal.Models.Entities;


namespace StudentAdminPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]

    public class AdminController(ApplicationDbContext dbContext) : ControllerBase

    {
        private readonly ApplicationDbContext dbContext = dbContext; 
        
    
        //Admin
        [HttpGet]
       public IActionResult GetAllStudents()
       {
        try
        {
           var allStudents = dbContext.Students
           .Include(s => s.StudentDetails)
           .ThenInclude(sc => sc.Course)
           .Include(s => s.StudentDetails)
           .ThenInclude(sc => sc.Semester)
           .Include(s => s.Reminders)
           .Select(s => new StudentDto
           {
             Id = s.Id,
             Name = s.Name,
             Email = s.Email,
             Courses = s.StudentDetails.Select(sc => new CoursesDto
             {
                CourseId = sc.CourseId,
                Subject = sc.Course.Subject
             }).ToList(),
             Reminders = s.Reminders.Select(r => new ReminderDto
             {
                  Id = r.Id,
                  Message = r.Message,
                  Date = r.SentDate
             }).ToList(),
             Semesters = s.StudentDetails.Select(sc => new SemestersDto
                {
                    SemesterId = sc.Semester.SemesterId,
                    Semester = sc.Semester.Semester
                }).Distinct().ToList()
           }).ToList();

            return Ok(allStudents);
        }
        catch (Exception)
        {
             return StatusCode(500, "An error occurred while retrieving students. Please try again later.");
        }
          
       }

       
       [HttpGet("{id}")]
        public IActionResult GetStudentById(Guid id)
        {
          var student = dbContext.Students
          .Include(s => s.StudentDetails)
          .ThenInclude(sc => sc.Course)
          .Include(s => s.StudentDetails)
          .ThenInclude(sc => sc.Semester)
          .Include(s => s.Reminders)
          .FirstOrDefault(s => s.Id == id);

          if (student == null)
           {
              return NotFound("Student not found.");
           }
           var studentDto = new StudentDto
           {
            Id = student.Id,
            Name = student.Name,
            Email = student.Email,
            Courses = student.StudentDetails.Select(sc => new CoursesDto
            {
                CourseId = sc.Course.Id,
                Subject = sc.Course.Subject
            }).ToList(),
            Reminders = student.Reminders.Select(r => new ReminderDto
            {
              Id = r.Id,
              Message = r.Message,
              Date = r.SentDate
            }).ToList(),
            Semesters = student.StudentDetails.Select(sc => new SemestersDto
            {
            SemesterId = sc.Semester.SemesterId,
            Semester = sc.Semester.Semester
            }).Distinct().ToList()
            };

             return Ok(studentDto);
        }
        
        [HttpPost("addCourse")]
        public IActionResult AddCourse(AddCourseDto addCourseDto)
        {
            try
            {
                var semesters = dbContext.Semesters
               .Where(s => addCourseDto.SemesterIds.Contains(s.SemesterId))
               .ToList();

               var courseEntity = new Course
               {
                Subject = addCourseDto.Subject,
                Semesters = semesters
               };

               dbContext.Courses.Add(courseEntity);
               dbContext.SaveChanges();

               var courseDto = new AddCourseDto
              {
                Subject = courseEntity.Subject,
                SemesterIds = courseEntity.Semesters.Select(s => s.SemesterId).ToList()
              };

               return Ok(courseDto);

               
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while adding the course. Please try again later.");
            }
        }

        [HttpPost("addSemester")]
             public IActionResult AddSemester([FromBody] AddSemesterDto addSemesterDto)
        {
              try
            {
                   var semesterEntity = new Semesters
                {
                     SemesterId = Guid.NewGuid(),
                     Semester = addSemesterDto.Semester
                };

                    dbContext.Semesters.Add(semesterEntity);
                    dbContext.SaveChanges();

                  return Ok(new { Message = "Semester added successfully", SemesterId = semesterEntity.SemesterId });
            }
             catch (Exception)
            {
                return StatusCode(500, "An error occurred while adding the semester. Please try again later.");
            }
        }

        [HttpPost("sendReminder")]
        public IActionResult SendReminder(SendReminderDto sendReminderDto)
        {
            var student = dbContext.Students
                .FirstOrDefault(s => s.Id == sendReminderDto.StudentId);

                if (student == null)
                {
                    return NotFound("Student not dound.");
                }
             

             var reminderEntity = new Reminder  
             {
                 Id = Guid.NewGuid(), // Generate a new Guid
                 StudentId = student.Id,
                 Student = student,
                 Message = sendReminderDto.Message,
                 SentDate = DateTime.UtcNow
             };

        
             dbContext.Reminders.Add(reminderEntity);   
             dbContext.SaveChanges();   
 
             
             var reminderMessage = $"Reminder sent to {student.Name} ({student.Email}): {sendReminderDto.Message}";   
             Console.WriteLine(reminderMessage);

            // Return success response
            return Ok(reminderMessage);
        }

        [HttpGet("getCourses")]
             public IActionResult GetAllCourses()
         {
                 try
            {
                  var courses = dbContext.Courses
                 .Select(c => new CoursesDto
              {
                CourseId = c.Id,
                Subject = c.Subject
              })
               .ToList();
 
             return Ok(courses);
             }
             catch (Exception)
           {
             return StatusCode(500, "An error occurred while retrieving courses. Please try again later.");
           }
        }

        [HttpGet("getSemesters")]
            public IActionResult GetAllSemesters()
       {
              try
            {
                    var semesters = dbContext.Semesters
                   .Select(s => new SemestersDto
                 {
                        SemesterId = s.SemesterId,
                         Semester = s.Semester
                 })
                    .ToList();

                  return Ok(semesters);
            }
              catch (Exception)
            {
              return StatusCode(500, "An error occurred while retrieving semesters. Please try again later.");
            }
        }


        [HttpPost("assign-semester")]
        public async Task<IActionResult>AssignSemester([FromBody] AssignSemesterDto assignSemesterDto)
        {
            try
            {
                var student = await dbContext.Students.FindAsync(assignSemesterDto.StudentId);
                var semester = await dbContext.Semesters.FindAsync(assignSemesterDto.SemesterId);

                if (student == null || semester == null)
                {
                    return NotFound("Student or Semester not found.");
                }

                var studentSemester = new StudentSemester
                {
                    StudentId = assignSemesterDto.StudentId,
                    SemesterId = assignSemesterDto.SemesterId
                };
                dbContext.StudentSemester.Add(studentSemester);
                await dbContext.SaveChangesAsync();

                return Ok("Semester assigned successfully.");

            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while assigning the semester. Please try again later.");
            }
        }

    }
}