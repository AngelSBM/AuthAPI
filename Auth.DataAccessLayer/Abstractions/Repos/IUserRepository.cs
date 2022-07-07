using Auth.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.DataAccessLayer.Abstractions.Repos
{
    public interface IUserRepository : IRepository<User>
    {
        public IEnumerable<User> GetUsersWithRoles();
    }
}
