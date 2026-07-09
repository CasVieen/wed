using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WedsiteBanHang.Models
{
    // Bắt buộc phải kế thừa : IdentityUser để giữ lại các trường mặc định (Email, Password...)
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FullName { get; set; }

        public string? Address { get; set; }

        public string? Age { get; set; }
    }
}