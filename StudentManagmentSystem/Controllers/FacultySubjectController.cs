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
    public class FacultySubjectController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IValidator<CreateFacultySubjectDto> createValidator;
        private readonly IValidator<UpdateFacultySubjectDto> updateValidator;

        public FacultySubjectController(AppDbContext _context,
            IValidator<CreateFacultySubjectDto> _createValidator,
            IValidator<UpdateFacultySubjectDto> _updateValidator)
        {
            context = _context;
            createValidator = _createValidator;
            updateValidator = _updateValidator;
        }

        private static FacultySubjectResponseDto MapToDto(FacultySubject fs) => new FacultySubjectResponseDto
        {
            FacultySubjectId = fs.FacultySubjectId,
            FacultyId = fs.FacultyId,
            FacultyEmployeeNumber = fs.Faculty != null ? fs.Faculty.EmployeeNumber : string.Empty,
            FacultyName = fs.Faculty?.User?.UserName ?? string.Empty,
            SemesterSubjectId = fs.SemesterSubjectId,
            SubjectName = fs.SemesterSubject?.Subject?.SubjectName ?? string.Empty,
            AcademicYearId = fs.AcademicYearId,
            AcademicYear = fs.AcademicYear != null ? fs.AcademicYear.Year : string.Empty,
            CreatedAt = fs.CreatedAt,
            UpdatedAt = fs.UpdatedAt
        };

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var items = await context.FacultySubjects
                .Include(fs => fs.Faculty).ThenInclude(f => f.User)
                .Include(fs => fs.SemesterSubject).ThenInclude(ss => ss.Subject)
                .Include(fs => fs.AcademicYear)
                .Select(fs => MapToDto(fs)).ToListAsync();

            return Ok(new CommonApiResponse<List<FacultySubjectResponseDto>>
            {
                Success = true,
                StatusCode = 200,
                Message = "Faculty subjects retrieved successfully",
                Data = items
            });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById([FromRoute] int id)
        {
            var fs = await context.FacultySubjects
                .Include(fs => fs.Faculty).ThenInclude(f => f.User)
                .Include(fs => fs.SemesterSubject).ThenInclude(ss => ss.Subject)
                .Include(fs => fs.AcademicYear)
                .FirstOrDefaultAsync(fs => fs.FacultySubjectId == id);

            if (fs is null)
                return NotFound(new CommonApiResponse<FacultySubjectResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Faculty subject not found"
                });

            return Ok(new CommonApiResponse<FacultySubjectResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Faculty subject retrieved successfully",
                Data = MapToDto(fs)
            });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateFacultySubjectDto dto)
        {
            var validation = await createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new CommonApiResponse<FacultySubjectResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });

            var duplicate = await context.FacultySubjects.FirstOrDefaultAsync(fs =>
                fs.FacultyId == dto.FacultyId && fs.SemesterSubjectId == dto.SemesterSubjectId && fs.AcademicYearId == dto.AcademicYearId);
            if (duplicate != null)
                return BadRequest(new CommonApiResponse<FacultySubjectResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "This faculty member is already assigned to this subject for the given academic year."
                });

            var entity = new FacultySubject
            {
                FacultyId = dto.FacultyId,
                SemesterSubjectId = dto.SemesterSubjectId,
                AcademicYearId = dto.AcademicYearId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            context.FacultySubjects.Add(entity);
            await context.SaveChangesAsync();

            await context.Entry(entity).Reference(fs => fs.Faculty).LoadAsync();
            if (entity.Faculty != null)
                await context.Entry(entity.Faculty).Reference(f => f.User).LoadAsync();
            await context.Entry(entity).Reference(fs => fs.SemesterSubject).LoadAsync();
            if (entity.SemesterSubject != null)
                await context.Entry(entity.SemesterSubject).Reference(ss => ss.Subject).LoadAsync();
            await context.Entry(entity).Reference(fs => fs.AcademicYear).LoadAsync();

            return StatusCode(201, new CommonApiResponse<FacultySubjectResponseDto>
            {
                Success = true,
                StatusCode = 201,
                Message = "Faculty subject created successfully",
                Data = MapToDto(entity)
            });
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateFacultySubjectDto dto)
        {
            var validation = await updateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new CommonApiResponse<FacultySubjectResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });

            var existing = await context.FacultySubjects.FindAsync(id);
            if (existing is null)
                return NotFound(new CommonApiResponse<FacultySubjectResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Faculty subject not found"
                });

            var duplicate = await context.FacultySubjects.FirstOrDefaultAsync(fs =>
                fs.FacultySubjectId != id &&
                fs.FacultyId == dto.FacultyId && fs.SemesterSubjectId == dto.SemesterSubjectId && fs.AcademicYearId == dto.AcademicYearId);
            if (duplicate != null)
                return BadRequest(new CommonApiResponse<FacultySubjectResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "This faculty member is already assigned to this subject for the given academic year."
                });

            existing.FacultyId = dto.FacultyId;
            existing.SemesterSubjectId = dto.SemesterSubjectId;
            existing.AcademicYearId = dto.AcademicYearId;
            existing.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();

            await context.Entry(existing).Reference(fs => fs.Faculty).LoadAsync();
            if (existing.Faculty != null)
                await context.Entry(existing.Faculty).Reference(f => f.User).LoadAsync();
            await context.Entry(existing).Reference(fs => fs.SemesterSubject).LoadAsync();
            if (existing.SemesterSubject != null)
                await context.Entry(existing.SemesterSubject).Reference(ss => ss.Subject).LoadAsync();
            await context.Entry(existing).Reference(fs => fs.AcademicYear).LoadAsync();

            return Ok(new CommonApiResponse<FacultySubjectResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Faculty subject updated successfully",
                Data = MapToDto(existing)
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var entity = await context.FacultySubjects
                .Include(fs => fs.Faculty).ThenInclude(f => f.User)
                .Include(fs => fs.SemesterSubject).ThenInclude(ss => ss.Subject)
                .Include(fs => fs.AcademicYear)
                .FirstOrDefaultAsync(fs => fs.FacultySubjectId == id);

            if (entity is null)
                return NotFound(new CommonApiResponse<FacultySubjectResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Faculty subject not found"
                });

            context.FacultySubjects.Remove(entity);
            await context.SaveChangesAsync();

            return Ok(new CommonApiResponse<FacultySubjectResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Faculty subject deleted successfully",
                Data = MapToDto(entity)
            });
        }
    }
}
