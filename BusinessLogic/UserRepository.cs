using CLModels;
using Database;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class UserRepository : IUserRepository
    {
        // Injection IConfiguration (appsettings.json file)
        private readonly IConfiguration _configuration;
        private readonly DbService dbService;
        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            dbService = new DbService();
        }

        /// <summary>
        /// Login method calling database service
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns>JWT Token</returns>
        public async Task<string> Login(UserDTO userDTO)
        {
            UserDTO userFromDb = await dbService.GetOneUser(userDTO.Username);
            if (userFromDb == null)
            {
                return null;
            }
            return (GenerateToken(userFromDb));

        }

        /// <summary>
        /// Generates a JWT Bearer Token
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns>JWT Bearer Token</returns>
        private string GenerateToken(UserDTO userDTO)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userDTO.Username), // Add a claim for the username
                new Claim(ClaimTypes.NameIdentifier, userDTO.UID) // Add a claim for the Age
            };

            // As mentioned above i am using the _configuration file to access Jwt values
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
