using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Data.Model;

namespace UserService.Data.Repository
{
    public interface IUserRoleRepository
    {
        public Task AssignRoleToUserAsync(User user, List<Role> roles);

        public Task<List<Role>> FindRolesByUserIdAsync();

        public Task DeleteRolesByUserId(int id);
    }
}
