using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Common;
using UserService.Data.Model;

namespace UserService.Data.Repository
{
    public interface IRoleRepository
    {
        public Task<List<Role>> GetRolesByRoleNames(List<RoleName> roleNames); 
    }
}
