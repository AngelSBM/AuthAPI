using Auth.DataAccessLayer.Abstractions.Repos;
using Auth.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.DataAccessLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthContext _authContext;

        public UserRepository(AuthContext context)
        {
            this._authContext = context;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _authContext.Users.ToList();
        }
    }
}
