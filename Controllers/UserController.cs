using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shortener.Model;
using Shortener.Services;
using Shortener.ViewModel;

namespace Shortener.Controllers
{
    [EnableCors("Client", PolicyName = "Client")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtService _jweService;
        private const string USER_ROLE = "User";
        public UserController(UserManager<User> userManager, JwtService jwtService)
        {
            _userManager = userManager;
            _jweService = jwtService;
        }
        // GET: api/User/GetUsername
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("GetUsername")]
        public async Task<ActionResult> GetUserName()
        {
            User user = await _userManager.FindByNameAsync(User.Identity?.Name);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(new { user.UserName });
        }
        // POST: api/User/Register
        [HttpPost("Register")]
        public async Task<ActionResult<AuthenticationResponse>> Register(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }
            var result = await _userManager.CreateAsync(user, user.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            user.Password = null;
            user.ConfirmPassword = null;
            await _userManager.AddToRoleAsync(user, USER_ROLE);
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jweService.CreateToken(user, roles);
            return Ok(token);
        }
        // POST: api/User/SignIn
        [HttpPost("SignIn")]
        public async Task<ActionResult<AuthenticationResponse>> SignIn(AuthenticationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Bad credentials");
            }
            var user = await _userManager.FindByEmailAsync(request.EmailAddress);

            if(user == null)
            {
                return BadRequest("User not found");
            }
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {
                return BadRequest("Wrong password");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jweService.CreateToken(user, roles);
            return Ok(token);

        }
    }
}
