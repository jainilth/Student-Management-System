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
    public class AcademicProgramController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IValidator<CreateAcademicProgramDto> createValidator;
        private readonly IValidator<UpdateAcademicProgramDto> updateValidator;

        public AcademicProgramController(
            AppDbContext _context,
            IValidator<CreateAcademicProgramDto> _createValidator,
            IValidator<UpdateAcademicProgramDto> _updateValidator)
        {
            context = _context;
            createValidator = _createValidator;
            updateValidator = _updateValidator;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var programs = await context.Programs.Include(p => p.Department).Select(p => new AcademicProgramResponseDto
            {
                ProgramId = p.ProgramId, ProgramName = p.ProgramName, ProgramCode = p.ProgramCode,
                DepartmentId = p.DepartmentId, DepartmentName = p.Department != null ? p.Department.DepartmentName : string.Empty,
                DurationYears = p.DurationYears, TotalSemesters = p.TotalSemesters,
                IsActive = p.IsActive, CreatedAt = p.CreatedAt, UpdatedAt = p.UpdatedAt
            }).ToListAsync();

            return Ok(new CommonApiResponse<List<AcademicProgramResponseDto>>
            {
                Success = true, StatusCode = 200, Message = "Academic programs retrieved successfully", Data = programs
            });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById([FromRoute] int id)
        {
            var p = await context.Programs.Include(p => p.Department).FirstOrDefaultAsync(p => p.ProgramId == id);
            if (p is null)
                return NotFound(new CommonApiResponse<AcademicProgramResponseDto>
                {
                    Success = false, StatusCode = 404, Message = "Academic program not found"
                });

            return Ok(new CommonApiResponse<AcademicProgramResponseDto>
            {
                Success = true, StatusCode = 200, Message = "Academic program retrieved successfully",
                Data = new AcademicProgramResponseDto
                {
                    ProgramId = p.ProgramId, ProgramName = p.ProgramName, ProgramCode = p.ProgramCode,
                    DepartmentId = p.DepartmentId, DepartmentName = p.Department != null ? p.Department.DepartmentName : string.Empty,
                    DurationYears = p.DurationYears, TotalSemesters = p.TotalSemesters,
                    IsActive = p.IsActive, CreatedAt = p.CreatedAt, UpdatedAt = p.UpdatedAt
                }
            });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateAcademicProgramDto dto)
        {
            var validation = await createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new CommonApiResponse<AcademicProgramResponseDto>
                {
                    Success = false, StatusCode = 400, Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });

            var duplicate = await context.Programs.FirstOrDefaultAsync(p => p.ProgramCode == dto.ProgramCode);
            if (duplicate != null)
                return BadRequest(new CommonApiResponse<AcademicProgramResponseDto>
                {
                    Success = false, StatusCode = 400, Message = "Program code is already in use."
                });

            var entity = new AcademicProgram
            {
                ProgramName = dto.ProgramName, ProgramCode = dto.ProgramCode,
                DepartmentId = dto.DepartmentId, DurationYears = dto.DurationYears,
                TotalSemesters = dto.TotalSemesters, IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
            };
            context.Programs.Add(entity);
            await context.SaveChangesAsync();
            await context.Entry(entity).Reference(p => p.Department).LoadAsync();

            return StatusCode(201, new CommonApiResponse<AcademicProgramResponseDto>
            {
                Success = true, StatusCode = 201, Message = "Academic program created successfully",
                Data = new AcademicProgramResponseDto
                {
                    ProgramId = entity.ProgramId, ProgramName = entity.ProgramName, ProgramCode = entity.ProgramCode,
                    DepartmentId = entity.DepartmentId, DepartmentName = entity.Department != null ? entity.Department.DepartmentName : string.Empty,
                    DurationYears = entity.DurationYears, TotalSemesters = entity.TotalSemesters,
                    IsActive = entity.IsActive, CreatedAt = entity.CreatedAt, UpdatedAt = entity.UpdatedAt
                }
            });
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateAcademicProgramDto dto)
        {
            var validation = await updateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new CommonApiResponse<AcademicProgramResponseDto>
                {
                    Success = false, StatusCode = 400, Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });

            var existing = await context.Programs.FindAsync(id);
            if (existing is null)
                return NotFound(new CommonApiResponse<AcademicProgramResponseDto>
                {
                    Success = false, StatusCode = 404, Message = "Academic program not found"
                });

            var duplicate = await context.Programs.FirstOrDefaultAsync(p =>
                p.ProgramId != id && p.ProgramCode == dto.ProgramCode);
            if (duplicate != null)
                return BadRequest(new CommonApiResponse<AcademicProgramResponseDto>
                {
                    Success = false, StatusCode = 400, Message = "Program code is already in use."
                });

            existing.ProgramName = dto.ProgramName;
            existing.ProgramCode = dto.ProgramCode;
            existing.DepartmentId = dto.DepartmentId;
            existing.DurationYears = dto.DurationYears;
            existing.TotalSemesters = dto.TotalSemesters;
            existing.IsActive = dto.IsActive;
            existing.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();
            await context.Entry(existing).Reference(p => p.Department).LoadAsync();

            return Ok(new CommonApiResponse<AcademicProgramResponseDto>
            {
                Success = true, StatusCode = 200, Message = "Academic program updated successfully",
                Data = new AcademicProgramResponseDto
                {
                    ProgramId = existing.ProgramId, ProgramName = existing.ProgramName, ProgramCode = existing.ProgramCode,
                    DepartmentId = existing.DepartmentId, DepartmentName = existing.Department != null ? existing.Department.DepartmentName : string.Empty,
                    DurationYears = existing.DurationYears, TotalSemesters = existing.TotalSemesters,
                    IsActive = existing.IsActive, CreatedAt = existing.CreatedAt, UpdatedAt = existing.UpdatedAt
                }
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var entity = await context.Programs.Include(p => p.Department).FirstOrDefaultAsync(p => p.ProgramId == id);
            if (entity is null)
                return NotFound(new CommonApiResponse<AcademicProgramResponseDto>
                {
                    Success = false, StatusCode = 404, Message = "Academic program not found"
                });

            context.Programs.Remove(entity);
            await context.SaveChangesAsync();

            return Ok(new CommonApiResponse<AcademicProgramResponseDto>
            {
                Success = true, StatusCode = 200, Message = "Academic program deleted successfully",
                Data = new AcademicProgramResponseDto
                {
                    ProgramId = entity.ProgramId, ProgramName = entity.ProgramName, ProgramCode = entity.ProgramCode,
                    DepartmentId = entity.DepartmentId, DepartmentName = entity.Department != null ? entity.Department.DepartmentName : string.Empty,
                    DurationYears = entity.DurationYears, TotalSemesters = entity.TotalSemesters,
                    IsActive = entity.IsActive, CreatedAt = entity.CreatedAt, UpdatedAt = entity.UpdatedAt
                }
            });
        }
    }
}
