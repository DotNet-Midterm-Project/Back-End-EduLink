//using EduLink.Data;
//using EduLink.Models;
//using EduLink.Models.DTO.Request;
//using EduLink.Models.DTO.Response;
//using EduLink.Repositories.Interfaces;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc.ModelBinding;

//namespace EduLink.Repositories.Services
//{
//    public class IdentityAccountService : IAccount
//    {
//        private readonly EduLinkDbContext _context;
//        private readonly UserManager<User> _userManager;
//        private readonly SignInManager<User> _signInManager;
//        private readonly JwtTokenService _jwtTokenService;
//        private RoleManager<IdentityRole> _roleManager;


//        public IdentityAccountService(EduLinkDbContext context, UserManager<Student> userManager,
//            SignInManager<Student> signInManager, JwtTokenService jwtTokenService, RoleManager<IdentityRole> roleManager)
//        {
//            _context = context;
//            _userManager = userManager;
//            _signInManager = signInManager;
//            _jwtTokenService = jwtTokenService;
//            _roleManager = roleManager;
//        }

//        public async Task<RegisterStudentResDTO> RegisterStudentAsync(RegisterStudentReqDTO registerStudentDto, ModelStateDictionary modelState)
//        {
//            if (!modelState.IsValid)
//            {
//                return null;
//            }

//            var user = new User
//            {
//                UserName = registerStudentDto.UserName,
//                Email = registerStudentDto.Email
//            };

//            var result = await _userManager.CreateAsync(user, registerStudentDto.Password);

//            if (!result.Succeeded)
//            {
//                foreach (var error in result.Errors)
//                {
//                    modelState.AddModelError("", error.ArticaleContent);
//                }
//                return null;
//            }

//            if (await _roleManager.RoleExistsAsync("Student"))
//                await _userManager.AddToRoleAsync(user, "Student");

//            var student = new Student
//            {
//                StudentID = user.Id,
//                Student = user,
//                DepartmentID = registerStudentDto.DepartmentID
//            };

//            _context.Students.Add(student);
//            await _context.SaveChangesAsync();

//            var roles = await _userManager.GetRolesAsync(user);
//            var token = await _jwtTokenService.GenerateToken(user, TimeSpan.FromMinutes(60));

//            return new RegisterStudentDtoResponse
//            {
//                StudentID = student.StudentID,
//                UserName = user.UserName,
//                Email = user.Email,
//                DepartmentID = student.DepartmentID,
//                Token = token,  // Return the generated token
//                Roles = roles

//            };
//        }

//        public async Task<RegisterAdminResDTO> RegisterAdminAsync(RegisterAdminReqDTO registerAdminDto, ModelStateDictionary modelState)
//        {Student
//            if (!modelState.IsValid)
//            {
//                return null;
//            }

//            var user = new 
//            {
//                UserName = registerAdminDto.UserName,
//                Email = registerAdminDto.Email
//            };

//            var result = await _userManager.CreateAsync(user, registerAdminDto.Password);

//            if (!result.Succeeded)
//            {
//                foreach (var error in result.Errors)
//                {
//                    modelState.AddModelError("", error.ArticaleContent);
//                }
//                return null;
//            }

//            await _userManager.AddToRoleAsync(user, "Admin");

          

          
//            await _context.SaveChangesAsync();

//            var roles = await _userManager.GetRolesAsync(user);
//            var token = await _jwtTokenService.GenerateToken(user, TimeSpan.FromMinutes(60));

//            return new RegisterAdminDtoResponse
//            {
//                AdminID = admin.AdminID,
//                UserName = user.UserName,
//                Email = user.Email,
//                Token = token,  // Return the generated token
//                Roles = roles
//            };
//        }

//        public async Task<bool> AddStudentToVolunteerRoleAsync(string StudentID)
//        {
//            var student = await _context.Students.FindAsync(StudentID);
//            if (student == null)
//            {
//                return false;
//            }

//            var user = await _userManager.FindByIdAsync(student.StudentID);
//            if (user == null)
//            {
//                return false;
//            }

//            await _userManager.AddToRoleAsync(user, "Student");

//            var volunteer = new Student
//            {

//                StudentID = student.StudentID,
//                Student = student

//            };

//            _context.Volunteers.Add(volunteer);
//            await _context.SaveChangesAsync();

//            return true;
//        }

//        public async Task<LoginResDTO> LoginAsync(LoginReqDTO loginDto)
//        {
//            var user = await _userManager.FindByEmailAsync(loginDto.Email);
//            if (user == null)
//            {
//                return null;
//            }

//            var result = await _signInManager.PasswordSignInAsync(user.UserName, loginDto.Password, false, false);
//            if (!result.Succeeded)
//            {
//                return null;
//            }

//            var roles = await _userManager.GetRolesAsync(user);

//            var token = await _jwtTokenService.GenerateToken(user, TimeSpan.FromMinutes(60));

//            return new LoginResDTO
//            {
//                Token = token,
//                UserId = user.Id,
//                UserName = user.UserName,
//                Email = user.Email,
//                Roles = roles
//            };
//        }

//        public async Task LogoutAsync()
//        {
//            await _signInManager.SignOutAsync();
//        }


//    }
//}
