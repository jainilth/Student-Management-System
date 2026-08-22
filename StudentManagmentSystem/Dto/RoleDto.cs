using System.ComponentModel.DataAnnotations;

namespace StudentManagmentSystem.Dto
{
    public class CreateRoleDto
    {
        [Required]
        public string RoleName { get; set; }
    }
    public class UpdateRoleDto
    {
        [Required]
        public string RoleName { get; set; }
    }
    public class ResponseDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
