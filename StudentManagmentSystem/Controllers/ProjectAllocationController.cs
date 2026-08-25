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
    public class ProjectAllocationController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IValidator<CreateProjectAllocationDto> createValidator;
        private readonly IValidator<UpdateProjectAllocationDto> updateValidator;

        public ProjectAllocationController(AppDbContext _context,
            IValidator<CreateProjectAllocationDto> _createValidator,
            IValidator<UpdateProjectAllocationDto> _updateValidator)
        {
            context = _context;
            createValidator = _createValidator;
            updateValidator = _updateValidator;
        }

        private static ProjectAllocationResponseDto MapToDto(ProjectAllocation pa) => new ProjectAllocationResponseDto
        {
            AllocationId = pa.AllocationId,
            ProjectId = pa.ProjectId,
            ProjectTitle = pa.Project?.Title ?? string.Empty,
            StudentId = pa.StudentId,
            StudentEnrollmentNumber = pa.Student?.EnrollmentNumber ?? string.Empty,
            StudentName = pa.Student?.User?.UserName ?? string.Empty,
            FacultyId = pa.FacultyId,
            FacultyEmployeeNumber = pa.Faculty?.EmployeeNumber ?? string.Empty,
            FacultyName = pa.Faculty?.User?.UserName ?? string.Empty,
            FinalScore = pa.FinalScore,
            Grade = pa.Grade,
            Status = pa.Status,
            RepositoryUrl = pa.RepositoryUrl,
            CreatedAt = pa.CreatedAt,
            UpdatedAt = pa.UpdatedAt
        };

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var items = await context.ProjectAllocations
                .Include(pa => pa.Project)
                .Include(pa => pa.Student).ThenInclude(s => s.User)
                .Include(pa => pa.Faculty).ThenInclude(f => f.User)
                .Select(pa => MapToDto(pa)).ToListAsync();

            return Ok(new CommonApiResponse<List<ProjectAllocationResponseDto>>
            {
                Success = true,
                StatusCode = 200,
                Message = "Project allocations retrieved successfully",
                Data = items
            });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById([FromRoute] int id)
        {
            var pa = await context.ProjectAllocations
                .Include(pa => pa.Project)
                .Include(pa => pa.Student).ThenInclude(s => s.User)
                .Include(pa => pa.Faculty).ThenInclude(f => f.User)
                .FirstOrDefaultAsync(pa => pa.AllocationId == id);

            if (pa is null)
                return NotFound(new CommonApiResponse<ProjectAllocationResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Project allocation not found"
                });

            return Ok(new CommonApiResponse<ProjectAllocationResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Project allocation retrieved successfully",
                Data = MapToDto(pa)
            });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateProjectAllocationDto dto)
        {
            var validation = await createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new CommonApiResponse<ProjectAllocationResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });

            var duplicate = await context.ProjectAllocations.FirstOrDefaultAsync(pa =>
                pa.ProjectId == dto.ProjectId && pa.StudentId == dto.StudentId);
            if (duplicate != null)
                return BadRequest(new CommonApiResponse<ProjectAllocationResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Student is already allocated to this project."
                });

            var entity = new ProjectAllocation
            {
                ProjectId = dto.ProjectId,
                StudentId = dto.StudentId,
                FacultyId = dto.FacultyId,
                FinalScore = dto.FinalScore,
                Grade = dto.Grade,
                Status = dto.Status,
                RepositoryUrl = dto.RepositoryUrl,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            context.ProjectAllocations.Add(entity);
            await context.SaveChangesAsync();

            await context.Entry(entity).Reference(pa => pa.Project).LoadAsync();
            await context.Entry(entity).Reference(pa => pa.Student).LoadAsync();
            if (entity.Student != null)
                await context.Entry(entity.Student).Reference(s => s.User).LoadAsync();
            await context.Entry(entity).Reference(pa => pa.Faculty).LoadAsync();
            if (entity.Faculty != null)
                await context.Entry(entity.Faculty).Reference(f => f.User).LoadAsync();

            return StatusCode(201, new CommonApiResponse<ProjectAllocationResponseDto>
            {
                Success = true,
                StatusCode = 201,
                Message = "Project allocation created successfully",
                Data = MapToDto(entity)
            });
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateProjectAllocationDto dto)
        {
            var validation = await updateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new CommonApiResponse<ProjectAllocationResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });

            var existing = await context.ProjectAllocations.FindAsync(id);
            if (existing is null)
                return NotFound(new CommonApiResponse<ProjectAllocationResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Project allocation not found"
                });

            var duplicate = await context.ProjectAllocations.FirstOrDefaultAsync(pa =>
                pa.AllocationId != id &&
                pa.ProjectId == dto.ProjectId && pa.StudentId == dto.StudentId);
            if (duplicate != null)
                return BadRequest(new CommonApiResponse<ProjectAllocationResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Student is already allocated to this project."
                });

            existing.ProjectId = dto.ProjectId;
            existing.StudentId = dto.StudentId;
            existing.FacultyId = dto.FacultyId;
            existing.FinalScore = dto.FinalScore;
            existing.Grade = dto.Grade;
            existing.Status = dto.Status;
            existing.RepositoryUrl = dto.RepositoryUrl;
            existing.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();

            await context.Entry(existing).Reference(pa => pa.Project).LoadAsync();
            await context.Entry(existing).Reference(pa => pa.Student).LoadAsync();
            if (existing.Student != null)
                await context.Entry(existing.Student).Reference(s => s.User).LoadAsync();
            await context.Entry(existing).Reference(pa => pa.Faculty).LoadAsync();
            if (existing.Faculty != null)
                await context.Entry(existing.Faculty).Reference(f => f.User).LoadAsync();

            return Ok(new CommonApiResponse<ProjectAllocationResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Project allocation updated successfully",
                Data = MapToDto(existing)
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var entity = await context.ProjectAllocations
                .Include(pa => pa.Project)
                .Include(pa => pa.Student).ThenInclude(s => s.User)
                .Include(pa => pa.Faculty).ThenInclude(f => f.User)
                .FirstOrDefaultAsync(pa => pa.AllocationId == id);

            if (entity is null)
                return NotFound(new CommonApiResponse<ProjectAllocationResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Project allocation not found"
                });

            context.ProjectAllocations.Remove(entity);
            await context.SaveChangesAsync();

            return Ok(new CommonApiResponse<ProjectAllocationResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Project allocation deleted successfully",
                Data = MapToDto(entity)
            });
        }
    }
}
