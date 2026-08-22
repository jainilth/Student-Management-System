using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagmentSystem.Data;
using StudentManagmentSystem.Dto;
using StudentManagmentSystem.Models;

namespace StudentManagmentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IValidator<CreateUserDto> createUserValidator;
        private readonly IValidator<UpdateUserDto> updateUserValidator;

        public UserController(
            AppDbContext _context,
            IValidator<CreateUserDto> _createUserValidator,
            IValidator<UpdateUserDto> _updateUserValidator)
        {
            context = _context;
            createUserValidator = _createUserValidator;
            updateUserValidator = _updateUserValidator;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllUsers()
        {
            var users = await context.Users.Include(u => u.Role).Select(u => new UserResponseDto
            {
                UserId = u.UserId,
                UserName = u.UserName,
                Email = u.Email,
                Address = u.Address,
                Dob = u.Dob,
                Mobilenumber = u.Mobilenumber,
                ProfilePhoto = u.ProfilePhoto,
                IsActivate = u.IsActivate,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt,
                RoleId = u.RoleId,
                RoleName = u.Role != null ? u.Role.RoleName : string.Empty,
            }).ToListAsync();

            return Ok(new CommonApiResponse<List<UserResponseDto>>
            {
                Success = true,
                StatusCode = 200,
                Message = "Users retrieved successfully",
                Data = users
            });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById([FromRoute] int id)
        {
            var user = await context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == id);

            if (user is null)
            {
                return NotFound(new CommonApiResponse<UserResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "User not found"
                });
            }

            return Ok(new CommonApiResponse<UserResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "User retrieved successfully",
                Data = new UserResponseDto
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    Email = user.Email,
                    Address = user.Address,
                    Dob = user.Dob,
                    Mobilenumber = user.Mobilenumber,
                    ProfilePhoto = user.ProfilePhoto,
                    IsActivate = user.IsActivate,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt,
                    RoleId = user.RoleId,
                    RoleName = user.Role != null ? user.Role.RoleName : string.Empty
                }
            });
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser([FromBody] CreateUserDto userDto)
        {
            // FluentValidation
            var validationResult = await createUserValidator.ValidateAsync(userDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new CommonApiResponse<UserResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Validation failed",
                    Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList()
                });
            }

            // Duplicate check
            var duplicateUser = await context.Users
                .FirstOrDefaultAsync(u => u.Mobilenumber == userDto.Mobilenumber
                                       || u.Email == userDto.Email
                                       || u.UserName == userDto.UserName);

            if (duplicateUser != null)
            {
                string field = duplicateUser.Mobilenumber == userDto.Mobilenumber ? "Mobile number" :
                               duplicateUser.Email == userDto.Email ? "Email" : "Username";

                return BadRequest(new CommonApiResponse<UserResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = $"{field} is already in use."
                });
            }

            var user = new User
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                PasswordHash = userDto.Password,
                Address = userDto.Address,
                Dob = userDto.Dob,
                Mobilenumber = userDto.Mobilenumber,
                ProfilePhoto = userDto.ProfilePhoto,
                RoleId = userDto.RoleId,
                IsActivate = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();
            await context.Entry(user).Reference(u => u.Role).LoadAsync();

            return StatusCode(201, new CommonApiResponse<UserResponseDto>
            {
                Success = true,
                StatusCode = 201,
                Message = "User created successfully",
                Data = new UserResponseDto
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    Email = user.Email,
                    Address = user.Address,
                    Dob = user.Dob,
                    Mobilenumber = user.Mobilenumber,
                    ProfilePhoto = user.ProfilePhoto,
                    IsActivate = user.IsActivate,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt,
                    RoleId = user.RoleId,
                    RoleName = user.Role != null ? user.Role.RoleName : string.Empty
                }
            });
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateUserDto userDto)
        {
            // FluentValidation
            var validationResult = await updateUserValidator.ValidateAsync(userDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new CommonApiResponse<UserResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Validation failed",
                    Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList()
                });
            }

            var existing = await context.Users.FindAsync(id);
            if (existing is null)
            {
                return NotFound(new CommonApiResponse<UserResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "User not found"
                });
            }

            // Duplicate check (exclude self)
            var duplicateUser = await context.Users
                .FirstOrDefaultAsync(u => u.UserId != id
                                       && (u.Mobilenumber == userDto.Mobilenumber
                                           || u.Email == userDto.Email
                                           || u.UserName == userDto.UserName));

            if (duplicateUser != null)
            {
                string field = duplicateUser.Mobilenumber == userDto.Mobilenumber ? "Mobile number" :
                               duplicateUser.Email == userDto.Email ? "Email" : "Username";

                return BadRequest(new CommonApiResponse<UserResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = $"{field} is already in use."
                });
            }

            existing.UserName = userDto.UserName;
            existing.Email = userDto.Email;

            if (!string.IsNullOrEmpty(userDto.Password))
            {
                existing.PasswordHash = userDto.Password;
            }

            existing.Address = userDto.Address;
            existing.Dob = userDto.Dob;
            existing.Mobilenumber = userDto.Mobilenumber;
            existing.ProfilePhoto = userDto.ProfilePhoto;
            existing.IsActivate = userDto.IsActivate;
            existing.RoleId = userDto.RoleId;
            existing.UpdatedAt = DateTime.UtcNow;

            await context.SaveChangesAsync();
            await context.Entry(existing).Reference(u => u.Role).LoadAsync();

            return Ok(new CommonApiResponse<UserResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "User updated successfully",
                Data = new UserResponseDto
                {
                    UserId = existing.UserId,
                    UserName = existing.UserName,
                    Email = existing.Email,
                    Address = existing.Address,
                    Dob = existing.Dob,
                    Mobilenumber = existing.Mobilenumber,
                    ProfilePhoto = existing.ProfilePhoto,
                    IsActivate = existing.IsActivate,
                    CreatedAt = existing.CreatedAt,
                    UpdatedAt = existing.UpdatedAt,
                    RoleId = existing.RoleId,
                    RoleName = existing.Role != null ? existing.Role.RoleName : string.Empty
                }
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var user = await context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == id);
            if (user is null)
            {
                return NotFound(new CommonApiResponse<UserResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "User not found"
                });
            }

            context.Users.Remove(user);
            await context.SaveChangesAsync();

            return Ok(new CommonApiResponse<UserResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "User deleted successfully",
                Data = new UserResponseDto
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    Email = user.Email,
                    Address = user.Address,
                    Dob = user.Dob,
                    Mobilenumber = user.Mobilenumber,
                    ProfilePhoto = user.ProfilePhoto,
                    IsActivate = user.IsActivate,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt,
                    RoleId = user.RoleId,
                    RoleName = user.Role != null ? user.Role.RoleName : string.Empty
                }
            });
        }
    }
}
