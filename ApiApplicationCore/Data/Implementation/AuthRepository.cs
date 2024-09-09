﻿using ApiApplicationCore.Data.Contract;
using ApiApplicationCore.Models;

namespace ApiApplicationCore.Data.Implementation
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IAppDbContext _appDbContext;

        public AuthRepository(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public bool RegisterUser(User user)
        {
            var result = false;
            if (user != null)
            {
                _appDbContext.users.Add(user);
                _appDbContext.SaveChanges();

                result = true;
            }

            return result;
        }

        public User? ValidateUser(string username)
        {
            User? user = _appDbContext.users.FirstOrDefault(c => c.LoginId.ToLower() == username.ToLower() || c.Email == username.ToLower());
            return user;
        }

        public bool UserExists(string loginId, string email)
        {
            if (_appDbContext.users.Any(c => c.LoginId.ToLower() == loginId.ToLower() || c.Email.ToLower() == email.ToLower()))
            {
                return true;
            }

            return false;
        }
        public bool UpdateUser(User user)
        {
            var result = false;
            if (user != null)
            {
                _appDbContext.users.Update(user);
                _appDbContext.SaveChanges();

                result = true;
            }
            return result;

        }
        public bool UserExist(int userId, string loginId, string email)
        {
            var user = _appDbContext.users.FirstOrDefault(c => c.userId != userId && (c.LoginId.ToLower() == loginId.ToLower() || c.Email.ToLower() == email.ToLower()));
            if (user != null)
            {
                return true;
            }
            return false;
        }
        public User? GetUser(string id)
        {
            var user = _appDbContext.users.FirstOrDefault(c => c.LoginId == id || c.Email == id);
            return user;
        }

    }
}
