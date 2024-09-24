using Microsoft.EntityFrameworkCore;
using StudentAdminPortal.Models;
using StudentAdminPortal.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace StudentAdminPortal.Data
{
   public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
         // Constructor that passes the options to the base class
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentDetails> StudentDetails{ get; set; }
        public DbSet<Semesters> Semesters { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
        public DbSet<StudentSemester> StudentSemester { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Composite key for StudentCourse
            modelBuilder.Entity<StudentDetails>()
                .HasKey(sd => new { sd.StudentId, sd.CourseId });
             
            modelBuilder.Entity<Semesters>()
                .HasKey(s => s.SemesterId);  

            modelBuilder.Entity<StudentSemester>()
                .HasKey(ss => new { ss.StudentId, ss.SemesterId });
            

            // Define relationships

            // Student and StudentDetails
            modelBuilder.Entity<StudentDetails>()
                .HasOne(sd => sd.Student)
                .WithMany(s => s.StudentDetails)
                .HasForeignKey(sd => sd.StudentId);

            // Course and StudentDetails
            modelBuilder.Entity<StudentDetails>()
                .HasOne(sd => sd.Course)
                .WithMany(c => c.StudentDetails)
                .HasForeignKey(sd => sd.CourseId);
            
            // Semester and StudentDetails
             modelBuilder.Entity<StudentDetails>()
                .HasOne(sd => sd.Semester)
                .WithMany(c => c.StudentDetails)
                .HasForeignKey(sd => sd.SemesterId);

            
            // Student and StudentSemester
            modelBuilder.Entity<StudentSemester>()
                .HasOne(ss => ss.Student)
                .WithMany(s => s.CurrentSemester)
                .HasForeignKey(ss => ss.StudentId);
            
            modelBuilder.Entity<StudentSemester>()
              .HasOne(ss => ss.Semester)
              .WithMany(se => se.StudentSemester)
              .HasForeignKey(ss => ss.SemesterId);

            // Student and Reminder
            modelBuilder.Entity<Reminder>()
                .HasOne(r => r.Student)
                .WithMany(s => s.Reminders)
                .HasForeignKey(r => r.StudentId);

            modelBuilder.Entity<Student>()
                 .HasMany(s => s.Reminders)
                 .WithOne(r => r.Student)
                 .HasForeignKey(r => r.StudentId);

            // ApplicationUser and Admin/Student
            
            modelBuilder.Entity<ApplicationUser>()
               .HasOne(u => u.Student)
               .WithOne(s => s.ApplicationUser)
               .HasForeignKey<Student>(s => s.ApplicationUserId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationUser>()
               .HasOne(u => u.Admin)
               .WithOne(a => a.ApplicationUser)
               .HasForeignKey<Admin>(a => a.ApplicationUserId)
               .OnDelete(DeleteBehavior.Cascade);

             var roles = new List<IdentityRole>
            {
                new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Name = "Student", NormalizedName = "STUDENT" }
            };

            modelBuilder.Entity<IdentityRole>().HasData(roles);   

          


        }
    }
}

