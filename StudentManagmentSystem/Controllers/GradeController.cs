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
    public class GradeController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IValidator<CreateGradeDto> createValidator;
        private readonly IValidator<UpdateGradeDto> updateValidator;

        public GradeController(
            AppDbContext _context,
            IValidator<CreateGradeDto> _createValidator,
            IValidator<UpdateGradeDto> _updateValidator)
        {
            context = _context;
            createValidator = _createValidator;
            updateValidator = _updateValidator;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var grades = await context.Set<Grade>().Select(g => new GradeResponseDto
            {
                GradeId = g.GradeId, GradeCode = g.GradeCode, GradeName = g.GradeName,
                GradePoint = g.GradePoint, MinMarks = g.MinMarks, MaxMarks = g.MaxMarks
            }).ToListAsync();

            return Ok(new CommonApiResponse<List<GradeResponseDto>>
            {
                Success = true, StatusCode = 200, Message = "Grades retrieved successfully", Data = grades
            });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById([FromRoute] int id)
        {
            var g = await context.Set<Grade>().FindAsync(id);
            if (g is null)
                return NotFound(new CommonApiResponse<GradeResponseDto>
                {
                    Success = false, StatusCode = 404, Message = "Grade not found"
                });

            return Ok(new CommonApiResponse<GradeResponseDto>
            {
                Success = true, StatusCode = 200, Message = "Grade retrieved successfully",
                Data = new GradeResponseDto
                {
                    GradeId = g.GradeId, GradeCode = g.GradeCode, GradeName = g.GradeName,
                    GradePoint = g.GradePoint, MinMarks = g.MinMarks, MaxMarks = g.MaxMarks
                }
            });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateGradeDto dto)
        {
            var validation = await createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new CommonApiResponse<GradeResponseDto>
                {
                    Success = false, StatusCode = 400, Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });

            var duplicate = await context.Set<Grade>().FirstOrDefaultAsync(g => g.GradeCode == dto.GradeCode);
            if (duplicate != null)
                return BadRequest(new CommonApiResponse<GradeResponseDto>
                {
                    Success = false, StatusCode = 400, Message = "Grade code is already in use."
                });

            var entity = new Grade
            {
                GradeCode = dto.GradeCode, GradeName = dto.GradeName,
                GradePoint = dto.GradePoint, MinMarks = dto.MinMarks, MaxMarks = dto.MaxMarks
            };
            context.Set<Grade>().Add(entity);
            await context.SaveChangesAsync();

            return StatusCode(201, new CommonApiResponse<GradeResponseDto>
            {
                Success = true, StatusCode = 201, Message = "Grade created successfully",
                Data = new GradeResponseDto
                {
                    GradeId = entity.GradeId, GradeCode = entity.GradeCode, GradeName = entity.GradeName,
                    GradePoint = entity.GradePoint, MinMarks = entity.MinMarks, MaxMarks = entity.MaxMarks
                }
            });
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateGradeDto dto)
        {
            var validation = await updateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new CommonApiResponse<GradeResponseDto>
                {
                    Success = false, StatusCode = 400, Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });

            var existing = await context.Set<Grade>().FindAsync(id);
            if (existing is null)
                return NotFound(new CommonApiResponse<GradeResponseDto>
                {
                    Success = false, StatusCode = 404, Message = "Grade not found"
                });

            var duplicate = await context.Set<Grade>().FirstOrDefaultAsync(g =>
                g.GradeId != id && g.GradeCode == dto.GradeCode);
            if (duplicate != null)
                return BadRequest(new CommonApiResponse<GradeResponseDto>
                {
                    Success = false, StatusCode = 400, Message = "Grade code is already in use."
                });

            existing.GradeCode = dto.GradeCode;
            existing.GradeName = dto.GradeName;
            existing.GradePoint = dto.GradePoint;
            existing.MinMarks = dto.MinMarks;
            existing.MaxMarks = dto.MaxMarks;
            await context.SaveChangesAsync();

            return Ok(new CommonApiResponse<GradeResponseDto>
            {
                Success = true, StatusCode = 200, Message = "Grade updated successfully",
                Data = new GradeResponseDto
                {
                    GradeId = existing.GradeId, GradeCode = existing.GradeCode, GradeName = existing.GradeName,
                    GradePoint = existing.GradePoint, MinMarks = existing.MinMarks, MaxMarks = existing.MaxMarks
                }
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var entity = await context.Set<Grade>().FindAsync(id);
            if (entity is null)
                return NotFound(new CommonApiResponse<GradeResponseDto>
                {
                    Success = false, StatusCode = 404, Message = "Grade not found"
                });

            context.Set<Grade>().Remove(entity);
            await context.SaveChangesAsync();

            return Ok(new CommonApiResponse<GradeResponseDto>
            {
                Success = true, StatusCode = 200, Message = "Grade deleted successfully",
                Data = new GradeResponseDto
                {
                    GradeId = entity.GradeId, GradeCode = entity.GradeCode, GradeName = entity.GradeName,
                    GradePoint = entity.GradePoint, MinMarks = entity.MinMarks, MaxMarks = entity.MaxMarks
                }
            });
        }
    }
}
