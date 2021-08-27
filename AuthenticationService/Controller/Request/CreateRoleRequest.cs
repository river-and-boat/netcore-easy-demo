using System.ComponentModel.DataAnnotations;
using UserService.Common;

namespace UserService.Controller.Request
{
    public class CreateRoleRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
