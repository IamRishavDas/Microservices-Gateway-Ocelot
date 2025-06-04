using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserAuthenticationService.Models;
using UserAuthenticationService.Services;

namespace UserAuthenticationService.Controllers
{
    [ApiController, Route("api/auth")]
    public class AuthController: ControllerBase
    {
        private readonly JwtTokenService _tokenService;
        private readonly UserManager _userManager;

        public AuthController(JwtTokenService tokenService, UserManager userManager)
        {
            _tokenService = tokenService;
            _userManager = userManager;
        }

        [HttpPost("login")]
        public ActionResult<UserResponse?> Login([FromBody]UserRequest userRequest)
        {
            var response = _tokenService.GenerateJwtToken(userRequest);
            return response == null ? Unauthorized() : Ok(response);
        }

        [HttpPost("users")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateUser(User user)
        {
            return Ok(_userManager.CreateUser(user));
        }


    }
}
