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
    public class FacultyController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IValidator<CreateFacultyDto> createValidator;
        private readonly IValidator<UpdateFacultyDto> updateValidator;

        public FacultyController(
            AppDbContext _context,
            IValidator<CreateFacultyDto> _createValidator,
            IValidator<UpdateFacultyDto> _updateValidator)
        {
            context = _context;
            createValidator = _createValidator;
            updateValidator = _updateValidator;
        }

        private static FacultyResponseDto MapToDto(Faculty f) => new FacultyResponseDto
        {
            FacultyId = f.FacultyId, UserId = f.UserId,
            UserName = f.User != null ? f.User.UserName : string.Empty,
            EmployeeNumber = f.EmployeeNumber,
            DepartmentId = f.DepartmentId, DepartmentName = f.Department != null ? f.Department.DepartmentName : string.Empty,
            Designation = f.Designation, JoiningDate = f.JoiningDate,
            CreatedAt = f.CreatedAt, UpdatedAt = f.UpdatedAt
        };

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var faculties = await context.Faculties
                .Include(f => f.User)
                .Include(f => f.Department)
                .Select(f => MapToDto(f))
                .ToListAsync();

            return Ok(new CommonApiResponse<List<FacultyResponseDto>>
            {
                Success = true, StatusCode = 200, Message = "Faculty members retrieved successfully", Data = faculties
            });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById([FromRoute] int id)
        {
            var f = await context.Faculties
                .Include(f => f.User)
                .Include(f => f.Department)
                .FirstOrDefaultAsync(f => f.FacultyId == id);

            if (f is null)
                return NotFound(new CommonApiResponse<FacultyResponseDto>
                {
                    Success = false, StatusCode = 404, Message = "Faculty member not found"
                });

            return Ok(new CommonApiResponse<FacultyResponseDto>
            {
                Success = true, StatusCode = 200, Message = "Faculty member retrieved successfully", Data = MapToDto(f)
            });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateFacultyDto dto)
        {
            var validation = await createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new CommonApiResponse<FacultyResponseDto>
                {
                    Success = false, StatusCode = 400, Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });

            var duplicate = await context.Faculties.FirstOrDefaultAsync(f =>
                f.EmployeeNumber == dto.EmployeeNumber || f.UserId == dto.UserId);
            if (duplicate != null)
            {
                string field = duplicate.UserId == dto.UserId ? "User ID" : "Employee number";
                return BadRequest(new CommonApiResponse<FacultyResponseDto>
                {
                    Success = false, StatusCode = 400, Message = $"{field} is already in use."
                });
            }

            var entity = new Faculty
            {
                UserId = dto.UserId, EmployeeNumber = dto.EmployeeNumber,
                DepartmentId = dto.DepartmentId, Designation = dto.Designation,
                JoiningDate = dto.JoiningDate, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
            };
            context.Faculties.Add(entity);
            await context.SaveChangesAsync();

            await context.Entry(entity).Reference(f => f.User).LoadAsync();
            await context.Entry(entity).Reference(f => f.Department).LoadAsync();

            return StatusCode(201, new CommonApiResponse<FacultyResponseDto>
            {
                Success = true, StatusCode = 201, Message = "Faculty member created successfully", Data = MapToDto(entity)
            });
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateFacultyDto dto)
        {
            var validation = await updateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new CommonApiResponse<FacultyResponseDto>
                {
                    Success = false, StatusCode = 400, Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });

            var existing = await context.Faculties.FindAsync(id);
            if (existing is null)
                return NotFound(new CommonApiResponse<FacultyResponseDto>
                {
                    Success = false, StatusCode = 404, Message = "Faculty member not found"
                });

            var duplicate = await context.Faculties.FirstOrDefaultAsync(f =>
                f.FacultyId != id && (f.EmployeeNumber == dto.EmployeeNumber || f.UserId == dto.UserId));
            if (duplicate != null)
            {
                string field = duplicate.UserId == dto.UserId ? "User ID" : "Employee number";
                return BadRequest(new CommonApiResponse<FacultyResponseDto>
                {
                    Success = false, StatusCode = 400, Message = $"{field} is already in use."
                });
            }

            existing.UserId = dto.UserId;
            existing.EmployeeNumber = dto.EmployeeNumber;
            existing.DepartmentId = dto.DepartmentId;
            existing.Designation = dto.Designation;
            existing.JoiningDate = dto.JoiningDate;
            existing.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();

            await context.Entry(existing).Reference(f => f.User).LoadAsync();
            await context.Entry(existing).Reference(f => f.Department).LoadAsync();

            return Ok(new CommonApiResponse<FacultyResponseDto>
            {
                Success = true, StatusCode = 200, Message = "Faculty member updated successfully", Data = MapToDto(existing)
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var entity = await context.Faculties
                .Include(f => f.User)
                .Include(f => f.Department)
                .FirstOrDefaultAsync(f => f.FacultyId == id);

            if (entity is null)
                return NotFound(new CommonApiResponse<FacultyResponseDto>
                {
                    Success = false, StatusCode = 404, Message = "Faculty member not found"
                });

            context.Faculties.Remove(entity);
            await context.SaveChangesAsync();

            return Ok(new CommonApiResponse<FacultyResponseDto>
            {
                Success = true, StatusCode = 200, Message = "Faculty member deleted successfully", Data = MapToDto(entity)
            });
        }
    }
}
