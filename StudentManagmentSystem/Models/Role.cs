using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace StudentManagmentSystem.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Required, MaxLength(100)]
        public string RoleName { get; set; } = string.Empty;

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
