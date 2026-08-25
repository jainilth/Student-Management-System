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
    public class MaterialController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IValidator<CreateMaterialDto> createValidator;
        private readonly IValidator<UpdateMaterialDto> updateValidator;

        public MaterialController(
            AppDbContext _context,
            IValidator<CreateMaterialDto> _createValidator,
            IValidator<UpdateMaterialDto> _updateValidator)
        {
            context = _context;
            createValidator = _createValidator;
            updateValidator = _updateValidator;
        }

        private static MaterialResponseDto MapToDto(Material m) => new MaterialResponseDto
        {
            MaterialId = m.MaterialId,
            Title = m.Title,
            Description = m.Description,
            SemesterSubjectId = m.SemesterSubjectId,
            SubjectName = m.SemesterSubject?.Subject?.SubjectName ?? string.Empty,
            UploadedBy = m.UploadedBy,
            UploadedByUserName = m.UploadedByUser != null ? m.UploadedByUser.UserName : string.Empty,
            MaterialType = m.MaterialType,
            FileName = m.FileName,
            FileUrl = m.FileUrl,
            FileSize = m.FileSize,
            CreatedAt = m.CreatedAt,
            UpdatedAt = m.UpdatedAt
        };

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var items = await context.Materials
                .Include(m => m.SemesterSubject).ThenInclude(ss => ss.Subject)
                .Include(m => m.UploadedByUser)
                .Select(m => MapToDto(m))
                .ToListAsync();

            return Ok(new CommonApiResponse<List<MaterialResponseDto>>
            {
                Success = true,
                StatusCode = 200,
                Message = "Materials retrieved successfully",
                Data = items
            });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById([FromRoute] int id)
        {
            var m = await context.Materials
                .Include(m => m.SemesterSubject).ThenInclude(ss => ss.Subject)
                .Include(m => m.UploadedByUser)
                .FirstOrDefaultAsync(m => m.MaterialId == id);

            if (m is null)
                return NotFound(new CommonApiResponse<MaterialResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Material not found"
                });

            return Ok(new CommonApiResponse<MaterialResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Material retrieved successfully",
                Data = MapToDto(m)
            });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateMaterialDto dto)
        {
            var validation = await createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new CommonApiResponse<MaterialResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });

            var entity = new Material
            {
                Title = dto.Title,
                Description = dto.Description,
                SemesterSubjectId = dto.SemesterSubjectId,
                UploadedBy = dto.UploadedBy,
                MaterialType = dto.MaterialType,
                FileName = dto.FileName,
                FileUrl = dto.FileUrl,
                FileSize = dto.FileSize,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            context.Materials.Add(entity);
            await context.SaveChangesAsync();

            await context.Entry(entity).Reference(m => m.SemesterSubject).LoadAsync();
            if (entity.SemesterSubject != null)
                await context.Entry(entity.SemesterSubject).Reference(ss => ss.Subject).LoadAsync();
            await context.Entry(entity).Reference(m => m.UploadedByUser).LoadAsync();

            return StatusCode(201, new CommonApiResponse<MaterialResponseDto>
            {
                Success = true,
                StatusCode = 201,
                Message = "Material created successfully",
                Data = MapToDto(entity)
            });
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateMaterialDto dto)
        {
            var validation = await updateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new CommonApiResponse<MaterialResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });

            var existing = await context.Materials.FindAsync(id);
            if (existing is null)
                return NotFound(new CommonApiResponse<MaterialResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Material not found"
                });

            existing.Title = dto.Title;
            existing.Description = dto.Description;
            existing.SemesterSubjectId = dto.SemesterSubjectId;
            existing.UploadedBy = dto.UploadedBy;
            existing.MaterialType = dto.MaterialType;
            existing.FileName = dto.FileName;
            existing.FileUrl = dto.FileUrl;
            existing.FileSize = dto.FileSize;
            existing.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();

            await context.Entry(existing).Reference(m => m.SemesterSubject).LoadAsync();
            if (existing.SemesterSubject != null)
                await context.Entry(existing.SemesterSubject).Reference(ss => ss.Subject).LoadAsync();
            await context.Entry(existing).Reference(m => m.UploadedByUser).LoadAsync();

            return Ok(new CommonApiResponse<MaterialResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Material updated successfully",
                Data = MapToDto(existing)
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var entity = await context.Materials
                .Include(m => m.SemesterSubject).ThenInclude(ss => ss.Subject)
                .Include(m => m.UploadedByUser)
                .FirstOrDefaultAsync(m => m.MaterialId == id);

            if (entity is null)
                return NotFound(new CommonApiResponse<MaterialResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Material not found"
                });

            context.Materials.Remove(entity);
            await context.SaveChangesAsync();

            return Ok(new CommonApiResponse<MaterialResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Material deleted successfully",
                Data = MapToDto(entity)
            });
        }
    }
}
