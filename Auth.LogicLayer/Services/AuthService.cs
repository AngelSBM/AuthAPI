
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
    public class AuthService : IAuthService
    {

        private readonly IUserRepository _userRepo;
        private readonly IAuthRepository _authRepo;
        private readonly IMapper mapper;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(
            IUserRepository userRepo,
            IAuthRepository authRepo,
            IMapper mapper,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            this._userRepo = userRepo;
            this._authRepo = authRepo;
            this.mapper = mapper;
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

            _userRepo.Register(userDB);
            _userRepo.SaveChanges();

            return new UserDTO();
        }

        public UserCrendentialsDTO Login(UserLoginDTO user)
        {
            User userDB = _userRepo.GetUserByEmail(user.Email);
            if (userDB == null)
            {
                throw new Exception("User not found");
            }

            var hashLoginPassword = hashPassword(user.Password, userDB.Salt);

            if (!(hashLoginPassword == userDB.Password))
            {
                throw new Exception("Incorrect password");
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
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("UID")?.Value);
            var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];

            validateOldSession(userId, refreshToken);

            var newSessionCredentials = new UserCrendentialsDTO();

            var userDB = _userRepo.GetUserById(userId);
            newSessionCredentials.AccessToken = createToken(userDB);
            newSessionCredentials.RefreshToken = createRefreshToken(userDB);

            return newSessionCredentials;
        }

        private void validateOldSession(int userId, string refreshToken)
        {
            if(userId == null || refreshToken == null)
            {
                throw new Exception("Invalid session");
            }

            var session = _authRepo.findRefreshTokenByUserId(userId);
            if(session.Token.ToString() != refreshToken)
            {
                throw new Exception("Invalid session");
            }

            if(session.ExpiresAt < DateTime.Now)
            {
                throw new Exception("Session expired!");
            }

            _authRepo.DeleteRefreshToken(session);
            _authRepo.SaveChanges();
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

                _authRepo.CreateUserSession(user, refreshToken);
                _authRepo.SaveChanges();

                return refreshToken.Token.ToString();

            }
            catch (Exception)
            {

                throw;
            }

        }

        private void validateNewUser(UserRegisterDTO newUser)
        {
            if (newUser == null)
            {
                throw new ArgumentNullException("Invalid data");
            }

            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(newUser.Email);
            if (!match.Success)
            {
                throw new Exception("Invalid email format");
            }

            bool userExists = _userRepo.UserExists(newUser.Email);
            if (userExists)
            {
                throw new Exception("User already exists!");
            }

            bool invalidObject = newUser.Name == "" || newUser.LastName == "" || newUser.Password == "" || newUser.Phone == "";

            if (invalidObject)
            {
                throw new Exception("Some of the arguments for the registration are invalid!");
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
