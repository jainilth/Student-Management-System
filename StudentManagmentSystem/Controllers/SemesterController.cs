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
    public class SemesterController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IValidator<CreateSemesterDto> createValidator;
        private readonly IValidator<UpdateSemesterDto> updateValidator;

        public SemesterController(
            AppDbContext _context,
            IValidator<CreateSemesterDto> _createValidator,
            IValidator<UpdateSemesterDto> _updateValidator)
        {
            context = _context;
            createValidator = _createValidator;
            updateValidator = _updateValidator;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var semesters = await context.Semesters.Select(s => new SemesterResponseDto
            {
                SemesterId = s.SemesterId,
                SemesterNumber = s.SemesterNumber,
                SemesterName = s.SemesterName,
                IsActive = s.IsActive,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            }).ToListAsync();

            return Ok(new CommonApiResponse<List<SemesterResponseDto>>
            {
                Success = true,
                StatusCode = 200,
                Message = "Semesters retrieved successfully",
                Data = semesters
            });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById([FromRoute] int id)
        {
            var s = await context.Semesters.FindAsync(id);
            if (s is null)
                return NotFound(new CommonApiResponse<SemesterResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Semester not found"
                });

            return Ok(new CommonApiResponse<SemesterResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Semester retrieved successfully",
                Data = new SemesterResponseDto
                {
                    SemesterId = s.SemesterId,
                    SemesterNumber = s.SemesterNumber,
                    SemesterName = s.SemesterName,
                    IsActive = s.IsActive,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt
                }
            });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateSemesterDto dto)
        {
            var validation = await createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new CommonApiResponse<SemesterResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });

            var duplicate = await context.Semesters.FirstOrDefaultAsync(s =>
                s.SemesterNumber == dto.SemesterNumber || s.SemesterName == dto.SemesterName);
            if (duplicate != null)
            {
                string field = duplicate.SemesterNumber == dto.SemesterNumber ? "Semester number" : "Semester name";
                return BadRequest(new CommonApiResponse<SemesterResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = $"{field} is already in use."
                });
            }

            var entity = new Semester
            {
                SemesterNumber = dto.SemesterNumber,
                SemesterName = dto.SemesterName,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            context.Semesters.Add(entity);
            await context.SaveChangesAsync();

            return StatusCode(201, new CommonApiResponse<SemesterResponseDto>
            {
                Success = true,
                StatusCode = 201,
                Message = "Semester created successfully",
                Data = new SemesterResponseDto
                {
                    SemesterId = entity.SemesterId,
                    SemesterNumber = entity.SemesterNumber,
                    SemesterName = entity.SemesterName,
                    IsActive = entity.IsActive,
                    CreatedAt = entity.CreatedAt,
                    UpdatedAt = entity.UpdatedAt
                }
            });
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateSemesterDto dto)
        {
            var validation = await updateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new CommonApiResponse<SemesterResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });

            var existing = await context.Semesters.FindAsync(id);
            if (existing is null)
                return NotFound(new CommonApiResponse<SemesterResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Semester not found"
                });

            var duplicate = await context.Semesters.FirstOrDefaultAsync(s =>
                s.SemesterId != id && (s.SemesterNumber == dto.SemesterNumber || s.SemesterName == dto.SemesterName));
            if (duplicate != null)
            {
                string field = duplicate.SemesterNumber == dto.SemesterNumber ? "Semester number" : "Semester name";
                return BadRequest(new CommonApiResponse<SemesterResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = $"{field} is already in use."
                });
            }

            existing.SemesterNumber = dto.SemesterNumber;
            existing.SemesterName = dto.SemesterName;
            existing.IsActive = dto.IsActive;
            existing.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();

            return Ok(new CommonApiResponse<SemesterResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Semester updated successfully",
                Data = new SemesterResponseDto
                {
                    SemesterId = existing.SemesterId,
                    SemesterNumber = existing.SemesterNumber,
                    SemesterName = existing.SemesterName,
                    IsActive = existing.IsActive,
                    CreatedAt = existing.CreatedAt,
                    UpdatedAt = existing.UpdatedAt
                }
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var entity = await context.Semesters.FindAsync(id);
            if (entity is null)
                return NotFound(new CommonApiResponse<SemesterResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Semester not found"
                });

            context.Semesters.Remove(entity);
            await context.SaveChangesAsync();

            return Ok(new CommonApiResponse<SemesterResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Semester deleted successfully",
                Data = new SemesterResponseDto
                {
                    SemesterId = entity.SemesterId,
                    SemesterNumber = entity.SemesterNumber,
                    SemesterName = entity.SemesterName,
                    IsActive = entity.IsActive,
                    CreatedAt = entity.CreatedAt,
                    UpdatedAt = entity.UpdatedAt
                }
            });
        }
    }
}
