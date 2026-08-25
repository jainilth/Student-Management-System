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
    public class ClassSessionController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IValidator<CreateClassSessionDto> createValidator;
        private readonly IValidator<UpdateClassSessionDto> updateValidator;

        public ClassSessionController(AppDbContext _context,
            IValidator<CreateClassSessionDto> _createValidator,
            IValidator<UpdateClassSessionDto> _updateValidator)
        {
            context = _context;
            createValidator = _createValidator;
            updateValidator = _updateValidator;
        }

        private static ClassSessionResponseDto MapToDto(ClassSession cs) => new ClassSessionResponseDto
        {
            SessionId = cs.SessionId,
            SemesterSubjectId = cs.SemesterSubjectId,
            SubjectName = cs.SemesterSubject?.Subject?.SubjectName ?? string.Empty,
            FacultyId = cs.FacultyId,
            FacultyName = cs.Faculty?.User?.UserName ?? string.Empty,
            SessionDate = cs.SessionDate,
            StartTime = cs.StartTime,
            EndTime = cs.EndTime,
            Topic = cs.Topic,
            CreatedAt = cs.CreatedAt,
            UpdatedAt = cs.UpdatedAt
        };

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var items = await context.Set<ClassSession>()
                .Include(cs => cs.SemesterSubject).ThenInclude(ss => ss.Subject)
                .Include(cs => cs.Faculty).ThenInclude(f => f.User)
                .Select(cs => MapToDto(cs)).ToListAsync();

            return Ok(new CommonApiResponse<List<ClassSessionResponseDto>>
            {
                Success = true,
                StatusCode = 200,
                Message = "Class sessions retrieved successfully",
                Data = items
            });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById([FromRoute] int id)
        {
            var cs = await context.Set<ClassSession>()
                .Include(cs => cs.SemesterSubject).ThenInclude(ss => ss.Subject)
                .Include(cs => cs.Faculty).ThenInclude(f => f.User)
                .FirstOrDefaultAsync(cs => cs.SessionId == id);

            if (cs is null)
                return NotFound(new CommonApiResponse<ClassSessionResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Class session not found"
                });

            return Ok(new CommonApiResponse<ClassSessionResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Class session retrieved successfully",
                Data = MapToDto(cs)
            });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateClassSessionDto dto)
        {
            var validation = await createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new CommonApiResponse<ClassSessionResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });

            var entity = new ClassSession
            {
                SemesterSubjectId = dto.SemesterSubjectId,
                FacultyId = dto.FacultyId,
                SessionDate = dto.SessionDate,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Topic = dto.Topic,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            context.Set<ClassSession>().Add(entity);
            await context.SaveChangesAsync();

            await context.Entry(entity).Reference(cs => cs.SemesterSubject).LoadAsync();
            if (entity.SemesterSubject != null)
                await context.Entry(entity.SemesterSubject).Reference(ss => ss.Subject).LoadAsync();
            await context.Entry(entity).Reference(cs => cs.Faculty).LoadAsync();
            if (entity.Faculty != null)
                await context.Entry(entity.Faculty).Reference(f => f.User).LoadAsync();

            return StatusCode(201, new CommonApiResponse<ClassSessionResponseDto>
            {
                Success = true,
                StatusCode = 201,
                Message = "Class session created successfully",
                Data = MapToDto(entity)
            });
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateClassSessionDto dto)
        {
            var validation = await updateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new CommonApiResponse<ClassSessionResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });

            var existing = await context.Set<ClassSession>().FindAsync(id);
            if (existing is null)
                return NotFound(new CommonApiResponse<ClassSessionResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Class session not found"
                });

            existing.SemesterSubjectId = dto.SemesterSubjectId;
            existing.FacultyId = dto.FacultyId;
            existing.SessionDate = dto.SessionDate;
            existing.StartTime = dto.StartTime;
            existing.EndTime = dto.EndTime;
            existing.Topic = dto.Topic;
            existing.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();

            await context.Entry(existing).Reference(cs => cs.SemesterSubject).LoadAsync();
            if (existing.SemesterSubject != null)
                await context.Entry(existing.SemesterSubject).Reference(ss => ss.Subject).LoadAsync();
            await context.Entry(existing).Reference(cs => cs.Faculty).LoadAsync();
            if (existing.Faculty != null)
                await context.Entry(existing.Faculty).Reference(f => f.User).LoadAsync();

            return Ok(new CommonApiResponse<ClassSessionResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Class session updated successfully",
                Data = MapToDto(existing)
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var entity = await context.Set<ClassSession>()
                .Include(cs => cs.SemesterSubject).ThenInclude(ss => ss.Subject)
                .Include(cs => cs.Faculty).ThenInclude(f => f.User)
                .FirstOrDefaultAsync(cs => cs.SessionId == id);

            if (entity is null)
                return NotFound(new CommonApiResponse<ClassSessionResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Class session not found"
                });

            context.Set<ClassSession>().Remove(entity);
            await context.SaveChangesAsync();

            return Ok(new CommonApiResponse<ClassSessionResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Class session deleted successfully",
                Data = MapToDto(entity)
            });
        }
    }
}
