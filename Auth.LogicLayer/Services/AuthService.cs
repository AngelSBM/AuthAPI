
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
using System.Net;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Auth.DataAccessLayer.Abstractions;
using Auth.ClientLayer.Helpers.Exceptions;

namespace Auth.LogicLayer.Services
{
    public class AuthService : IAuthService
    {

        IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;  
            this._configuration = configuration;
            this._httpContextAccessor = httpContextAccessor;
        }


        public UserDTO RegisterUser(UserRegisterDTO newUser)
        {

            validateNewUser(newUser);

            User userDB = new User();
            userDB.Name = newUser.Name;
            userDB.LastName = newUser.LastName;
            userDB.Email = newUser.Email;
            userDB.BirthDate = newUser.BirthDate;
            userDB.Phone = newUser.Phone;
            userDB.Salt = generateSalt();
            userDB.Password = hashPassword(newUser.Password, userDB.Salt);
            userDB.PublicId = Guid.NewGuid();

            _unitOfWork.userRepo.Add(userDB);
            _unitOfWork.Complete();

            return _mapper.Map<User, UserDTO>(userDB);
        }

        public UserCrendentialsDTO Login(UserLoginDTO user)
        {
            User userDB = _unitOfWork.userRepo.Find(u => u.Email == user.Email);
            if (userDB == null)
            {
                throw new NotFoundException("User not found");
            }

            var hashLoginPassword = hashPassword(user.Password, userDB.Salt);

            if (!(hashLoginPassword == userDB.Password))
            {
                throw new BadRequestException("Incorrect password");
            }

            string accessToken = createToken(userDB);
            string refreshToken = createRefreshToken(userDB);

            return new UserCrendentialsDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

        }


        public UserCrendentialsDTO RefreshSession()
        {
            var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];

            var session = _unitOfWork.authRepo.Find(session => session.Token.ToString() == refreshToken);

            var newSessionCredentials = new UserCrendentialsDTO();

            var userDB = _unitOfWork.userRepo.GetById(session.UserId);
            newSessionCredentials.AccessToken = createToken(userDB);
            newSessionCredentials.RefreshToken = createRefreshToken(userDB);
            deleteOldSession(session);

            return newSessionCredentials;
        }

        private void deleteOldSession(RefreshToken refreshToken)
        {

            _unitOfWork.authRepo.Remove(refreshToken);
            _unitOfWork.Complete();
        }


        private string createRefreshToken(User user)
        {
            try
            {
                var refreshToken = new RefreshToken()
                {
                    Token = Guid.NewGuid(),
                    UserId = user.Id,
                    CreatedAt = DateTime.Now,
                    ExpiresAt = DateTime.Now.AddDays(7),
                };

                _unitOfWork.authRepo.Add(refreshToken);            

                _unitOfWork.Complete();

                return refreshToken.Token.ToString();

            }
            catch (Exception)
            {

                throw;
            }

        }

        private void validateNewUser(UserRegisterDTO newUser)
        {

            bool userExists = _unitOfWork.userRepo.Exists(user => user.Email == newUser.Email);
            if (userExists)
            {
                throw new BadRequestException("User already exists!");
            }

        }


        private string createToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("UID", user.Id.ToString())
            };

            string secretKey = _configuration.GetSection("AppSettings:Token").Value;

            var key = new SymmetricSecurityKey
                (System.Text.Encoding.UTF8.GetBytes(secretKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);


            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddSeconds(30),
                signingCredentials: creds);

            string jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private byte[] generateSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }

            return salt;
        }

        private string hashPassword(string password, byte[] salt)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));

            return hashed;
        }


    }
}
