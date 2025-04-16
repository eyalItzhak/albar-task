using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace UserManagementAPI.DAL.Models
{
    [Index(nameof(Username), IsUnique = true)] // Move the Index attribute to the class level
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Username { get; set; }
        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; }

        public DateTime? LastLogIn { get; set; }
    }
}