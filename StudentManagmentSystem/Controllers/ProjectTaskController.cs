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
    public class ProjectTaskController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IValidator<CreateProjectTaskDto> createValidator;
        private readonly IValidator<UpdateProjectTaskDto> updateValidator;

        public ProjectTaskController(AppDbContext _context,
            IValidator<CreateProjectTaskDto> _createValidator,
            IValidator<UpdateProjectTaskDto> _updateValidator)
        {
            context = _context;
            createValidator = _createValidator;
            updateValidator = _updateValidator;
        }

        private static ProjectTaskResponseDto MapToDto(ProjectTask pt) => new ProjectTaskResponseDto
        {
            TaskId = pt.TaskId,
            ProjectAllocationId = pt.ProjectAllocationId,
            ProjectTitle = pt.ProjectAllocation?.Project?.Title ?? string.Empty,
            StudentEnrollmentNumber = pt.ProjectAllocation?.Student?.EnrollmentNumber ?? string.Empty,
            StudentName = pt.ProjectAllocation?.Student?.User?.UserName ?? string.Empty,
            TaskTitle = pt.TaskTitle,
            TaskDescription = pt.TaskDescription,
            TaskStatus = pt.TaskStatus,
            AssignedScore = pt.AssignedScore,
            EarnedScore = pt.EarnedScore,
            StartDate = pt.StartDate,
            DueDate = pt.DueDate,
            CompletedDate = pt.CompletedDate,
            FacultyRemarks = pt.FacultyRemarks,
            StudentRemarks = pt.StudentRemarks,
            CreatedAt = pt.CreatedAt,
            UpdatedAt = pt.UpdatedAt
        };

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var items = await context.ProjectTasks
                .Include(pt => pt.ProjectAllocation).ThenInclude(pa => pa.Project)
                .Include(pt => pt.ProjectAllocation).ThenInclude(pa => pa.Student).ThenInclude(s => s.User)
                .Select(pt => MapToDto(pt)).ToListAsync();

            return Ok(new CommonApiResponse<List<ProjectTaskResponseDto>>
            {
                Success = true,
                StatusCode = 200,
                Message = "Project tasks retrieved successfully",
                Data = items
            });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById([FromRoute] int id)
        {
            var pt = await context.ProjectTasks
                .Include(pt => pt.ProjectAllocation).ThenInclude(pa => pa.Project)
                .Include(pt => pt.ProjectAllocation).ThenInclude(pa => pa.Student).ThenInclude(s => s.User)
                .FirstOrDefaultAsync(pt => pt.TaskId == id);
            if (pt is null)
                return NotFound(new CommonApiResponse<ProjectTaskResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Project task not found"
                });

            return Ok(new CommonApiResponse<ProjectTaskResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Project task retrieved successfully",
                Data = MapToDto(pt)
            });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateProjectTaskDto dto)
        {
            var validation = await createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new CommonApiResponse<ProjectTaskResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });

            var entity = new ProjectTask
            {
                ProjectAllocationId = dto.ProjectAllocationId,
                TaskTitle = dto.TaskTitle,
                TaskDescription = dto.TaskDescription,
                TaskStatus = dto.TaskStatus,
                AssignedScore = dto.AssignedScore,
                EarnedScore = dto.EarnedScore,
                StartDate = dto.StartDate,
                DueDate = dto.DueDate,
                CompletedDate = dto.CompletedDate,
                FacultyRemarks = dto.FacultyRemarks,
                StudentRemarks = dto.StudentRemarks,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            context.ProjectTasks.Add(entity);
            await context.SaveChangesAsync();

            await context.Entry(entity).Reference(pt => pt.ProjectAllocation).LoadAsync();
            if (entity.ProjectAllocation != null)
            {
                await context.Entry(entity.ProjectAllocation).Reference(pa => pa.Project).LoadAsync();
                await context.Entry(entity.ProjectAllocation).Reference(pa => pa.Student).LoadAsync();
                if (entity.ProjectAllocation.Student != null)
                    await context.Entry(entity.ProjectAllocation.Student).Reference(s => s.User).LoadAsync();
            }

            return StatusCode(201, new CommonApiResponse<ProjectTaskResponseDto>
            {
                Success = true,
                StatusCode = 201,
                Message = "Project task created successfully",
                Data = MapToDto(entity)
            });
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateProjectTaskDto dto)
        {
            var validation = await updateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new CommonApiResponse<ProjectTaskResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });

            var existing = await context.ProjectTasks.FindAsync(id);
            if (existing is null)
                return NotFound(new CommonApiResponse<ProjectTaskResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Project task not found"
                });

            existing.ProjectAllocationId = dto.ProjectAllocationId;
            existing.TaskTitle = dto.TaskTitle;
            existing.TaskDescription = dto.TaskDescription;
            existing.TaskStatus = dto.TaskStatus;
            existing.AssignedScore = dto.AssignedScore;
            existing.EarnedScore = dto.EarnedScore;
            existing.StartDate = dto.StartDate;
            existing.DueDate = dto.DueDate;
            existing.CompletedDate = dto.CompletedDate;
            existing.FacultyRemarks = dto.FacultyRemarks;
            existing.StudentRemarks = dto.StudentRemarks;
            existing.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();

            await context.Entry(existing).Reference(pt => pt.ProjectAllocation).LoadAsync();
            if (existing.ProjectAllocation != null)
            {
                await context.Entry(existing.ProjectAllocation).Reference(pa => pa.Project).LoadAsync();
                await context.Entry(existing.ProjectAllocation).Reference(pa => pa.Student).LoadAsync();
                if (existing.ProjectAllocation.Student != null)
                    await context.Entry(existing.ProjectAllocation.Student).Reference(s => s.User).LoadAsync();
            }

            return Ok(new CommonApiResponse<ProjectTaskResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Project task updated successfully",
                Data = MapToDto(existing)
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var entity = await context.ProjectTasks
                .Include(pt => pt.ProjectAllocation).ThenInclude(pa => pa.Project)
                .Include(pt => pt.ProjectAllocation).ThenInclude(pa => pa.Student).ThenInclude(s => s.User)
                .FirstOrDefaultAsync(pt => pt.TaskId == id);
            if (entity is null)
                return NotFound(new CommonApiResponse<ProjectTaskResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Project task not found"
                });

            context.ProjectTasks.Remove(entity);
            await context.SaveChangesAsync();

            return Ok(new CommonApiResponse<ProjectTaskResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Project task deleted successfully",
                Data = MapToDto(entity)
            });
        }
    }
}
