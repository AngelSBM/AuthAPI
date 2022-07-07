using Auth.DataAccessLayer.Abstractions.Repos;
using Auth.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.DataAccessLayer.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly AuthContext _authContext;

        public UserRepository(AuthContext context) : base(context)
        {
            this._authContext = context;
        }

        /// <summary>
        /// Get users with roles
        /// </summary>
        /// <returns>User</returns>
        public IEnumerable<User> GetUsersWithRoles()
        {
            return _authContext.Users.Include(ur => ur.UsersRoles).ThenInclude(u => u.Role).ToList();
        }

    }
}
