using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserAuthenticationService.Models;

namespace UserAuthenticationService.Services
{
    public class JwtTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager _userManager;

        public JwtTokenService(IConfiguration configuration, UserManager userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public UserResponse? GenerateJwtToken(UserRequest userRequest)
        {
            if (IsValidUser(userRequest))
            {
                var user = _userManager.GetUsers().Where(user => user.UserName == userRequest.UserName).FirstOrDefault();
                if (int.TryParse(_configuration["Jwt:TokenValidityMins"], out int tokenValidityMinutes))
                {
                    var tokenExpiryTime = DateTime.Now.AddMinutes(tokenValidityMinutes);
                    var tokenKey = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
                    var claimsIdentity = new ClaimsIdentity(
                            new List<Claim>()
                            {
                                new Claim(JwtRegisteredClaimNames.Name, userRequest.UserName),
                                new Claim(ClaimTypes.Role, user.Role),

                                #region use when we have to do the authentication and authorization check in gateway
                                //new Claim("Role", user.Role), 
                                #endregion
                            }
                        );

                    var signingCredentials = new SigningCredentials(
                            new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256
                        );

                    var securityTokenDescriptor = new SecurityTokenDescriptor()
                    {
                        Subject = claimsIdentity,
                        Expires = tokenExpiryTime,
                        SigningCredentials = signingCredentials
                    };

                    var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                    var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
                    var token = jwtSecurityTokenHandler.WriteToken(securityToken);

                    return new UserResponse
                    (
                        UserName: user.UserName,
                        ExpiresIn: (int)tokenExpiryTime.Subtract(DateTime.Now).TotalSeconds,
                        Token: token
                    );
                }
            }

            return null;
        }

        private bool IsValidUser(UserRequest userRequest)
        {
            return _userManager.GetUsers().Any(user => user.UserName == userRequest.UserName && user.Password == userRequest.Password);
        }
    }
}
