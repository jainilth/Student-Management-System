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
    public class ProjectController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IValidator<CreateProjectDto> createValidator;
        private readonly IValidator<UpdateProjectDto> updateValidator;

        public ProjectController(AppDbContext _context,
            IValidator<CreateProjectDto> _createValidator,
            IValidator<UpdateProjectDto> _updateValidator)
        {
            context = _context;
            createValidator = _createValidator;
            updateValidator = _updateValidator;
        }

        private static ProjectResponseDto MapToDto(Project p) => new ProjectResponseDto
        {
            ProjectId = p.ProjectId,
            Title = p.Title,
            Description = p.Description,
            SemesterId = p.SemesterId,
            SemesterName = p.Semester != null ? p.Semester.SemesterName : string.Empty,
            ProgramId = p.ProgramId,
            ProgramName = p.AcademicProgram != null ? p.AcademicProgram.ProgramName : string.Empty,
            StartDate = p.StartDate,
            EndDate = p.EndDate,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt
        };

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var items = await context.Projects
                .Include(p => p.Semester)
                .Include(p => p.AcademicProgram)
                .Select(p => MapToDto(p)).ToListAsync();

            return Ok(new CommonApiResponse<List<ProjectResponseDto>>
            {
                Success = true,
                StatusCode = 200,
                Message = "Projects retrieved successfully",
                Data = items
            });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById([FromRoute] int id)
        {
            var p = await context.Projects
                .Include(p => p.Semester)
                .Include(p => p.AcademicProgram)
                .FirstOrDefaultAsync(p => p.ProjectId == id);

            if (p is null)
                return NotFound(new CommonApiResponse<ProjectResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Project not found"
                });

            return Ok(new CommonApiResponse<ProjectResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Project retrieved successfully",
                Data = MapToDto(p)
            });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateProjectDto dto)
        {
            var validation = await createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new CommonApiResponse<ProjectResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });

            var entity = new Project
            {
                Title = dto.Title,
                Description = dto.Description,
                SemesterId = dto.SemesterId,
                ProgramId = dto.ProgramId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            context.Projects.Add(entity);
            await context.SaveChangesAsync();

            await context.Entry(entity).Reference(p => p.Semester).LoadAsync();
            await context.Entry(entity).Reference(p => p.AcademicProgram).LoadAsync();

            return StatusCode(201, new CommonApiResponse<ProjectResponseDto>
            {
                Success = true,
                StatusCode = 201,
                Message = "Project created successfully",
                Data = MapToDto(entity)
            });
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateProjectDto dto)
        {
            var validation = await updateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new CommonApiResponse<ProjectResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });

            var existing = await context.Projects.FindAsync(id);
            if (existing is null)
                return NotFound(new CommonApiResponse<ProjectResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Project not found"
                });

            existing.Title = dto.Title;
            existing.Description = dto.Description;
            existing.SemesterId = dto.SemesterId;
            existing.ProgramId = dto.ProgramId;
            existing.StartDate = dto.StartDate;
            existing.EndDate = dto.EndDate;
            existing.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();

            await context.Entry(existing).Reference(p => p.Semester).LoadAsync();
            await context.Entry(existing).Reference(p => p.AcademicProgram).LoadAsync();

            return Ok(new CommonApiResponse<ProjectResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Project updated successfully",
                Data = MapToDto(existing)
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var entity = await context.Projects
                .Include(p => p.Semester)
                .Include(p => p.AcademicProgram)
                .FirstOrDefaultAsync(p => p.ProjectId == id);

            if (entity is null)
                return NotFound(new CommonApiResponse<ProjectResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Project not found"
                });

            context.Projects.Remove(entity);
            await context.SaveChangesAsync();

            return Ok(new CommonApiResponse<ProjectResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Project deleted successfully",
                Data = MapToDto(entity)
            });
        }
    }
}
