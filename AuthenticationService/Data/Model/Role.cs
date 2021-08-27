using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UserService.Common;

namespace UserService.Data.Model
{
    [Table("Role")]
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public RoleName Name { get; set; }

        public List<UserRole> Users { get; set; }

        public Role()
        {
            Users = new List<UserRole>();
        }
    }
}
