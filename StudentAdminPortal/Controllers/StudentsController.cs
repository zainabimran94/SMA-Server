using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAdminPortal.Data;
using StudentAdminPortal.Models;
using StudentAdminPortal.Models.AdmiinDto;
using StudentAdminPortal.Models.Entities;
using StudentAdminPortal.Models.StudentDto;

namespace StudentAdminPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Student")]
    public class StudentsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(ApplicationDbContext dbContext, UserManager<ApplicationUser>userManager, ILogger<StudentsController> logger)
        {
            _dbContext = dbContext;
            _userManager = userManager;
             _logger = logger;
        }

        [HttpGet("available-courses")]
        public IActionResult GetAvailableCourses()
        {
            try
            {
                var courses = _dbContext.Courses
                    .Select(c => new CourseDto
                    {
                        CourseId = c.Id,
                        Subject = c.Subject
                    }).ToList();

                var availableCoursesDto = new GetAvailableCoursesDto
                {
                    Courses = courses
                };

                return Ok(availableCoursesDto);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving available courses. Please try again later.");
            }
        }

        [HttpPost("selectCourse")]
        public IActionResult SelectCourse(CourseSelectionDto courseSelectionDto)
        {
            var student = _dbContext.Students
                .Include(s => s.StudentDetails)
                .FirstOrDefault(s => s.Id == courseSelectionDto.StudentId);

            if (student == null)
            {
                return NotFound("Student not found.");
            }

            if (student.StudentDetails.Any(sd => sd.SemesterId != courseSelectionDto.SemesterId))
            {
                return BadRequest("The selected semester is not assigned to the student.");
            }

            var courses = _dbContext.Courses.Where(c => courseSelectionDto.CourseId.Contains(c.Id)).ToList();
            if (courses.Count != courseSelectionDto.CourseId.Count)
            {
                return NotFound("One or more courses not found.");
            }

            foreach (var course in courses)
            {
                var semester = _dbContext.Semesters.Find(courseSelectionDto.SemesterId);
                if (semester == null)
                {
                    return NotFound("Semester not found.");
                }

                if (!student.StudentDetails.Any(sd => sd.CourseId == course.Id && sd.SemesterId == courseSelectionDto.SemesterId))
                {
                    student.StudentDetails.Add(new StudentDetails
                    {
                        StudentId = student.Id,
                        Student = student,
                        CourseId = course.Id,
                        Course = course,
                        SemesterId = semester.SemesterId,
                        Semester = semester
                    });
                }
            }

            _dbContext.SaveChanges();
            return Ok("Course selected successfully.");
        }
        
       

        [HttpGet("student-profile")]
        public async Task<IActionResult> GetStudentProfile()
        {
            // Extract the username from the JWT claims
            
            var user = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(user))
            {
                
                return Unauthorized();
                
            }

            // Find the student by username
            var student = await _dbContext.Students
                .Include(s => s.StudentDetails)
                .ThenInclude(sc => sc.Course)
                .Include(s => s.StudentDetails)
                .ThenInclude(sc => sc.Semester)
                .Include(s => s.Reminders) // Assuming navigation properties exist
                .FirstOrDefaultAsync(s => s.ApplicationUser!.Email == user);

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
    }
}

        
        

    