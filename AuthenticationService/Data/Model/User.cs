using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.Data.Model
{
    [Table("User")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [MaxLength(50)]
        [Required]
        public string Password { get; set; }

        [MaxLength(250)]
        public string Address { get; set; }

        [MaxLength(50)]
        public string Country { get; set; }

        [Required]
        [MaxLength(15)]
        public string Mobile { get; set; }

        [Required]
        public string Email { get; set; }

        public List<UserRole> Roles { get; set; }

        public User()
        {
            Roles = new List<UserRole>();
        }
    }
}
