using System.ComponentModel.DataAnnotations;

namespace JobPortal.Models
{
    public class UserModel
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }

    }
}
