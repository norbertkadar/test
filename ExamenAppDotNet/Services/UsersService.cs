using ExamenAppDotNet.Models;
using ExamenAppDotNet.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace ExamenAppDotNet.Services
{
    public interface IUsersService
    {
        UserGetModel Authenticate(string username, string password);
        UserGetModel Register(RegisterPostModel registerInfo);
        User GetCurrentUser(HttpContext httpContext);
        IEnumerable<UserGetModel> GetAll();
        User DeleteUser(int id);
        User UpsertUser(int id, User modifiedUser);
    }

    public class UsersService : IUsersService
    {
        private ExamDbContext context;
        private readonly AppSettings appSettings;

        public UsersService(ExamDbContext context, IOptions<AppSettings> appSettings)
        {
            this.context = context;
            this.appSettings = appSettings.Value;
        }

        public UserGetModel Authenticate(string username, string password)
        {
            var user = context.Users
                .SingleOrDefault(x => x.Username == username &&
                                 x.Password == ComputeSha256Hash(password));

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username.ToString()),
                    new Claim(ClaimTypes.Role, user.UserRole.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var result = new UserGetModel
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                Token = tokenHandler.WriteToken(token)
            };
            // remove password before returning
            return result;
        }

        public static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            // TODO: also use salt
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public UserGetModel Register(RegisterPostModel registerInfo)
        {
            User existing = context.Users.FirstOrDefault(u => u.Username == registerInfo.Username);
            if (existing != null)
            {
                return null;
            }

            context.Users.Add(new User
            {
                Email = registerInfo.Email,
                LastName = registerInfo.LastName,
                FirstName = registerInfo.FirstName,
                Password = ComputeSha256Hash(registerInfo.Password),
                Username = registerInfo.Username,
                UserRole = UserRole.Regular
            });
            context.SaveChanges();
            return Authenticate(registerInfo.Username, registerInfo.Password);
        }

        public User GetCurrentUser(HttpContext httpContext)
        {
            string username = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
            return context.Users.FirstOrDefault(u => u.Username == username);
        }

        public IEnumerable<UserGetModel> GetAll()
        {
            // return users without passwords
            return context.Users.Select(user => new UserGetModel
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName =user.LastName,
                Role = user.UserRole.ToString(),
                Token = null
            });
        }

        public User DeleteUser(int id)
        {
            User existing = context.Users.FirstOrDefault(u => u.Id == id);

            if (existing == null)
            {
                return null;
            }
            context.Users.Remove(existing);
            context.SaveChanges();

            return existing;
        }

        public User UpsertUser(int id, User modifiedUser)
        {
            var existing = context.Users.AsNoTracking().FirstOrDefault(user => user.Id == id);

            if (existing == null)
            {
                return null;
            }
            modifiedUser.Id = existing.Id;
            context.Users.Update(modifiedUser);
            context.SaveChanges();

            return modifiedUser;
        }
    }
}
