using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAdminPortal.Data;
using StudentAdminPortal.Interfaces;
using StudentAdminPortal.Models;
using StudentAdminPortal.Models.AuthDto;
using StudentAdminPortal.Models.Entities;


namespace StudentAdminPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager ;
        private readonly RoleManager<IdentityRole> _roleManager ;
        private readonly IConfiguration _configuration ;
        private readonly ApplicationDbContext _dbContext ;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ApplicationDbContext dbContext, SignInManager<ApplicationUser> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _dbContext = dbContext;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }
        

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!IsValidEmail(model.Email)) 
            {
                return BadRequest(new {message = "Invalid email format." });
            }
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if(existingUser != null )
            {
                return BadRequest(new { message = "Email already exists." });
            }

              if(model.Role != "Student" && model.Role != "Admin")
            {
                 return BadRequest(new { message = "Invalid role specified." });
            }


            var user = new ApplicationUser
            {
              UserName = model.UserName,
              FullName = model.Name, 
              Email = model.Email,
              IsStudent = model.Role == "Student",
              IsAdmin = model.Role == "Admin"
            };
            
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {   
                if(!await _roleManager.RoleExistsAsync(model.Role))
                {
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole(model.Role));
                     if (!roleResult.Succeeded)
                    {
                        await _userManager.DeleteAsync(user);
                        return StatusCode(500, new { message = "User role creation failed.", errors = roleResult.Errors });
                    }
                }
                  // Assign role to user
                  var roleAssignment = await _userManager.AddToRoleAsync(user, model.Role);
                   if (!roleAssignment.Succeeded)
                   {
                    await _userManager.DeleteAsync(user);
                    return StatusCode(500, new { message = "User role assignment failed.", errors = roleAssignment.Errors });
                   }
                   // Create student or admin entity
                  var role = await _roleManager.FindByNameAsync(model.Role);
                  var roleId = role?.Id;
                  
                 await _userManager.AddToRoleAsync(user, model.Role);

                if (model.Role == "Student")
                {
                    var student = new Student
                    {
                        Id = Guid.NewGuid(),
                        Name = model.Name,
                        Email = model.Email,
                        ApplicationUserId = user.Id,
                        ApplicationUser = user,
                    };
                      _dbContext.Students.Add(student);
                }
                else if (model.Role == "Admin")
                {
                     var admin = new Admin
                     {
                        Id = Guid.NewGuid(),
                        Name = model.Name,
                        Email = model.Email,
                        ApplicationUserId = user.Id,
                        ApplicationUser = user,
                     };
                     _dbContext.Admins.Add(admin);
                }

                 await _dbContext.SaveChangesAsync();

                 var roles = new List<string> { model.Role }; // Include the role assigned to the user
                 var token = _tokenService.CreateToken(user, roles);

                return Ok(new { message = $"{model.Role} registered successfully" });
            }

            var errors = result.Errors.Select(e => e.Description);
            return BadRequest(new {message = "Registration Failed.",errors});
        }

        [HttpPost("login")]
         public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
             return BadRequest(ModelState);
             Console.WriteLine($"Attempting login with email: {model.Email}");
             var normalizedEmail = model.Email.ToUpper();

            var user = await _userManager.Users.Where(u => u.NormalizedEmail == normalizedEmail).FirstOrDefaultAsync();


            if (user == null)  return Unauthorized("Invalid email or password");
            
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded) return Unauthorized("Username not found and/or password incorrect");

            var userRole = await _userManager.GetRolesAsync(user);
            var token = _tokenService.CreateToken(user, [.. userRole]);
            
            return Ok(new {token});
        }


        private static bool IsValidEmail(string email)
        {
            try
            {
                var address = new System.Net.Mail.MailAddress(email);
                return address.Address == email;
            }
            catch
            {
                return false;
            }
        }


       
    }        
}

