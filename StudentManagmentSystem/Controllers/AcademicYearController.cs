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
    public class AcademicYearController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IValidator<CreateAcademicYearDto> createValidator;
        private readonly IValidator<UpdateAcademicYearDto> updateValidator;

        public AcademicYearController(
            AppDbContext _context,
            IValidator<CreateAcademicYearDto> _createValidator,
            IValidator<UpdateAcademicYearDto> _updateValidator)
        {
            context = _context;
            createValidator = _createValidator;
            updateValidator = _updateValidator;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var years = await context.Set<AcademicYear>().Select(y => new AcademicYearResponseDto
            {
                AcademicYearId = y.AcademicYearId,
                Year = y.Year
            }).ToListAsync();

            return Ok(new CommonApiResponse<List<AcademicYearResponseDto>>
            {
                Success = true,
                StatusCode = 200,
                Message = "Academic years retrieved successfully",
                Data = years
            });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById([FromRoute] int id)
        {
            var year = await context.Set<AcademicYear>().FindAsync(id);
            if (year is null)
                return NotFound(new CommonApiResponse<AcademicYearResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Academic year not found"
                });

            return Ok(new CommonApiResponse<AcademicYearResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Academic year retrieved successfully",
                Data = new AcademicYearResponseDto { AcademicYearId = year.AcademicYearId, Year = year.Year }
            });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateAcademicYearDto dto)
        {
            var validation = await createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new CommonApiResponse<AcademicYearResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });

            var duplicate = await context.Set<AcademicYear>().FirstOrDefaultAsync(y => y.Year == dto.Year);
            if (duplicate != null)
                return BadRequest(new CommonApiResponse<AcademicYearResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Academic year already exists."
                });

            var entity = new AcademicYear { Year = dto.Year };
            context.Set<AcademicYear>().Add(entity);
            await context.SaveChangesAsync();

            return StatusCode(201, new CommonApiResponse<AcademicYearResponseDto>
            {
                Success = true,
                StatusCode = 201,
                Message = "Academic year created successfully",
                Data = new AcademicYearResponseDto { AcademicYearId = entity.AcademicYearId, Year = entity.Year }
            });
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateAcademicYearDto dto)
        {
            var validation = await updateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new CommonApiResponse<AcademicYearResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });

            var existing = await context.Set<AcademicYear>().FindAsync(id);
            if (existing is null)
                return NotFound(new CommonApiResponse<AcademicYearResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Academic year not found"
                });

            var duplicate = await context.Set<AcademicYear>().FirstOrDefaultAsync(y => y.Year == dto.Year && y.AcademicYearId != id);
            if (duplicate != null)
                return BadRequest(new CommonApiResponse<AcademicYearResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Academic year already exists."
                });

            existing.Year = dto.Year;
            await context.SaveChangesAsync();

            return Ok(new CommonApiResponse<AcademicYearResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Academic year updated successfully",
                Data = new AcademicYearResponseDto { AcademicYearId = existing.AcademicYearId, Year = existing.Year }
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var entity = await context.Set<AcademicYear>().FindAsync(id);
            if (entity is null)
                return NotFound(new CommonApiResponse<AcademicYearResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Academic year not found"
                });

            context.Set<AcademicYear>().Remove(entity);
            await context.SaveChangesAsync();

            return Ok(new CommonApiResponse<AcademicYearResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Academic year deleted successfully",
                Data = new AcademicYearResponseDto { AcademicYearId = entity.AcademicYearId, Year = entity.Year }
            });
        }
    }
}
