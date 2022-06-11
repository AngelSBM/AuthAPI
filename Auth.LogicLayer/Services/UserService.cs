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

        public void Login(UserLoginDTO user)
        {
            User userDB = _userRepo.GetUserByEmail(user.Email);
            if (userDB == null)
            {
                throw new Exception("User not found");
            }

            bool invalidPassword = user.Password == "";

            if (invalidPassword) { throw new Exception("Invalid Password"); }
            
            var hashLoginPassword = hashPassword(user.Password, userDB.Salt);               
            
            bool userAuthenticated = hashLoginPassword == userDB.Password;

            if (!userAuthenticated)
            {
                throw new Exception("Incorrect password");
            }
        }

        private void validateNewUser(UserRegisterDTO newUser)
        {
            if(newUser == null)
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
