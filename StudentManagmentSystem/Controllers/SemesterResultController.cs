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
    public class SemesterResultController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IValidator<CreateSemesterResultDto> createValidator;
        private readonly IValidator<UpdateSemesterResultDto> updateValidator;

        public SemesterResultController(AppDbContext _context,
            IValidator<CreateSemesterResultDto> _createValidator,
            IValidator<UpdateSemesterResultDto> _updateValidator)
        {
            context = _context;
            createValidator = _createValidator;
            updateValidator = _updateValidator;
        }

        private static SemesterResultResponseDto MapToDto(SemesterResult sr) => new SemesterResultResponseDto
        {
            SemesterResultId = sr.SemesterResultId,
            StudentSemesterId = sr.StudentSemesterId,
            StudentEnrollmentNumber = sr.StudentSemester?.Student?.EnrollmentNumber ?? string.Empty,
            StudentName = sr.StudentSemester?.Student?.User?.UserName ?? string.Empty,
            SGPA = sr.SGPA,
            TotalCredits = sr.TotalCredits,
            EarnedCredits = sr.EarnedCredits,
            ResultStatus = sr.ResultStatus,
            CreatedAt = sr.CreatedAt,
            UpdatedAt = sr.UpdatedAt
        };

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var items = await context.SemesterResults
                .Include(sr => sr.StudentSemester).ThenInclude(ss => ss.Student).ThenInclude(s => s.User)
                .Select(sr => MapToDto(sr)).ToListAsync();

            return Ok(new CommonApiResponse<List<SemesterResultResponseDto>>
            {
                Success = true,
                StatusCode = 200,
                Message = "Semester results retrieved successfully",
                Data = items
            });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById([FromRoute] int id)
        {
            var sr = await context.SemesterResults
                .Include(r => r.StudentSemester).ThenInclude(ss => ss.Student).ThenInclude(s => s.User)
                .FirstOrDefaultAsync(r => r.SemesterResultId == id);

            if (sr is null)
                return NotFound(new CommonApiResponse<SemesterResultResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Semester result not found"
                });

            return Ok(new CommonApiResponse<SemesterResultResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Semester result retrieved successfully",
                Data = MapToDto(sr)
            });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateSemesterResultDto dto)
        {
            var validation = await createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new CommonApiResponse<SemesterResultResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });

            var duplicate = await context.SemesterResults.FirstOrDefaultAsync(sr =>
                sr.StudentSemesterId == dto.StudentSemesterId);
            if (duplicate != null)
                return BadRequest(new CommonApiResponse<SemesterResultResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "A semester result already exists for this student semester."
                });

            var entity = new SemesterResult
            {
                StudentSemesterId = dto.StudentSemesterId,
                SGPA = dto.SGPA,
                TotalCredits = dto.TotalCredits,
                EarnedCredits = dto.EarnedCredits,
                ResultStatus = dto.ResultStatus,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            context.SemesterResults.Add(entity);
            await context.SaveChangesAsync();

            await context.Entry(entity).Reference(sr => sr.StudentSemester).LoadAsync();
            if (entity.StudentSemester != null)
            {
                await context.Entry(entity.StudentSemester).Reference(ss => ss.Student).LoadAsync();
                if (entity.StudentSemester.Student != null)
                    await context.Entry(entity.StudentSemester.Student).Reference(s => s.User).LoadAsync();
            }

            return StatusCode(201, new CommonApiResponse<SemesterResultResponseDto>
            {
                Success = true,
                StatusCode = 201,
                Message = "Semester result created successfully",
                Data = MapToDto(entity)
            });
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateSemesterResultDto dto)
        {
            var validation = await updateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new CommonApiResponse<SemesterResultResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });

            var existing = await context.SemesterResults.FindAsync(id);
            if (existing is null)
                return NotFound(new CommonApiResponse<SemesterResultResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Semester result not found"
                });

            var duplicate = await context.SemesterResults.FirstOrDefaultAsync(sr =>
                sr.SemesterResultId != id && sr.StudentSemesterId == dto.StudentSemesterId);
            if (duplicate != null)
                return BadRequest(new CommonApiResponse<SemesterResultResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "A semester result already exists for this student semester."
                });

            existing.StudentSemesterId = dto.StudentSemesterId;
            existing.SGPA = dto.SGPA;
            existing.TotalCredits = dto.TotalCredits;
            existing.EarnedCredits = dto.EarnedCredits;
            existing.ResultStatus = dto.ResultStatus;
            existing.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();

            await context.Entry(existing).Reference(sr => sr.StudentSemester).LoadAsync();
            if (existing.StudentSemester != null)
            {
                await context.Entry(existing.StudentSemester).Reference(ss => ss.Student).LoadAsync();
                if (existing.StudentSemester.Student != null)
                    await context.Entry(existing.StudentSemester.Student).Reference(s => s.User).LoadAsync();
            }

            return Ok(new CommonApiResponse<SemesterResultResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Semester result updated successfully",
                Data = MapToDto(existing)
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var entity = await context.SemesterResults
                .Include(sr => sr.StudentSemester).ThenInclude(ss => ss.Student).ThenInclude(s => s.User)
                .FirstOrDefaultAsync(sr => sr.SemesterResultId == id);

            if (entity is null)
                return NotFound(new CommonApiResponse<SemesterResultResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Semester result not found"
                });

            context.SemesterResults.Remove(entity);
            await context.SaveChangesAsync();

            return Ok(new CommonApiResponse<SemesterResultResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Semester result deleted successfully",
                Data = MapToDto(entity)
            });
        }
    }
}
