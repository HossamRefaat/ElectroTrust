using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectroTrust.Entities.Enums;

namespace ElectroTrust.Entities.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public UserRole Role { get; set; } // Enum: Admin, Seller, Customer

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? ProfilePictureURL { get; set; }

        // Navigation properties
        public virtual Seller Seller { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
