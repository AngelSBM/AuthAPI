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

        public void Register(User newUser)
        {
            _authContext.Set<User>().Add(newUser);
        }

        public User GetUserByEmail(string email)
        {
            return _authContext.Users.Where(user => user.Email == email).FirstOrDefault();
        }

        public bool UserExists(string email)
        {
            return _authContext.Users.Any(user => user.Email == email);
        }


        public IEnumerable<User> GetAllUsers()
        {
            return _authContext.Users.ToList();
        }

        public void SaveChanges()
        {
            _authContext.SaveChanges();
        }

    }
}
