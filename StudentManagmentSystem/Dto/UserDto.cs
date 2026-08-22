using System;
using System.ComponentModel.DataAnnotations;

namespace StudentManagmentSystem.Dto
{
    public class CreateUserDto
    {
        [Required, MaxLength(100)]
        public string UserName { get; set; } = string.Empty;

        [Required, MaxLength(150), EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MaxLength(255)]
        public string Password { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Address { get; set; }

        public DateTime? Dob { get; set; }

        [MaxLength(15)]
        public string Mobilenumber { get; set; }

        [MaxLength(500)]
        public string ProfilePhoto { get; set; }

        public int RoleId { get; set; }
    }

    public class UpdateUserDto
    {
        [Required, MaxLength(100)]
        public string UserName { get; set; } = string.Empty;

        [Required, MaxLength(150), EmailAddress]
        public string Email { get; set; } = string.Empty;

        [MaxLength(255)]
        public string Password { get; set; }

        [MaxLength(500)]
        public string Address { get; set; }

        public DateTime? Dob { get; set; }

        [MaxLength(15)]
        public string Mobilenumber { get; set; }

        [MaxLength(500)]
        public string ProfilePhoto { get; set; }

        public bool IsActivate { get; set; }

        public int RoleId { get; set; }
    }

    public class UserResponseDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; }
        public DateTime? Dob { get; set; }
        public string Mobilenumber { get; set; }
        public string ProfilePhoto { get; set; }
        public bool IsActivate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int RoleId { get; set; }
        public string RoleName{get; set;}
    }
}
