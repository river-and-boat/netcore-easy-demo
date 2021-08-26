using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.Data.Model
{
    [Table("UserRole")]
    public class UserRole
    {
        [Key]
        public int UserId { get; set; }

        [Key]
        public int RoleId { get; set; }

        public User User { get; set; }

        public Role Role { get; set; }
    }
}
