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
            StudentSemesterId = sr.StudentSemesterId,
            StudentEnrollmentNumber = sr.StudentSemester?.Student?.EnrollmentNumber ?? string.Empty,
            StudentName = sr.StudentSemester?.Student?.User?.UserName ?? string.Empty,
            SemesterSubjectId = sr.SemesterSubjectId,
            SubjectCode = sr.SemesterSubject?.Subject?.SubjectCode ?? string.Empty,
            SubjectName = sr.SemesterSubject?.Subject?.SubjectName ?? string.Empty,
            InternalMarks = sr.InternalMarks,
            ExternalMarks = sr.ExternalMarks,
            PracticalMarks = sr.PracticalMarks,
            GradeId = sr.GradeId,
            GradeCode = sr.Grade?.GradeCode ?? string.Empty,
            GradePoint = sr.Grade?.GradePoint ?? 0,
            GradePoints = sr.GradePoints,
            CreditPoint = (sr.Grade?.GradePoint ?? 0) * sr.GradePoints,
            ResultStatus = sr.ResultStatus,
            CreatedAt = sr.CreatedAt,
            UpdatedAt = sr.UpdatedAt
        };

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var items = await context.SubjectResults
                .Include(sr => sr.StudentSemester).ThenInclude(ss => ss != null ? ss.Student : null).ThenInclude(s => s != null ? s.User : null)
                .Include(sr => sr.SemesterSubject).ThenInclude(ss => ss != null ? ss.Subject : null)
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
                .Include(r => r.StudentSemester).ThenInclude(ss => ss != null ? ss.Student : null).ThenInclude(s => s != null ? s.User : null)
                .Include(r => r.SemesterSubject).ThenInclude(ss => ss != null ? ss.Subject : null)
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
                sr.StudentSemesterId == dto.StudentSemesterId && sr.SemesterSubjectId == dto.SemesterSubjectId);
            if (duplicate != null)
                return BadRequest(new CommonApiResponse<SubjectResultResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "A result already exists for this subject in the given semester result."
                });



            var totalMarks = dto.InternalMarks + dto.PracticalMarks + dto.ExternalMarks;

            var grade = await context.Grades.SingleOrDefaultAsync(g => totalMarks >= g.MinMarks && totalMarks <= g.MaxMarks);

            if (grade == null)
            {
                return BadRequest(new CommonApiResponse<SubjectResultResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Grade not found."
                });

            }
            var resultStatus = "";
            if (grade.GradePoint == 0)
            {
                resultStatus = "Fail";
            }
            else
            {
                resultStatus = "Pass";
            }

            var entity = new SubjectResult
            {
                StudentSemesterId = dto.StudentSemesterId,
                SemesterSubjectId = dto.SemesterSubjectId,
                InternalMarks = dto.InternalMarks,
                ExternalMarks = dto.ExternalMarks,
                PracticalMarks = dto.PracticalMarks,
                GradeId = grade.GradeId,
                GradePoints = grade.GradePoint,
                ResultStatus = resultStatus,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            context.SubjectResults.Add(entity);
            await context.SaveChangesAsync();

            await context.Entry(entity).Reference(sr => sr.StudentSemester).LoadAsync();
            if (entity.StudentSemester != null)
            {
                await context.Entry(entity.StudentSemester).Reference(ss => ss.Student).LoadAsync();
                if (entity.StudentSemester.Student != null)
                    await context.Entry(entity.StudentSemester.Student).Reference(s => s.User).LoadAsync();
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
                sr.StudentSemesterId == dto.StudentSemesterId && sr.SemesterSubjectId == dto.SemesterSubjectId);
            if (duplicate != null)
                return BadRequest(new CommonApiResponse<SubjectResultResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "A result already exists for this subject in the given semester result."
                });

            var totalMarks = dto.InternalMarks + dto.PracticalMarks + dto.ExternalMarks;

            var grade = await context.Grades.SingleOrDefaultAsync(g => totalMarks >= g.MinMarks && totalMarks <= g.MaxMarks);

            if (grade == null)
            {
                return BadRequest(new CommonApiResponse<SubjectResultResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Grade not found."
                });

            }

            var resultStatus = "";
            if (grade.GradePoint == 0)
            {
                resultStatus = "Fail";
            }
            else
            {
                resultStatus = "Pass";
            }

            existing.StudentSemesterId = dto.StudentSemesterId;
            existing.SemesterSubjectId = dto.SemesterSubjectId;
            existing.InternalMarks = dto.InternalMarks;
            existing.ExternalMarks = dto.ExternalMarks;
            existing.PracticalMarks = dto.PracticalMarks;
            existing.GradeId = grade.GradeId;
            existing.GradePoints = grade.GradePoint;
            existing.ResultStatus = resultStatus;
            existing.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();

            await context.Entry(existing).Reference(sr => sr.StudentSemester).LoadAsync();
            if (existing.StudentSemester != null)
            {
                await context.Entry(existing.StudentSemester).Reference(ss => ss.Student).LoadAsync();
                if (existing.StudentSemester.Student != null)
                    await context.Entry(existing.StudentSemester.Student).Reference(s => s.User).LoadAsync();
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
                .Include(sr => sr.StudentSemester).ThenInclude(ss => ss != null ? ss.Student : null).ThenInclude(s => s != null ? s.User : null)
                .Include(sr => sr.SemesterSubject).ThenInclude(ss => ss != null ? ss.Subject : null)
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
