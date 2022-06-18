using Auth.DataAccessLayer;
using Auth.DataAccessLayer.Abstractions.Repos;
using Auth.DataAccessLayer.Entities;
using Auth.LogicLayer.Abstractions;
using Auth.LogicLayer.DTOs;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;

namespace Auth.LogicLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;        
        private readonly IMapper mapper;
        private readonly IConfiguration _configuration;

        public UserService(
            IUserRepository userRepo, 
            IMapper mapper, 
            IConfiguration configuration)            
        {
            this._userRepo = userRepo;
            this.mapper = mapper;
            this._configuration = configuration;
        }

        public IEnumerable<UserDTO> getUsers()
        {
            var usersDB = _userRepo.GetAllUsers();

            var users = mapper.Map<IEnumerable<UserDTO>>(usersDB);
            return users;
        }


    }
}
