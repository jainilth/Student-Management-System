using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagmentSystem.Data;
using StudentManagmentSystem.Models;

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
            var roles = await context.Roles.ToListAsync();
            var response = new CommonApiResponse<List<Role>>
            {
                Success = true,
                Message="Roles retrived sucessfully",
                Data=roles,
            };
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Role>> GetById(int id)
        {
            var role = await context.Roles.FindAsync(id);

            return role is null ? NotFound() : Ok(role);
        }

        [HttpPost]
        public async Task<ActionResult<Role>> CreateRole(Role role)
        {
            context.Roles.Add(role);
            await context.SaveChangesAsync();
            return Ok(new {message="added sucessfully",data = role });
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, Role role)
        {
            var existing = await context.Roles.FindAsync(id);
            if (existing is null)
            {
                return NotFound();
            }

            existing.RoleName = role.RoleName;

            await context.SaveChangesAsync();
            return Ok(new { message = "added Updated", data = existing });
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var role = await context.Roles.FindAsync(id);
            if (role is null)
            {
                return NotFound();
            }

            context.Roles.Remove(role);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
