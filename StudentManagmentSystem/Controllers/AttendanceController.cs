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
    public class AttendanceController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IValidator<CreateAttendanceDto> createValidator;
        private readonly IValidator<UpdateAttendanceDto> updateValidator;

        public AttendanceController(AppDbContext _context,
            IValidator<CreateAttendanceDto> _createValidator,
            IValidator<UpdateAttendanceDto> _updateValidator)
        {
            context = _context;
            createValidator = _createValidator;
            updateValidator = _updateValidator;
        }

        private static AttendanceResponseDto MapToDto(Attendance a) => new AttendanceResponseDto
        {
            AttendanceId = a.AttendanceId, StudentSemesterId = a.StudentSemesterId,
            StudentEnrollmentNumber = a.StudentSemester?.Student?.EnrollmentNumber ?? string.Empty,
            StudentName = a.StudentSemester?.Student?.User?.UserName ?? string.Empty,
            SemesterSubjectId = a.SemesterSubjectId,
            SubjectName = a.SemesterSubject?.Subject?.SubjectName ?? string.Empty,
            ClassesHeld = a.ClassesHeld, ClassesAttended = a.ClassesAttended,
            AttendancePercentage = a.AttendancePercentage,
            CreatedAt = a.CreatedAt, UpdatedAt = a.UpdatedAt
        };

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var items = await context.Attendances
                .Include(a => a.StudentSemester).ThenInclude(ss => ss.Student).ThenInclude(s => s.User)
                .Include(a => a.SemesterSubject).ThenInclude(ss => ss.Subject)
                .Select(a => MapToDto(a)).ToListAsync();

            return Ok(new CommonApiResponse<List<AttendanceResponseDto>>
            {
                Success = true, StatusCode = 200, Message = "Attendance records retrieved successfully", Data = items
            });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById([FromRoute] int id)
        {
            var a = await context.Attendances
                .Include(a => a.StudentSemester).ThenInclude(ss => ss.Student).ThenInclude(s => s.User)
                .Include(a => a.SemesterSubject).ThenInclude(ss => ss.Subject)
                .FirstOrDefaultAsync(a => a.AttendanceId == id);

            if (a is null)
                return NotFound(new CommonApiResponse<AttendanceResponseDto>
                {
                    Success = false, StatusCode = 404, Message = "Attendance record not found"
                });

            return Ok(new CommonApiResponse<AttendanceResponseDto>
            {
                Success = true, StatusCode = 200, Message = "Attendance record retrieved successfully", Data = MapToDto(a)
            });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateAttendanceDto dto)
        {
            var validation = await createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new CommonApiResponse<AttendanceResponseDto>
                {
                    Success = false, StatusCode = 400, Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });

            var duplicate = await context.Attendances.FirstOrDefaultAsync(a =>
                a.StudentSemesterId == dto.StudentSemesterId && a.SemesterSubjectId == dto.SemesterSubjectId);
            if (duplicate != null)
                return BadRequest(new CommonApiResponse<AttendanceResponseDto>
                {
                    Success = false, StatusCode = 400, Message = "Attendance record already exists for this student semester and subject."
                });

            // Auto-calculate attendance percentage
            decimal percentage = dto.ClassesHeld > 0
                ? Math.Round((decimal)dto.ClassesAttended / dto.ClassesHeld * 100, 2)
                : 0m;

            var entity = new Attendance
            {
                StudentSemesterId = dto.StudentSemesterId, SemesterSubjectId = dto.SemesterSubjectId,
                ClassesHeld = dto.ClassesHeld, ClassesAttended = dto.ClassesAttended,
                AttendancePercentage = percentage, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
            };
            context.Attendances.Add(entity);
            await context.SaveChangesAsync();

            await context.Entry(entity).Reference(a => a.StudentSemester).LoadAsync();
            if (entity.StudentSemester != null)
            {
                await context.Entry(entity.StudentSemester).Reference(ss => ss.Student).LoadAsync();
                if (entity.StudentSemester.Student != null)
                    await context.Entry(entity.StudentSemester.Student).Reference(s => s.User).LoadAsync();
            }
            await context.Entry(entity).Reference(a => a.SemesterSubject).LoadAsync();
            if (entity.SemesterSubject != null)
                await context.Entry(entity.SemesterSubject).Reference(ss => ss.Subject).LoadAsync();

            return StatusCode(201, new CommonApiResponse<AttendanceResponseDto>
            {
                Success = true, StatusCode = 201, Message = "Attendance record created successfully", Data = MapToDto(entity)
            });
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateAttendanceDto dto)
        {
            var validation = await updateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new CommonApiResponse<AttendanceResponseDto>
                {
                    Success = false, StatusCode = 400, Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });

            var existing = await context.Attendances.FindAsync(id);
            if (existing is null)
                return NotFound(new CommonApiResponse<AttendanceResponseDto>
                {
                    Success = false, StatusCode = 404, Message = "Attendance record not found"
                });

            var duplicate = await context.Attendances.FirstOrDefaultAsync(a =>
                a.AttendanceId != id &&
                a.StudentSemesterId == dto.StudentSemesterId && a.SemesterSubjectId == dto.SemesterSubjectId);
            if (duplicate != null)
                return BadRequest(new CommonApiResponse<AttendanceResponseDto>
                {
                    Success = false, StatusCode = 400, Message = "Attendance record already exists for this student semester and subject."
                });

            // Auto-calculate attendance percentage
            decimal percentage = dto.ClassesHeld > 0
                ? Math.Round((decimal)dto.ClassesAttended / dto.ClassesHeld * 100, 2)
                : 0m;

            existing.StudentSemesterId = dto.StudentSemesterId;
            existing.SemesterSubjectId = dto.SemesterSubjectId;
            existing.ClassesHeld = dto.ClassesHeld;
            existing.ClassesAttended = dto.ClassesAttended;
            existing.AttendancePercentage = percentage;
            existing.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();

            await context.Entry(existing).Reference(a => a.StudentSemester).LoadAsync();
            if (existing.StudentSemester != null)
            {
                await context.Entry(existing.StudentSemester).Reference(ss => ss.Student).LoadAsync();
                if (existing.StudentSemester.Student != null)
                    await context.Entry(existing.StudentSemester.Student).Reference(s => s.User).LoadAsync();
            }
            await context.Entry(existing).Reference(a => a.SemesterSubject).LoadAsync();
            if (existing.SemesterSubject != null)
                await context.Entry(existing.SemesterSubject).Reference(ss => ss.Subject).LoadAsync();

            return Ok(new CommonApiResponse<AttendanceResponseDto>
            {
                Success = true, StatusCode = 200, Message = "Attendance record updated successfully", Data = MapToDto(existing)
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var entity = await context.Attendances
                .Include(a => a.StudentSemester).ThenInclude(ss => ss.Student).ThenInclude(s => s.User)
                .Include(a => a.SemesterSubject).ThenInclude(ss => ss.Subject)
                .FirstOrDefaultAsync(a => a.AttendanceId == id);

            if (entity is null)
                return NotFound(new CommonApiResponse<AttendanceResponseDto>
                {
                    Success = false, StatusCode = 404, Message = "Attendance record not found"
                });

            context.Attendances.Remove(entity);
            await context.SaveChangesAsync();

            return Ok(new CommonApiResponse<AttendanceResponseDto>
            {
                Success = true, StatusCode = 200, Message = "Attendance record deleted successfully", Data = MapToDto(entity)
            });
        }
    }
}
