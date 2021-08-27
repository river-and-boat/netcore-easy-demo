using System.ComponentModel.DataAnnotations;

namespace UserService.Controller.Request
{
    public class RoleChangeRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
