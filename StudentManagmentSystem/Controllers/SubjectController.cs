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
    public class SubjectController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IValidator<CreateSubjectDto> createValidator;
        private readonly IValidator<UpdateSubjectDto> updateValidator;

        public SubjectController(
            AppDbContext _context,
            IValidator<CreateSubjectDto> _createValidator,
            IValidator<UpdateSubjectDto> _updateValidator)
        {
            context = _context;
            createValidator = _createValidator;
            updateValidator = _updateValidator;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var subjects = await context.Subjects.Select(s => new SubjectResponseDto
            {
                SubjectId = s.SubjectId,
                SubjectCode = s.SubjectCode,
                SubjectName = s.SubjectName,
                SubjectType = s.SubjectType,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            }).ToListAsync();

            return Ok(new CommonApiResponse<List<SubjectResponseDto>>
            {
                Success = true,
                StatusCode = 200,
                Message = "Subjects retrieved successfully",
                Data = subjects
            });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById([FromRoute] int id)
        {
            var s = await context.Subjects.FindAsync(id);
            if (s is null)
                return NotFound(new CommonApiResponse<SubjectResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Subject not found"
                });

            return Ok(new CommonApiResponse<SubjectResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Subject retrieved successfully",
                Data = new SubjectResponseDto
                {
                    SubjectId = s.SubjectId,
                    SubjectCode = s.SubjectCode,
                    SubjectName = s.SubjectName,
                    SubjectType = s.SubjectType,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt
                }
            });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateSubjectDto dto)
        {
            var validation = await createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new CommonApiResponse<SubjectResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });

            var duplicate = await context.Subjects.FirstOrDefaultAsync(s => s.SubjectCode == dto.SubjectCode);
            if (duplicate != null)
                return BadRequest(new CommonApiResponse<SubjectResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Subject code is already in use."
                });

            var entity = new Subject
            {
                SubjectCode = dto.SubjectCode,
                SubjectName = dto.SubjectName,
                SubjectType = dto.SubjectType,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            context.Subjects.Add(entity);
            await context.SaveChangesAsync();

            return StatusCode(201, new CommonApiResponse<SubjectResponseDto>
            {
                Success = true,
                StatusCode = 201,
                Message = "Subject created successfully",
                Data = new SubjectResponseDto
                {
                    SubjectId = entity.SubjectId,
                    SubjectCode = entity.SubjectCode,
                    SubjectName = entity.SubjectName,
                    SubjectType = entity.SubjectType,
                    CreatedAt = entity.CreatedAt,
                    UpdatedAt = entity.UpdatedAt
                }
            });
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateSubjectDto dto)
        {
            var validation = await updateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new CommonApiResponse<SubjectResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });

            var existing = await context.Subjects.FindAsync(id);
            if (existing is null)
                return NotFound(new CommonApiResponse<SubjectResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Subject not found"
                });

            var duplicate = await context.Subjects.FirstOrDefaultAsync(s =>
                s.SubjectId != id && s.SubjectCode == dto.SubjectCode);
            if (duplicate != null)
                return BadRequest(new CommonApiResponse<SubjectResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Subject code is already in use."
                });

            existing.SubjectCode = dto.SubjectCode;
            existing.SubjectName = dto.SubjectName;
            existing.SubjectType = dto.SubjectType;
            existing.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();

            return Ok(new CommonApiResponse<SubjectResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Subject updated successfully",
                Data = new SubjectResponseDto
                {
                    SubjectId = existing.SubjectId,
                    SubjectCode = existing.SubjectCode,
                    SubjectName = existing.SubjectName,
                    SubjectType = existing.SubjectType,
                    CreatedAt = existing.CreatedAt,
                    UpdatedAt = existing.UpdatedAt
                }
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var entity = await context.Subjects.FindAsync(id);
            if (entity is null)
                return NotFound(new CommonApiResponse<SubjectResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Subject not found"
                });

            context.Subjects.Remove(entity);
            await context.SaveChangesAsync();

            return Ok(new CommonApiResponse<SubjectResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Subject deleted successfully",
                Data = new SubjectResponseDto
                {
                    SubjectId = entity.SubjectId,
                    SubjectCode = entity.SubjectCode,
                    SubjectName = entity.SubjectName,
                    SubjectType = entity.SubjectType,
                    CreatedAt = entity.CreatedAt,
                    UpdatedAt = entity.UpdatedAt
                }
            });
        }
    }
}
