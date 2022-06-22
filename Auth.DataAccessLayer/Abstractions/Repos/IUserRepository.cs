﻿using Auth.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.DataAccessLayer.Abstractions.Repos
{
    public interface IUserRepository
    {
        public IEnumerable<User> GetAllUsers();
        public void Register(User newUser);
        public User GetUserByEmail(string email);
        public User GetUserById(int userId);
        public bool UserExists(string email);
        public void SaveChanges();

    }
}
