using Auth.DataAccessLayer.Abstractions.Repos;
using Auth.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.DataAccessLayer.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private AuthContext _context;

        public AuthRepository(AuthContext context)
        {
            this._context = context;
        }

        public void CreateUserSession(User user, RefreshToken refreshToken)
        {            
            _context.Set<RefreshToken>().Add(refreshToken);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
