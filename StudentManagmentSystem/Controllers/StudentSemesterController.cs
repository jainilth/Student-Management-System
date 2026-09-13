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
    public class StudentSemesterController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IValidator<CreateStudentSemesterDto> createValidator;
        private readonly IValidator<UpdateStudentSemesterDto> updateValidator;

        public StudentSemesterController(AppDbContext _context,
            IValidator<CreateStudentSemesterDto> _createValidator,
            IValidator<UpdateStudentSemesterDto> _updateValidator)
        {
            context = _context;
            createValidator = _createValidator;
            updateValidator = _updateValidator;
        }

        private static StudentSemesterResponseDto MapToDto(StudentSemester ss) => new StudentSemesterResponseDto
        {
            StudentSemesterId = ss.StudentSemesterId,
            StudentId = ss.StudentId,
            StudentEnrollmentNumber = ss.Student?.EnrollmentNumber ?? string.Empty,
            StudentName = ss.Student?.User?.UserName ?? string.Empty,
            AcademicProgramId = ss.Student?.ProgramId ?? 0,
            AcademicProgramName = ss.Student?.AcademicProgram?.ProgramName ?? string.Empty,
            SemesterId = ss.SemesterId,
            SemesterName = ss.Semester?.SemesterName ?? string.Empty,
            AcademicYearId = ss.AcademicYearId,
            AcademicYear = ss.AcademicYear != null ? ss.AcademicYear.Year : string.Empty,
            EnrollmentDate = ss.EnrollmentDate,
            Status = ss.Status,
            CreatedAt = ss.CreatedAt,
            UpdatedAt = ss.UpdatedAt
        };

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var items = await context.StudentSemesters
                .Include(ss => ss.Student).ThenInclude(s => s.User)
                .Include(ss => ss.Student).ThenInclude(s => s!.AcademicProgram)
                .Include(ss => ss.Semester)
                .Include(ss => ss.AcademicYear)
                .Select(ss => MapToDto(ss)).ToListAsync();

            return Ok(new CommonApiResponse<List<StudentSemesterResponseDto>>
            {
                Success = true,
                StatusCode = 200,
                Message = "Student semesters retrieved successfully",
                Data = items
            });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById([FromRoute] int id)
        {
            var ss = await context.StudentSemesters
                .Include(ss => ss.Student).ThenInclude(s => s.User)
                .Include(ss => ss.Student).ThenInclude(s => s!.AcademicProgram)
                .Include(ss => ss.Semester)
                .Include(ss => ss.AcademicYear)
                .FirstOrDefaultAsync(ss => ss.StudentSemesterId == id);

            if (ss is null)
                return NotFound(new CommonApiResponse<StudentSemesterResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Student semester not found"
                });

            return Ok(new CommonApiResponse<StudentSemesterResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Student semester retrieved successfully",
                Data = MapToDto(ss)
            });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateStudentSemesterDto dto)
        {
            // Step 1: Validate incoming DTO
            var validation = await createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
            {
                return BadRequest(new CommonApiResponse<StudentSemesterResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });
            }

            // Step 2: Validate Academic Year exists and parse format
            var academicYear = await context.AcademicYears
                .FirstOrDefaultAsync(ay => ay.AcademicYearId == dto.AcademicYearId);

            if (academicYear == null)
            {
                return BadRequest(new CommonApiResponse<StudentSemesterResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Academic year not found."
                });
            }

            var parts = academicYear.Year.Split("-");
            if (parts.Length != 2 || !int.TryParse(parts[0], out int startYear))
            {
                return BadRequest(new CommonApiResponse<StudentSemesterResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Invalid academic year format."
                });
            }

            int endYear = startYear + 1;
            DateTime startDate = new DateTime(startYear, 6, 1);
            DateTime endDate = new DateTime(endYear, 5, 31);

            // Step 3: Validate Student exists
            var student = await context.Students
                .Include(s => s.AcademicProgram)
                .FirstOrDefaultAsync(s => s.StudentId == dto.StudentId);

            if (student == null)
            {
                return BadRequest(new CommonApiResponse<StudentSemesterResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Student not found."
                });
            }

            // Step 4: Validate Admission Year and Program Duration
            var maxEligibleYear = student.AdmissionYear + (student.AcademicProgram?.DurationYears ?? 0);
            if (student.AdmissionYear > startYear || maxEligibleYear < endYear)
            {
                return BadRequest(new CommonApiResponse<StudentSemesterResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = $"Enrollment year must be between {student.AdmissionYear} and {maxEligibleYear}."
                });
            }

            // Step 5: Validate Enrollment Date range
            if (dto.EnrollmentDate < startDate || dto.EnrollmentDate > endDate)
            {
                return BadRequest(new CommonApiResponse<StudentSemesterResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = $"Enrollment date must be between {startDate:dd-MM-yyyy} and {endDate:dd-MM-yyyy}."
                });
            }

            // Step 6: Validate Requested Semester exists
            var requestedSemester = await context.Semesters.FindAsync(dto.SemesterId);
            if (requestedSemester == null)
            {
                return BadRequest(new CommonApiResponse<StudentSemesterResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Requested semester not found."
                });
            }

            // Step 7: Check Duplicate Semester Enrollment (Never allow same student in same semester)
            var semesterDuplicate = await context.StudentSemesters
                .FirstOrDefaultAsync(ss =>
                    ss.StudentId == dto.StudentId &&
                    ss.SemesterId == dto.SemesterId);

            if (semesterDuplicate != null)
            {
                return BadRequest(new CommonApiResponse<StudentSemesterResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "This student is already enrolled in this semester. A student cannot be enrolled in the same semester more than once."
                });
            }

            // Step 8: Validate Semester Progression, Previous Completion Status, and Academic Year Progression
            var existingEnrollments = await context.StudentSemesters
                .Include(ss => ss.Semester)
                .Include(ss => ss.AcademicYear)
                .Where(ss => ss.StudentId == dto.StudentId)
                .ToListAsync();

            var allEnrollmentsMock = existingEnrollments.Select(ss => (
                SemNum: ss.Semester!.SemesterNumber,
                AcYearStr: ss.AcademicYear!.Year,
                Status: ss.Status
            )).ToList();

            allEnrollmentsMock.Add((
                SemNum: requestedSemester.SemesterNumber,
                AcYearStr: academicYear.Year,
                Status: dto.Status
            ));

            allEnrollmentsMock = allEnrollmentsMock.OrderBy(e => e.SemNum).ToList();

            for (int i = 0; i < allEnrollmentsMock.Count; i++)
            {
                // Condition: Semesters must be strictly consecutive (1, 2, 3...)
                if (allEnrollmentsMock[i].SemNum != i + 1)
                {
                    return BadRequest(new CommonApiResponse<StudentSemesterResponseDto>
                    {
                        Success = false,
                        StatusCode = 400,
                        Message = "Student semesters must form a continuous sequence starting from semester 1. Ensure the semester is exactly one more than the current maximum."
                    });
                }

                if (i > 0)
                {
                    var prev = allEnrollmentsMock[i - 1];
                    var curr = allEnrollmentsMock[i];

                    // Condition: Previous semester must be Completed or Failed
                    var allowedStatuses = new[] { "completed", "failed" };
                    if (!allowedStatuses.Contains(prev.Status.ToLower()))
                    {
                        return BadRequest(new CommonApiResponse<StudentSemesterResponseDto>
                        {
                            Success = false,
                            StatusCode = 400,
                            Message = $"Semester {curr.SemNum} cannot be added because previous semester {prev.SemNum} status is '{prev.Status}'. It must be Completed or Failed."
                        });
                    }

                    // Condition: Odd -> Even must be the SAME Academic Year
                    if (prev.SemNum % 2 != 0 && curr.SemNum % 2 == 0)
                    {
                        if (prev.AcYearStr != curr.AcYearStr)
                        {
                            return BadRequest(new CommonApiResponse<StudentSemesterResponseDto>
                            {
                                Success = false,
                                StatusCode = 400,
                                Message = $"Academic year must remain '{prev.AcYearStr}' when moving from odd semester {prev.SemNum} to even semester {curr.SemNum}."
                            });
                        }
                    }
                    // Condition: Even -> Odd must be NEXT Academic Year (+1 year)
                    else if (prev.SemNum % 2 == 0 && curr.SemNum % 2 != 0)
                    {
                        if (int.TryParse(prev.AcYearStr.Split("-")[0], out int prevStart) &&
                            int.TryParse(curr.AcYearStr.Split("-")[0], out int currStart))
                        {
                            if (currStart != prevStart + 1)
                            {
                                return BadRequest(new CommonApiResponse<StudentSemesterResponseDto>
                                {
                                    Success = false,
                                    StatusCode = 400,
                                    Message = $"Academic year must strictly increase by one year (from {prev.AcYearStr}) when moving from even semester {prev.SemNum} to odd semester {curr.SemNum}."
                                });
                            }
                        }
                        else
                        {
                            return BadRequest(new CommonApiResponse<StudentSemesterResponseDto>
                            {
                                Success = false,
                                StatusCode = 400,
                                Message = "Invalid academic year format."
                            });
                        }
                    }
                }
            }

            // Step 9: Create and Save Entity
            var entity = new StudentSemester
            {
                StudentId = dto.StudentId,
                SemesterId = dto.SemesterId,
                AcademicYearId = dto.AcademicYearId,
                EnrollmentDate = dto.EnrollmentDate,
                Status = dto.Status,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.StudentSemesters.Add(entity);
            student.CurrentSemesterId = dto.SemesterId;
            student.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();

            // Step 10: Load Navigation References & Return Response
            await context.Entry(entity).Reference(ss => ss.Student).LoadAsync();
            if (entity.Student != null)
            {
                await context.Entry(entity.Student).Reference(s => s.User).LoadAsync();
                await context.Entry(entity.Student).Reference(s => s.AcademicProgram).LoadAsync();
            }
            await context.Entry(entity).Reference(ss => ss.Semester).LoadAsync();
            await context.Entry(entity).Reference(ss => ss.AcademicYear).LoadAsync();

            return StatusCode(201, new CommonApiResponse<StudentSemesterResponseDto>
            {
                Success = true,
                StatusCode = 201,
                Message = "Student semester created successfully",
                Data = MapToDto(entity)
            });
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateStudentSemesterDto dto)
        {
            // Step 1: Validate incoming DTO
            var validation = await updateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
            {
                return BadRequest(new CommonApiResponse<StudentSemesterResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Validation failed",
                    Errors = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });
            }

            // Step 2: Validate existing record exists
            var existing = await context.StudentSemesters.FindAsync(id);
            if (existing is null)
            {
                return NotFound(new CommonApiResponse<StudentSemesterResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Student semester not found"
                });
            }

            var student = await context.Students.FindAsync(dto.StudentId);
            if (student is null)
            {
                return BadRequest(new CommonApiResponse<StudentSemesterResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Student not found."
                });
            }

            // Step 3: Validate duplicate semester enrollment (excluding current record)
            var semesterDuplicate = await context.StudentSemesters.FirstOrDefaultAsync(ss =>
                ss.StudentSemesterId != id &&
                ss.StudentId == dto.StudentId &&
                ss.SemesterId == dto.SemesterId);

            if (semesterDuplicate != null)
            {
                return BadRequest(new CommonApiResponse<StudentSemesterResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "This student is already enrolled in this semester. A student cannot be enrolled in the same semester more than once."
                });
            }

            // Step 4: Validate Requested Semester exists
            var requestedSemester = await context.Semesters.FindAsync(dto.SemesterId);
            if (requestedSemester == null)
            {
                return BadRequest(new CommonApiResponse<StudentSemesterResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Requested semester not found."
                });
            }

            // Step 5: Validate Academic Year exists
            var newAcYearObj = await context.AcademicYears.FindAsync(dto.AcademicYearId);
            if (newAcYearObj == null)
            {
                return BadRequest(new CommonApiResponse<StudentSemesterResponseDto>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Requested academic year not found."
                });
            }

            // Step 6: Validate Semester Progression, Previous Completion Status, and Academic Year Progression
            var otherEnrollments = await context.StudentSemesters
                .Include(ss => ss.Semester)
                .Include(ss => ss.AcademicYear)
                .Where(ss => ss.StudentId == dto.StudentId && ss.StudentSemesterId != id)
                .ToListAsync();

            var allEnrollmentsMock = otherEnrollments.Select(ss => (
                SemNum: ss.Semester!.SemesterNumber,
                AcYearStr: ss.AcademicYear!.Year,
                Status: ss.Status
            )).ToList();

            allEnrollmentsMock.Add((
                SemNum: requestedSemester.SemesterNumber,
                AcYearStr: newAcYearObj.Year,
                Status: dto.Status
            ));

            allEnrollmentsMock = allEnrollmentsMock.OrderBy(e => e.SemNum).ToList();

            for (int i = 0; i < allEnrollmentsMock.Count; i++)
            {
                // Condition: Semesters must be strictly consecutive (1, 2, 3...)
                if (allEnrollmentsMock[i].SemNum != i + 1)
                {
                    return BadRequest(new CommonApiResponse<StudentSemesterResponseDto>
                    {
                        Success = false,
                        StatusCode = 400,
                        Message = "Updating to this semester would break the continuous sequence of enrolled semesters (1, 2, 3...)."
                    });
                }

                if (i > 0)
                {
                    var prev = allEnrollmentsMock[i - 1];
                    var curr = allEnrollmentsMock[i];

                    // Condition: Previous semester must be Completed or Failed
                    var allowedStatuses = new[] { "completed", "failed" };
                    if (!allowedStatuses.Contains(prev.Status.ToLower()))
                    {
                        return BadRequest(new CommonApiResponse<StudentSemesterResponseDto>
                        {
                            Success = false,
                            StatusCode = 400,
                            Message = $"Semester {curr.SemNum} cannot exist while previous semester {prev.SemNum} status is '{prev.Status}'. It must be Completed or Failed."
                        });
                    }

                    // Condition: Odd -> Even must be the SAME Academic Year
                    if (prev.SemNum % 2 != 0 && curr.SemNum % 2 == 0)
                    {
                        if (prev.AcYearStr != curr.AcYearStr)
                        {
                            return BadRequest(new CommonApiResponse<StudentSemesterResponseDto>
                            {
                                Success = false,
                                StatusCode = 400,
                                Message = $"Academic year must remain '{prev.AcYearStr}' when moving from odd semester {prev.SemNum} to even semester {curr.SemNum}."
                            });
                        }
                    }
                    // Condition: Even -> Odd must be NEXT Academic Year (+1 year)
                    else if (prev.SemNum % 2 == 0 && curr.SemNum % 2 != 0)
                    {
                        if (int.TryParse(prev.AcYearStr.Split("-")[0], out int prevStart) &&
                            int.TryParse(curr.AcYearStr.Split("-")[0], out int currStart))
                        {
                            if (currStart != prevStart + 1)
                            {
                                return BadRequest(new CommonApiResponse<StudentSemesterResponseDto>
                                {
                                    Success = false,
                                    StatusCode = 400,
                                    Message = $"Academic year must strictly increase by one year (from {prev.AcYearStr}) when moving from even semester {prev.SemNum} to odd semester {curr.SemNum}."
                                });
                            }
                        }
                        else
                        {
                            return BadRequest(new CommonApiResponse<StudentSemesterResponseDto>
                            {
                                Success = false,
                                StatusCode = 400,
                                Message = "Invalid academic year format."
                            });
                        }
                    }
                }
            }

            // Step 7: Update entity and save changes
            existing.StudentId = dto.StudentId;
            existing.SemesterId = dto.SemesterId;
            existing.AcademicYearId = dto.AcademicYearId;
            existing.EnrollmentDate = dto.EnrollmentDate;
            existing.Status = dto.Status;
            existing.UpdatedAt = DateTime.UtcNow;
            student.CurrentSemesterId = dto.SemesterId;
            student.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();

            // Step 8: Load Navigation References & Return Response
            await context.Entry(existing).Reference(ss => ss.Student).LoadAsync();
            if (existing.Student != null)
            {
                await context.Entry(existing.Student).Reference(s => s.User).LoadAsync();
                await context.Entry(existing.Student).Reference(s => s.AcademicProgram).LoadAsync();
            }
            await context.Entry(existing).Reference(ss => ss.Semester).LoadAsync();
            await context.Entry(existing).Reference(ss => ss.AcademicYear).LoadAsync();

            return Ok(new CommonApiResponse<StudentSemesterResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Student semester updated successfully",
                Data = MapToDto(existing)
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var entity = await context.StudentSemesters
                .Include(ss => ss.Student).ThenInclude(s => s.User)
                .Include(ss => ss.Student).ThenInclude(s => s!.AcademicProgram)
                .Include(ss => ss.Semester)
                .Include(ss => ss.AcademicYear)
                .FirstOrDefaultAsync(ss => ss.StudentSemesterId == id);

            if (entity is null)
            {
                return NotFound(new CommonApiResponse<StudentSemesterResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Student semester not found"
                });
            }

            context.StudentSemesters.Remove(entity);
            await context.SaveChangesAsync();

            return Ok(new CommonApiResponse<StudentSemesterResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Student semester deleted successfully",
                Data = MapToDto(entity)
            });
        }
    }
}
