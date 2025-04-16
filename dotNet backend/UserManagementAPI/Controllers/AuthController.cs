using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.BL.Services;
using UserManagementAPI.DAL.Dtos.Auth;
using UserManagementAPI.DAL.Dtos.Results;

namespace UserManagementAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        public AuthController(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        private void SetAuthCookie(string token)
        {
            HttpContext.Response.Cookies.Append("jwt_token", token, new CookieOptions
            {
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(1)
            });
        }

        private IActionResult HandleTokenResult(Result<string> tokenResult, string username)
        {
            if (!tokenResult.IsSuccess)
            {
                return Unauthorized(new { Message = tokenResult.Errors.FirstOrDefault() });
            }

            SetAuthCookie(tokenResult.Value);
            _userService.UpdateUserLastLogIn(username);
            return Ok(new { Token = tokenResult.Value });
        }

        [HttpPost("signup")]
        public IActionResult SignUp([FromBody] SignInUpRequestDto request)
        {
            var result = _userService.SignUp(request.Username, request.Password);

            if (!result.IsSuccess)
            {
                return BadRequest(new { Message = string.Join(", ", result.Errors) });
            }

            var tokenResult = _jwtService.GenerateToken(result.Value);
            return HandleTokenResult(tokenResult, result.Value.Username);
        }

        [HttpPost("signin")]
        public IActionResult SignIn([FromBody] SignInUpRequestDto request)
        {
            var result = _userService.ValidateUser(request.Username, request.Password);

            if (!result.IsSuccess)
            {
                return Unauthorized(new { Message = string.Join(", ", result.Errors) });
            }

            var tokenResult = _jwtService.GenerateToken(result.Value);
            return HandleTokenResult(tokenResult, result.Value.Username);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt_token");
            return Ok(new { Message = "Logged out successfully" });
        }
    }
}