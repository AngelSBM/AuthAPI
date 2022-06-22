using Auth.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.DataAccessLayer.Abstractions.Repos
{
    public interface IAuthRepository
    {
        public void CreateUserSession(User user, RefreshToken token);
        public RefreshToken findRefreshTokenByUserId(int userId);
        public void DeleteRefreshToken(RefreshToken refreshToken);
        public void SaveChanges();
    }
}
