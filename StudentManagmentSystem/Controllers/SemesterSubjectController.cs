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
    public class SemesterSubjectController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IValidator<CreateSemesterSubjectDto> createValidator;
        private readonly IValidator<UpdateSemesterSubjectDto> updateValidator;

        public SemesterSubjectController(
            AppDbContext _context,
            IValidator<CreateSemesterSubjectDto> _createValidator,
            IValidator<UpdateSemesterSubjectDto> _updateValidator)
        {
            context = _context;
            createValidator = _createValidator;
            updateValidator = _updateValidator;
        }

        private static SemesterSubjectResponseDto MapToDto(SemesterSubject ss) => new SemesterSubjectResponseDto
        {
            SemesterSubjectId = ss.SemesterSubjectId,
            ProgramId = ss.ProgramId, ProgramName = ss.AcademicProgram != null ? ss.AcademicProgram.ProgramName : string.Empty,
            SemesterId = ss.SemesterId, SemesterName = ss.Semester != null ? ss.Semester.SemesterName : string.Empty,
            SubjectId = ss.SubjectId, SubjectName = ss.Subject != null ? ss.Subject.SubjectName : string.Empty,
            Credits = ss.Credits, CreatedAt = ss.CreatedAt, UpdatedAt = ss.UpdatedAt
        };

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var items = await context.SemesterSubjects
                .Include(ss => ss.AcademicProgram)
                .Include(ss => ss.Semester)
                .Include(ss => ss.Subject)
                .Select(ss => MapToDto(ss))
                .ToListAsync();

            return Ok(new CommonApiResponse<List<SemesterSubjectResponseDto>>
            {
                Success = true, StatusCode = 200, Message = "Semester subjects retrieved successfully", Data = items
            });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById([FromRoute] int id)
        {
            var ss = await context.SemesterSubjects
                .Include(ss => ss.AcademicProgram)
                .Include(ss => ss.Semester)
                .Include(ss => ss.Subject)
                .FirstOrDefaultAsync(ss => ss.SemesterSubjectId == id);

            if (ss is null)
                return NotFound(new CommonApiResponse<SemesterSubjectResponseDto>
                {
                    Success = false, StatusCode = 404, Message = "Semester subject not found"
                });

            return Ok(new CommonApiResponse<SemesterSubjectResponseDto>
            {
                Success = true, StatusCode = 200, Message = "Semester subject retrieved successfully", Data = MapToDto(ss)
            });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateSemesterSubjectDto dto)
        {
            var validation = await createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new CommonApiResponse<SemesterSubjectResponseDto>
                {
                    Success = false, StatusCode = 400, Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });

            var duplicate = await context.SemesterSubjects.FirstOrDefaultAsync(ss =>
                ss.ProgramId == dto.ProgramId && ss.SemesterId == dto.SemesterId && ss.SubjectId == dto.SubjectId);
            if (duplicate != null)
                return BadRequest(new CommonApiResponse<SemesterSubjectResponseDto>
                {
                    Success = false, StatusCode = 400, Message = "This subject is already assigned to the program and semester."
                });

            var entity = new SemesterSubject
            {
                ProgramId = dto.ProgramId, SemesterId = dto.SemesterId,
                SubjectId = dto.SubjectId, Credits = dto.Credits,
                CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
            };
            context.SemesterSubjects.Add(entity);
            await context.SaveChangesAsync();

            await context.Entry(entity).Reference(ss => ss.AcademicProgram).LoadAsync();
            await context.Entry(entity).Reference(ss => ss.Semester).LoadAsync();
            await context.Entry(entity).Reference(ss => ss.Subject).LoadAsync();

            return StatusCode(201, new CommonApiResponse<SemesterSubjectResponseDto>
            {
                Success = true, StatusCode = 201, Message = "Semester subject created successfully", Data = MapToDto(entity)
            });
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateSemesterSubjectDto dto)
        {
            var validation = await updateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new CommonApiResponse<SemesterSubjectResponseDto>
                {
                    Success = false, StatusCode = 400, Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });

            var existing = await context.SemesterSubjects.FindAsync(id);
            if (existing is null)
                return NotFound(new CommonApiResponse<SemesterSubjectResponseDto>
                {
                    Success = false, StatusCode = 404, Message = "Semester subject not found"
                });

            var duplicate = await context.SemesterSubjects.FirstOrDefaultAsync(ss =>
                ss.SemesterSubjectId != id &&
                ss.ProgramId == dto.ProgramId && ss.SemesterId == dto.SemesterId && ss.SubjectId == dto.SubjectId);
            if (duplicate != null)
                return BadRequest(new CommonApiResponse<SemesterSubjectResponseDto>
                {
                    Success = false, StatusCode = 400, Message = "This subject is already assigned to the program and semester."
                });

            existing.ProgramId = dto.ProgramId;
            existing.SemesterId = dto.SemesterId;
            existing.SubjectId = dto.SubjectId;
            existing.Credits = dto.Credits;
            existing.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();

            await context.Entry(existing).Reference(ss => ss.AcademicProgram).LoadAsync();
            await context.Entry(existing).Reference(ss => ss.Semester).LoadAsync();
            await context.Entry(existing).Reference(ss => ss.Subject).LoadAsync();

            return Ok(new CommonApiResponse<SemesterSubjectResponseDto>
            {
                Success = true, StatusCode = 200, Message = "Semester subject updated successfully", Data = MapToDto(existing)
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var entity = await context.SemesterSubjects
                .Include(ss => ss.AcademicProgram)
                .Include(ss => ss.Semester)
                .Include(ss => ss.Subject)
                .FirstOrDefaultAsync(ss => ss.SemesterSubjectId == id);

            if (entity is null)
                return NotFound(new CommonApiResponse<SemesterSubjectResponseDto>
                {
                    Success = false, StatusCode = 404, Message = "Semester subject not found"
                });

            context.SemesterSubjects.Remove(entity);
            await context.SaveChangesAsync();

            return Ok(new CommonApiResponse<SemesterSubjectResponseDto>
            {
                Success = true, StatusCode = 200, Message = "Semester subject deleted successfully", Data = MapToDto(entity)
            });
        }
    }
}
