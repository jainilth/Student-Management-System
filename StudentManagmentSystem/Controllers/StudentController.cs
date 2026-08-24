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
    public class StudentController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IValidator<CreateStudentDto> createValidator;
        private readonly IValidator<UpdateStudentDto> updateValidator;

        public StudentController(
            AppDbContext _context,
            IValidator<CreateStudentDto> _createValidator,
            IValidator<UpdateStudentDto> _updateValidator)
        {
            context = _context;
            createValidator = _createValidator;
            updateValidator = _updateValidator;
        }

        private static StudentResponseDto MapToDto(Student s) => new StudentResponseDto
        {
            StudentId = s.StudentId,
            UserId = s.UserId,
            UserName = s.User != null ? s.User.UserName : string.Empty,
            EnrollmentNumber = s.EnrollmentNumber,
            AdmissionYear = s.AdmissionYear,
            ProgramId = s.ProgramId,
            ProgramName = s.AcademicProgram != null ? s.AcademicProgram.ProgramName : string.Empty,
            CurrentSemesterId = s.CurrentSemesterId,
            SemesterName = s.CurrentSemester != null ? s.CurrentSemester.SemesterName : string.Empty,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt
        };

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var students = await context.Students
                .Include(s => s.User)
                .Include(s => s.AcademicProgram)
                .Include(s => s.CurrentSemester)
                .Select(s => MapToDto(s))
                .ToListAsync();

            return Ok(new CommonApiResponse<List<StudentResponseDto>>
            {
                Success = true,
                StatusCode = 200,
                Message = "Students retrieved successfully",
                Data = students
            });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById([FromRoute] int id)
        {
            var s = await context.Students
                .Include(s => s.User)
                .Include(s => s.AcademicProgram)
                .Include(s => s.CurrentSemester)
                .FirstOrDefaultAsync(s => s.StudentId == id);

            if (s is null)
                return NotFound(new CommonApiResponse<StudentResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Student not found"
                });

            return Ok(new CommonApiResponse<StudentResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Student retrieved successfully",
                Data = MapToDto(s)
            });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateStudentDto dto)
        {
            var validation = await createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new CommonApiResponse<StudentResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });

            var duplicate = await context.Students.FirstOrDefaultAsync(s =>
                s.EnrollmentNumber == dto.EnrollmentNumber || s.UserId == dto.UserId);
            if (duplicate != null)
            {
                string field = duplicate.UserId == dto.UserId ? "User ID" : "Enrollment number";
                return BadRequest(new CommonApiResponse<StudentResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = $"{field} is already in use."
                });
            }

            var firstSemester = await context.Semesters
                .FirstOrDefaultAsync(s => s.SemesterNumber == 1);
            if (firstSemester is null)
                return BadRequest(new CommonApiResponse<StudentResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Semester 1 must exist before creating a student."
                });

            var entity = new Student
            {
                UserId = dto.UserId,
                EnrollmentNumber = dto.EnrollmentNumber,
                AdmissionYear = dto.AdmissionYear,
                ProgramId = dto.ProgramId,
                CurrentSemesterId = firstSemester.SemesterId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            context.Students.Add(entity);
            await context.SaveChangesAsync();

            await context.Entry(entity).Reference(s => s.User).LoadAsync();
            await context.Entry(entity).Reference(s => s.AcademicProgram).LoadAsync();
            await context.Entry(entity).Reference(s => s.CurrentSemester).LoadAsync();

            return StatusCode(201, new CommonApiResponse<StudentResponseDto>
            {
                Success = true,
                StatusCode = 201,
                Message = "Student created successfully",
                Data = MapToDto(entity)
            });
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateStudentDto dto)
        {
            var validation = await updateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new CommonApiResponse<StudentResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });

            var existing = await context.Students.FindAsync(id);
            if (existing is null)
                return NotFound(new CommonApiResponse<StudentResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Student not found"
                });

            var duplicate = await context.Students.FirstOrDefaultAsync(s =>
                s.StudentId != id && (s.EnrollmentNumber == dto.EnrollmentNumber || s.UserId == dto.UserId));
            if (duplicate != null)
            {
                string field = duplicate.UserId == dto.UserId ? "User ID" : "Enrollment number";
                return BadRequest(new CommonApiResponse<StudentResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = $"{field} is already in use."
                });
            }

            existing.UserId = dto.UserId;
            existing.EnrollmentNumber = dto.EnrollmentNumber;
            existing.AdmissionYear = dto.AdmissionYear;
            existing.ProgramId = dto.ProgramId;
            existing.CurrentSemesterId = dto.CurrentSemesterId;
            existing.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();

            await context.Entry(existing).Reference(s => s.User).LoadAsync();
            await context.Entry(existing).Reference(s => s.AcademicProgram).LoadAsync();
            await context.Entry(existing).Reference(s => s.CurrentSemester).LoadAsync();

            return Ok(new CommonApiResponse<StudentResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Student updated successfully",
                Data = MapToDto(existing)
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var entity = await context.Students
                .Include(s => s.User)
                .Include(s => s.AcademicProgram)
                .Include(s => s.CurrentSemester)
                .FirstOrDefaultAsync(s => s.StudentId == id);

            if (entity is null)
                return NotFound(new CommonApiResponse<StudentResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Student not found"
                });

            context.Students.Remove(entity);
            await context.SaveChangesAsync();

            return Ok(new CommonApiResponse<StudentResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Student deleted successfully",
                Data = MapToDto(entity)
            });
        }
    }
}
