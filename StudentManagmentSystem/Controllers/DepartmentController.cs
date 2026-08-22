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
    public class DepartmentController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IValidator<CreateDepartmentDto> createValidator;
        private readonly IValidator<UpdateDepartmentDto> updateValidator;

        public DepartmentController(
            AppDbContext _context,
            IValidator<CreateDepartmentDto> _createValidator,
            IValidator<UpdateDepartmentDto> _updateValidator)
        {
            context = _context;
            createValidator = _createValidator;
            updateValidator = _updateValidator;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var departments = await context.Departments.Select(d => new DepartmentResponseDto
            {
                DepartmentId = d.DepartmentId,
                DepartmentName = d.DepartmentName,
                DepartmentCode = d.DepartmentCode,
                Description = d.Description,
                IsActive = d.IsActive,
                CreatedAt = d.CreatedAt,
                UpdatedAt = d.UpdatedAt
            }).ToListAsync();

            return Ok(new CommonApiResponse<List<DepartmentResponseDto>>
            {
                Success = true, StatusCode = 200, Message = "Departments retrieved successfully", Data = departments
            });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById([FromRoute] int id)
        {
            var d = await context.Departments.FindAsync(id);
            if (d is null)
                return NotFound(new CommonApiResponse<DepartmentResponseDto>
                {
                    Success = false, StatusCode = 404, Message = "Department not found"
                });

            return Ok(new CommonApiResponse<DepartmentResponseDto>
            {
                Success = true, StatusCode = 200, Message = "Department retrieved successfully",
                Data = new DepartmentResponseDto
                {
                    DepartmentId = d.DepartmentId, DepartmentName = d.DepartmentName,
                    DepartmentCode = d.DepartmentCode, Description = d.Description,
                    IsActive = d.IsActive, CreatedAt = d.CreatedAt, UpdatedAt = d.UpdatedAt
                }
            });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateDepartmentDto dto)
        {
            var validation = await createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new CommonApiResponse<DepartmentResponseDto>
                {
                    Success = false, StatusCode = 400, Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });

            var duplicate = await context.Departments.FirstOrDefaultAsync(d =>
                d.DepartmentCode == dto.DepartmentCode || d.DepartmentName == dto.DepartmentName);
            if (duplicate != null)
            {
                string field = duplicate.DepartmentCode == dto.DepartmentCode ? "Department code" : "Department name";
                return BadRequest(new CommonApiResponse<DepartmentResponseDto>
                {
                    Success = false, StatusCode = 400, Message = $"{field} is already in use."
                });
            }

            var entity = new Department
            {
                DepartmentName = dto.DepartmentName,
                DepartmentCode = dto.DepartmentCode,
                Description = dto.Description,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            context.Departments.Add(entity);
            await context.SaveChangesAsync();

            return StatusCode(201, new CommonApiResponse<DepartmentResponseDto>
            {
                Success = true, StatusCode = 201, Message = "Department created successfully",
                Data = new DepartmentResponseDto
                {
                    DepartmentId = entity.DepartmentId, DepartmentName = entity.DepartmentName,
                    DepartmentCode = entity.DepartmentCode, Description = entity.Description,
                    IsActive = entity.IsActive, CreatedAt = entity.CreatedAt, UpdatedAt = entity.UpdatedAt
                }
            });
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateDepartmentDto dto)
        {
            var validation = await updateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new CommonApiResponse<DepartmentResponseDto>
                {
                    Success = false, StatusCode = 400, Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });

            var existing = await context.Departments.FindAsync(id);
            if (existing is null)
                return NotFound(new CommonApiResponse<DepartmentResponseDto>
                {
                    Success = false, StatusCode = 404, Message = "Department not found"
                });

            var duplicate = await context.Departments.FirstOrDefaultAsync(d =>
                d.DepartmentId != id && (d.DepartmentCode == dto.DepartmentCode || d.DepartmentName == dto.DepartmentName));
            if (duplicate != null)
            {
                string field = duplicate.DepartmentCode == dto.DepartmentCode ? "Department code" : "Department name";
                return BadRequest(new CommonApiResponse<DepartmentResponseDto>
                {
                    Success = false, StatusCode = 400, Message = $"{field} is already in use."
                });
            }

            existing.DepartmentName = dto.DepartmentName;
            existing.DepartmentCode = dto.DepartmentCode;
            existing.Description = dto.Description;
            existing.IsActive = dto.IsActive;
            existing.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();

            return Ok(new CommonApiResponse<DepartmentResponseDto>
            {
                Success = true, StatusCode = 200, Message = "Department updated successfully",
                Data = new DepartmentResponseDto
                {
                    DepartmentId = existing.DepartmentId, DepartmentName = existing.DepartmentName,
                    DepartmentCode = existing.DepartmentCode, Description = existing.Description,
                    IsActive = existing.IsActive, CreatedAt = existing.CreatedAt, UpdatedAt = existing.UpdatedAt
                }
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var entity = await context.Departments.FindAsync(id);
            if (entity is null)
                return NotFound(new CommonApiResponse<DepartmentResponseDto>
                {
                    Success = false, StatusCode = 404, Message = "Department not found"
                });

            context.Departments.Remove(entity);
            await context.SaveChangesAsync();

            return Ok(new CommonApiResponse<DepartmentResponseDto>
            {
                Success = true, StatusCode = 200, Message = "Department deleted successfully",
                Data = new DepartmentResponseDto
                {
                    DepartmentId = entity.DepartmentId, DepartmentName = entity.DepartmentName,
                    DepartmentCode = entity.DepartmentCode, Description = entity.Description,
                    IsActive = entity.IsActive, CreatedAt = entity.CreatedAt, UpdatedAt = entity.UpdatedAt
                }
            });
        }
    }
}
