using ExamenAppDotNet.Models;
using ExamenAppDotNet.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExamenAppDotNet.ViewModels
{
    public class RegisterPostModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public static User ToUser(RegisterPostModel registerPostModel)
        {
            return new User()
            {
                FirstName = registerPostModel.FirstName,
                LastName = registerPostModel.LastName,
                Email = registerPostModel.Email,
                Username = registerPostModel.Username,
                Password = UsersService.ComputeSha256Hash(registerPostModel.Password)
            };
        }

        public static User ToUpdateUser(User existingUser, RegisterPostModel registerPostModel)
        {
            existingUser.FirstName = registerPostModel.FirstName;
            existingUser.LastName = registerPostModel.LastName;
            existingUser.Email = registerPostModel.Email;
            existingUser.Username = registerPostModel.Username;
            existingUser.Password = UsersService.ComputeSha256Hash(registerPostModel.Password);

            return existingUser;
        }

    }

}

