using Auth.LogicLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.LogicLayer.Abstractions
{
    public interface IAuthService
    {
        public UserDTO RegisterUser(UserRegisterDTO newUser);
        public UserCrendentialsDTO Login(UserLoginDTO user);
    }
}
