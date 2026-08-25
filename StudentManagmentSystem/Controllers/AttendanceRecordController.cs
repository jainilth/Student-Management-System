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
    public class AttendanceRecordController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IValidator<CreateAttendanceRecordDto> createValidator;
        private readonly IValidator<UpdateAttendanceRecordDto> updateValidator;

        public AttendanceRecordController(AppDbContext _context,
            IValidator<CreateAttendanceRecordDto> _createValidator,
            IValidator<UpdateAttendanceRecordDto> _updateValidator)
        {
            context = _context;
            createValidator = _createValidator;
            updateValidator = _updateValidator;
        }

        private static AttendanceRecordResponseDto MapToDto(AttendanceRecord ar) => new AttendanceRecordResponseDto
        {
            AttendanceRecordId = ar.AttendanceRecordId,
            SessionId = ar.SessionId,
            Topic = ar.Session?.Topic ?? string.Empty,
            StudentSemesterId = ar.StudentSemesterId,
            StudentEnrollmentNumber = ar.StudentSemester?.Student?.EnrollmentNumber ?? string.Empty,
            StudentName = ar.StudentSemester?.Student?.User?.UserName ?? string.Empty,
            Status = ar.Status,
            Remarks = ar.Remarks,
            CreatedAt = ar.CreatedAt
        };

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var items = await context.AttendanceRecords
                .Include(ar => ar.Session)
                .Include(ar => ar.StudentSemester).ThenInclude(ss => ss.Student).ThenInclude(s => s.User)
                .Select(ar => MapToDto(ar)).ToListAsync();

            return Ok(new CommonApiResponse<List<AttendanceRecordResponseDto>>
            {
                Success = true,
                StatusCode = 200,
                Message = "Attendance records retrieved successfully",
                Data = items
            });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById([FromRoute] int id)
        {
            var ar = await context.AttendanceRecords
                .Include(ar => ar.Session)
                .Include(ar => ar.StudentSemester).ThenInclude(ss => ss.Student).ThenInclude(s => s.User)
                .FirstOrDefaultAsync(ar => ar.AttendanceRecordId == id);
            if (ar is null)
                return NotFound(new CommonApiResponse<AttendanceRecordResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Attendance record not found"
                });

            return Ok(new CommonApiResponse<AttendanceRecordResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Attendance record retrieved successfully",
                Data = MapToDto(ar)
            });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateAttendanceRecordDto dto)
        {
            var validation = await createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new CommonApiResponse<AttendanceRecordResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });

            var duplicate = await context.AttendanceRecords.FirstOrDefaultAsync(ar =>
                ar.SessionId == dto.SessionId && ar.StudentSemesterId == dto.StudentSemesterId);
            if (duplicate != null)
                return BadRequest(new CommonApiResponse<AttendanceRecordResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "An attendance record already exists for this session and student."
                });

            var entity = new AttendanceRecord
            {
                SessionId = dto.SessionId,
                StudentSemesterId = dto.StudentSemesterId,
                Status = dto.Status,
                Remarks = dto.Remarks,
                CreatedAt = DateTime.UtcNow
            };
            context.AttendanceRecords.Add(entity);
            await context.SaveChangesAsync();

            await context.Entry(entity).Reference(ar => ar.Session).LoadAsync();
            await context.Entry(entity).Reference(ar => ar.StudentSemester).LoadAsync();
            if (entity.StudentSemester != null)
            {
                await context.Entry(entity.StudentSemester).Reference(ss => ss.Student).LoadAsync();
                if (entity.StudentSemester.Student != null)
                    await context.Entry(entity.StudentSemester.Student).Reference(s => s.User).LoadAsync();
            }

            return StatusCode(201, new CommonApiResponse<AttendanceRecordResponseDto>
            {
                Success = true,
                StatusCode = 201,
                Message = "Attendance record created successfully",
                Data = MapToDto(entity)
            });
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateAttendanceRecordDto dto)
        {
            var validation = await updateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new CommonApiResponse<AttendanceRecordResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });

            var existing = await context.AttendanceRecords.FindAsync(id);
            if (existing is null)
                return NotFound(new CommonApiResponse<AttendanceRecordResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Attendance record not found"
                });

            var duplicate = await context.AttendanceRecords.FirstOrDefaultAsync(ar =>
                ar.AttendanceRecordId != id &&
                ar.SessionId == dto.SessionId && ar.StudentSemesterId == dto.StudentSemesterId);
            if (duplicate != null)
                return BadRequest(new CommonApiResponse<AttendanceRecordResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "An attendance record already exists for this session and student."
                });

            existing.SessionId = dto.SessionId;
            existing.StudentSemesterId = dto.StudentSemesterId;
            existing.Status = dto.Status;
            existing.Remarks = dto.Remarks;
            // Record doesn't have UpdatedAt in model
            await context.SaveChangesAsync();

            await context.Entry(existing).Reference(ar => ar.Session).LoadAsync();
            await context.Entry(existing).Reference(ar => ar.StudentSemester).LoadAsync();
            if (existing.StudentSemester != null)
            {
                await context.Entry(existing.StudentSemester).Reference(ss => ss.Student).LoadAsync();
                if (existing.StudentSemester.Student != null)
                    await context.Entry(existing.StudentSemester.Student).Reference(s => s.User).LoadAsync();
            }

            return Ok(new CommonApiResponse<AttendanceRecordResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Attendance record updated successfully",
                Data = MapToDto(existing)
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var entity = await context.AttendanceRecords
                .Include(ar => ar.Session)
                .Include(ar => ar.StudentSemester).ThenInclude(ss => ss.Student).ThenInclude(s => s.User)
                .FirstOrDefaultAsync(ar => ar.AttendanceRecordId == id);
            if (entity is null)
                return NotFound(new CommonApiResponse<AttendanceRecordResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Attendance record not found"
                });

            context.AttendanceRecords.Remove(entity);
            await context.SaveChangesAsync();

            return Ok(new CommonApiResponse<AttendanceRecordResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Attendance record deleted successfully",
                Data = MapToDto(entity)
            });
        }
    }
}
