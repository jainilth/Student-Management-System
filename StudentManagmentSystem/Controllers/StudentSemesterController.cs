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
    public class StudentSemesterController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IValidator<CreateStudentSemesterDto> createValidator;
        private readonly IValidator<UpdateStudentSemesterDto> updateValidator;

        public StudentSemesterController(AppDbContext _context,
            IValidator<CreateStudentSemesterDto> _createValidator,
            IValidator<UpdateStudentSemesterDto> _updateValidator)
        {
            context = _context;
            createValidator = _createValidator;
            updateValidator = _updateValidator;
        }

        private static StudentSemesterResponseDto MapToDto(StudentSemester ss) => new StudentSemesterResponseDto
        {
            StudentSemesterId = ss.StudentSemesterId,
            StudentId = ss.StudentId,
            StudentEnrollmentNumber = ss.Student?.EnrollmentNumber ?? string.Empty,
            StudentName = ss.Student?.User?.UserName ?? string.Empty,
            SemesterId = ss.SemesterId,
            SemesterName = ss.Semester?.SemesterName ?? string.Empty,
            AcademicYearId = ss.AcademicYearId,
            AcademicYear = ss.AcademicYear != null ? ss.AcademicYear.Year : string.Empty,
            EnrollmentDate = ss.EnrollmentDate,
            Status = ss.Status,
            CreatedAt = ss.CreatedAt,
            UpdatedAt = ss.UpdatedAt
        };

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var items = await context.StudentSemesters
                .Include(ss => ss.Student).ThenInclude(s => s.User)
                .Include(ss => ss.Semester)
                .Include(ss => ss.AcademicYear)
                .Select(ss => MapToDto(ss)).ToListAsync();

            return Ok(new CommonApiResponse<List<StudentSemesterResponseDto>>
            {
                Success = true,
                StatusCode = 200,
                Message = "Student semesters retrieved successfully",
                Data = items
            });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById([FromRoute] int id)
        {
            var ss = await context.StudentSemesters
                .Include(ss => ss.Student).ThenInclude(s => s.User)
                .Include(ss => ss.Semester)
                .Include(ss => ss.AcademicYear)
                .FirstOrDefaultAsync(ss => ss.StudentSemesterId == id);

            if (ss is null)
                return NotFound(new CommonApiResponse<StudentSemesterResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Student semester not found"
                });

            return Ok(new CommonApiResponse<StudentSemesterResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Student semester retrieved successfully",
                Data = MapToDto(ss)
            });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateStudentSemesterDto dto)
        {
            var validation = await createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new CommonApiResponse<StudentSemesterResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });

            var duplicate = await context.StudentSemesters.FirstOrDefaultAsync(ss =>
                ss.StudentId == dto.StudentId && ss.SemesterId == dto.SemesterId && ss.AcademicYearId == dto.AcademicYearId);
            if (duplicate != null)
                return BadRequest(new CommonApiResponse<StudentSemesterResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Student is already enrolled in this semester for the given academic year."
                });

            var entity = new StudentSemester
            {
                StudentId = dto.StudentId,
                SemesterId = dto.SemesterId,
                AcademicYearId = dto.AcademicYearId,
                EnrollmentDate = dto.EnrollmentDate,
                Status = dto.Status,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            context.StudentSemesters.Add(entity);
            await context.SaveChangesAsync();

            await context.Entry(entity).Reference(ss => ss.Student).LoadAsync();
            if (entity.Student != null)
                await context.Entry(entity.Student).Reference(s => s.User).LoadAsync();
            await context.Entry(entity).Reference(ss => ss.Semester).LoadAsync();
            await context.Entry(entity).Reference(ss => ss.AcademicYear).LoadAsync();

            return StatusCode(201, new CommonApiResponse<StudentSemesterResponseDto>
            {
                Success = true,
                StatusCode = 201,
                Message = "Student semester created successfully",
                Data = MapToDto(entity)
            });
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateStudentSemesterDto dto)
        {
            var validation = await updateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new CommonApiResponse<StudentSemesterResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });

            var existing = await context.StudentSemesters.FindAsync(id);
            if (existing is null)
                return NotFound(new CommonApiResponse<StudentSemesterResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Student semester not found"
                });

            var duplicate = await context.StudentSemesters.FirstOrDefaultAsync(ss =>
                ss.StudentSemesterId != id &&
                ss.StudentId == dto.StudentId && ss.SemesterId == dto.SemesterId && ss.AcademicYearId == dto.AcademicYearId);
            if (duplicate != null)
                return BadRequest(new CommonApiResponse<StudentSemesterResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Student is already enrolled in this semester for the given academic year."
                });

            existing.StudentId = dto.StudentId;
            existing.SemesterId = dto.SemesterId;
            existing.AcademicYearId = dto.AcademicYearId;
            existing.EnrollmentDate = dto.EnrollmentDate;
            existing.Status = dto.Status;
            existing.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();

            await context.Entry(existing).Reference(ss => ss.Student).LoadAsync();
            if (existing.Student != null)
                await context.Entry(existing.Student).Reference(s => s.User).LoadAsync();
            await context.Entry(existing).Reference(ss => ss.Semester).LoadAsync();
            await context.Entry(existing).Reference(ss => ss.AcademicYear).LoadAsync();

            return Ok(new CommonApiResponse<StudentSemesterResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Student semester updated successfully",
                Data = MapToDto(existing)
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var entity = await context.StudentSemesters
                .Include(ss => ss.Student).ThenInclude(s => s.User)
                .Include(ss => ss.Semester)
                .Include(ss => ss.AcademicYear)
                .FirstOrDefaultAsync(ss => ss.StudentSemesterId == id);

            if (entity is null)
                return NotFound(new CommonApiResponse<StudentSemesterResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Student semester not found"
                });

            context.StudentSemesters.Remove(entity);
            await context.SaveChangesAsync();

            return Ok(new CommonApiResponse<StudentSemesterResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Student semester deleted successfully",
                Data = MapToDto(entity)
            });
        }
    }
}
