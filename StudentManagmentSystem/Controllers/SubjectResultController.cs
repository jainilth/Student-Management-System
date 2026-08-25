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
    public class SubjectResultController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IValidator<CreateSubjectResultDto> createValidator;
        private readonly IValidator<UpdateSubjectResultDto> updateValidator;

        public SubjectResultController(AppDbContext _context,
            IValidator<CreateSubjectResultDto> _createValidator,
            IValidator<UpdateSubjectResultDto> _updateValidator)
        {
            context = _context;
            createValidator = _createValidator;
            updateValidator = _updateValidator;
        }

        private static SubjectResultResponseDto MapToDto(SubjectResult sr) => new SubjectResultResponseDto
        {
            SubjectResultId = sr.SubjectResultId,
            SemesterResultId = sr.SemesterResultId,
            StudentEnrollmentNumber = sr.SemesterResult?.StudentSemester?.Student?.EnrollmentNumber ?? string.Empty,
            StudentName = sr.SemesterResult?.StudentSemester?.Student?.User?.UserName ?? string.Empty,
            SemesterSubjectId = sr.SemesterSubjectId,
            SubjectName = sr.SemesterSubject?.Subject?.SubjectName ?? string.Empty,
            InternalMarks = sr.InternalMarks,
            ExternalMarks = sr.ExternalMarks,
            PracticalMarks = sr.PracticalMarks,
            TotalMarks = sr.TotalMarks,
            GradeId = sr.GradeId,
            GradeCode = sr.Grade?.GradeCode ?? string.Empty,
            CreditsEarned = sr.CreditsEarned,
            ResultStatus = sr.ResultStatus,
            CreatedAt = sr.CreatedAt,
            UpdatedAt = sr.UpdatedAt
        };

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var items = await context.SubjectResults
                .Include(sr => sr.SemesterResult).ThenInclude(sr => sr.StudentSemester).ThenInclude(ss => ss.Student).ThenInclude(s => s.User)
                .Include(sr => sr.SemesterSubject).ThenInclude(ss => ss.Subject)
                .Include(sr => sr.Grade)
                .Select(sr => MapToDto(sr)).ToListAsync();

            return Ok(new CommonApiResponse<List<SubjectResultResponseDto>>
            {
                Success = true,
                StatusCode = 200,
                Message = "Subject results retrieved successfully",
                Data = items
            });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById([FromRoute] int id)
        {
            var sr = await context.SubjectResults
                .Include(r => r.SemesterResult).ThenInclude(sr => sr.StudentSemester).ThenInclude(ss => ss.Student).ThenInclude(s => s.User)
                .Include(r => r.SemesterSubject).ThenInclude(ss => ss.Subject)
                .Include(r => r.Grade)
                .FirstOrDefaultAsync(r => r.SubjectResultId == id);

            if (sr is null)
                return NotFound(new CommonApiResponse<SubjectResultResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Subject result not found"
                });

            return Ok(new CommonApiResponse<SubjectResultResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Subject result retrieved successfully",
                Data = MapToDto(sr)
            });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateSubjectResultDto dto)
        {
            var validation = await createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new CommonApiResponse<SubjectResultResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });

            var duplicate = await context.SubjectResults.FirstOrDefaultAsync(sr =>
                sr.SemesterResultId == dto.SemesterResultId && sr.SemesterSubjectId == dto.SemesterSubjectId);
            if (duplicate != null)
                return BadRequest(new CommonApiResponse<SubjectResultResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "A result already exists for this subject in the given semester result."
                });

            var entity = new SubjectResult
            {
                SemesterResultId = dto.SemesterResultId,
                SemesterSubjectId = dto.SemesterSubjectId,
                InternalMarks = dto.InternalMarks,
                ExternalMarks = dto.ExternalMarks,
                PracticalMarks = dto.PracticalMarks,
                TotalMarks = dto.TotalMarks,
                GradeId = dto.GradeId,
                CreditsEarned = dto.CreditsEarned,
                ResultStatus = dto.ResultStatus,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            context.SubjectResults.Add(entity);
            await context.SaveChangesAsync();

            await context.Entry(entity).Reference(sr => sr.SemesterResult).LoadAsync();
            if (entity.SemesterResult != null)
            {
                await context.Entry(entity.SemesterResult).Reference(sr => sr.StudentSemester).LoadAsync();
                if (entity.SemesterResult.StudentSemester != null)
                {
                    await context.Entry(entity.SemesterResult.StudentSemester).Reference(ss => ss.Student).LoadAsync();
                    if (entity.SemesterResult.StudentSemester.Student != null)
                        await context.Entry(entity.SemesterResult.StudentSemester.Student).Reference(s => s.User).LoadAsync();
                }
            }
            await context.Entry(entity).Reference(sr => sr.SemesterSubject).LoadAsync();
            if (entity.SemesterSubject != null)
                await context.Entry(entity.SemesterSubject).Reference(ss => ss.Subject).LoadAsync();
            await context.Entry(entity).Reference(sr => sr.Grade).LoadAsync();

            return StatusCode(201, new CommonApiResponse<SubjectResultResponseDto>
            {
                Success = true,
                StatusCode = 201,
                Message = "Subject result created successfully",
                Data = MapToDto(entity)
            });
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateSubjectResultDto dto)
        {
            var validation = await updateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new CommonApiResponse<SubjectResultResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });

            var existing = await context.SubjectResults.FindAsync(id);
            if (existing is null)
                return NotFound(new CommonApiResponse<SubjectResultResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Subject result not found"
                });

            var duplicate = await context.SubjectResults.FirstOrDefaultAsync(sr =>
                sr.SubjectResultId != id &&
                sr.SemesterResultId == dto.SemesterResultId && sr.SemesterSubjectId == dto.SemesterSubjectId);
            if (duplicate != null)
                return BadRequest(new CommonApiResponse<SubjectResultResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "A result already exists for this subject in the given semester result."
                });

            existing.SemesterResultId = dto.SemesterResultId;
            existing.SemesterSubjectId = dto.SemesterSubjectId;
            existing.InternalMarks = dto.InternalMarks;
            existing.ExternalMarks = dto.ExternalMarks;
            existing.PracticalMarks = dto.PracticalMarks;
            existing.TotalMarks = dto.TotalMarks;
            existing.GradeId = dto.GradeId;
            existing.CreditsEarned = dto.CreditsEarned;
            existing.ResultStatus = dto.ResultStatus;
            existing.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();

            await context.Entry(existing).Reference(sr => sr.SemesterResult).LoadAsync();
            if (existing.SemesterResult != null)
            {
                await context.Entry(existing.SemesterResult).Reference(sr => sr.StudentSemester).LoadAsync();
                if (existing.SemesterResult.StudentSemester != null)
                {
                    await context.Entry(existing.SemesterResult.StudentSemester).Reference(ss => ss.Student).LoadAsync();
                    if (existing.SemesterResult.StudentSemester.Student != null)
                        await context.Entry(existing.SemesterResult.StudentSemester.Student).Reference(s => s.User).LoadAsync();
                }
            }
            await context.Entry(existing).Reference(sr => sr.SemesterSubject).LoadAsync();
            if (existing.SemesterSubject != null)
                await context.Entry(existing.SemesterSubject).Reference(ss => ss.Subject).LoadAsync();
            await context.Entry(existing).Reference(sr => sr.Grade).LoadAsync();

            return Ok(new CommonApiResponse<SubjectResultResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Subject result updated successfully",
                Data = MapToDto(existing)
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var entity = await context.SubjectResults
                .Include(sr => sr.SemesterResult).ThenInclude(sr => sr.StudentSemester).ThenInclude(ss => ss.Student).ThenInclude(s => s.User)
                .Include(sr => sr.SemesterSubject).ThenInclude(ss => ss.Subject)
                .Include(sr => sr.Grade)
                .FirstOrDefaultAsync(sr => sr.SubjectResultId == id);

            if (entity is null)
                return NotFound(new CommonApiResponse<SubjectResultResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Subject result not found"
                });

            context.SubjectResults.Remove(entity);
            await context.SaveChangesAsync();

            return Ok(new CommonApiResponse<SubjectResultResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Subject result deleted successfully",
                Data = MapToDto(entity)
            });
        }
    }
}
