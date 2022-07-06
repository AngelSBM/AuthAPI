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
using Auth.DataAccessLayer.Abstractions;

namespace Auth.LogicLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;
        private readonly IConfiguration _configuration;

        public UserService(
            IUnitOfWork unitOfWork,
            IMapper mapper, 
            IConfiguration configuration)            
        {
            this._unitOfWork = unitOfWork;
            this.mapper = mapper;
            this._configuration = configuration;
        }

        public IEnumerable<UserDTO> getUsers()
        {
            var usersDB = _unitOfWork.userRepo.GetAllUsers();

            var users = mapper.Map<IEnumerable<UserDTO>>(usersDB);
            return users;
        }


    }
}
