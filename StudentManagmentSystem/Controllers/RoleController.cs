using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagmentSystem.Data;
using StudentManagmentSystem.Models;
using StudentManagmentSystem.Dto;


namespace StudentManagmentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly AppDbContext context;
        public RoleController(AppDbContext _context)
        {
            context = _context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllRoles()
        {
            var roles = await context.Roles.Select(r => new ResponseDto 
            {
                RoleId = r.RoleId,
                RoleName = r.RoleName
            }).ToListAsync();
            var response = new CommonApiResponse<List<ResponseDto>>
            {
                Success = true,
                StatusCode = 200,
                Message = "Roles retrieved successfully",
                Data = roles
            };
            return Ok(response);
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById([FromRoute] int id)
        {
            var role = await context.Roles.FindAsync(id);

            if (role is null)
            {
                return NotFound(new CommonApiResponse<ResponseDto> { Success = false, StatusCode = 404, Message = "Role not found" });
            }

            return Ok(new CommonApiResponse<ResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Role retrieved successfully",
                Data = new ResponseDto 
                { 
                    RoleId = role.RoleId, 
                    RoleName = role.RoleName 
                }
            });
        }

        [HttpPost]
        public async Task<ActionResult> CreateRole([FromBody] CreateRoleDto roleDto)
        {
            var role = new Role { RoleName = roleDto.RoleName };
            context.Roles.Add(role);
            await context.SaveChangesAsync();

            return StatusCode(201, new CommonApiResponse<ResponseDto>
            {
                Success = true,
                StatusCode = 201,
                Message = "Role created successfully",
                Data = new ResponseDto 
                { 
                    RoleId = role.RoleId, 
                    RoleName = role.RoleName 
                }
            });
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateRoleDto roleDto)
        {
            var existing = await context.Roles.FindAsync(id);
            if (existing is null)
            {
                return NotFound(new CommonApiResponse<ResponseDto> { Success = false, StatusCode = 404, Message = "Role not found" });
            }

            existing.RoleName = roleDto.RoleName;

            await context.SaveChangesAsync();

            return Ok(new CommonApiResponse<ResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Role updated successfully",
                Data = new ResponseDto 
                { 
                    RoleId = existing.RoleId, 
                    RoleName = existing.RoleName 
                }
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var role = await context.Roles.FindAsync(id);
            if (role is null)
            {
                return NotFound(new CommonApiResponse<ResponseDto> { Success = false, StatusCode = 404, Message = "Role not found" });
            }

            context.Roles.Remove(role);
            await context.SaveChangesAsync();
            
            return Ok(new CommonApiResponse<ResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Role deleted successfully",
                Data = new ResponseDto 
                { 
                    RoleId = role.RoleId, 
                    RoleName = role.RoleName 
                }
            });
        }
    }
}
