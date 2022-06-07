using Auth.DataAccessLayer;
using Auth.DataAccessLayer.Abstractions.Repos;
using Auth.DataAccessLayer.Entities;
using Auth.LogicLayer.Abstractions;
using Auth.LogicLayer.DTOs;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.LogicLayer.Services
{
    public class UserService : IUserService
    {
        public readonly IUserRepository _userRepo;
        public readonly IMapper mapper;
        public UserService(IUserRepository userRepo, IMapper mapper)
        {
            this._userRepo = userRepo;
            this.mapper = mapper;
        }

        public IEnumerable<UserDTO> getUsers()
        {
            var usersDB = _userRepo.GetAllUsers();

            var users = mapper.Map<IEnumerable<UserDTO>>(usersDB);
            return users;
        }

    }
}
